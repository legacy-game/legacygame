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
        private static readonly WorldEntityId PlayerId = new("citizen_noaharan");
        private static readonly WorldEntityId CafeWorkplaceId = new("workplace_linden_cafe");
        private static readonly WorldEntityId PharmacyWorkplaceId = new("workplace_pell_pharmacy");

        private string _message = "WASD to move. E at doors. F1 debug.";
        private string _lastTaskSummary = "No task completed yet.";
        private string _lastShiftSummary = "No shift ended yet.";
        private PanelMode _mode = PanelMode.Overview;
        private WorldActionKind _activeMiniGameAction;
        private WorldEntityId _activeMiniGameActorId;
        private WorldEntityId _activeMiniGamePlaceId;
        private WorldEntityId _activeTaskId;
        private Vector2 _panelScroll;
        private string _lastDialogueLine = "No conversation yet.";
        private string _lastPublicRecord = "No public record queried yet.";
        private bool _isSavingOrLoading;

        private enum PanelMode
        {
            Overview,
            Jobs,
            History,
            Property,
            Inventory,
            Dialogue,
            Civic,
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

            if (TryPrepareActiveTask(actorId, action, targetPlaceId, out _activeTaskId)) {
                _message = "OK: job task ready. Complete the placeholder mini-game in the UI.";
            } else {
                _mode = PanelMode.Overview;
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(16, 16, 560, 640), GUI.skin.box);
            GUILayout.Label("legacy smoke - playable slice");
            GUILayout.Space(4);
            DrawHud();
            GUILayout.Space(6);
            GUILayout.Label($"Objective: {GetObjectiveText()}");
            GUILayout.Space(4);
            DrawNavigation();
            GUILayout.Space(4);
            GUILayout.Label($"Message: {_message}");
            GUILayout.Space(8);
            _panelScroll = GUILayout.BeginScrollView(_panelScroll, GUILayout.Height(430));
            DrawCurrentPanel();
            GUILayout.EndScrollView();
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
            if (GUILayout.Button("Inventory")) {
                _mode = PanelMode.Inventory;
            }

            if (GUILayout.Button("Dialogue")) {
                _mode = PanelMode.Dialogue;
            }

            if (GUILayout.Button("Civic")) {
                _mode = PanelMode.Civic;
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

            if (GUILayout.Button("End Morning")) {
                ExecuteAndShow(new EndMorningCommand(new WorldEntityId("citizen_noaharan")));
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
                case PanelMode.Inventory:
                    DrawInventoryPanel();
                    break;
                case PanelMode.Dialogue:
                    DrawDialoguePanel();
                    break;
                case PanelMode.Civic:
                    DrawCivicPanel();
                    break;
                case PanelMode.MiniGame:
                    DrawMiniGamePanel();
                    break;
                default:
                    DrawOverviewPanel();
                    break;
            }
        }

        private void DrawHud()
        {
            if (WorldBootstrap.Runtime == null) {
                GUILayout.Label("HUD: world not loaded.");
                return;
            }

            WorldState state = WorldBootstrap.Runtime.State;
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label($"Time: {state.CurrentTime.Time} on {state.CurrentTime.Date}");
            GUILayout.Label($"Location: {GetPlayerLocationText(state)}");
            if (TryFindActivePlayerShift(state, out ShiftState shift)) {
                GUILayout.Label($"Current shift: {GetWorkplaceName(state, shift.WorkplaceId)} | until {shift.ExpectedEndAt.Time} | earned {EconomySystem.FormatMoney(shift.EarnedCents)} | tasks {shift.CompletedTaskIds.Count}");
            } else {
                GUILayout.Label("Current shift: none - open Jobs to accept work and start a shift.");
            }

            if (TryFindActivePlayerTask(state, out JobTaskState task)) {
                GUILayout.Label($"Current task: {GetTaskName(task)} | {task.Status} | quality {task.Quality}");
            } else {
                GUILayout.Label("Current task: none - use a workplace counter while on shift.");
            }

            GUILayout.EndVertical();
        }

        private string GetObjectiveText()
        {
            if (WorldBootstrap.Runtime == null) {
                return "World not loaded.";
            }

            WorldState state = WorldBootstrap.Runtime.State;
            if (!state.TryGetCitizenRegistration(PlayerId, out CitizenRegistrationState _)) {
                return "Find the blue civic desk and register.";
            }

            if (!TryFindActivePlayerShift(state, out ShiftState _)) {
                return "Open Jobs, accept a shift, then use a workplace counter.";
            }

            if (state.Transactions.Count == 0) {
                return "Wait for Holland or Sasha, use the matching counter, and finish the queued task.";
            }

            if (state.GetHistoryByKind(HistoryEventKind.BuildingOwnershipTransferred).Count == 0) {
                return "Open Property and transfer cafe ownership.";
            }

            return "Save, load, wait, and inspect history to verify persistence.";
        }

        private void DrawOverviewPanel()
        {
            DrawShiftSummary();
            GUILayout.Space(6);
            DrawTaskSummary();
            GUILayout.Space(6);
            DrawMoneySummary();
            GUILayout.Space(6);
            DrawMorningSummary();
            GUILayout.Space(6);
            DrawVisits();
            GUILayout.Space(6);
            DrawQueuedTasks();
            GUILayout.Space(6);
            DrawInventorySummary();
            GUILayout.Space(6);
            DrawRecentTransactions();
            GUILayout.Space(6);
            DrawRecentHistory(3);
        }

        private void DrawMorningSummary()
        {
            if (WorldBootstrap.Runtime?.State.Morning == null) {
                return;
            }

            MorningState morning = WorldBootstrap.Runtime.State.Morning;
            GUILayout.Label($"Morning: {morning.Status} | Tasks {morning.TasksCompleted} | Earned {EconomySystem.FormatMoney(morning.MoneyEarnedCents)}");
        }

        private void DrawVisits()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            GUILayout.Label("Visits:");
            int shown = 0;
            foreach (VisitState visit in WorldBootstrap.Runtime.State.VisitsById.Values) {
                string visitor = WorldBootstrap.Runtime.State.TryGetCitizen(visit.VisitorCitizenId, out CitizenState citizen)
                    ? citizen.DisplayName
                    : visit.VisitorCitizenId.ToString();
                GUILayout.Label($"- {visitor}: {visit.Status} ({visit.Intent})");
                shown++;
                if (shown >= 3) {
                    break;
                }
            }

            if (shown == 0) {
                GUILayout.Label("- none yet; wait for visitors");
            }
        }

        private void DrawQueuedTasks()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            GUILayout.Label("Queued Tasks:");
            int shown = 0;
            foreach (JobTaskState task in WorldBootstrap.Runtime.State.JobTasksById.Values) {
                if (task.Status != JobTaskStatus.Queued && task.Status != JobTaskStatus.Active) {
                    continue;
                }

                string name = JobTaskCatalog.TryGet(task.DefinitionId, out JobTaskDefinition definition)
                    ? definition.DisplayName
                    : task.DefinitionId;
                GUILayout.Label($"- {name}: {task.Status}");
                shown++;
                if (shown >= 3) {
                    break;
                }
            }

            if (shown == 0) {
                GUILayout.Label("- none waiting");
            }
        }

        private void DrawInventorySummary()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            GUILayout.Label("Inventory:");
            DrawInventory(WorldBootstrap.Runtime.State, new WorldEntityId("workplace_linden_cafe"), "Cafe");
            DrawInventory(WorldBootstrap.Runtime.State, new WorldEntityId("workplace_pell_pharmacy"), "Pharmacy");
        }

        private void DrawInventory(WorldState state, WorldEntityId workplaceId, string label)
        {
            if (!state.TryGetWorkplaceInventory(workplaceId, out WorkplaceInventoryState inventory)) {
                GUILayout.Label($"- {label}: missing inventory");
                return;
            }

            string summary = string.Empty;
            foreach (InventoryStackState stack in inventory.Stacks) {
                if (!string.IsNullOrEmpty(summary)) {
                    summary += ", ";
                }

                summary += $"{stack.ItemId} x{stack.Count}";
            }

            GUILayout.Label(string.IsNullOrEmpty(summary)
                ? $"- {label}: empty"
                : $"- {label}: {summary}");
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
                CafeWorkplaceId);
            DrawJobOffer(
                state,
                "Pell Pharmacy shelf work",
                "Stock shelves at the pharmacy counter.",
                new WorldEntityId("posting_pharmacy_clerk_001"),
                new WorldEntityId("application_noah_pharmacy_ui"),
                new WorldEntityId("contract_noah_pharmacy_ui"),
                new WorldEntityId("shift_noah_pharmacy_ui"),
                PharmacyWorkplaceId);
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
            if (!state.TryGetWorkplace(workplaceId, out WorkplaceState workplace)) {
                return;
            }

            string placeName = state.TryGetPlace(workplace.PlaceId, out PlaceState place)
                ? place.DisplayName
                : workplace.PlaceId.ToString();
            bool hasContract = false;
            if (state.TryGetJobPosting(postingId, out JobPostingState posting)) {
                hasContract = state.TryGetActiveContract(PlayerId, workplaceId, posting.RoleId, out EmploymentContractState _);
            }

            bool hasShift = state.TryGetActiveShift(PlayerId, workplaceId, out ShiftState activeShift);

            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label($"{title} - {placeName}");
            GUILayout.Label(description);
            GUILayout.Label(hasContract ? hasShift ? "Status: on shift" : "Status: hired" : "Status: available");
            if (!hasContract && GUILayout.Button($"Apply and accept {title}")) {
                ExecuteAndShow(new ApplyForJobCommand(applicationId, postingId, PlayerId));
                ExecuteAndShow(new OfferJobCommand(applicationId, workplace.OwnerCitizenId));
                ExecuteAndShow(new AcceptJobOfferCommand(contractId, applicationId, PlayerId));
            } else if (hasContract && !hasShift && GUILayout.Button($"Start {title}")) {
                ExecuteAndShow(new StartShiftCommand(CreateShiftId(state, shiftId), contractId, 120));
            } else if (hasShift && GUILayout.Button($"End {title}")) {
                WorldCommandResult result = ExecuteAndShow(new EndShiftCommand(activeShift.Id, PlayerId));
                if (result.Succeeded) {
                    _lastShiftSummary = BuildShiftSummary(activeShift);
                }
            }

            GUILayout.EndVertical();
        }

        private void DrawHistoryPanel()
        {
            DrawRecentHistory(8);
            GUILayout.Space(6);
            DrawShiftSummaries();
        }

        private void DrawShiftSummaries()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            GUILayout.Label("Shift Summaries:");
            int start = Mathf.Max(0, WorldBootstrap.Runtime.State.ShiftSummaries.Count - 3);
            if (WorldBootstrap.Runtime.State.ShiftSummaries.Count == 0) {
                GUILayout.Label("- none yet");
                return;
            }

            for (int i = start; i < WorldBootstrap.Runtime.State.ShiftSummaries.Count; i++) {
                ShiftSummaryState summary = WorldBootstrap.Runtime.State.ShiftSummaries[i];
                GUILayout.Label($"- {summary.TasksCompleted} tasks, {EconomySystem.FormatMoney(summary.EarnedCents)} earned");
            }
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

        private void DrawInventoryPanel()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            WorldState state = WorldBootstrap.Runtime.State;
            GUILayout.Label("Inventory / Containers:");
            DrawInventoryContainerByOwner(state, CafeWorkplaceId, InventoryContainerKind.WorkplaceStorage, "Cafe storage");
            DrawInventoryContainerByOwner(state, PharmacyWorkplaceId, InventoryContainerKind.WorkplaceStorage, "Pharmacy storage");
            DrawInventorySummary();

            GUILayout.Space(8);
            GUILayout.Label("Wallets / Tills:");
            DrawCashDrawerByOwner(state, PlayerId, CashContainerKind.Wallet, "Player wallet");
            DrawCashDrawerByOwner(state, CafeWorkplaceId, CashContainerKind.Till, "Cafe till");
            DrawCashDrawerByOwner(state, PharmacyWorkplaceId, CashContainerKind.Till, "Pharmacy till");
            foreach (CashDrawerState drawer in state.CashDrawersById.Values) {
                DrawCashDrawer(drawer);
            }

            GUILayout.Space(8);
            DrawRecentTransactions();
        }

        private void DrawInventoryContainerByOwner(
            WorldState state,
            WorldEntityId ownerId,
            InventoryContainerKind kind,
            string missingLabel)
        {
            if (!state.TryGetInventoryContainerForOwner(ownerId, kind, out InventoryContainerState container)) {
                GUILayout.Label($"- {missingLabel}: no container yet");
                return;
            }

            DrawInventoryContainer(container);
        }

        private void DrawInventoryContainer(InventoryContainerState container)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label($"{container.DisplayName} ({container.Kind})");
            if (container.Stacks.Count == 0) {
                GUILayout.Label("- empty");
            }

            foreach (InventoryStackState stack in container.Stacks) {
                string itemName = ItemCatalog.TryGet(stack.ItemId, out ItemDefinition item)
                    ? item.DisplayName
                    : stack.ItemId;
                string value = item == null || item.BaseValueCents <= 0
                    ? string.Empty
                    : $" | {EconomySystem.FormatMoney(item.BaseValueCents)} each";
                GUILayout.Label($"- {itemName} x{stack.Count}{value}");
            }

            GUILayout.EndVertical();
        }

        private void DrawCashDrawerByOwner(WorldState state, WorldEntityId ownerId, CashContainerKind kind, string missingLabel)
        {
            if (!state.TryGetCashDrawerForOwner(ownerId, kind, out CashDrawerState drawer)) {
                GUILayout.Label($"- {missingLabel}: no cash drawer yet");
                return;
            }

            DrawCashDrawer(drawer);
        }

        private void DrawCashDrawer(CashDrawerState drawer)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label($"{drawer.DisplayName} ({drawer.Kind}) - {EconomySystem.FormatMoney(drawer.TotalCents)}");
            if (drawer.Stacks.Count == 0) {
                GUILayout.Label("- empty");
            }

            foreach (CashDenominationStackState stack in drawer.Stacks) {
                GUILayout.Label($"- {FormatDenomination(stack.Denomination)} x{stack.Count} = {EconomySystem.FormatMoney(stack.TotalCents)}");
            }

            GUILayout.EndVertical();
        }

        private void DrawDialoguePanel()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            WorldState state = WorldBootstrap.Runtime.State;
            GUILayout.Label("Dialogue:");
            GUILayout.Label(_lastDialogueLine);
            GUILayout.Space(6);
            GUILayout.Label("People in this scene:");
            int shown = 0;
            foreach (CitizenState citizen in state.CitizensById.Values) {
                if (citizen.Id == PlayerId || citizen.CurrentSceneId != state.CurrentSceneId) {
                    continue;
                }

                DrawDialogueCandidate(state, citizen);
                shown++;
            }

            if (shown == 0) {
                GUILayout.Label("- no one else is here right now");
            }

            GUILayout.Space(8);
            GUILayout.Label("Known citizens:");
            DrawDialogueShortcut(state, new WorldEntityId("citizen_rowan"));
            DrawDialogueShortcut(state, new WorldEntityId("citizen_old_mr_pell"));
            DrawDialogueShortcut(state, new WorldEntityId("citizen_mr_holland"));
            DrawDialogueShortcut(state, new WorldEntityId("citizen_sasha"));
        }

        private void DrawDialogueShortcut(WorldState state, WorldEntityId citizenId)
        {
            if (state.TryGetCitizen(citizenId, out CitizenState citizen)) {
                DrawDialogueCandidate(state, citizen);
            }
        }

        private void DrawDialogueCandidate(WorldState state, CitizenState citizen)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label($"{citizen.DisplayName} - {citizen.Activity} at {GetPlaceName(state, citizen.CurrentPlaceId)}");
            if (TryGetOpenVisitForCitizen(state, citizen.Id, out VisitState visit)) {
                GUILayout.Label($"Visit: {visit.Status} / {visit.Intent}");
                if (!string.IsNullOrWhiteSpace(visit.ArrivalLine)) {
                    GUILayout.Label($"Arrival: \"{visit.ArrivalLine}\"");
                }

                if (!string.IsNullOrWhiteSpace(visit.CompletionLine)) {
                    GUILayout.Label($"After service: \"{visit.CompletionLine}\"");
                }
            }

            if (state.DialogueStatesByCitizenId.TryGetValue(citizen.Id, out DialogueState dialogue)) {
                string lastLine = DialogueCatalog.TryGet(dialogue.LastLineId, out DialogueLine line)
                    ? line.Text
                    : dialogue.LastLineId;
                GUILayout.Label($"Talked {dialogue.ConversationCount} time(s). Last: {lastLine}");
            }

            if (TryGetRelationship(state, citizen.Id, PlayerId, out RelationshipState relationship)) {
                GUILayout.Label($"Relationship: affinity {relationship.Affinity}, familiarity {relationship.Familiarity}");
            }

            if (GUILayout.Button($"Talk to {citizen.DisplayName}")) {
                WorldCommandResult result = ExecuteAndShow(new TalkToCitizenCommand(PlayerId, citizen.Id, GetDialogueTopic(citizen.Id)));
                _lastDialogueLine = result.Message;
            }

            GUILayout.EndVertical();
        }

        private void DrawCivicPanel()
        {
            if (WorldBootstrap.Runtime == null) {
                return;
            }

            WorldState state = WorldBootstrap.Runtime.State;
            GUILayout.Label("Public Records:");
            GUILayout.BeginHorizontal();
            DrawPublicRecordButton("Noaharan", PlayerId);
            DrawPublicRecordButton("Rowan", new WorldEntityId("citizen_rowan"));
            DrawPublicRecordButton("Pell", new WorldEntityId("citizen_old_mr_pell"));
            DrawPublicRecordButton("Holland", new WorldEntityId("citizen_mr_holland"));
            DrawPublicRecordButton("Sasha", new WorldEntityId("citizen_sasha"));
            GUILayout.EndHorizontal();
            GUILayout.TextArea(_lastPublicRecord, GUILayout.Height(140));

            GUILayout.Space(8);
            GUILayout.Label("Known record summaries:");
            foreach (PublicRecordState record in state.PublicRecordsByCitizenId.Values) {
                string citizenName = state.TryGetCitizen(record.CitizenId, out CitizenState citizen)
                    ? citizen.DisplayName
                    : record.CitizenId.ToString();
                GUILayout.Label($"- {citizenName}: reputation {record.ReputationScore}, filed {record.ReportsFiled}, received {record.ReportsReceived}");
            }

            GUILayout.Space(6);
            GUILayout.Label("Recent civic registry:");
            int start = Mathf.Max(0, state.CivicRegistryEntries.Count - 5);
            if (state.CivicRegistryEntries.Count == 0) {
                GUILayout.Label("- none yet");
            }

            for (int i = start; i < state.CivicRegistryEntries.Count; i++) {
                CivicRegistryEntryState entry = state.CivicRegistryEntries[i];
                string citizenName = state.TryGetCitizen(entry.CitizenId, out CitizenState citizen)
                    ? citizen.DisplayName
                    : entry.CitizenId.ToString();
                GUILayout.Label($"- {entry.Kind}: {citizenName} - {entry.Summary}");
            }

            GUILayout.Space(6);
            DrawRecentHistory(5);
        }

        private void DrawPublicRecordButton(string label, WorldEntityId citizenId)
        {
            if (GUILayout.Button(label)) {
                WorldCommandResult result = ExecuteAndShow(new QueryPublicRecordCommand(citizenId, PlayerId, 5, 5));
                _lastPublicRecord = result.Message;
            }
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
                WorldCommandResult submit = ExecuteAndShow(new SubmitMiniGameResultCommand(_activeTaskId, _activeMiniGameActorId, 85, 100, 45, 1));
                if (submit.Succeeded) {
                    WorldCommandResult complete = ExecuteAndShow(new CompleteJobTaskCommand(_activeTaskId, _activeMiniGameActorId));
                    if (complete.Succeeded && state.TryGetJobTask(_activeTaskId, out JobTaskState completedTask)) {
                        _lastTaskSummary = BuildTaskSummary(completedTask);
                    }

                    _mode = PanelMode.Overview;
                }
            }

            GUILayout.EndHorizontal();
        }

        private bool TryPrepareActiveTask(WorldEntityId actorId, WorldActionKind action, WorldEntityId targetPlaceId, out WorldEntityId taskId)
        {
            taskId = default;
            if (WorldBootstrap.Runtime == null ||
                !WorldBootstrap.Runtime.State.TryGetWorkplaceByPlace(targetPlaceId, out WorkplaceState workplace)) {
                Show("No workplace is attached to this counter.");
                return false;
            }

            if (!WorldBootstrap.Runtime.State.TryGetActiveShift(actorId, workplace.Id, out ShiftState shift)) {
                Show("Start a shift before working this counter.");
                return false;
            }

            if (!WorldBootstrap.Runtime.State.TryGetNextQueuedTask(workplace.Id, action, out JobTaskState queuedTask)) {
                Show("No queued work is waiting here. Wait for a customer or delivery.");
                return false;
            }

            if (!CanCompleteTaskNow(workplace, queuedTask, out string blocker)) {
                Show(blocker);
                return false;
            }

            taskId = queuedTask.Id;
            WorldCommandResult start = WorldBootstrap.Runtime.Execute(new StartJobTaskCommand(taskId, shift.Id, actorId));
            if (!start.Succeeded) {
                Show(start.Message);
                return false;
            }

            return true;
        }

        private bool CanCompleteTaskNow(WorkplaceState workplace, JobTaskState task, out string blocker)
        {
            blocker = string.Empty;
            WorldState state = WorldBootstrap.Runtime.State;
            if (!JobTaskCatalog.TryGet(task.DefinitionId, out JobTaskDefinition definition)) {
                blocker = $"Task definition not found: {task.DefinitionId}";
                return false;
            }

            if (!state.TryGetWorkplaceInventory(workplace.Id, out WorkplaceInventoryState inventory)) {
                blocker = "Workplace inventory not found.";
                return false;
            }

            if (!string.IsNullOrEmpty(definition.InputItemId) && inventory.CountOf(definition.InputItemId) < definition.InputItemCount) {
                blocker = $"Missing inventory: needs {definition.InputItemCount} {definition.InputItemId}.";
                return false;
            }

            if (!state.TryGetMoneyAccount(workplace.BusinessAccountId, out MoneyAccountState businessAccount)) {
                blocker = "Business account not found.";
                return false;
            }

            if (businessAccount.BalanceCents < definition.BasePayCents) {
                blocker = $"Business cannot pay {EconomySystem.FormatMoney(definition.BasePayCents)} for this task.";
                return false;
            }

            blocker = string.Empty;
            return true;
        }

        private WorldCommandResult ExecuteAndShow(IWorldCommand command)
        {
            if (WorldBootstrap.Runtime == null) {
                WorldCommandResult failure = WorldCommandResult.Failure("World runtime is not loaded.");
                Show(FormatCommandMessage(failure));
                return failure;
            }

            WorldCommandResult result = WorldBootstrap.Runtime.Execute(command);
            Show(FormatCommandMessage(result));
            return result;
        }

        private static string FormatCommandMessage(WorldCommandResult result)
        {
            string message = string.IsNullOrWhiteSpace(result.Message) ? "No details returned." : result.Message;
            return result.Succeeded ? $"OK: {message}" : $"Can't do that yet: {message}";
        }

        private static string GetPlayerLocationText(WorldState state)
        {
            if (!state.TryGetCitizen(PlayerId, out CitizenState player)) {
                return "unknown player";
            }

            string placeName = state.TryGetPlace(player.CurrentPlaceId, out PlaceState place)
                ? place.DisplayName
                : player.CurrentPlaceId.ToString();
            string sceneName = state.TryGetScene(player.CurrentSceneId, out WorldSceneState scene)
                ? scene.DisplayName
                : player.CurrentSceneId.ToString();
            return $"{placeName} in {sceneName}";
        }

        private static bool TryFindActivePlayerShift(WorldState state, out ShiftState shift)
        {
            foreach (ShiftState candidate in state.ShiftsById.Values) {
                if (candidate.WorkerCitizenId == PlayerId && candidate.Status == ShiftStatus.Active) {
                    shift = candidate;
                    return true;
                }
            }

            shift = null;
            return false;
        }

        private static bool TryFindActivePlayerTask(WorldState state, out JobTaskState task)
        {
            foreach (JobTaskState candidate in state.JobTasksById.Values) {
                if (candidate.AssignedWorkerId == PlayerId &&
                    (candidate.Status == JobTaskStatus.Active || candidate.Status == JobTaskStatus.ResultSubmitted)) {
                    task = candidate;
                    return true;
                }
            }

            task = null;
            return false;
        }

        private static string GetTaskName(JobTaskState task)
        {
            return JobTaskCatalog.TryGet(task.DefinitionId, out JobTaskDefinition definition)
                ? definition.DisplayName
                : task.DefinitionId;
        }

        private static string GetWorkplaceName(WorldState state, WorldEntityId workplaceId)
        {
            if (!state.TryGetWorkplace(workplaceId, out WorkplaceState workplace)) {
                return workplaceId.ToString();
            }

            return state.TryGetPlace(workplace.PlaceId, out PlaceState place)
                ? place.DisplayName
                : workplace.Id.ToString();
        }

        private static string GetPlaceName(WorldState state, WorldEntityId placeId)
        {
            return state.TryGetPlace(placeId, out PlaceState place)
                ? place.DisplayName
                : placeId.ToString();
        }

        private static bool TryGetOpenVisitForCitizen(WorldState state, WorldEntityId citizenId, out VisitState visit)
        {
            foreach (VisitState candidate in state.VisitsById.Values) {
                if (candidate.VisitorCitizenId != citizenId) {
                    continue;
                }

                if (candidate.Status == VisitStatus.Served ||
                    candidate.Status == VisitStatus.Left ||
                    candidate.Status == VisitStatus.Failed) {
                    continue;
                }

                visit = candidate;
                return true;
            }

            visit = null;
            return false;
        }

        private static bool TryGetRelationship(
            WorldState state,
            WorldEntityId ownerCitizenId,
            WorldEntityId otherCitizenId,
            out RelationshipState relationship)
        {
            foreach (RelationshipState candidate in state.Relationships) {
                if (candidate.OwnerCitizenId == ownerCitizenId && candidate.OtherCitizenId == otherCitizenId) {
                    relationship = candidate;
                    return true;
                }
            }

            relationship = null;
            return false;
        }

        private static string GetDialogueTopic(WorldEntityId citizenId)
        {
            return citizenId == new WorldEntityId("citizen_rowan") ||
                citizenId == new WorldEntityId("citizen_old_mr_pell")
                ? "morning"
                : "greeting";
        }

        private static string FormatDenomination(CashDenomination denomination)
        {
            switch (denomination) {
                case CashDenomination.Penny:
                    return "Penny";
                case CashDenomination.Nickel:
                    return "Nickel";
                case CashDenomination.Dime:
                    return "Dime";
                case CashDenomination.Quarter:
                    return "Quarter";
                case CashDenomination.OneDollar:
                    return "$1";
                case CashDenomination.FiveDollar:
                    return "$5";
                case CashDenomination.TenDollar:
                    return "$10";
                case CashDenomination.TwentyDollar:
                    return "$20";
                default:
                    return denomination.ToString();
            }
        }

        private static string BuildTaskSummary(JobTaskState task)
        {
            string result = task.MiniGameResult == null
                ? "no mini-game result"
                : $"{task.MiniGameResult.Score}/{task.MiniGameResult.MaxScore}, {task.MiniGameResult.Mistakes} mistake(s), {task.MiniGameResult.DurationSeconds}s";
            return $"{GetTaskName(task)} finished at {task.CompletedAt.Time}. Quality {task.Quality}. Result: {result}.";
        }

        private static string BuildShiftSummary(ShiftState shift)
        {
            return $"Shift ended at {shift.EndedAt.Time}. Earned {EconomySystem.FormatMoney(shift.EarnedCents)} across {shift.CompletedTaskIds.Count} task(s).";
        }

        private static WorldEntityId CreateShiftId(WorldState state, WorldEntityId preferredId)
        {
            return state.TryGetShift(preferredId, out ShiftState _)
                ? new WorldEntityId($"{preferredId.Value}_{state.ShiftsById.Count + 1:000}")
                : preferredId;
        }

        private void DrawTaskSummary()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Last Task Result:");
            GUILayout.Label(_lastTaskSummary);
            GUILayout.EndVertical();
        }

        private void DrawShiftSummary()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Last Shift Summary:");
            GUILayout.Label(_lastShiftSummary);
            GUILayout.EndVertical();
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
