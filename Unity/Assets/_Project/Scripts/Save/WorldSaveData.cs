using System;
using System.Collections.Generic;

namespace Legacy.Save
{
    [Serializable]
    public sealed class WorldSaveData
    {
        public int schemaVersion;
        public long worldSeed;
        public DateTimeSaveData currentTime;
        public string currentSceneId;
        public List<RegionSaveData> regions = new();
        public List<WorldSceneSaveData> scenes = new();
        public List<PlaceSaveData> places = new();
        public List<TerritoryChunkSaveData> territoryChunks = new();
        public List<CitizenSaveData> citizens = new();
        public List<CitizenRegistrationSaveData> citizenRegistrations = new();
        public List<CitizenGoalSaveData> citizenGoals = new();
        public List<RoleAssignmentSaveData> roleAssignments = new();
        public List<JobPostingSaveData> jobPostings = new();
        public List<JobApplicationSaveData> jobApplications = new();
        public List<EmploymentContractSaveData> employmentContracts = new();
        public List<WorkplaceSaveData> workplaces = new();
        public List<ShiftSaveData> shifts = new();
        public List<JobTaskSaveData> jobTasks = new();
        public List<WorkplaceInventorySaveData> workplaceInventories = new();
        public List<SkillSaveData> skills = new();
        public List<PerformanceRecordSaveData> performanceRecords = new();
        public List<MoneyAccountSaveData> moneyAccounts = new();
        public List<TransactionSaveData> transactions = new();
        public List<PlotSaveData> plots = new();
        public List<BuildingSaveData> buildings = new();
        public List<HistoryEventSaveData> recentHistory = new();
        public List<HistoryEventSaveData> archivedHistory = new();
    }
}
