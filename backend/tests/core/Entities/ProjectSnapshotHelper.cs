using Pims.Dal;
using Pims.Dal.Entities;
using System;
using System.Collections.Generic;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a Project Snapshot.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="snapshotOn"></param>
        /// <returns></returns>
        public static Entity.ProjectSnapshot CreateProjectSnapshot(int id, DateTime snapshotOn)
        {
            return CreateProjectSnapshot(id, 0, snapshotOn);
        }

        /// <summary>
        /// Create a new instance of a Project Snapshot.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectId"></param>
        /// /// <param name="projectId"></param>
        /// <returns></returns>
        public static Entity.ProjectSnapshot CreateProjectSnapshot(int id, int projectId = 0, DateTime? snapshotOn = null, Agency agency = null)
        {
            var user = CreateUser(1, Guid.NewGuid(), "project tester", "asasa", "asasa", agency: agency);
            return new Entity.ProjectSnapshot()
            {
                Id = id,
                ProjectId = projectId,
                SnapshotOn = snapshotOn ?? DateTime.UtcNow,
                CreatedByName = user.DisplayName,
                CreatedBy = user.Username,
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = user.Username,
                UpdatedByName = user.DisplayName,
                UpdatedOn = DateTime.UtcNow,
                RowVersion = 1
            };
        }

        /// <summary>
        /// Create a new List with new instances of Project Snapshots.
        /// </summary>
        /// <param name="startId"></param>
        /// <param name="count"></param>
        /// <param name="projectId"></param>
        /// <param name="snapshotOn"></param>
        /// <returns></returns>
        public static List<Entity.ProjectSnapshot> CreateProjectSnapshots(int startId, int count, int projectId = 0, DateTime? snapshotOn = null, Agency agency = null)
        {
            var projectSnapshots = new List<Entity.ProjectSnapshot>(count);
            for (var i = startId; i < (startId + count); i++)
            {
                projectSnapshots.Add(CreateProjectSnapshot(i, projectId, snapshotOn));
            }
            return projectSnapshots;
        }

        /// <summary>
        /// Create a new instance of a Project Snapshot.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.ProjectSnapshot CreateProjectSnapshot(this PimsContext context, int id)
        {
            return context.CreateProjectSnapshot(id, 0, DateTime.UtcNow);
        }

        /// <summary>
        /// Create a new instance of a Project Snapshot.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="projectId"></param>
        /// <param name="snapshotOn"></param>
        /// <returns></returns>
        public static Entity.ProjectSnapshot CreateProjectSnapshot(this PimsContext context, int id, int projectId = 0, DateTime? snapshotOn = null, Agency agency = null)
        {
            var projectSnapshot = EntityHelper.CreateProjectSnapshot(id, projectId, snapshotOn, agency);
            context.ProjectSnapshots.Add(projectSnapshot);
            return projectSnapshot;
        }

        /// <summary>
        /// Create a new List with new instances of Project Snapshots.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="startId"></param>
        /// <param name="projectId"></param>
        /// <param name="snapshotOn"></param>
        /// <returns></returns>
        public static List<Entity.ProjectSnapshot> CreateProjectSnapshots(this PimsContext context, int startId, int count, int projectId = 0, DateTime? snapshotOn = null)
        {
            var projectSnapshots = new List<Entity.ProjectSnapshot>(count);
            for (var i = startId; i < (startId + count); i++)
            {
                projectSnapshots.Add(context.CreateProjectSnapshot(i, projectId, snapshotOn));
            }
            return projectSnapshots;
        }
    }
}
