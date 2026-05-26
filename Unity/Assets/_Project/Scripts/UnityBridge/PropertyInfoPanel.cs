using System;
using Legacy.Commands;
using Legacy.Save;
using UnityEngine;
using Legacy.History;
using Legacy.World;

namespace Legacy.UnityBridge
{
    public sealed class PropertyInfoPanel : MonoBehaviour
    {
        private string _message = "WASD to move. E at doors. F1 debug.";
        private PanelMode _mode = PanelMode.Overview;
        private WorldActionKind _activeMiniGameAction;
        private WorldEntityId _activeMiniGameActorId;
        private WorldEntityId _activeMiniGamePlaceId;
        private WorldEntityId _activeTaskId;
        private bool _isSavingOrLoading;

        private enum PanelMode
        {
            Overview,
            Jobs,
            History,
            Property,
            MiniGame
        }

        public void Show(string message)
        {
            _message = string.IsNullOrWhiteSpace(message) ? string.Empty : message;
        }

        public void ShowJobMiniGame(WorldEntityId actorId, WorldActionKind action, WorldEntityId targetPlaceId)
        {
            _activeMiniGameActorId = actorId;
            _activeMiniGameAction = action;
            _activeMiniGamePlaceId = targetPlaceId;
            _mode = PanelMode.MiniGame;

            _message = TryPrepareActiveTask(actorId, action, targetPlaceId, out _activeTaskId)
                ? "Job task ready. Complete the placeholder mini-game in the UI."
                : "You need a contract and active shift from the Jobs UI first.";
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(16, 16, 470, 420), GUI.skin.box);
            GUILayout.Label("legacy smoke");
            GUILayout.Space(4);
            GUILayout.Label($"Objective: {GetObjectiveText()}");
            GUILayout.Space(4);
            DrawNavigation();
            GUILayout.Space(4);
            GUILayout.Label(_message);
            GUILayout.Space(8);
            DrawCurrentPanel();
            GUILayout.Space(8);
            GUILayout.Label("World: WASD move, E interact with doors/counters/NPCs/buildings | UI: jobs, history, save/load, wait");
            GUILayout.EndArea();
        }

        private void DrawNavigation()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Overview")) {
                _mode = PanelMode.Overview;
            }

            if (GUILayout.Button("Jobs")) {
                _mode = PanelMode.Jobs;
            }

            if (GUILayout.Button("History")) {
                _mode = PanelMode.History;
            }

            if (GUILayout.Button("Property")) {
                _mode = PanelMode.Property;
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Wait 30m")) {
                ExecuteAndShow(new AdvanceTimeCommand(30));
            }

            if (GUILayout.Button("Save")) {
                SaveWorldAsync();
            }

            if (GUILayout.Button("Load")) {
                LoadWorldAsync();
            }

            GUILayout.EndHorizontal();
        }

        private void DrawCurrentPanel()
        {
            switch (_mode) {
                case PanelMode.Jobs:
                    DrawJobsPanel();
                    break;
                case PanelMode.History:
                    DrawHistoryPanel();
                    break;
                case PanelMode.Property:
                    DrawPropertyPanel();
                    break;
                case PanelMode.MiniGame:
                    DrawMiniGamePanel();
                    break;
                default:
                    DrawOverviewPanel();
                    break;
            }
        }

        private string GetObjectiveText()
        {
            if (WorldBootstrap.Runtime == null) {
                return "World not loaded.";
            }

            WorldState state = WorldBootstrap.Runtime.State;
            var playerId = new WorldEntityId("citizen_noaharan");
            if (!state.TryGetCitizenRegistration(playerId, out CitizenRegistrationState _)) {
                return "Find the blue civic desk and register.";
            }

            if (state.GetHistoryByKind(HistoryEventKind.WorldActionPerformed).Count == 0) {
                return "Open Jobs, accept a shift, then use a workplace counter.";
            }

            if (state.Transactions.Count == 0) {
                return "Use the counter, complete the UI mini-game, and move money.";
            }

            if (state.GetHistoryByKind(HistoryEventKind.BuildingOwnershipTransferred).Count == 0) {
                return "Open Property and transfer cafe ownership.";
            }

            return "Save, load, wait, and inspect history to verify persistence.";
        }

        private void DrawOverviewPanel()
        {
            DrawMoneySummary();
            GUILayout.Space(6);
            DrawRecentTransactions();
            GUILayout.Space(6);
            DrawRecentHistory(3);
        }

        private void DrawMoneySummary()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            WorldState state = WorldBootstrap.Runtime.State;
            GUILayout.Label("Money:");
            DrawAccount(state, "account_noaharan_cash");
            DrawAccount(state, "account_linden_cafe");
            DrawAccount(state, "account_pell_pharmacy");
        }

        private void DrawAccount(WorldState state, string accountId)
        {
            if (state.TryGetMoneyAccount(new WorldEntityId(accountId), out MoneyAccountState account)) {
                GUILayout.Label($"- {account.DisplayName}: {EconomySystem.FormatMoney(account.BalanceCents)}");
            }
        }

        private void DrawRecentTransactions()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            GUILayout.Label("Transactions:");
            WorldState state = WorldBootstrap.Runtime.State;
            int start = Mathf.Max(0, state.Transactions.Count - 3);
            if (state.Transactions.Count == 0) {
                GUILayout.Label("- none yet");
                return;
            }

            for (int i = start; i < state.Transactions.Count; i++) {
                TransactionState transaction = state.Transactions[i];
                GUILayout.Label($"- {transaction.Timestamp.Time} {EconomySystem.FormatMoney(transaction.AmountCents)} {transaction.Reason}");
            }
        }

        private void DrawRecentHistory(int count)
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            GUILayout.Label("History:");
            foreach (HistoryEvent historyEvent in HistoryQuery.Last(WorldBootstrap.Runtime.State.RecentHistory, count)) {
                GUILayout.Label($"- {historyEvent.Timestamp.Time} {historyEvent.Description}");
            }
        }

        private void DrawJobsPanel()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            WorldState state = WorldBootstrap.Runtime.State;
            GUILayout.Label("Available Jobs:");
            DrawJobOffer(
                state,
                "Linden Cafe shift",
                "Serve customers at the cafe counter.",
                new WorldEntityId("posting_cafe_worker_001"),
                new WorldEntityId("application_noah_cafe_ui"),
                new WorldEntityId("contract_noah_cafe_ui"),
                new WorldEntityId("shift_noah_cafe_ui"),
                new WorldEntityId("workplace_linden_cafe"));
            DrawJobOffer(
                state,
                "Pell Pharmacy shelf work",
                "Stock shelves at the pharmacy counter.",
                new WorldEntityId("posting_pharmacy_clerk_001"),
                new WorldEntityId("application_noah_pharmacy_ui"),
                new WorldEntityId("contract_noah_pharmacy_ui"),
                new WorldEntityId("shift_noah_pharmacy_ui"),
                new WorldEntityId("workplace_pell_pharmacy"));
        }

        private void DrawJobOffer(
            WorldState state,
            string title,
            string description,
            WorldEntityId postingId,
            WorldEntityId applicationId,
            WorldEntityId contractId,
            WorldEntityId shiftId,
            WorldEntityId workplaceId)
        {
            var playerId = new WorldEntityId("citizen_noaharan");
            if (!state.TryGetWorkplace(workplaceId, out WorkplaceState workplace)) {
                return;
            }

            string placeName = state.TryGetPlace(workplace.PlaceId, out PlaceState place)
                ? place.DisplayName
                : workplace.PlaceId.ToString();
            bool hasContract = false;
            if (state.TryGetJobPosting(postingId, out JobPostingState posting)) {
                hasContract = state.TryGetActiveContract(playerId, workplaceId, posting.RoleId, out EmploymentContractState _);
            }

            bool hasShift = state.TryGetActiveShift(playerId, workplaceId, out ShiftState _);

            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label($"{title} - {placeName}");
            GUILayout.Label(description);
            GUILayout.Label(hasContract ? hasShift ? "Status: on shift" : "Status: hired" : "Status: available");
            if (!hasContract && GUILayout.Button($"Apply and accept {title}")) {
                ExecuteAndShow(new ApplyForJobCommand(applicationId, postingId, playerId));
                ExecuteAndShow(new OfferJobCommand(applicationId, workplace.OwnerCitizenId));
                ExecuteAndShow(new AcceptJobOfferCommand(contractId, applicationId, playerId));
            } else if (hasContract && !hasShift && GUILayout.Button($"Start {title}")) {
                ExecuteAndShow(new StartShiftCommand(shiftId, contractId, 120));
            }

            GUILayout.EndVertical();
        }

        private void DrawHistoryPanel()
        {
            DrawRecentHistory(8);
        }

        private void DrawPropertyPanel()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            WorldState state = WorldBootstrap.Runtime.State;
            GUILayout.Label("Property:");
            DrawBuildingOwner(state, new WorldEntityId("building_linden_cafe"));
            DrawBuildingOwner(state, new WorldEntityId("building_pell_pharmacy"));
            DrawBuildingOwner(state, new WorldEntityId("building_marlows_books"));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Inspect cafe plot")) {
                ExecuteAndShow(new InspectPlotCommand(
                    new WorldEntityId("plot_linden_14"),
                    new WorldEntityId("citizen_noaharan")));
            }

            if (GUILayout.Button("Transfer cafe to Rowan")) {
                ExecuteAndShow(new TransferBuildingOwnershipCommand(
                    new WorldEntityId("building_linden_cafe"),
                    new WorldEntityId("citizen_rowan"),
                    new WorldEntityId("citizen_noaharan")));
            }

            GUILayout.EndHorizontal();
        }

        private void DrawBuildingOwner(WorldState state, WorldEntityId buildingId)
        {
            if (!state.TryGetBuilding(buildingId, out BuildingState building)) {
                return;
            }

            string ownerName = state.TryGetCitizen(building.OwnerCitizenId, out CitizenState owner)
                ? owner.DisplayName
                : building.OwnerCitizenId.ToString();
            GUILayout.Label($"- {building.DisplayName}: owned by {ownerName}");
        }

        private void DrawMiniGamePanel()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            WorldState state = WorldBootstrap.Runtime.State;
            string placeName = state.TryGetPlace(_activeMiniGamePlaceId, out PlaceState place)
                ? place.DisplayName
                : _activeMiniGamePlaceId.ToString();
            RoleAuthorizationResult authorization = new RoleSystem(state).CanPerform(_activeMiniGameActorId, _activeMiniGameAction, _activeMiniGamePlaceId);

            GUILayout.Label($"Active job task: {_activeMiniGameAction}");
            GUILayout.Label($"Workplace: {placeName}");
            if (!authorization.IsAllowed || string.IsNullOrEmpty(_activeTaskId.Value)) {
                GUILayout.Label("You have not acquired this job and started a shift yet.");
                if (GUILayout.Button("Open Jobs")) {
                    _mode = PanelMode.Jobs;
                }
                return;
            }

            GUILayout.Label(_activeMiniGameAction == WorldActionKind.ServeCustomer
                ? "Placeholder mini-game: read order, prep drink, make change."
                : "Placeholder mini-game: compare shelf list, move stock, verify count.");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Step 1")) {
                _message = "Step 1 complete.";
            }

            if (GUILayout.Button("Step 2")) {
                _message = "Step 2 complete.";
            }

            if (GUILayout.Button("Complete Task")) {
                ExecuteAndShow(new SubmitMiniGameResultCommand(_activeTaskId, _activeMiniGameActorId, 85, 100, 45, 1));
                ExecuteAndShow(new CompleteJobTaskCommand(_activeTaskId, _activeMiniGameActorId));
                _mode = PanelMode.Overview;
            }

            GUILayout.EndHorizontal();
        }

        private bool TryPrepareActiveTask(WorldEntityId actorId, WorldActionKind action, WorldEntityId targetPlaceId, out WorldEntityId taskId)
        {
            taskId = default;
            if (WorldBootstrap.Runtime == null ||
                !WorldBootstrap.Runtime.State.TryGetWorkplaceByPlace(targetPlaceId, out WorkplaceState workplace) ||
                !WorldBootstrap.Runtime.State.TryGetActiveShift(actorId, workplace.Id, out ShiftState shift)) {
                return false;
            }

            string definitionId = action == WorldActionKind.ServeCustomer
                ? JobTaskCatalog.ServeCafeCustomer
                : JobTaskCatalog.StockPharmacyShelves;
            taskId = new WorldEntityId($"task_ui_{definitionId}_{WorldBootstrap.Runtime.State.JobTasksById.Count + 1:000}");
            WorldCommandResult create = WorldBootstrap.Runtime.Execute(new CreateJobTaskCommand(taskId, definitionId, workplace.Id));
            if (!create.Succeeded) {
                Show(create.Message);
                return false;
            }

            WorldCommandResult start = WorldBootstrap.Runtime.Execute(new StartJobTaskCommand(taskId, shift.Id, actorId));
            if (!start.Succeeded) {
                Show(start.Message);
                return false;
            }

            return true;
        }

        private void ExecuteAndShow(IWorldCommand command)
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            WorldCommandResult result = WorldBootstrap.Runtime.Execute(command);
            Show(result.Message);
        }

        private async void SaveWorldAsync()
        {
            if (_isSavingOrLoading || WorldBootstrap.SaveManager == null || WorldBootstrap.Runtime == null) {
                return;
            }

            _isSavingOrLoading = true;
            try {
                SaveResult result = await WorldBootstrap.SaveManager.SaveAsync("world-memory-smoke", WorldBootstrap.Runtime.State);
                Show(result == SaveResult.Ok ? "Saved shared world." : "Save failed.");
            } catch (Exception ex) {
                Debug.LogException(ex);
                Show("Save failed. See Console.");
            } finally {
                _isSavingOrLoading = false;
            }
        }

        private async void LoadWorldAsync()
        {
            if (_isSavingOrLoading || WorldBootstrap.SaveManager == null || WorldBootstrap.Runtime == null) {
                return;
            }

            _isSavingOrLoading = true;
            try {
                WorldState loaded = await WorldBootstrap.SaveManager.LoadAsync("world-memory-smoke");
                WorldBootstrap.Runtime.ReplaceState(loaded);
                Show("Loaded shared world.");
            } catch (Exception ex) {
                Debug.LogException(ex);
                Show("Load failed. Save first.");
            } finally {
                _isSavingOrLoading = false;
            }
        }
    }
}
