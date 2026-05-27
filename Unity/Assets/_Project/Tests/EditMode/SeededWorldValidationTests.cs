using System.Collections.Generic;
using Legacy.World;
using NUnit.Framework;

namespace Legacy.Tests.EditMode
{
    public sealed class SeededWorldValidationTests
    {
        [Test]
        public void VeyneSeedWorld_ReferencesOnlyKnownWorldIds()
        {
            WorldState state = WorldFactory.CreateVeyneSeedWorld();
            var errors = new List<string>();

            RequireId(state.ScenesById.ContainsKey(state.CurrentSceneId), "world", state.CurrentSceneId, "current scene", errors);

            foreach (WorldSceneState scene in state.ScenesById.Values) {
                RequireId(state.RegionsById.ContainsKey(scene.RegionId), scene.Id, scene.RegionId, "scene region", errors);
            }

            foreach (PlaceState place in state.PlacesById.Values) {
                RequireId(state.RegionsById.ContainsKey(place.RegionId), place.Id, place.RegionId, "place region", errors);
                RequireId(state.ScenesById.ContainsKey(place.SceneId), place.Id, place.SceneId, "place scene", errors);
            }

            foreach (CitizenState citizen in state.CitizensById.Values) {
                RequireId(state.BuildingsById.ContainsKey(citizen.HomeBuildingId), citizen.Id, citizen.HomeBuildingId, "citizen home building", errors);
                RequireId(state.BuildingsById.ContainsKey(citizen.WorkplaceBuildingId), citizen.Id, citizen.WorkplaceBuildingId, "citizen workplace building", errors);
                RequireId(state.RegionsById.ContainsKey(citizen.CurrentRegionId), citizen.Id, citizen.CurrentRegionId, "citizen current region", errors);
                RequireId(state.ScenesById.ContainsKey(citizen.CurrentSceneId), citizen.Id, citizen.CurrentSceneId, "citizen current scene", errors);
                RequireId(state.PlacesById.ContainsKey(citizen.CurrentPlaceId), citizen.Id, citizen.CurrentPlaceId, "citizen current place", errors);
            }

            foreach (PlotState plot in state.PlotsById.Values) {
                RequireId(state.RegionsById.ContainsKey(plot.RegionId), plot.Id, plot.RegionId, "plot region", errors);
                RequireId(state.CitizensById.ContainsKey(plot.OwnerCitizenId), plot.Id, plot.OwnerCitizenId, "plot owner citizen", errors);
            }

            foreach (BuildingState building in state.BuildingsById.Values) {
                RequireId(state.RegionsById.ContainsKey(building.RegionId), building.Id, building.RegionId, "building region", errors);
                RequireId(state.PlotsById.ContainsKey(building.PlotId), building.Id, building.PlotId, "building plot", errors);
                RequireId(state.ScenesById.ContainsKey(building.ExteriorSceneId), building.Id, building.ExteriorSceneId, "building exterior scene", errors);
                RequireId(state.ScenesById.ContainsKey(building.InteriorSceneId), building.Id, building.InteriorSceneId, "building interior scene", errors);
                RequireId(state.PlacesById.ContainsKey(building.InteriorPlaceId), building.Id, building.InteriorPlaceId, "building interior place", errors);
                RequireId(state.CitizensById.ContainsKey(building.OwnerCitizenId), building.Id, building.OwnerCitizenId, "building owner citizen", errors);
            }

            foreach (MoneyAccountState account in state.MoneyAccountsById.Values) {
                bool ownerExists = state.CitizensById.ContainsKey(account.OwnerEntityId) || state.BuildingsById.ContainsKey(account.OwnerEntityId);
                RequireId(ownerExists, account.Id, account.OwnerEntityId, "money account owner", errors);
            }

            foreach (WorkplaceState workplace in state.WorkplacesById.Values) {
                RequireId(state.BuildingsById.ContainsKey(workplace.BuildingId), workplace.Id, workplace.BuildingId, "workplace building", errors);
                RequireId(state.PlacesById.ContainsKey(workplace.PlaceId), workplace.Id, workplace.PlaceId, "workplace place", errors);
                RequireId(state.CitizensById.ContainsKey(workplace.OwnerCitizenId), workplace.Id, workplace.OwnerCitizenId, "workplace owner citizen", errors);
                RequireId(state.MoneyAccountsById.ContainsKey(workplace.BusinessAccountId), workplace.Id, workplace.BusinessAccountId, "workplace business account", errors);
                RequireId(state.WorkplaceInventoriesById.ContainsKey(workplace.Id), workplace.Id, workplace.Id, "workplace inventory", errors);
            }

            foreach (WorkplaceInventoryState inventory in state.WorkplaceInventoriesById.Values) {
                RequireId(state.WorkplacesById.ContainsKey(inventory.WorkplaceId), inventory.WorkplaceId, inventory.WorkplaceId, "inventory workplace", errors);
            }

            foreach (RoleAssignmentState assignment in state.RoleAssignments) {
                RequireId(state.CitizensById.ContainsKey(assignment.CitizenId), assignment.Id, assignment.CitizenId, "role assignment citizen", errors);
                RequireId(RoleCatalog.TryGet(assignment.RoleId, out _), assignment.Id, assignment.RoleId, "role assignment role", errors);
                RequireId(state.PlacesById.ContainsKey(assignment.WorkplacePlaceId), assignment.Id, assignment.WorkplacePlaceId, "role assignment workplace place", errors);
            }

            foreach (JobPostingState posting in state.JobPostingsById.Values) {
                RequireId(JobCatalog.TryGet(posting.JobDefinitionId, out _), posting.Id, posting.JobDefinitionId, "job posting definition", errors);
                RequireId(state.CitizensById.ContainsKey(posting.EmployerCitizenId), posting.Id, posting.EmployerCitizenId, "job posting employer", errors);
                RequireId(state.WorkplacesById.ContainsKey(posting.WorkplaceId), posting.Id, posting.WorkplaceId, "job posting workplace", errors);
                RequireId(RoleCatalog.TryGet(posting.RoleId, out _), posting.Id, posting.RoleId, "job posting role", errors);
            }

            foreach (JobTaskState task in state.JobTasksById.Values) {
                RequireId(JobTaskCatalog.TryGet(task.DefinitionId, out _), task.Id, task.DefinitionId, "job task definition", errors);
                RequireId(state.WorkplacesById.ContainsKey(task.WorkplaceId), task.Id, task.WorkplaceId, "job task workplace", errors);
                RequireOptionalId(!IsSet(task.AssignedWorkerId) || state.CitizensById.ContainsKey(task.AssignedWorkerId), task.Id, task.AssignedWorkerId, "job task assigned worker", errors);
                RequireOptionalId(!IsSet(task.ShiftId) || state.ShiftsById.ContainsKey(task.ShiftId), task.Id, task.ShiftId, "job task shift", errors);
                RequireOptionalId(!IsSet(task.TargetEntityId) || AnyWorldEntityExists(state, task.TargetEntityId), task.Id, task.TargetEntityId, "job task target entity", errors);
            }

            foreach (VisitState visit in state.VisitsById.Values) {
                RequireId(state.CitizensById.ContainsKey(visit.VisitorCitizenId), visit.Id, visit.VisitorCitizenId, "visit visitor", errors);
                RequireId(state.WorkplacesById.ContainsKey(visit.WorkplaceId), visit.Id, visit.WorkplaceId, "visit workplace", errors);
                RequireId(state.PlacesById.ContainsKey(visit.PlaceId), visit.Id, visit.PlaceId, "visit place", errors);
                RequireOptionalId(!IsSet(visit.LinkedTaskId) || state.JobTasksById.ContainsKey(visit.LinkedTaskId), visit.Id, visit.LinkedTaskId, "visit linked task", errors);
            }

            Assert.That(errors, Is.Empty, string.Join("\n", errors));
        }

        private static void RequireId(bool condition, WorldEntityId ownerId, WorldEntityId referencedId, string relationship, List<string> errors)
        {
            if (!condition) {
                errors.Add($"{ownerId} references missing {relationship}: {referencedId}");
            }
        }

        private static void RequireId(bool condition, WorldEntityId ownerId, string referencedId, string relationship, List<string> errors)
        {
            if (!condition) {
                errors.Add($"{ownerId} references missing {relationship}: {referencedId}");
            }
        }

        private static void RequireId(bool condition, string ownerId, WorldEntityId referencedId, string relationship, List<string> errors)
        {
            if (!condition) {
                errors.Add($"{ownerId} references missing {relationship}: {referencedId}");
            }
        }

        private static void RequireOptionalId(bool condition, WorldEntityId ownerId, WorldEntityId referencedId, string relationship, List<string> errors)
        {
            if (!condition) {
                errors.Add($"{ownerId} references missing {relationship}: {referencedId}");
            }
        }

        private static bool AnyWorldEntityExists(WorldState state, WorldEntityId id)
        {
            return state.RegionsById.ContainsKey(id) ||
                state.ScenesById.ContainsKey(id) ||
                state.PlacesById.ContainsKey(id) ||
                state.CitizensById.ContainsKey(id) ||
                state.PlotsById.ContainsKey(id) ||
                state.BuildingsById.ContainsKey(id) ||
                state.TerritoryChunksById.ContainsKey(id) ||
                state.WorkplacesById.ContainsKey(id) ||
                state.MoneyAccountsById.ContainsKey(id) ||
                state.JobPostingsById.ContainsKey(id) ||
                state.JobTasksById.ContainsKey(id);
        }

        private static bool IsSet(WorldEntityId id)
        {
            return !string.IsNullOrWhiteSpace(id.Value);
        }
    }
}
