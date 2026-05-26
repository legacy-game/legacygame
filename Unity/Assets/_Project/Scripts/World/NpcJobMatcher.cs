namespace Legacy.World
{
    public static class NpcJobMatcher
    {
        public static bool TryFindWorkerForPosting(WorldState state, JobPostingState posting, out CitizenState worker)
        {
            foreach (CitizenState citizen in state.CitizensById.Values) {
                if (citizen.Id == posting.EmployerCitizenId) {
                    continue;
                }

                if (state.TryGetActiveContract(citizen.Id, posting.WorkplaceId, posting.RoleId, out EmploymentContractState _)) {
                    continue;
                }

                // For now, NPC backfill simply chooses the first non-player citizen.
                if (citizen.Id.Value != "citizen_noaharan") {
                    worker = citizen;
                    return true;
                }
            }

            worker = null;
            return false;
        }
    }
}
