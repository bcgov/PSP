using System;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of AcquisitionFileNote.
        /// </summary>
        /// <param name="acquisitionFile"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static Entity.PimsAcquisitionFileNote CreateAcquisitionFileNote(Entity.PimsAcquisitionFile acquisitionFile = null, Entity.PimsNote note = null)
        {
            note ??= EntityHelper.CreateNote("Test Note");
            acquisitionFile ??= EntityHelper.CreateAcquisitionFile(1);

            return new Entity.PimsAcquisitionFileNote()
            {
                Note = note,
                AcquisitionFile = acquisitionFile,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppCreateUserDirectory = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                ConcurrencyControlNumber = 1,
            };
        }

        /// <summary>
        /// Create a new instance of DispositionFileNote.
        /// </summary>
        /// <param name="dispositionFile"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static Entity.PimsDispositionFileNote CreateDispositionFileNote(Entity.PimsDispositionFile dispositionFile = null, Entity.PimsNote note = null)
        {
            note ??= EntityHelper.CreateNote("Test Note");
            dispositionFile ??= EntityHelper.CreateDispositionFile(1);

            return new Entity.PimsDispositionFileNote()
            {
                Note = note,
                DispositionFile = dispositionFile,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppCreateUserDirectory = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                ConcurrencyControlNumber = 1,
            };
        }

        /// <summary>
        /// Create a new instance of LeaseNote.
        /// </summary>
        /// <param name="lease"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static Entity.PimsLeaseNote CreateLeaseNote(Entity.PimsLease lease = null, Entity.PimsNote note = null)
        {
            note ??= EntityHelper.CreateNote("Test Note");
            lease ??= EntityHelper.CreateLease(1);

            return new Entity.PimsLeaseNote()
            {
                Note = note,
                Lease = lease,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppCreateUserDirectory = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                ConcurrencyControlNumber = 1,
            };
        }

        /// <summary>
        /// Create a new instance of ProjectFileNote.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static Entity.PimsProjectNote CreateProjectNote(Entity.PimsProject project = null, Entity.PimsNote note = null)
        {
            note ??= EntityHelper.CreateNote("Test Note");
            project ??= EntityHelper.CreateProject(1, "9999", "TEST PROJECT");

            return new Entity.PimsProjectNote()
            {
                Note = note,
                Project = project,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppCreateUserDirectory = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                ConcurrencyControlNumber = 1,
            };
        }

        /// <summary>
        /// Create a new instance of ResearchFileNote.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static Entity.PimsResearchFileNote CreateResearchNote(Entity.PimsResearchFile researchFile = null, Entity.PimsNote note = null)
        {
            note ??= EntityHelper.CreateNote("Test Note");
            researchFile ??= EntityHelper.CreateResearchFile(1);

            return new Entity.PimsResearchFileNote()
            {
                Note = note,
                ResearchFile = researchFile,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppCreateUserDirectory = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                ConcurrencyControlNumber = 1,
            };
        }

        /// <summary>
        /// Create a new instance of ManagementFileNote.
        /// </summary>
        /// <param name="managementFile"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static Entity.PimsManagementFileNote CreateManagementFileNote(Entity.PimsManagementFile managementFile = null, Entity.PimsNote note = null)
        {
            note ??= EntityHelper.CreateNote("Test Note");
            managementFile ??= EntityHelper.CreateManagementFile(1);

            return new Entity.PimsManagementFileNote()
            {
                Note = note,
                ManagementFile = managementFile,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppCreateUserDirectory = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                ConcurrencyControlNumber = 1,
            };
        }

        /// <summary>
        /// Create a new instance of a Note.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="surname"></param>
        /// <param name="firstName"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Entity.PimsNote CreateNote(string note = "Test Note", long id = 1)
        {
            return new Entity.PimsNote()
            {
                Internal_Id = id,
                NoteTxt = note,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppCreateUserDirectory = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                ConcurrencyControlNumber = 1,
            };
        }
    }
}
