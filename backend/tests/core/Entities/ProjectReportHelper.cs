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
        /// Create a new instance of a Project Report.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.ProjectReport CreateProjectReport(long id, string name)
        {
            return CreateProjectReport(id, name, DateTime.Now, DateTime.Now.AddDays(-1), false);
        }

        /// <summary>
        /// Create a new instance of a Project Report.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="toDate"></param>
        /// <param name="fromDate"></param>
        /// <param name="isFinal"></param>
        /// <returns></returns>
        public static Entity.ProjectReport CreateProjectReport(long id, string name = null, DateTime? toDate = null, DateTime? fromDate = null, bool isFinal = false, Agency agency = null)
        {
            var user = CreateUser(1, Guid.NewGuid(), "project tester", "asasa", "asasa", null, agency);
            return new Entity.ProjectReport()
            {
                Id = id,
                Name = name,
                To = toDate,
                From = fromDate,
                IsFinal = isFinal,
                ReportType = 0,
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
        /// Create a new List with new instances of Project Reports.
        /// </summary>
        /// <param name="startId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<Entity.ProjectReport> CreateProjectReports(long startId, int count)
        {
            var projects = new List<Entity.ProjectReport>(count);
            for (var i = startId; i < (startId + count); i++)
            {
                projects.Add(CreateProjectReport(i, "projectreport" + i));
            }
            return projects;
        }

        /// <summary>
        /// Create a new instance of a Project Report.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.ProjectReport CreateProjectReport(this PimsContext context, long id, string name)
        {
            return context.CreateProjectReport(id, name);
        }

        /// <summary>
        /// Create a new instance of a Project Reports.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="toDate"></param>
        /// <param name="fromDate"></param>
        /// <param name="isFinal"></param>
        /// <returns></returns>
        public static Entity.ProjectReport CreateProjectReport(this PimsContext context, long id, string name = null, DateTime? toDate = null, DateTime? fromDate = null, bool isFinal = false, Agency agency = null)
        {
            var projectReport = EntityHelper.CreateProjectReport(id, name, toDate, fromDate, isFinal, agency);
            context.ProjectReports.Add(projectReport);
            return projectReport;
        }

        /// <summary>
        /// Create a new List with new instances of Project Reports.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="startId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<Entity.ProjectReport> CreateProjectReports(this PimsContext context, long startId, int count)
        {

            var projectReports = new List<Entity.ProjectReport>(count);
            for (var i = startId; i < (startId + count); i++)
            {
                projectReports.Add(context.CreateProjectReport(i, "projectReport" + i));
            }
            return projectReports;
        }
    }
}
