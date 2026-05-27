using System.Collections.Generic;
using Legacy.History;
using Legacy.Time;

namespace Legacy.World
{
    public sealed class WorldState
    {
        public const int MaxRecentHistoryEvents = HistoryStore.MaxRecentHistoryEvents;

        private readonly WorldEntityStore _entities = new();
        private readonly SceneEntityIndex _sceneIndex = new();
        private readonly WorldSpatialIndex _spatialIndex = new();
        private readonly List<CitizenGoalState> _citizenGoals = new();
        private readonly List<RoleAssignmentState> _roleAssignments = new();
        private readonly List<CitizenRegistrationState> _citizenRegistrations = new();
        private readonly Dictionary<WorldEntityId, JobPostingState> _jobPostingsById = new();
        private readonly Dictionary<WorldEntityId, JobApplicationState> _jobApplicationsById = new();
        private readonly Dictionary<WorldEntityId, EmploymentContractState> _employmentContractsById = new();
        private readonly Dictionary<WorldEntityId, WorkplaceState> _workplacesById = new();
        private readonly Dictionary<WorldEntityId, ShiftState> _shiftsById = new();
        private readonly Dictionary<WorldEntityId, JobTaskState> _jobTasksById = new();
        private readonly Dictionary<WorldEntityId, VisitState> _visitsById = new();
        private readonly Dictionary<WorldEntityId, WorkplaceInventoryState> _workplaceInventoriesById = new();
        private readonly Dictionary<WorldEntityId, InventoryContainerState> _inventoryContainersById = new();
        private readonly List<SkillState> _skillStates = new();
        private readonly List<PerformanceRecordState> _performanceRecords = new();
        private readonly List<ShiftSummaryState> _shiftSummaries = new();
        private readonly Dictionary<WorldEntityId, MoneyAccountState> _moneyAccountsById = new();
        private readonly Dictionary<WorldEntityId, CashDrawerState> _cashDrawersById = new();
        private readonly List<TransactionState> _transactions = new();
        private readonly Dictionary<WorldEntityId, DialogueState> _dialogueStatesByCitizenId = new();
        private readonly List<RelationshipState> _relationships = new();
        private readonly Dictionary<WorldEntityId, CitizenMemoryState> _citizenMemoriesById = new();
        private readonly Dictionary<WorldEntityId, PublicRecordState> _publicRecordsByCitizenId = new();
        private readonly Dictionary<WorldEntityId, CivicReportState> _civicReportsById = new();
        private readonly List<CivicRegistryEntryState> _civicRegistryEntries = new();
        private readonly Dictionary<WorldEntityId, LeaseContractState> _leaseContractsById = new();
        private readonly Dictionary<WorldEntityId, TenantRecordState> _tenantRecordsById = new();
        private readonly List<BusinessLedgerEntryState> _businessLedgerEntries = new();
        private readonly List<FinalGameScaffoldState> _finalGameScaffolds = new();
        private readonly HistoryStore _history;
        private long _nextTransactionNumber = 1;
        private long _nextCitizenMemoryNumber = 1;
        private long _nextBusinessLedgerEntryNumber = 1;
        private MorningState _morning;

        public int SchemaVersion { get; }
        public long WorldSeed { get; }
        public GameDateTime CurrentTime { get; private set; }
        public WorldEntityId CurrentSceneId { get; private set; }

        public IReadOnlyDictionary<WorldEntityId, RegionState> RegionsById => _entities.RegionsById;
        public IReadOnlyDictionary<WorldEntityId, WorldSceneState> ScenesById => _entities.ScenesById;
        public IReadOnlyDictionary<WorldEntityId, PlaceState> PlacesById => _entities.PlacesById;
        public IReadOnlyDictionary<WorldEntityId, CitizenState> CitizensById => _entities.CitizensById;
        public IReadOnlyDictionary<WorldEntityId, PlotState> PlotsById => _entities.PlotsById;
        public IReadOnlyDictionary<WorldEntityId, BuildingState> BuildingsById => _entities.BuildingsById;
        public IReadOnlyDictionary<WorldEntityId, TerritoryChunkState> TerritoryChunksById => _entities.TerritoryChunksById;
        public IReadOnlyList<CitizenGoalState> CitizenGoals => _citizenGoals;
        public IReadOnlyList<RoleAssignmentState> RoleAssignments => _roleAssignments;
        public IReadOnlyList<CitizenRegistrationState> CitizenRegistrations => _citizenRegistrations;
        public IReadOnlyDictionary<WorldEntityId, JobPostingState> JobPostingsById => _jobPostingsById;
        public IReadOnlyDictionary<WorldEntityId, JobApplicationState> JobApplicationsById => _jobApplicationsById;
        public IReadOnlyDictionary<WorldEntityId, EmploymentContractState> EmploymentContractsById => _employmentContractsById;
        public IReadOnlyDictionary<WorldEntityId, WorkplaceState> WorkplacesById => _workplacesById;
        public IReadOnlyDictionary<WorldEntityId, ShiftState> ShiftsById => _shiftsById;
        public IReadOnlyDictionary<WorldEntityId, JobTaskState> JobTasksById => _jobTasksById;
        public IReadOnlyDictionary<WorldEntityId, VisitState> VisitsById => _visitsById;
        public IReadOnlyDictionary<WorldEntityId, WorkplaceInventoryState> WorkplaceInventoriesById => _workplaceInventoriesById;
        public IReadOnlyDictionary<WorldEntityId, InventoryContainerState> InventoryContainersById => _inventoryContainersById;
        public IReadOnlyList<SkillState> SkillStates => _skillStates;
        public IReadOnlyList<PerformanceRecordState> PerformanceRecords => _performanceRecords;
        public IReadOnlyList<ShiftSummaryState> ShiftSummaries => _shiftSummaries;
        public IReadOnlyDictionary<WorldEntityId, MoneyAccountState> MoneyAccountsById => _moneyAccountsById;
        public IReadOnlyDictionary<WorldEntityId, CashDrawerState> CashDrawersById => _cashDrawersById;
        public IReadOnlyList<TransactionState> Transactions => _transactions;
        public IReadOnlyDictionary<WorldEntityId, DialogueState> DialogueStatesByCitizenId => _dialogueStatesByCitizenId;
        public IReadOnlyList<RelationshipState> Relationships => _relationships;
        public IReadOnlyDictionary<WorldEntityId, CitizenMemoryState> CitizenMemoriesById => _citizenMemoriesById;
        public IReadOnlyDictionary<WorldEntityId, PublicRecordState> PublicRecordsByCitizenId => _publicRecordsByCitizenId;
        public IReadOnlyDictionary<WorldEntityId, CivicReportState> CivicReportsById => _civicReportsById;
        public IReadOnlyList<CivicRegistryEntryState> CivicRegistryEntries => _civicRegistryEntries;
        public IReadOnlyDictionary<WorldEntityId, LeaseContractState> LeaseContractsById => _leaseContractsById;
        public IReadOnlyDictionary<WorldEntityId, TenantRecordState> TenantRecordsById => _tenantRecordsById;
        public IReadOnlyList<BusinessLedgerEntryState> BusinessLedgerEntries => _businessLedgerEntries;
        public IReadOnlyList<FinalGameScaffoldState> FinalGameScaffolds => _finalGameScaffolds;
        public IReadOnlyList<HistoryEvent> RecentHistory => _history.RecentHistory;
        public IHistoryArchive HistoryArchive => _history.Archive;
        public MorningState Morning => _morning;

        public WorldState(int schemaVersion, long worldSeed, GameDateTime currentTime, WorldEntityId currentSceneId, IHistoryArchive historyArchive = null)
        {
            SchemaVersion = schemaVersion;
            WorldSeed = worldSeed;
            CurrentTime = currentTime;
            CurrentSceneId = currentSceneId;
            _history = new HistoryStore(historyArchive);
            _morning = new MorningState(MorningStatus.Active, currentTime, currentTime, 0, 0);
        }

        public void SetTime(GameDateTime time)
        {
            CurrentTime = time;
        }

        public void SetCurrentScene(WorldEntityId sceneId)
        {
            CurrentSceneId = sceneId;
        }

        public void AddRegion(RegionState region)
        {
            _entities.AddRegion(region);
        }

        public void AddScene(WorldSceneState scene)
        {
            _entities.AddScene(scene);
        }

        public void AddPlace(PlaceState place)
        {
            _entities.AddPlace(place);
        }

        public void AddCitizen(CitizenState citizen)
        {
            _entities.AddCitizen(citizen);
            _sceneIndex.AddCitizen(citizen);
            _spatialIndex.AddCitizen(citizen);
        }

        public void AddPlot(PlotState plot)
        {
            _entities.AddPlot(plot);
        }

        public void AddBuilding(BuildingState building)
        {
            _entities.AddBuilding(building);
            _sceneIndex.AddBuilding(building);
            _spatialIndex.AddBuilding(building);
        }

        public void AddTerritoryChunk(TerritoryChunkState territoryChunk)
        {
            _entities.AddTerritoryChunk(territoryChunk);
        }

        public void AddCitizenGoal(CitizenGoalState goal)
        {
            _citizenGoals.Add(goal);
        }

        public void AddRoleAssignment(RoleAssignmentState assignment)
        {
            _roleAssignments.Add(assignment);
        }

        public void AddCitizenRegistration(CitizenRegistrationState registration)
        {
            _citizenRegistrations.Add(registration);
        }

        public void AddJobPosting(JobPostingState posting)
        {
            _jobPostingsById.Add(posting.Id, posting);
        }

        public void AddJobApplication(JobApplicationState application)
        {
            _jobApplicationsById.Add(application.Id, application);
        }

        public void AddEmploymentContract(EmploymentContractState contract)
        {
            _employmentContractsById.Add(contract.Id, contract);
        }

        public void AddWorkplace(WorkplaceState workplace)
        {
            _workplacesById.Add(workplace.Id, workplace);
        }

        public void AddShift(ShiftState shift)
        {
            _shiftsById.Add(shift.Id, shift);
            if (_workplacesById.TryGetValue(shift.WorkplaceId, out WorkplaceState workplace)) {
                workplace.AddActiveShift(shift.Id);
            }
        }

        public void AddJobTask(JobTaskState task)
        {
            _jobTasksById.Add(task.Id, task);
            if (_workplacesById.TryGetValue(task.WorkplaceId, out WorkplaceState workplace)) {
                workplace.EnqueueTask(task.Id);
            }
        }

        public void AddVisit(VisitState visit)
        {
            _visitsById.Add(visit.Id, visit);
        }

        public void SetMorning(MorningState morning)
        {
            _morning = morning;
        }

        public void AddWorkplaceInventory(WorkplaceInventoryState inventory)
        {
            _workplaceInventoriesById.Add(inventory.WorkplaceId, inventory);
        }

        public void AddInventoryContainer(InventoryContainerState inventory)
        {
            _inventoryContainersById.Add(inventory.Id, inventory);
        }

        public void AddSkillState(SkillState skill)
        {
            _skillStates.Add(skill);
        }

        public void AddPerformanceRecord(PerformanceRecordState record)
        {
            _performanceRecords.Add(record);
        }

        public void AddShiftSummary(ShiftSummaryState summary)
        {
            _shiftSummaries.Add(summary);
        }

        public void AddMoneyAccount(MoneyAccountState account)
        {
            _moneyAccountsById.Add(account.Id, account);
        }

        public void AddCashDrawer(CashDrawerState drawer)
        {
            _cashDrawersById.Add(drawer.Id, drawer);
        }

        public void AddTransaction(TransactionState transaction)
        {
            _transactions.Add(transaction);
            IncludeTransactionId(transaction.Id);
        }

        public void AddDialogueState(DialogueState dialogue)
        {
            _dialogueStatesByCitizenId[dialogue.CitizenId] = dialogue;
        }

        public void AddRelationship(RelationshipState relationship)
        {
            _relationships.Add(relationship);
        }

        public void AddCitizenMemory(CitizenMemoryState memory)
        {
            _citizenMemoriesById.Add(memory.Id, memory);
            IncludeCitizenMemoryId(memory.Id);
        }

        public void AddPublicRecord(PublicRecordState record)
        {
            _publicRecordsByCitizenId[record.CitizenId] = record;
        }

        public void AddCivicReport(CivicReportState report)
        {
            _civicReportsById.Add(report.Id, report);
        }

        public void AddCivicRegistryEntry(CivicRegistryEntryState entry)
        {
            _civicRegistryEntries.Add(entry);
        }

        public void AddLeaseContract(LeaseContractState contract)
        {
            _leaseContractsById.Add(contract.Id, contract);
        }

        public void AddTenantRecord(TenantRecordState record)
        {
            _tenantRecordsById.Add(record.Id, record);
        }

        public void AddBusinessLedgerEntry(BusinessLedgerEntryState entry)
        {
            _businessLedgerEntries.Add(entry);
            IncludeBusinessLedgerEntryId(entry.Id);
        }

        public void AddFinalGameScaffold(FinalGameScaffoldState scaffold)
        {
            _finalGameScaffolds.Add(scaffold);
        }

        public WorldEntityId CreateNextTransactionId()
        {
            return new WorldEntityId($"txn_{_nextTransactionNumber++:000000}");
        }

        public WorldEntityId CreateNextCitizenMemoryId()
        {
            return new WorldEntityId($"mem_{_nextCitizenMemoryNumber++:000000}");
        }

        public WorldEntityId CreateNextBusinessLedgerEntryId()
        {
            return new WorldEntityId($"ledger_{_nextBusinessLedgerEntryNumber++:000000}");
        }

        public bool TryGetMoneyAccount(WorldEntityId id, out MoneyAccountState account)
        {
            return _moneyAccountsById.TryGetValue(id, out account);
        }

        public bool TryGetPublicRecord(WorldEntityId citizenId, out PublicRecordState record)
        {
            return _publicRecordsByCitizenId.TryGetValue(citizenId, out record);
        }

        public PublicRecordState GetOrCreatePublicRecord(WorldEntityId citizenId, GameDateTime createdAt)
        {
            if (!_publicRecordsByCitizenId.TryGetValue(citizenId, out PublicRecordState record)) {
                record = new PublicRecordState(citizenId, 0, 0, 0, createdAt);
                _publicRecordsByCitizenId.Add(citizenId, record);
            }

            return record;
        }

        public bool TryGetCivicReport(WorldEntityId id, out CivicReportState report)
        {
            return _civicReportsById.TryGetValue(id, out report);
        }

        public List<CivicRegistryEntryState> GetCivicRegistryEntriesForCitizen(WorldEntityId citizenId)
        {
            var entries = new List<CivicRegistryEntryState>();
            for (int i = 0; i < _civicRegistryEntries.Count; i++) {
                if (_civicRegistryEntries[i].CitizenId == citizenId) {
                    entries.Add(_civicRegistryEntries[i]);
                }
            }

            return entries;
        }

        public bool TryGetJobPosting(WorldEntityId id, out JobPostingState posting)
        {
            return _jobPostingsById.TryGetValue(id, out posting);
        }

        public bool TryGetJobApplication(WorldEntityId id, out JobApplicationState application)
        {
            return _jobApplicationsById.TryGetValue(id, out application);
        }

        public bool TryGetEmploymentContract(WorldEntityId id, out EmploymentContractState contract)
        {
            return _employmentContractsById.TryGetValue(id, out contract);
        }

        public bool TryGetWorkplace(WorldEntityId id, out WorkplaceState workplace)
        {
            return _workplacesById.TryGetValue(id, out workplace);
        }

        public bool TryGetWorkplaceByPlace(WorldEntityId placeId, out WorkplaceState workplace)
        {
            foreach (WorkplaceState candidate in _workplacesById.Values) {
                if (candidate.PlaceId == placeId) {
                    workplace = candidate;
                    return true;
                }
            }

            workplace = null;
            return false;
        }

        public bool TryGetShift(WorldEntityId id, out ShiftState shift)
        {
            return _shiftsById.TryGetValue(id, out shift);
        }

        public bool TryGetActiveShift(WorldEntityId workerId, WorldEntityId workplaceId, out ShiftState shift)
        {
            foreach (ShiftState candidate in _shiftsById.Values) {
                if (candidate.WorkerCitizenId == workerId &&
                    candidate.WorkplaceId == workplaceId &&
                    candidate.Status == ShiftStatus.Active) {
                    shift = candidate;
                    return true;
                }
            }

            shift = null;
            return false;
        }

        public bool TryGetJobTask(WorldEntityId id, out JobTaskState task)
        {
            return _jobTasksById.TryGetValue(id, out task);
        }

        public bool TryGetVisit(WorldEntityId id, out VisitState visit)
        {
            return _visitsById.TryGetValue(id, out visit);
        }

        public bool TryGetLeaseContract(WorldEntityId id, out LeaseContractState contract)
        {
            return _leaseContractsById.TryGetValue(id, out contract);
        }

        public bool TryGetTenantRecord(WorldEntityId id, out TenantRecordState record)
        {
            return _tenantRecordsById.TryGetValue(id, out record);
        }

        public bool TryGetActiveLeaseForBuilding(WorldEntityId buildingId, out LeaseContractState contract)
        {
            foreach (LeaseContractState candidate in _leaseContractsById.Values) {
                if (candidate.BuildingId == buildingId && candidate.Status == LeaseContractStatus.Active) {
                    contract = candidate;
                    return true;
                }
            }

            contract = null;
            return false;
        }

        public bool HasActiveTenantAccess(WorldEntityId citizenId, WorldEntityId buildingId)
        {
            foreach (TenantRecordState record in _tenantRecordsById.Values) {
                if (record.TenantCitizenId == citizenId &&
                    record.BuildingId == buildingId &&
                    record.Status == TenantRecordStatus.Active) {
                    return true;
                }
            }

            return false;
        }

        public bool TryGetVisitForTask(WorldEntityId taskId, out VisitState visit)
        {
            foreach (VisitState candidate in _visitsById.Values) {
                if (candidate.LinkedTaskId == taskId) {
                    visit = candidate;
                    return true;
                }
            }

            visit = null;
            return false;
        }

        public bool HasOpenVisitFor(WorldEntityId visitorId, WorldEntityId workplaceId, out VisitState visit)
        {
            foreach (VisitState candidate in _visitsById.Values) {
                if (candidate.VisitorCitizenId == visitorId &&
                    candidate.WorkplaceId == workplaceId &&
                    candidate.Status != VisitStatus.Served &&
                    candidate.Status != VisitStatus.Left &&
                    candidate.Status != VisitStatus.Failed) {
                    visit = candidate;
                    return true;
                }
            }

            visit = null;
            return false;
        }

        public bool TryGetNextQueuedTask(WorldEntityId workplaceId, WorldActionKind action, out JobTaskState task)
        {
            if (!_workplacesById.TryGetValue(workplaceId, out WorkplaceState workplace)) {
                task = null;
                return false;
            }

            foreach (WorldEntityId taskId in workplace.QueuedTaskIds) {
                if (!_jobTasksById.TryGetValue(taskId, out JobTaskState candidate) ||
                    candidate.WorkplaceId != workplaceId ||
                    candidate.Status != JobTaskStatus.Queued) {
                    continue;
                }

                if (!JobTaskCatalog.TryGet(candidate.DefinitionId, out JobTaskDefinition definition) || definition.ActionKind != action) {
                    continue;
                }

                task = candidate;
                return true;
            }

            task = null;
            return false;
        }

        public bool TryGetWorkplaceInventory(WorldEntityId workplaceId, out WorkplaceInventoryState inventory)
        {
            return _workplaceInventoriesById.TryGetValue(workplaceId, out inventory);
        }

        public bool TryGetInventoryContainer(WorldEntityId id, out InventoryContainerState inventory)
        {
            return _inventoryContainersById.TryGetValue(id, out inventory);
        }

        public bool TryGetInventoryContainerForOwner(
            WorldEntityId ownerEntityId,
            InventoryContainerKind kind,
            out InventoryContainerState inventory)
        {
            foreach (InventoryContainerState candidate in _inventoryContainersById.Values) {
                if (candidate.OwnerEntityId == ownerEntityId && candidate.Kind == kind) {
                    inventory = candidate;
                    return true;
                }
            }

            inventory = null;
            return false;
        }

        public bool TryGetActiveContract(WorldEntityId workerId, WorldEntityId workplaceId, string roleId, out EmploymentContractState contract)
        {
            foreach (EmploymentContractState candidate in _employmentContractsById.Values) {
                if (candidate.WorkerCitizenId == workerId &&
                    candidate.WorkplaceId == workplaceId &&
                    candidate.RoleId == roleId &&
                    candidate.Status == EmploymentContractStatus.Active) {
                    contract = candidate;
                    return true;
                }
            }

            contract = null;
            return false;
        }

        public SkillState GetOrCreateSkill(WorldEntityId citizenId, SkillKind skill)
        {
            for (int i = 0; i < _skillStates.Count; i++) {
                if (_skillStates[i].CitizenId == citizenId && _skillStates[i].Skill == skill) {
                    return _skillStates[i];
                }
            }

            var state = new SkillState(citizenId, skill, 0);
            _skillStates.Add(state);
            return state;
        }

        public DialogueState GetOrCreateDialogueState(WorldEntityId citizenId)
        {
            if (_dialogueStatesByCitizenId.TryGetValue(citizenId, out DialogueState dialogue)) {
                return dialogue;
            }

            dialogue = new DialogueState(citizenId);
            _dialogueStatesByCitizenId.Add(citizenId, dialogue);
            return dialogue;
        }

        public RelationshipState GetOrCreateRelationship(WorldEntityId ownerCitizenId, WorldEntityId otherCitizenId)
        {
            for (int i = 0; i < _relationships.Count; i++) {
                RelationshipState relationship = _relationships[i];
                if (relationship.OwnerCitizenId == ownerCitizenId && relationship.OtherCitizenId == otherCitizenId) {
                    return relationship;
                }
            }

            var state = new RelationshipState(ownerCitizenId, otherCitizenId);
            _relationships.Add(state);
            return state;
        }

        public bool TryGetMoneyAccountForOwner(WorldEntityId ownerEntityId, out MoneyAccountState account)
        {
            foreach (MoneyAccountState candidate in _moneyAccountsById.Values) {
                if (candidate.OwnerEntityId == ownerEntityId) {
                    account = candidate;
                    return true;
                }
            }

            account = null;
            return false;
        }

        public bool TryGetCashDrawer(WorldEntityId id, out CashDrawerState drawer)
        {
            return _cashDrawersById.TryGetValue(id, out drawer);
        }

        public bool TryGetCashDrawerForOwner(WorldEntityId ownerEntityId, CashContainerKind kind, out CashDrawerState drawer)
        {
            foreach (CashDrawerState candidate in _cashDrawersById.Values) {
                if (candidate.OwnerEntityId == ownerEntityId && candidate.Kind == kind) {
                    drawer = candidate;
                    return true;
                }
            }

            drawer = null;
            return false;
        }

        public bool TryGetCitizenRegistration(WorldEntityId citizenId, out CitizenRegistrationState registration)
        {
            for (int i = 0; i < _citizenRegistrations.Count; i++) {
                if (_citizenRegistrations[i].CitizenId == citizenId) {
                    registration = _citizenRegistrations[i];
                    return true;
                }
            }

            registration = null;
            return false;
        }

        public bool TryGetActiveCitizenGoal(WorldEntityId citizenId, GameDateTime now, out CitizenGoalState goal)
        {
            goal = null;
            for (int i = 0; i < _citizenGoals.Count; i++) {
                CitizenGoalState candidate = _citizenGoals[i];
                if (candidate.CitizenId != citizenId || candidate.Status != CitizenGoalStatus.Active) {
                    continue;
                }

                if (IsAfter(now, candidate.ExpiresAt)) {
                    candidate.Expire();
                    continue;
                }

                if (goal == null || candidate.Urgency > goal.Urgency) {
                    goal = candidate;
                }
            }

            return goal != null;
        }

        public void MoveCitizen(WorldEntityId citizenId, WorldEntityId regionId, WorldEntityId sceneId, WorldEntityId placeId, GridCoord coord, CitizenActivityState activity)
        {
            if (!_entities.TryGetCitizen(citizenId, out CitizenState citizen)) {
                return;
            }

            _sceneIndex.MoveCitizen(citizen, sceneId);
            _spatialIndex.MoveCitizen(citizen, sceneId, coord);
            citizen.MoveTo(regionId, sceneId, placeId, coord, activity);
        }

        public IReadOnlyList<WorldEntityId> GetBuildingIdsInScene(WorldEntityId sceneId)
        {
            return _sceneIndex.GetBuildingIdsInScene(sceneId);
        }

        public IReadOnlyList<WorldEntityId> GetCitizenIdsInScene(WorldEntityId sceneId)
        {
            return _sceneIndex.GetCitizenIdsInScene(sceneId);
        }

        public IReadOnlyList<WorldEntityId> GetBuildingIdsInSceneChunk(WorldEntityId sceneId, GridChunkCoord chunk)
        {
            return _spatialIndex.GetBuildingIdsInSceneChunk(sceneId, chunk);
        }

        public IReadOnlyList<WorldEntityId> GetCitizenIdsInSceneChunk(WorldEntityId sceneId, GridChunkCoord chunk)
        {
            return _spatialIndex.GetCitizenIdsInSceneChunk(sceneId, chunk);
        }

        public List<WorldEntityId> GetCitizenIdsNear(WorldEntityId sceneId, GridCoord center, int chunkRadius = 1)
        {
            return _spatialIndex.GetCitizenIdsNear(sceneId, center, chunkRadius);
        }

        public void AddHistoryEvent(HistoryEvent historyEvent)
        {
            _history.Add(historyEvent);
        }

        public IReadOnlyList<HistoryEvent> GetHistoryForActor(WorldEntityId actorId)
        {
            return _history.GetForActor(actorId);
        }

        public IReadOnlyList<HistoryEvent> GetHistoryForPlace(WorldEntityId placeId)
        {
            return _history.GetForPlace(placeId);
        }

        public IReadOnlyList<HistoryEvent> GetHistoryByKind(HistoryEventKind kind)
        {
            return _history.GetByKind(kind);
        }

        public List<CitizenMemoryState> GetMemoriesForCitizen(WorldEntityId citizenId)
        {
            var memories = new List<CitizenMemoryState>();
            foreach (CitizenMemoryState memory in _citizenMemoriesById.Values) {
                if (memory.CitizenId == citizenId) {
                    memories.Add(memory);
                }
            }

            return memories;
        }

        public List<HistoryEvent> GetHistoryBetween(GameDateTime startInclusive, GameDateTime endInclusive)
        {
            return _history.GetBetween(startInclusive, endInclusive);
        }

        public bool TryGetCitizen(WorldEntityId id, out CitizenState citizen)
        {
            return _entities.TryGetCitizen(id, out citizen);
        }

        public bool TryGetBuilding(WorldEntityId id, out BuildingState building)
        {
            return _entities.TryGetBuilding(id, out building);
        }

        public bool TryGetScene(WorldEntityId id, out WorldSceneState scene)
        {
            return _entities.TryGetScene(id, out scene);
        }

        public bool TryGetPlace(WorldEntityId id, out PlaceState place)
        {
            return _entities.TryGetPlace(id, out place);
        }

        public bool TryGetPlot(WorldEntityId id, out PlotState plot)
        {
            return _entities.TryGetPlot(id, out plot);
        }

        public bool TryGetTerritoryChunk(WorldEntityId id, out TerritoryChunkState territoryChunk)
        {
            return _entities.TryGetTerritoryChunk(id, out territoryChunk);
        }

        private static bool IsAfter(GameDateTime left, GameDateTime right)
        {
            long leftMinute = ToAbsoluteMinute(left);
            long rightMinute = ToAbsoluteMinute(right);
            return leftMinute > rightMinute;
        }

        private static long ToAbsoluteMinute(GameDateTime dateTime)
        {
            return (long)dateTime.Date.AbsoluteDay * 1440L + dateTime.Time.TotalMinutes;
        }

        private void IncludeTransactionId(WorldEntityId id)
        {
            const string prefix = "txn_";
            string value = id.Value;
            if (string.IsNullOrEmpty(value) || !value.StartsWith(prefix, System.StringComparison.Ordinal)) {
                return;
            }

            string numberText = value.Substring(prefix.Length);
            if (long.TryParse(numberText, out long number) && number >= _nextTransactionNumber) {
                _nextTransactionNumber = number + 1;
            }
        }

        private void IncludeCitizenMemoryId(WorldEntityId id)
        {
            const string prefix = "mem_";
            string value = id.Value;
            if (string.IsNullOrEmpty(value) || !value.StartsWith(prefix, System.StringComparison.Ordinal)) {
                return;
            }

            string numberText = value.Substring(prefix.Length);
            if (long.TryParse(numberText, out long number) && number >= _nextCitizenMemoryNumber) {
                _nextCitizenMemoryNumber = number + 1;
            }
        }

        private void IncludeBusinessLedgerEntryId(WorldEntityId id)
        {
            const string prefix = "ledger_";
            string value = id.Value;
            if (string.IsNullOrEmpty(value) || !value.StartsWith(prefix, System.StringComparison.Ordinal)) {
                return;
            }

            string numberText = value.Substring(prefix.Length);
            if (long.TryParse(numberText, out long number) && number >= _nextBusinessLedgerEntryNumber) {
                _nextBusinessLedgerEntryNumber = number + 1;
            }
        }
    }
}
