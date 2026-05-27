using System;
using Legacy.History;
using Legacy.Time;
using Legacy.World;

namespace Legacy.Save
{
    public static class WorldSaveMapper
    {
        public static WorldSaveData ToSaveData(WorldState state)
        {
            var data = new WorldSaveData {
                schemaVersion = state.SchemaVersion,
                worldSeed = state.WorldSeed,
                currentTime = ToSaveData(state.CurrentTime),
                currentSceneId = state.CurrentSceneId.Value,
                morning = ToSaveData(state.Morning)
            };

            foreach (RegionState region in state.RegionsById.Values) {
                data.regions.Add(new RegionSaveData {
                    id = region.Id.Value,
                    displayName = region.DisplayName,
                    bounds = ToSaveData(region.Bounds)
                });
            }

            foreach (WorldSceneState scene in state.ScenesById.Values) {
                data.scenes.Add(new WorldSceneSaveData {
                    id = scene.Id.Value,
                    regionId = scene.RegionId.Value,
                    displayName = scene.DisplayName
                });
            }

            foreach (PlaceState place in state.PlacesById.Values) {
                data.places.Add(new PlaceSaveData {
                    id = place.Id.Value,
                    regionId = place.RegionId.Value,
                    sceneId = place.SceneId.Value,
                    displayName = place.DisplayName,
                    kind = place.Kind.ToString()
                });
            }

            foreach (TerritoryChunkState territory in state.TerritoryChunksById.Values) {
                data.territoryChunks.Add(ToSaveData(territory));
            }

            foreach (CitizenState citizen in state.CitizensById.Values) {
                data.citizens.Add(new CitizenSaveData {
                    id = citizen.Id.Value,
                    displayName = citizen.DisplayName,
                    homeBuildingId = citizen.HomeBuildingId.Value,
                    workplaceBuildingId = citizen.WorkplaceBuildingId.Value,
                    currentRegionId = citizen.CurrentRegionId.Value,
                    currentSceneId = citizen.CurrentSceneId.Value,
                    currentPlaceId = citizen.CurrentPlaceId.Value,
                    currentCoord = ToSaveData(citizen.CurrentCoord),
                    activity = citizen.Activity.ToString(),
                    scheduleStage = citizen.ScheduleStage,
                    routineId = citizen.Routine.RoutineId,
                    activeRoutineStepId = citizen.Routine.ActiveStepId,
                    currentIntent = citizen.Routine.CurrentIntent,
                    lastRoutineAbsoluteMinute = citizen.Routine.LastProcessedAbsoluteMinute
                });
            }

            foreach (CitizenRegistrationState registration in state.CitizenRegistrations) {
                data.citizenRegistrations.Add(ToSaveData(registration));
            }

            foreach (CitizenGoalState goal in state.CitizenGoals) {
                data.citizenGoals.Add(ToSaveData(goal));
            }

            foreach (RoleAssignmentState assignment in state.RoleAssignments) {
                data.roleAssignments.Add(ToSaveData(assignment));
            }

            foreach (JobPostingState posting in state.JobPostingsById.Values) {
                data.jobPostings.Add(ToSaveData(posting));
            }

            foreach (JobApplicationState application in state.JobApplicationsById.Values) {
                data.jobApplications.Add(ToSaveData(application));
            }

            foreach (EmploymentContractState contract in state.EmploymentContractsById.Values) {
                data.employmentContracts.Add(ToSaveData(contract));
            }

            foreach (WorkplaceState workplace in state.WorkplacesById.Values) {
                data.workplaces.Add(ToSaveData(workplace));
            }

            foreach (ShiftState shift in state.ShiftsById.Values) {
                data.shifts.Add(ToSaveData(shift));
            }

            foreach (ShiftSummaryState summary in state.ShiftSummaries) {
                data.shiftSummaries.Add(ToSaveData(summary));
            }

            foreach (JobTaskState task in state.JobTasksById.Values) {
                data.jobTasks.Add(ToSaveData(task));
            }

            foreach (VisitState visit in state.VisitsById.Values) {
                data.visits.Add(ToSaveData(visit));
            }

            foreach (WorkplaceInventoryState inventory in state.WorkplaceInventoriesById.Values) {
                data.workplaceInventories.Add(ToSaveData(inventory));
            }

            foreach (InventoryContainerState inventory in state.InventoryContainersById.Values) {
                data.inventoryContainers.Add(ToSaveData(inventory));
            }

            foreach (SkillState skill in state.SkillStates) {
                data.skills.Add(ToSaveData(skill));
            }

            foreach (PerformanceRecordState record in state.PerformanceRecords) {
                data.performanceRecords.Add(ToSaveData(record));
            }

            foreach (MoneyAccountState account in state.MoneyAccountsById.Values) {
                data.moneyAccounts.Add(ToSaveData(account));
            }

            foreach (CashDrawerState drawer in state.CashDrawersById.Values) {
                data.cashDrawers.Add(ToSaveData(drawer));
            }

            foreach (TransactionState transaction in state.Transactions) {
                data.transactions.Add(ToSaveData(transaction));
            }

            foreach (DialogueState dialogue in state.DialogueStatesByCitizenId.Values) {
                data.dialogues.Add(ToSaveData(dialogue));
            }

            foreach (RelationshipState relationship in state.Relationships) {
                data.relationships.Add(ToSaveData(relationship));
            }

            foreach (CitizenMemoryState memory in state.CitizenMemoriesById.Values) {
                data.citizenMemories.Add(ToSaveData(memory));
            }

            foreach (PublicRecordState record in state.PublicRecordsByCitizenId.Values) {
                data.publicRecords.Add(ToSaveData(record));
            }

            foreach (CivicReportState report in state.CivicReportsById.Values) {
                data.civicReports.Add(ToSaveData(report));
            }

            foreach (CivicRegistryEntryState entry in state.CivicRegistryEntries) {
                data.civicRegistryEntries.Add(ToSaveData(entry));
            }

            foreach (LeaseContractState contract in state.LeaseContractsById.Values) {
                data.leaseContracts.Add(ToSaveData(contract));
            }

            foreach (TenantRecordState record in state.TenantRecordsById.Values) {
                data.tenantRecords.Add(ToSaveData(record));
            }

            foreach (BusinessLedgerEntryState entry in state.BusinessLedgerEntries) {
                data.businessLedgerEntries.Add(ToSaveData(entry));
            }

            foreach (PlotState plot in state.PlotsById.Values) {
                data.plots.Add(new PlotSaveData {
                    id = plot.Id.Value,
                    regionId = plot.RegionId.Value,
                    displayName = plot.DisplayName,
                    bounds = ToSaveData(plot.Bounds),
                    ownerCitizenId = plot.OwnerCitizenId.Value,
                    accessRule = plot.AccessRule.ToString()
                });
            }

            foreach (BuildingState building in state.BuildingsById.Values) {
                data.buildings.Add(new BuildingSaveData {
                    id = building.Id.Value,
                    regionId = building.RegionId.Value,
                    plotId = building.PlotId.Value,
                    exteriorSceneId = building.ExteriorSceneId.Value,
                    interiorSceneId = building.InteriorSceneId.Value,
                    interiorPlaceId = building.InteriorPlaceId.Value,
                    displayName = building.DisplayName,
                    ownerCitizenId = building.OwnerCitizenId.Value,
                    kind = building.Kind.ToString(),
                    entranceCoord = ToSaveData(building.EntranceCoord)
                });
            }

            foreach (HistoryEvent historyEvent in state.RecentHistory) {
                data.recentHistory.Add(ToSaveData(historyEvent));
            }

            foreach (HistoryEvent historyEvent in state.HistoryArchive.Events) {
                data.archivedHistory.Add(ToSaveData(historyEvent));
            }

            return data;
        }

        public static WorldState ToRuntime(WorldSaveData data)
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }

            var archive = new InMemoryHistoryArchive();
            if (data.archivedHistory != null) {
                foreach (HistoryEventSaveData history in data.archivedHistory) {
                    archive.Add(ToRuntime(history));
                }
            }

            var state = new WorldState(data.schemaVersion, data.worldSeed, ToRuntime(data.currentTime), Id(data.currentSceneId), archive);
            if (data.morning != null) {
                state.SetMorning(ToRuntime(data.morning));
            }

            foreach (RegionSaveData region in data.regions) {
                state.AddRegion(new RegionState(Id(region.id), region.displayName, ToRuntime(region.bounds)));
            }

            foreach (WorldSceneSaveData scene in data.scenes) {
                state.AddScene(new WorldSceneState(Id(scene.id), Id(scene.regionId), scene.displayName));
            }

            foreach (PlaceSaveData place in data.places) {
                state.AddPlace(new PlaceState(
                    Id(place.id),
                    Id(place.regionId),
                    Id(place.sceneId),
                    place.displayName,
                    Enum.Parse<PlaceKind>(place.kind)));
            }

            if (data.territoryChunks != null) {
                foreach (TerritoryChunkSaveData territory in data.territoryChunks) {
                    state.AddTerritoryChunk(ToRuntime(territory));
                }
            }

            foreach (CitizenSaveData citizen in data.citizens) {
                state.AddCitizen(new CitizenState(
                    Id(citizen.id),
                    citizen.displayName,
                    Id(citizen.homeBuildingId),
                    Id(citizen.workplaceBuildingId),
                    Id(citizen.currentRegionId),
                    Id(citizen.currentSceneId),
                    Id(citizen.currentPlaceId),
                    ToRuntime(citizen.currentCoord),
                    Enum.Parse<CitizenActivityState>(citizen.activity),
                    citizen.scheduleStage,
                    new CitizenRoutineState(
                        citizen.routineId ?? string.Empty,
                        citizen.activeRoutineStepId ?? string.Empty,
                        citizen.currentIntent ?? string.Empty,
                        citizen.lastRoutineAbsoluteMinute == 0 && string.IsNullOrEmpty(citizen.activeRoutineStepId)
                            ? -1
                            : citizen.lastRoutineAbsoluteMinute)));
            }

            if (data.citizenRegistrations != null) {
                foreach (CitizenRegistrationSaveData registration in data.citizenRegistrations) {
                    state.AddCitizenRegistration(ToRuntime(registration));
                }
            }

            if (data.citizenGoals != null) {
                foreach (CitizenGoalSaveData goal in data.citizenGoals) {
                    state.AddCitizenGoal(ToRuntime(goal));
                }
            }

            if (data.roleAssignments != null) {
                foreach (RoleAssignmentSaveData assignment in data.roleAssignments) {
                    state.AddRoleAssignment(ToRuntime(assignment));
                }
            }

            if (data.jobPostings != null) {
                foreach (JobPostingSaveData posting in data.jobPostings) {
                    state.AddJobPosting(ToRuntime(posting));
                }
            }

            if (data.jobApplications != null) {
                foreach (JobApplicationSaveData application in data.jobApplications) {
                    state.AddJobApplication(ToRuntime(application));
                }
            }

            if (data.employmentContracts != null) {
                foreach (EmploymentContractSaveData contract in data.employmentContracts) {
                    state.AddEmploymentContract(ToRuntime(contract));
                }
            }

            if (data.workplaces != null) {
                foreach (WorkplaceSaveData workplace in data.workplaces) {
                    state.AddWorkplace(ToRuntime(workplace));
                }
            }

            if (data.shifts != null) {
                foreach (ShiftSaveData shift in data.shifts) {
                    state.AddShift(ToRuntime(shift));
                }
            }

            if (data.shiftSummaries != null) {
                foreach (ShiftSummarySaveData summary in data.shiftSummaries) {
                    state.AddShiftSummary(ToRuntime(summary));
                }
            }

            if (data.jobTasks != null) {
                foreach (JobTaskSaveData task in data.jobTasks) {
                    state.AddJobTask(ToRuntime(task));
                }
            }

            if (data.visits != null) {
                foreach (VisitSaveData visit in data.visits) {
                    state.AddVisit(ToRuntime(visit));
                }
            }

            if (data.workplaceInventories != null) {
                foreach (WorkplaceInventorySaveData inventory in data.workplaceInventories) {
                    state.AddWorkplaceInventory(ToRuntime(inventory));
                }
            }

            if (data.inventoryContainers != null) {
                foreach (InventoryContainerSaveData inventory in data.inventoryContainers) {
                    state.AddInventoryContainer(ToRuntime(inventory));
                }
            }

            if (data.skills != null) {
                foreach (SkillSaveData skill in data.skills) {
                    state.AddSkillState(ToRuntime(skill));
                }
            }

            if (data.performanceRecords != null) {
                foreach (PerformanceRecordSaveData record in data.performanceRecords) {
                    state.AddPerformanceRecord(ToRuntime(record));
                }
            }

            if (data.moneyAccounts != null) {
                foreach (MoneyAccountSaveData account in data.moneyAccounts) {
                    state.AddMoneyAccount(ToRuntime(account));
                }
            }

            if (data.cashDrawers != null) {
                foreach (CashDrawerSaveData drawer in data.cashDrawers) {
                    state.AddCashDrawer(ToRuntime(drawer));
                }
            }

            if (data.transactions != null) {
                foreach (TransactionSaveData transaction in data.transactions) {
                    state.AddTransaction(ToRuntime(transaction));
                }
            }

            if (data.dialogues != null) {
                foreach (DialogueSaveData dialogue in data.dialogues) {
                    state.AddDialogueState(ToRuntime(dialogue));
                }
            }

            if (data.relationships != null) {
                foreach (RelationshipSaveData relationship in data.relationships) {
                    state.AddRelationship(ToRuntime(relationship));
                }
            }

            if (data.citizenMemories != null) {
                foreach (CitizenMemorySaveData memory in data.citizenMemories) {
                    state.AddCitizenMemory(ToRuntime(memory));
                }
            }

            if (data.publicRecords != null) {
                foreach (PublicRecordSaveData record in data.publicRecords) {
                    state.AddPublicRecord(ToRuntime(record));
                }
            }

            if (data.civicReports != null) {
                foreach (CivicReportSaveData report in data.civicReports) {
                    state.AddCivicReport(ToRuntime(report));
                }
            }

            if (data.civicRegistryEntries != null) {
                foreach (CivicRegistryEntrySaveData entry in data.civicRegistryEntries) {
                    state.AddCivicRegistryEntry(ToRuntime(entry));
                }
            }

            if (data.leaseContracts != null) {
                foreach (LeaseContractSaveData contract in data.leaseContracts) {
                    state.AddLeaseContract(ToRuntime(contract));
                }
            }

            if (data.tenantRecords != null) {
                foreach (TenantRecordSaveData record in data.tenantRecords) {
                    state.AddTenantRecord(ToRuntime(record));
                }
            }

            if (data.businessLedgerEntries != null) {
                foreach (BusinessLedgerEntrySaveData entry in data.businessLedgerEntries) {
                    state.AddBusinessLedgerEntry(ToRuntime(entry));
                }
            }

            foreach (PlotSaveData plot in data.plots) {
                state.AddPlot(new PlotState(
                    Id(plot.id),
                    Id(plot.regionId),
                    plot.displayName,
                    ToRuntime(plot.bounds),
                    Id(plot.ownerCitizenId),
                    Enum.Parse<AccessRule>(plot.accessRule)));
            }

            foreach (BuildingSaveData building in data.buildings) {
                state.AddBuilding(new BuildingState(
                    Id(building.id),
                    Id(building.regionId),
                    Id(building.plotId),
                    Id(building.exteriorSceneId),
                    Id(building.interiorSceneId),
                    Id(building.interiorPlaceId),
                    building.displayName,
                    Id(building.ownerCitizenId),
                    Enum.Parse<PlaceKind>(building.kind),
                    ToRuntime(building.entranceCoord)));
            }

            if (data.recentHistory != null) {
                foreach (HistoryEventSaveData history in data.recentHistory) {
                    state.AddHistoryEvent(ToRuntime(history));
                }
            }

            return state;
        }

        private static WorldEntityId Id(string value)
        {
            return new WorldEntityId(value);
        }

        private static GridSaveData ToSaveData(GridCoord coord)
        {
            return new GridSaveData { x = coord.X, y = coord.Y };
        }

        private static GridCoord ToRuntime(GridSaveData data)
        {
            return new GridCoord(data.x, data.y);
        }

        private static GridBoundsSaveData ToSaveData(GridBounds bounds)
        {
            return new GridBoundsSaveData {
                min = ToSaveData(bounds.Min),
                width = bounds.Width,
                height = bounds.Height
            };
        }

        private static GridBounds ToRuntime(GridBoundsSaveData data)
        {
            return new GridBounds(ToRuntime(data.min), data.width, data.height);
        }

        private static DateTimeSaveData ToSaveData(GameDateTime dateTime)
        {
            return new DateTimeSaveData {
                year = dateTime.Date.Year,
                month = dateTime.Date.Month,
                day = dateTime.Date.Day,
                absoluteDay = dateTime.Date.AbsoluteDay,
                hour = dateTime.Time.Hour,
                minute = dateTime.Time.Minute
            };
        }

        private static GameDateTime ToRuntime(DateTimeSaveData data)
        {
            return new GameDateTime(
                new GameDate(data.year, data.month, data.day, data.absoluteDay),
                new TimeOfDay(data.hour, data.minute));
        }

        private static HistoryEventSaveData ToSaveData(HistoryEvent historyEvent)
        {
            var historyData = new HistoryEventSaveData {
                id = historyEvent.Id.Value,
                timestamp = ToSaveData(historyEvent.Timestamp),
                kind = historyEvent.Kind.ToString(),
                description = historyEvent.Description
            };

            foreach (WorldEntityId actorId in historyEvent.ActorIds) {
                historyData.actorIds.Add(actorId.Value);
            }

            foreach (WorldEntityId placeId in historyEvent.PlaceIds) {
                historyData.placeIds.Add(placeId.Value);
            }

            return historyData;
        }

        private static MorningSaveData ToSaveData(MorningState morning)
        {
            if (morning == null) {
                return null;
            }

            return new MorningSaveData {
                status = morning.Status.ToString(),
                startedAt = ToSaveData(morning.StartedAt),
                endedAt = ToSaveData(morning.EndedAt),
                tasksCompleted = morning.TasksCompleted,
                moneyEarnedCents = morning.MoneyEarnedCents
            };
        }

        private static MorningState ToRuntime(MorningSaveData morning)
        {
            return new MorningState(
                Enum.Parse<MorningStatus>(morning.status),
                ToRuntime(morning.startedAt),
                ToRuntime(morning.endedAt),
                morning.tasksCompleted,
                morning.moneyEarnedCents);
        }

        private static CitizenGoalSaveData ToSaveData(CitizenGoalState goal)
        {
            return new CitizenGoalSaveData {
                id = goal.Id.Value,
                citizenId = goal.CitizenId.Value,
                kind = goal.Kind.ToString(),
                targetPlaceId = goal.TargetPlaceId.Value,
                targetCoord = ToSaveData(goal.TargetCoord),
                activity = goal.Activity.ToString(),
                urgency = goal.Urgency,
                reason = goal.Reason,
                createdAt = ToSaveData(goal.CreatedAt),
                expiresAt = ToSaveData(goal.ExpiresAt),
                status = goal.Status.ToString()
            };
        }

        private static CitizenRegistrationSaveData ToSaveData(CitizenRegistrationState registration)
        {
            return new CitizenRegistrationSaveData {
                citizenId = registration.CitizenId.Value,
                registeredAt = ToSaveData(registration.RegisteredAt),
                startingResidencePlaceId = registration.StartingResidencePlaceId.Value
            };
        }

        private static CitizenRegistrationState ToRuntime(CitizenRegistrationSaveData registration)
        {
            return new CitizenRegistrationState(
                Id(registration.citizenId),
                ToRuntime(registration.registeredAt),
                Id(registration.startingResidencePlaceId));
        }

        private static TerritoryChunkSaveData ToSaveData(TerritoryChunkState territory)
        {
            return new TerritoryChunkSaveData {
                id = territory.Id.Value,
                regionId = territory.RegionId.Value,
                chunkX = territory.Coord.X,
                chunkY = territory.Coord.Y,
                displayName = territory.DisplayName,
                biome = territory.Biome.ToString(),
                claimStatus = territory.ClaimStatus.ToString(),
                claimOwnerId = territory.ClaimOwnerId.Value,
                settlementId = territory.SettlementId.Value,
                jurisdictionId = territory.JurisdictionId.Value,
                isBuildable = territory.IsBuildable,
                isDiscovered = territory.IsDiscovered
            };
        }

        private static RoleAssignmentSaveData ToSaveData(RoleAssignmentState assignment)
        {
            return new RoleAssignmentSaveData {
                id = assignment.Id.Value,
                citizenId = assignment.CitizenId.Value,
                roleId = assignment.RoleId,
                workplacePlaceId = assignment.WorkplacePlaceId.Value,
                isActive = assignment.IsActive
            };
        }

        private static RoleAssignmentState ToRuntime(RoleAssignmentSaveData assignment)
        {
            return new RoleAssignmentState(
                Id(assignment.id),
                Id(assignment.citizenId),
                assignment.roleId,
                Id(assignment.workplacePlaceId),
                assignment.isActive);
        }

        private static JobPostingSaveData ToSaveData(JobPostingState posting)
        {
            return new JobPostingSaveData {
                id = posting.Id.Value,
                jobDefinitionId = posting.JobDefinitionId,
                employerCitizenId = posting.EmployerCitizenId.Value,
                workplaceId = posting.WorkplaceId.Value,
                roleId = posting.RoleId,
                payModel = posting.PayModel.ToString(),
                payCents = posting.PayCents,
                openSlots = posting.OpenSlots,
                status = posting.Status.ToString(),
                createdAt = ToSaveData(posting.CreatedAt)
            };
        }

        private static JobPostingState ToRuntime(JobPostingSaveData posting)
        {
            return new JobPostingState(
                Id(posting.id),
                posting.jobDefinitionId,
                Id(posting.employerCitizenId),
                Id(posting.workplaceId),
                posting.roleId,
                Enum.Parse<PayModelKind>(posting.payModel),
                posting.payCents,
                posting.openSlots,
                Enum.Parse<JobPostingStatus>(posting.status),
                ToRuntime(posting.createdAt));
        }

        private static JobApplicationSaveData ToSaveData(JobApplicationState application)
        {
            return new JobApplicationSaveData {
                id = application.Id.Value,
                postingId = application.PostingId.Value,
                applicantCitizenId = application.ApplicantCitizenId.Value,
                status = application.Status.ToString(),
                createdAt = ToSaveData(application.CreatedAt)
            };
        }

        private static JobApplicationState ToRuntime(JobApplicationSaveData application)
        {
            return new JobApplicationState(
                Id(application.id),
                Id(application.postingId),
                Id(application.applicantCitizenId),
                Enum.Parse<JobApplicationStatus>(application.status),
                ToRuntime(application.createdAt));
        }

        private static EmploymentContractSaveData ToSaveData(EmploymentContractState contract)
        {
            return new EmploymentContractSaveData {
                id = contract.Id.Value,
                postingId = contract.PostingId.Value,
                employerCitizenId = contract.EmployerCitizenId.Value,
                workerCitizenId = contract.WorkerCitizenId.Value,
                workplaceId = contract.WorkplaceId.Value,
                jobDefinitionId = contract.JobDefinitionId,
                roleId = contract.RoleId,
                payModel = contract.PayModel.ToString(),
                payCents = contract.PayCents,
                status = contract.Status.ToString(),
                startedAt = ToSaveData(contract.StartedAt)
            };
        }

        private static EmploymentContractState ToRuntime(EmploymentContractSaveData contract)
        {
            return new EmploymentContractState(
                Id(contract.id),
                OptionalId(contract.postingId),
                Id(contract.employerCitizenId),
                Id(contract.workerCitizenId),
                Id(contract.workplaceId),
                contract.jobDefinitionId,
                contract.roleId,
                Enum.Parse<PayModelKind>(contract.payModel),
                contract.payCents,
                Enum.Parse<EmploymentContractStatus>(contract.status),
                ToRuntime(contract.startedAt));
        }

        private static WorkplaceSaveData ToSaveData(WorkplaceState workplace)
        {
            var data = new WorkplaceSaveData {
                id = workplace.Id.Value,
                buildingId = workplace.BuildingId.Value,
                placeId = workplace.PlaceId.Value,
                ownerCitizenId = workplace.OwnerCitizenId.Value,
                businessAccountId = workplace.BusinessAccountId.Value,
                status = workplace.Status.ToString(),
                opensAtMinute = workplace.OpensAtMinute,
                closesAtMinute = workplace.ClosesAtMinute
            };

            foreach (WorldEntityId id in workplace.ActiveShiftIds) {
                data.activeShiftIds.Add(id.Value);
            }

            foreach (WorldEntityId id in workplace.QueuedTaskIds) {
                data.queuedTaskIds.Add(id.Value);
            }

            return data;
        }

        private static WorkplaceState ToRuntime(WorkplaceSaveData workplace)
        {
            var state = new WorkplaceState(
                Id(workplace.id),
                Id(workplace.buildingId),
                Id(workplace.placeId),
                Id(workplace.ownerCitizenId),
                Id(workplace.businessAccountId),
                Enum.Parse<WorkplaceStatus>(workplace.status),
                workplace.opensAtMinute,
                workplace.closesAtMinute);

            foreach (string shiftId in workplace.activeShiftIds) {
                state.AddActiveShift(Id(shiftId));
            }

            foreach (string taskId in workplace.queuedTaskIds) {
                state.EnqueueTask(Id(taskId));
            }

            return state;
        }

        private static ShiftSaveData ToSaveData(ShiftState shift)
        {
            var data = new ShiftSaveData {
                id = shift.Id.Value,
                contractId = shift.ContractId.Value,
                workerCitizenId = shift.WorkerCitizenId.Value,
                workplaceId = shift.WorkplaceId.Value,
                startedAt = ToSaveData(shift.StartedAt),
                expectedEndAt = ToSaveData(shift.ExpectedEndAt),
                endedAt = ToSaveData(shift.EndedAt),
                status = shift.Status.ToString(),
                earnedCents = shift.EarnedCents
            };

            foreach (WorldEntityId taskId in shift.CompletedTaskIds) {
                data.completedTaskIds.Add(taskId.Value);
            }

            return data;
        }

        private static ShiftState ToRuntime(ShiftSaveData shift)
        {
            var state = new ShiftState(
                Id(shift.id),
                Id(shift.contractId),
                Id(shift.workerCitizenId),
                Id(shift.workplaceId),
                ToRuntime(shift.startedAt),
                ToRuntime(shift.expectedEndAt),
                ToRuntime(shift.endedAt),
                Enum.Parse<ShiftStatus>(shift.status),
                shift.earnedCents);

            foreach (string taskId in shift.completedTaskIds) {
                state.CompleteTask(Id(taskId), 0);
            }

            return state;
        }

        private static ShiftSummarySaveData ToSaveData(ShiftSummaryState summary)
        {
            return new ShiftSummarySaveData {
                shiftId = summary.ShiftId.Value,
                workerCitizenId = summary.WorkerCitizenId.Value,
                workplaceId = summary.WorkplaceId.Value,
                startedAt = ToSaveData(summary.StartedAt),
                endedAt = ToSaveData(summary.EndedAt),
                tasksCompleted = summary.TasksCompleted,
                earnedCents = summary.EarnedCents,
                historyEventsAtEnd = summary.HistoryEventsAtEnd
            };
        }

        private static ShiftSummaryState ToRuntime(ShiftSummarySaveData summary)
        {
            return new ShiftSummaryState(
                Id(summary.shiftId),
                Id(summary.workerCitizenId),
                Id(summary.workplaceId),
                ToRuntime(summary.startedAt),
                ToRuntime(summary.endedAt),
                summary.tasksCompleted,
                summary.earnedCents,
                summary.historyEventsAtEnd);
        }

        private static JobTaskSaveData ToSaveData(JobTaskState task)
        {
            var data = new JobTaskSaveData {
                id = task.Id.Value,
                definitionId = task.DefinitionId,
                workplaceId = task.WorkplaceId.Value,
                assignedWorkerId = task.AssignedWorkerId.Value,
                shiftId = task.ShiftId.Value,
                targetEntityId = task.TargetEntityId.Value,
                status = task.Status.ToString(),
                quality = task.Quality,
                createdAt = ToSaveData(task.CreatedAt),
                startedAt = ToSaveData(task.StartedAt),
                completedAt = ToSaveData(task.CompletedAt)
            };

            if (task.MiniGameResult != null) {
                data.hasMiniGameResult = true;
                data.miniGameScore = task.MiniGameResult.Score;
                data.miniGameMaxScore = task.MiniGameResult.MaxScore;
                data.miniGameQuality = task.MiniGameResult.Quality;
                data.miniGameDurationSeconds = task.MiniGameResult.DurationSeconds;
                data.miniGameMistakes = task.MiniGameResult.Mistakes;
            }

            return data;
        }

        private static JobTaskState ToRuntime(JobTaskSaveData task)
        {
            MiniGameResultState result = task.hasMiniGameResult
                ? new MiniGameResultState(task.miniGameScore, task.miniGameMaxScore, task.miniGameQuality, task.miniGameDurationSeconds, task.miniGameMistakes)
                : null;
            return new JobTaskState(
                Id(task.id),
                task.definitionId,
                Id(task.workplaceId),
                OptionalId(task.assignedWorkerId),
                OptionalId(task.shiftId),
                OptionalId(task.targetEntityId),
                Enum.Parse<JobTaskStatus>(task.status),
                task.quality,
                ToRuntime(task.createdAt),
                ToRuntime(task.startedAt),
                ToRuntime(task.completedAt),
                result);
        }

        private static VisitSaveData ToSaveData(VisitState visit)
        {
            return new VisitSaveData {
                id = visit.Id.Value,
                visitorCitizenId = visit.VisitorCitizenId.Value,
                workplaceId = visit.WorkplaceId.Value,
                placeId = visit.PlaceId.Value,
                intent = visit.Intent,
                requestedTaskDefinitionId = visit.RequestedTaskDefinitionId,
                linkedTaskId = visit.LinkedTaskId.Value,
                status = visit.Status.ToString(),
                arrivalTime = ToSaveData(visit.ArrivalTime),
                departureTime = ToSaveData(visit.DepartureTime),
                arrivalLine = visit.ArrivalLine,
                completionLine = visit.CompletionLine
            };
        }

        private static VisitState ToRuntime(VisitSaveData visit)
        {
            return new VisitState(
                Id(visit.id),
                Id(visit.visitorCitizenId),
                Id(visit.workplaceId),
                Id(visit.placeId),
                visit.intent,
                visit.requestedTaskDefinitionId,
                OptionalId(visit.linkedTaskId),
                Enum.Parse<VisitStatus>(visit.status),
                ToRuntime(visit.arrivalTime),
                ToRuntime(visit.departureTime),
                visit.arrivalLine,
                visit.completionLine);
        }

        private static WorkplaceInventorySaveData ToSaveData(WorkplaceInventoryState inventory)
        {
            var data = new WorkplaceInventorySaveData { workplaceId = inventory.WorkplaceId.Value };
            foreach (InventoryStackState stack in inventory.Stacks) {
                data.stacks.Add(new InventoryStackSaveData { itemId = stack.ItemId, count = stack.Count });
            }

            return data;
        }

        private static WorkplaceInventoryState ToRuntime(WorkplaceInventorySaveData inventory)
        {
            var state = new WorkplaceInventoryState(Id(inventory.workplaceId));
            foreach (InventoryStackSaveData stack in inventory.stacks) {
                state.Add(stack.itemId, stack.count);
            }

            return state;
        }

        private static InventoryContainerSaveData ToSaveData(InventoryContainerState inventory)
        {
            var data = new InventoryContainerSaveData {
                id = inventory.Id.Value,
                ownerEntityId = inventory.OwnerEntityId.Value,
                kind = inventory.Kind.ToString(),
                displayName = inventory.DisplayName
            };
            foreach (InventoryStackState stack in inventory.Stacks) {
                data.stacks.Add(new InventoryStackSaveData { itemId = stack.ItemId, count = stack.Count });
            }

            return data;
        }

        private static InventoryContainerState ToRuntime(InventoryContainerSaveData inventory)
        {
            var state = new InventoryContainerState(
                Id(inventory.id),
                Id(inventory.ownerEntityId),
                Enum.Parse<InventoryContainerKind>(inventory.kind),
                inventory.displayName);
            foreach (InventoryStackSaveData stack in inventory.stacks) {
                state.TryAdd(stack.itemId, stack.count);
            }

            return state;
        }

        private static SkillSaveData ToSaveData(SkillState skill)
        {
            return new SkillSaveData { citizenId = skill.CitizenId.Value, skill = skill.Skill.ToString(), experience = skill.Experience };
        }

        private static SkillState ToRuntime(SkillSaveData skill)
        {
            return new SkillState(Id(skill.citizenId), Enum.Parse<SkillKind>(skill.skill), skill.experience);
        }

        private static PerformanceRecordSaveData ToSaveData(PerformanceRecordState record)
        {
            return new PerformanceRecordSaveData {
                id = record.Id.Value,
                citizenId = record.CitizenId.Value,
                workplaceId = record.WorkplaceId.Value,
                note = record.Note,
                score = record.Score,
                createdAt = ToSaveData(record.CreatedAt)
            };
        }

        private static PerformanceRecordState ToRuntime(PerformanceRecordSaveData record)
        {
            return new PerformanceRecordState(Id(record.id), Id(record.citizenId), Id(record.workplaceId), record.note, record.score, ToRuntime(record.createdAt));
        }

        private static MoneyAccountSaveData ToSaveData(MoneyAccountState account)
        {
            return new MoneyAccountSaveData {
                id = account.Id.Value,
                ownerEntityId = account.OwnerEntityId.Value,
                displayName = account.DisplayName,
                balanceCents = account.BalanceCents
            };
        }

        private static MoneyAccountState ToRuntime(MoneyAccountSaveData account)
        {
            return new MoneyAccountState(
                Id(account.id),
                Id(account.ownerEntityId),
                account.displayName,
                account.balanceCents);
        }

        private static CashDrawerSaveData ToSaveData(CashDrawerState drawer)
        {
            var data = new CashDrawerSaveData {
                id = drawer.Id.Value,
                ownerEntityId = drawer.OwnerEntityId.Value,
                kind = drawer.Kind.ToString(),
                displayName = drawer.DisplayName
            };
            foreach (CashDenominationStackState stack in drawer.Stacks) {
                data.stacks.Add(new CashDenominationStackSaveData {
                    denomination = stack.Denomination.ToString(),
                    count = stack.Count
                });
            }

            return data;
        }

        private static CashDrawerState ToRuntime(CashDrawerSaveData drawer)
        {
            var state = new CashDrawerState(
                Id(drawer.id),
                Id(drawer.ownerEntityId),
                Enum.Parse<CashContainerKind>(drawer.kind),
                drawer.displayName);
            foreach (CashDenominationStackSaveData stack in drawer.stacks) {
                state.Add(Enum.Parse<CashDenomination>(stack.denomination), stack.count);
            }

            return state;
        }

        private static TransactionSaveData ToSaveData(TransactionState transaction)
        {
            return new TransactionSaveData {
                id = transaction.Id.Value,
                timestamp = ToSaveData(transaction.Timestamp),
                payerAccountId = transaction.PayerAccountId.Value,
                payeeAccountId = transaction.PayeeAccountId.Value,
                amountCents = transaction.AmountCents,
                reason = transaction.Reason,
                relatedPlaceId = transaction.RelatedPlaceId.Value,
                actionKind = transaction.ActionKind.ToString()
            };
        }

        private static TransactionState ToRuntime(TransactionSaveData transaction)
        {
            return new TransactionState(
                Id(transaction.id),
                ToRuntime(transaction.timestamp),
                Id(transaction.payerAccountId),
                Id(transaction.payeeAccountId),
                transaction.amountCents,
                transaction.reason,
                OptionalId(transaction.relatedPlaceId),
                Enum.Parse<WorldActionKind>(transaction.actionKind));
        }

        private static DialogueSaveData ToSaveData(DialogueState dialogue)
        {
            return new DialogueSaveData {
                citizenId = dialogue.CitizenId.Value,
                lastLineId = dialogue.LastLineId,
                conversationCount = dialogue.ConversationCount,
                lastTalkedAt = ToSaveData(dialogue.LastTalkedAt)
            };
        }

        private static DialogueState ToRuntime(DialogueSaveData dialogue)
        {
            return new DialogueState(
                Id(dialogue.citizenId),
                dialogue.lastLineId,
                dialogue.conversationCount,
                ToRuntime(dialogue.lastTalkedAt));
        }

        private static RelationshipSaveData ToSaveData(RelationshipState relationship)
        {
            return new RelationshipSaveData {
                ownerCitizenId = relationship.OwnerCitizenId.Value,
                otherCitizenId = relationship.OtherCitizenId.Value,
                affinity = relationship.Affinity,
                familiarity = relationship.Familiarity,
                lastInteractionAt = ToSaveData(relationship.LastInteractionAt)
            };
        }

        private static RelationshipState ToRuntime(RelationshipSaveData relationship)
        {
            return new RelationshipState(
                Id(relationship.ownerCitizenId),
                Id(relationship.otherCitizenId),
                relationship.affinity,
                relationship.familiarity,
                ToRuntime(relationship.lastInteractionAt));
        }

        private static CitizenMemorySaveData ToSaveData(CitizenMemoryState memory)
        {
            return new CitizenMemorySaveData {
                id = memory.Id.Value,
                citizenId = memory.CitizenId.Value,
                subjectCitizenId = memory.SubjectCitizenId.Value,
                kind = memory.Kind,
                summary = memory.Summary,
                createdAt = ToSaveData(memory.CreatedAt),
                sourceHistoryEventId = memory.SourceHistoryEventId.Value,
                salience = memory.Salience
            };
        }

        private static CitizenMemoryState ToRuntime(CitizenMemorySaveData memory)
        {
            return new CitizenMemoryState(
                Id(memory.id),
                Id(memory.citizenId),
                Id(memory.subjectCitizenId),
                memory.kind,
                memory.summary,
                ToRuntime(memory.createdAt),
                Id(memory.sourceHistoryEventId),
                memory.salience);
        }

        private static PublicRecordSaveData ToSaveData(PublicRecordState record)
        {
            return new PublicRecordSaveData {
                citizenId = record.CitizenId.Value,
                reputationScore = record.ReputationScore,
                reportsFiled = record.ReportsFiled,
                reportsReceived = record.ReportsReceived,
                lastUpdatedAt = ToSaveData(record.LastUpdatedAt)
            };
        }

        private static PublicRecordState ToRuntime(PublicRecordSaveData record)
        {
            return new PublicRecordState(
                Id(record.citizenId),
                record.reputationScore,
                record.reportsFiled,
                record.reportsReceived,
                ToRuntime(record.lastUpdatedAt));
        }

        private static CivicReportSaveData ToSaveData(CivicReportState report)
        {
            return new CivicReportSaveData {
                id = report.Id.Value,
                reporterCitizenId = report.ReporterCitizenId.Value,
                subjectCitizenId = report.SubjectCitizenId.Value,
                relatedPlaceId = report.RelatedPlaceId.Value,
                summary = report.Summary,
                createdAt = ToSaveData(report.CreatedAt),
                status = report.Status.ToString()
            };
        }

        private static CivicReportState ToRuntime(CivicReportSaveData report)
        {
            return new CivicReportState(
                Id(report.id),
                Id(report.reporterCitizenId),
                Id(report.subjectCitizenId),
                OptionalId(report.relatedPlaceId),
                report.summary,
                ToRuntime(report.createdAt),
                Enum.Parse<CivicReportStatus>(report.status));
        }

        private static CivicRegistryEntrySaveData ToSaveData(CivicRegistryEntryState entry)
        {
            return new CivicRegistryEntrySaveData {
                id = entry.Id.Value,
                citizenId = entry.CitizenId.Value,
                kind = entry.Kind.ToString(),
                summary = entry.Summary,
                createdAt = ToSaveData(entry.CreatedAt),
                sourceReportId = entry.SourceReportId.Value,
                filedByCitizenId = entry.FiledByCitizenId.Value,
                relatedPlaceId = entry.RelatedPlaceId.Value
            };
        }

        private static CivicRegistryEntryState ToRuntime(CivicRegistryEntrySaveData entry)
        {
            return new CivicRegistryEntryState(
                Id(entry.id),
                Id(entry.citizenId),
                Enum.Parse<CivicRegistryEntryKind>(entry.kind),
                entry.summary,
                ToRuntime(entry.createdAt),
                OptionalId(entry.sourceReportId),
                Id(entry.filedByCitizenId),
                OptionalId(entry.relatedPlaceId));
        }

        private static LeaseContractSaveData ToSaveData(LeaseContractState contract)
        {
            return new LeaseContractSaveData {
                id = contract.Id.Value,
                buildingId = contract.BuildingId.Value,
                landlordCitizenId = contract.LandlordCitizenId.Value,
                tenantCitizenId = contract.TenantCitizenId.Value,
                rentCents = contract.RentCents,
                dueDayOfMonth = contract.DueDayOfMonth,
                status = contract.Status.ToString(),
                startedAt = ToSaveData(contract.StartedAt),
                lastPaidAt = ToSaveData(contract.LastPaidAt),
                paymentsMade = contract.PaymentsMade
            };
        }

        private static LeaseContractState ToRuntime(LeaseContractSaveData contract)
        {
            return new LeaseContractState(
                Id(contract.id),
                Id(contract.buildingId),
                Id(contract.landlordCitizenId),
                Id(contract.tenantCitizenId),
                contract.rentCents,
                contract.dueDayOfMonth,
                Enum.Parse<LeaseContractStatus>(contract.status),
                ToRuntime(contract.startedAt),
                ToRuntime(contract.lastPaidAt),
                contract.paymentsMade);
        }

        private static TenantRecordSaveData ToSaveData(TenantRecordState record)
        {
            return new TenantRecordSaveData {
                id = record.Id.Value,
                leaseContractId = record.LeaseContractId.Value,
                buildingId = record.BuildingId.Value,
                tenantCitizenId = record.TenantCitizenId.Value,
                status = record.Status.ToString(),
                startedAt = ToSaveData(record.StartedAt),
                endedAt = ToSaveData(record.EndedAt)
            };
        }

        private static TenantRecordState ToRuntime(TenantRecordSaveData record)
        {
            return new TenantRecordState(
                Id(record.id),
                Id(record.leaseContractId),
                Id(record.buildingId),
                Id(record.tenantCitizenId),
                Enum.Parse<TenantRecordStatus>(record.status),
                ToRuntime(record.startedAt),
                ToRuntime(record.endedAt));
        }

        private static BusinessLedgerEntrySaveData ToSaveData(BusinessLedgerEntryState entry)
        {
            return new BusinessLedgerEntrySaveData {
                id = entry.Id.Value,
                workplaceId = entry.WorkplaceId.Value,
                accountId = entry.AccountId.Value,
                kind = entry.Kind.ToString(),
                amountCents = entry.AmountCents,
                reason = entry.Reason,
                timestamp = ToSaveData(entry.Timestamp),
                relatedEntityId = entry.RelatedEntityId.Value
            };
        }

        private static BusinessLedgerEntryState ToRuntime(BusinessLedgerEntrySaveData entry)
        {
            return new BusinessLedgerEntryState(
                Id(entry.id),
                Id(entry.workplaceId),
                Id(entry.accountId),
                Enum.Parse<BusinessLedgerEntryKind>(entry.kind),
                entry.amountCents,
                entry.reason,
                ToRuntime(entry.timestamp),
                OptionalId(entry.relatedEntityId));
        }

        private static TerritoryChunkState ToRuntime(TerritoryChunkSaveData territory)
        {
            return new TerritoryChunkState(
                Id(territory.id),
                Id(territory.regionId),
                new GridChunkCoord(territory.chunkX, territory.chunkY),
                territory.displayName,
                Enum.Parse<TerritoryBiome>(territory.biome),
                Enum.Parse<TerritoryClaimStatus>(territory.claimStatus),
                OptionalId(territory.claimOwnerId),
                OptionalId(territory.settlementId),
                OptionalId(territory.jurisdictionId),
                territory.isBuildable,
                territory.isDiscovered);
        }

        private static CitizenGoalState ToRuntime(CitizenGoalSaveData goal)
        {
            var definition = new CitizenGoalDefinition(
                Enum.Parse<CitizenGoalKind>(goal.kind),
                Id(goal.targetPlaceId),
                ToRuntime(goal.targetCoord),
                Enum.Parse<CitizenActivityState>(goal.activity),
                goal.urgency,
                goal.reason);

            return new CitizenGoalState(
                Id(goal.id),
                Id(goal.citizenId),
                definition,
                ToRuntime(goal.createdAt),
                ToRuntime(goal.expiresAt),
                Enum.Parse<CitizenGoalStatus>(goal.status));
        }

        private static HistoryEvent ToRuntime(HistoryEventSaveData history)
        {
            var actorIds = new WorldEntityId[history.actorIds.Count];
            for (int i = 0; i < actorIds.Length; i++) {
                actorIds[i] = Id(history.actorIds[i]);
            }

            var placeIds = new WorldEntityId[history.placeIds.Count];
            for (int i = 0; i < placeIds.Length; i++) {
                placeIds[i] = Id(history.placeIds[i]);
            }

            return new HistoryEvent(
                Id(history.id),
                ToRuntime(history.timestamp),
                Enum.Parse<HistoryEventKind>(history.kind),
                history.description,
                actorIds,
                placeIds);
        }

        private static WorldEntityId OptionalId(string value)
        {
            return string.IsNullOrEmpty(value)
                ? default
                : Id(value);
        }
    }
}
