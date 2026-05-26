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
        private readonly Dictionary<WorldEntityId, WorkplaceInventoryState> _workplaceInventoriesById = new();
        private readonly List<SkillState> _skillStates = new();
        private readonly List<PerformanceRecordState> _performanceRecords = new();
        private readonly Dictionary<WorldEntityId, MoneyAccountState> _moneyAccountsById = new();
        private readonly List<TransactionState> _transactions = new();
        private readonly HistoryStore _history;
        private long _nextTransactionNumber = 1;

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
        public IReadOnlyDictionary<WorldEntityId, WorkplaceInventoryState> WorkplaceInventoriesById => _workplaceInventoriesById;
        public IReadOnlyList<SkillState> SkillStates => _skillStates;
        public IReadOnlyList<PerformanceRecordState> PerformanceRecords => _performanceRecords;
        public IReadOnlyDictionary<WorldEntityId, MoneyAccountState> MoneyAccountsById => _moneyAccountsById;
        public IReadOnlyList<TransactionState> Transactions => _transactions;
        public IReadOnlyList<HistoryEvent> RecentHistory => _history.RecentHistory;
        public IHistoryArchive HistoryArchive => _history.Archive;

        public WorldState(int schemaVersion, long worldSeed, GameDateTime currentTime, WorldEntityId currentSceneId, IHistoryArchive historyArchive = null)
        {
            SchemaVersion = schemaVersion;
            WorldSeed = worldSeed;
            CurrentTime = currentTime;
            CurrentSceneId = currentSceneId;
            _history = new HistoryStore(historyArchive);
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

        public void AddWorkplaceInventory(WorkplaceInventoryState inventory)
        {
            _workplaceInventoriesById.Add(inventory.WorkplaceId, inventory);
        }

        public void AddSkillState(SkillState skill)
        {
            _skillStates.Add(skill);
        }

        public void AddPerformanceRecord(PerformanceRecordState record)
        {
            _performanceRecords.Add(record);
        }

        public void AddMoneyAccount(MoneyAccountState account)
        {
            _moneyAccountsById.Add(account.Id, account);
        }

        public void AddTransaction(TransactionState transaction)
        {
            _transactions.Add(transaction);
            IncludeTransactionId(transaction.Id);
        }

        public WorldEntityId CreateNextTransactionId()
        {
            return new WorldEntityId($"txn_{_nextTransactionNumber++:000000}");
        }

        public bool TryGetMoneyAccount(WorldEntityId id, out MoneyAccountState account)
        {
            return _moneyAccountsById.TryGetValue(id, out account);
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

        public bool TryGetWorkplaceInventory(WorldEntityId workplaceId, out WorkplaceInventoryState inventory)
        {
            return _workplaceInventoriesById.TryGetValue(workplaceId, out inventory);
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
    }
}
