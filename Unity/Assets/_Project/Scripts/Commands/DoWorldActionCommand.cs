using Legacy.History;
using Legacy.World;

namespace Legacy.Commands
{
    public sealed class DoWorldActionCommand : IWorldCommand
    {
        private readonly WorldEntityId _actorId;
        private readonly WorldActionKind _action;
        private readonly WorldEntityId _targetPlaceId;
        private readonly WorldEntityId _targetEntityId;

        public DoWorldActionCommand(
            WorldEntityId actorId,
            WorldActionKind action,
            WorldEntityId targetPlaceId,
            WorldEntityId targetEntityId = default)
        {
            _actorId = actorId;
            _action = action;
            _targetPlaceId = targetPlaceId;
            _targetEntityId = targetEntityId;
        }

        public WorldCommandResult Execute(WorldCommandContext context)
        {
            if (!context.State.TryGetCitizen(_actorId, out CitizenState actor)) {
                return WorldCommandResult.Failure($"Citizen not found: {_actorId}");
            }

            if (!context.State.TryGetPlace(_targetPlaceId, out PlaceState place)) {
                return WorldCommandResult.Failure($"Place not found: {_targetPlaceId}");
            }

            var roleSystem = new RoleSystem(context.State);
            RoleAuthorizationResult authorization = roleSystem.CanPerform(actor.Id, _action, place.Id);
            if (!authorization.IsAllowed) {
                return WorldCommandResult.Failure(authorization.Reason);
            }

            WorldCommandResult paidActionResult = TryApplyPaidAction(context, actor, place);
            if (paidActionResult != null) {
                return paidActionResult;
            }

            HistoryEvent historyEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.WorldActionPerformed,
                $"{actor.DisplayName} performed {_action} at {place.DisplayName}.",
                new[] { actor.Id },
                new[] { place.Id });

            WorldCommandResult result = WorldCommandResult
                .Success($"{actor.DisplayName} performed {_action} at {place.DisplayName}.")
                .WithChangedEntity(actor.Id)
                .WithChangedEntity(place.Id)
                .WithHistoryEvent(historyEvent);

            if (!string.IsNullOrEmpty(_targetEntityId.Value)) {
                result.WithChangedEntity(_targetEntityId);
            }

            return result;
        }

        private WorldCommandResult TryApplyPaidAction(WorldCommandContext context, CitizenState actor, PlaceState place)
        {
            if (_action != WorldActionKind.ServeCustomer && _action != WorldActionKind.StockShelves) {
                return null;
            }

            if (!TryGetBusinessForPlace(context.State, place.Id, out BuildingState business)) {
                return WorldCommandResult.Failure($"No business found for {place.DisplayName}.");
            }

            if (!context.State.TryGetMoneyAccountForOwner(actor.Id, out MoneyAccountState actorAccount)) {
                return WorldCommandResult.Failure($"No money account found for {actor.DisplayName}.");
            }

            if (!context.State.TryGetMoneyAccountForOwner(business.Id, out MoneyAccountState businessAccount)) {
                return WorldCommandResult.Failure($"No business account found for {business.DisplayName}.");
            }

            var economy = new EconomySystem(context.State);
            var result = WorldCommandResult
                .Success()
                .WithChangedEntity(actor.Id)
                .WithChangedEntity(place.Id)
                .WithChangedEntity(business.Id)
                .WithChangedEntity(actorAccount.Id)
                .WithChangedEntity(businessAccount.Id);

            if (_action == WorldActionKind.ServeCustomer) {
                if (!context.State.TryGetMoneyAccount(new WorldEntityId("account_holland_cash"), out MoneyAccountState customerAccount)) {
                    return WorldCommandResult.Failure("No customer account found for cafe sale.");
                }

                EconomyTransferResult sale = economy.Transfer(
                    customerAccount.Id,
                    businessAccount.Id,
                    425,
                    "Coffee and pastry sale",
                    place.Id,
                    _action,
                    context.State.CurrentTime);
                if (!sale.Succeeded) {
                    return WorldCommandResult.Failure(sale.Message);
                }

                EconomyTransferResult tip = economy.Transfer(
                    businessAccount.Id,
                    actorAccount.Id,
                    125,
                    "Cafe service tip",
                    place.Id,
                    _action,
                    context.State.CurrentTime);
                if (!tip.Succeeded) {
                    return WorldCommandResult.Failure(tip.Message);
                }

                result.WithChangedEntity(customerAccount.Id)
                    .WithHistoryEvent(CreatePaymentEvent(context, sale.Transaction))
                    .WithHistoryEvent(CreatePaymentEvent(context, tip.Transaction));
            } else {
                EconomyTransferResult wage = economy.Transfer(
                    businessAccount.Id,
                    actorAccount.Id,
                    300,
                    "Shelf stocking wage",
                    place.Id,
                    _action,
                    context.State.CurrentTime);
                if (!wage.Succeeded) {
                    return WorldCommandResult.Failure(wage.Message);
                }

                result.WithHistoryEvent(CreatePaymentEvent(context, wage.Transaction));
            }

            HistoryEvent jobEvent = context.History.Create(
                context.State.CurrentTime,
                HistoryEventKind.JobActionPerformed,
                $"{actor.DisplayName} performed paid work: {_action} at {place.DisplayName}.",
                new[] { actor.Id },
                new[] { place.Id, business.Id });

            string message = $"{actor.DisplayName} performed {_action} at {place.DisplayName}. Money changed.";
            return result
                .WithHistoryEvent(jobEvent)
                .WithHistoryEvent(context.History.Create(
                    context.State.CurrentTime,
                    HistoryEventKind.WorldActionPerformed,
                    message,
                    new[] { actor.Id },
                    new[] { place.Id }))
                .WithMessage(message);
        }

        private static HistoryEvent CreatePaymentEvent(WorldCommandContext context, TransactionState transaction)
        {
            string amount = EconomySystem.FormatMoney(transaction.AmountCents);
            WorldEntityId[] actorIds = System.Array.Empty<WorldEntityId>();
            if (context.State.TryGetMoneyAccount(transaction.PayerAccountId, out MoneyAccountState payer) &&
                context.State.TryGetMoneyAccount(transaction.PayeeAccountId, out MoneyAccountState payee)) {
                actorIds = new[] { payer.OwnerEntityId, payee.OwnerEntityId };
            }

            return context.History.Create(
                transaction.Timestamp,
                HistoryEventKind.PaymentRecorded,
                $"{amount} changed hands: {transaction.Reason}.",
                actorIds,
                new[] { transaction.RelatedPlaceId });
        }

        private static bool TryGetBusinessForPlace(WorldState state, WorldEntityId placeId, out BuildingState business)
        {
            foreach (BuildingState candidate in state.BuildingsById.Values) {
                if (candidate.InteriorPlaceId == placeId) {
                    business = candidate;
                    return true;
                }
            }

            business = null;
            return false;
        }
    }
}
