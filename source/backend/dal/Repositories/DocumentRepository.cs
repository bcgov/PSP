using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.Document;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// DocumentRepository class, provides a repository to interact with documents within the datasource.
    /// </summary>
    public class DocumentRepository : BaseRepository<PimsDocument>, IDocumentRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a DocumentActivityRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public DocumentRepository(
            PimsContext dbContext,
            ClaimsPrincipal user,
            ILogger<DocumentRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get the document from the database based on document id.
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public PimsDocument TryGet(long documentId)
        {
            return this.Context.PimsDocuments.AsNoTracking().FirstOrDefault(x => x.DocumentId == documentId);
        }

        public PimsDocument TryGetDocumentRelationships(long documentId)
        {
            var documentRelationships = Context.PimsDocuments.AsNoTracking()
                .Include(d => d.PimsResearchFileDocuments)
                .Include(d => d.PimsAcquisitionFileDocuments)
                .Include(d => d.PimsProjectDocuments)
                .Include(d => d.PimsFormTypes)
                .Include(d => d.PimsLeaseDocuments)
                .Include(d => d.PimsManagementFileDocuments)
                .Include(d => d.PimsMgmtActivityDocuments)
                .Include(d => d.PimsDispositionFileDocuments)
                .Where(d => d.DocumentId == documentId)
                .AsNoTracking()
                .FirstOrDefault();

            return documentRelationships;
        }

        /// <summary>
        /// Adds the passed document to the database.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public PimsDocument Add(PimsDocument document)
        {
            document.ThrowIfNull(nameof(document));

            var newEntry = this.Context.PimsDocuments.Add(document);
            if (newEntry.State == EntityState.Added)
            {
                return newEntry.Entity;
            }
            else
            {
                throw new InvalidOperationException("Could not create document");
            }
        }

        /// <summary>
        /// Updates the passed document in the database.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public PimsDocument Update(PimsDocument document, bool commitTransaction = true)
        {
            document.ThrowIfNull(nameof(document));
            User.ThrowIfNotAuthorized(Permissions.DocumentEdit);

            document = Context.Update(document).Entity;

            return document;
        }

        /// <summary>
        /// Deletes the passed document from the database.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public bool Delete(PimsDocument document, bool commitTransaction = true)
        {
            document.ThrowIfNull(nameof(document));

            // Need to load required related entities otherwise the below foreach may fail.
            var documentToDelete = Context.PimsDocuments.AsNoTracking()
                .Include(d => d.PimsResearchFileDocuments)
                .Include(d => d.PimsAcquisitionFileDocuments)
                .Include(d => d.PimsProjectDocuments)
                .Include(d => d.PimsFormTypes)
                .Include(d => d.PimsLeaseDocuments)
                .Include(d => d.PimsManagementFileDocuments)
                .Include(d => d.PimsMgmtActivityDocuments)
                .Include(d => d.PimsDispositionFileDocuments)
                .Where(d => d.DocumentId == document.Internal_Id)
                .FirstOrDefault();

            if (documentToDelete.PimsResearchFileDocuments.Count > 0)
            {
                Context.PimsResearchFileDocuments.RemoveRange(documentToDelete.PimsResearchFileDocuments);
            }

            if (documentToDelete.PimsAcquisitionFileDocuments.Count > 0)
            {
                Context.PimsAcquisitionFileDocuments.RemoveRange(documentToDelete.PimsAcquisitionFileDocuments);
            }

            if (documentToDelete.PimsProjectDocuments.Count > 0)
            {
                Context.PimsProjectDocuments.RemoveRange(documentToDelete.PimsProjectDocuments);
            }

            if (documentToDelete.PimsLeaseDocuments.Count > 0)
            {
                Context.PimsLeaseDocuments.RemoveRange(documentToDelete.PimsLeaseDocuments);
            }

            if (documentToDelete.PimsManagementFileDocuments.Count > 0)
            {
                Context.PimsManagementFileDocuments.RemoveRange(documentToDelete.PimsManagementFileDocuments);
            }

            if (documentToDelete.PimsMgmtActivityDocuments.Count > 0)
            {
                Context.PimsMgmtActivityDocuments.RemoveRange(documentToDelete.PimsMgmtActivityDocuments);
            }

            if (documentToDelete.PimsDispositionFileDocuments.Count > 0)
            {
                Context.PimsDispositionFileDocuments.RemoveRange(documentToDelete.PimsDispositionFileDocuments);
            }

            foreach (var pimsFormTypeDocument in documentToDelete.PimsFormTypes.ToList())
            {
                var updatedFormType = pimsFormTypeDocument;
                updatedFormType.DocumentId = null;
                Context.Entry(updatedFormType).Property(x => x.DocumentId).IsModified = true;
            }

            if (commitTransaction)
            {
                Context.CommitTransaction(); // TODO: required to enforce delete order. Can be removed when cascade deletes are implemented.
            }

            Context.PimsDocuments.Remove(documentToDelete);

            return true;
        }

        /// <summary>
        /// Deletes the passed document from the database.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public bool DeleteDocument(PimsDocument document)
        {
            document.ThrowIfNull(nameof(document));

            Context.PimsDocuments.Remove(document);

            return true;
        }

        public int DocumentRelationshipCount(long documentId)
        {
            var documentRelationships = Context.PimsDocuments.AsNoTracking()
                .Include(d => d.PimsResearchFileDocuments)
                .Include(d => d.PimsAcquisitionFileDocuments)
                .Include(d => d.PimsProjectDocuments)
                .Include(d => d.PimsFormTypes)
                .Include(d => d.PimsLeaseDocuments)
                .Include(d => d.PimsManagementFileDocuments)
                .Include(d => d.PimsMgmtActivityDocuments)
                .Include(d => d.PimsDispositionFileDocuments)
                .Where(d => d.DocumentId == documentId)
                .AsNoTracking()
                .FirstOrDefault();

            return documentRelationships.PimsResearchFileDocuments.Count +
                    documentRelationships.PimsAcquisitionFileDocuments.Count +
                    documentRelationships.PimsProjectDocuments.Count +
                    documentRelationships.PimsFormTypes.Count +
                    documentRelationships.PimsLeaseDocuments.Count +
                    documentRelationships.PimsManagementFileDocuments.Count +
                    documentRelationships.PimsMgmtActivityDocuments.Count +
                    documentRelationships.PimsDispositionFileDocuments.Count;
        }

        public Paged<PimsDocument> GetPageDeep(DocumentSearchFilterModel filter, DocumentAccessContext accessContext)
        {
            // RECOMMENDED - use a log scope to group all potential SQL statements generated by EF for this method call
            using var scope = Logger.QueryScope();

            IQueryable<PimsDocument> query = GetCommonQueryDeep(filter, accessContext);

            var skip = (filter.Page - 1) * filter.Quantity;
            var pageItems = query.Skip(skip).Take(filter.Quantity).ToList();

            return new Paged<PimsDocument>(pageItems, filter.Page, filter.Quantity, query.Count());
        }

        private IQueryable<PimsDocument> GetCommonQueryDeep(DocumentSearchFilterModel filter, DocumentAccessContext accessContext, bool excludeTemplates = true, bool excludeOrphans = true)
        {
            accessContext ??= new DocumentAccessContext();
            var predicate = PredicateBuilder.New<PimsDocument>(doc => true);

            if (!string.IsNullOrWhiteSpace(filter.DocumentName))
            {
                predicate = predicate.And(x => EF.Functions.Like(x.FileName, $"%{filter.DocumentName}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.DocumentTypTypeCode))
            {
                int typeCodeId = int.Parse(filter.DocumentTypTypeCode);
                predicate = predicate.And(acq => acq.DocumentTypeId == typeCodeId);
            }

            if (!string.IsNullOrWhiteSpace(filter.DocumentStatusTypeCode))
            {
                predicate = predicate.And(acq => acq.DocumentStatusTypeCode == filter.DocumentStatusTypeCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.Pid))
            {
                var pidValue = filter.Pid.Replace("-", string.Empty).Trim().TrimStart('0');
                predicate = predicate.And(pd => pd.PimsPropertyDocuments.Any(p => p != null && EF.Functions.Like(p.Property.Pid.ToString(), $"%{pidValue}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.Pin))
            {
                var pinValue = filter.Pin.Replace("-", string.Empty).Trim().TrimStart('0');
                predicate = predicate.And(pd => pd.PimsPropertyDocuments.Any(p => p != null && EF.Functions.Like(p.Property.Pin.ToString(), $"%{pinValue}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.Plan))
            {
                predicate = predicate.And(pd => pd.PimsPropertyDocuments.Any(p => p != null && EF.Functions.Like(p.Property.SurveyPlanNumber.ToString(), $"%{filter.Plan}%")));
            }

            predicate = ApplyMayanIdFilter(predicate, filter);
            predicate = ApplyTemplateAndOrphanExclusions(predicate, excludeTemplates, excludeOrphans);

            predicate = predicate.And(BuildTargetAccessPredicate(accessContext));

            var query = Context.PimsDocuments.AsNoTracking()
                .Include(s => s.DocumentStatusTypeCodeNavigation)
                .Include(t => t.DocumentType)
                .Include(q => q.PimsDocumentQueues)
                    .ThenInclude(x => x.DocumentQueueStatusTypeCodeNavigation)
                .Include(x => x.PimsAcquisitionFileDocuments)
                    .ThenInclude(y => y.AcquisitionFile)
                .Include(x => x.PimsDispositionFileDocuments)
                    .ThenInclude(y => y.DispositionFile)
                .Include(x => x.PimsLeaseDocuments)
                    .ThenInclude(y => y.Lease)
                .Include(x => x.PimsManagementFileDocuments)
                    .ThenInclude(y => y.ManagementFile)
                .Include(x => x.PimsMgmtActivityDocuments)
                    .ThenInclude(y => y.ManagementActivity)
                .Include(x => x.PimsProjectDocuments)
                    .ThenInclude(y => y.Project)
                .Include(x => x.PimsPropertyDocuments)
                    .ThenInclude(y => y.Property)
                        .ThenInclude(z => z.Address)
                            .ThenInclude(a => a.Country)
                .Include(x => x.PimsResearchFileDocuments)
                    .ThenInclude(y => y.ResearchFile)
                .Where(predicate);

            return query;
        }

        private static ExpressionStarter<PimsDocument> BuildTargetAccessPredicate(DocumentAccessContext accessContext)
        {
            var userRegions = accessContext.UserRegions ?? new HashSet<short>();
            var personId = accessContext.PersonId;
            var contractorPersonId = accessContext.ContractorPersonId;
            var targetAccessPredicate = PredicateBuilder.New<PimsDocument>(doc => false);

            if (accessContext.CanViewAcquisitionFiles)
            {
                targetAccessPredicate = targetAccessPredicate.Or(AcquisitionAccessPredicate(userRegions, personId, contractorPersonId));
            }

            if (accessContext.CanViewDispositionFiles)
            {
                targetAccessPredicate = targetAccessPredicate.Or(DispositionAccessPredicate(userRegions, personId, contractorPersonId));
            }

            if (accessContext.CanViewLeases)
            {
                targetAccessPredicate = targetAccessPredicate.Or(LeaseAccessPredicate(userRegions, personId, contractorPersonId));
            }

            if (accessContext.CanViewManagementFiles)
            {
                targetAccessPredicate = targetAccessPredicate.Or(ManagementAccessPredicate(userRegions, personId, contractorPersonId));
                targetAccessPredicate = targetAccessPredicate.Or(ManagementActivityAccessPredicate(userRegions, personId, contractorPersonId));
            }

            if (accessContext.CanViewResearchFiles)
            {
                targetAccessPredicate = targetAccessPredicate.Or(ResearchAccessPredicate());
            }

            if (accessContext.CanViewProjects)
            {
                targetAccessPredicate = targetAccessPredicate.Or(ProjectAccessPredicate(userRegions, personId, contractorPersonId));
            }

            if (accessContext.CanViewProperties)
            {
                targetAccessPredicate = targetAccessPredicate.Or(PropertyAccessPredicate());
            }

            return targetAccessPredicate;
        }

        private static ExpressionStarter<PimsDocument> AcquisitionAccessPredicate(HashSet<short> userRegions, long? personId, long? contractorPersonId)
        {
            var effectivePersonId = contractorPersonId ?? personId;
            return PredicateBuilder.New<PimsDocument>(doc =>
                doc.PimsAcquisitionFileDocuments.FirstOrDefault(rel =>
                    (effectivePersonId.HasValue
                        && (rel.AcquisitionFile.PimsAcquisitionFileTeams.Any(t => t.PersonId == effectivePersonId)
                            || (rel.AcquisitionFile.Project != null
                                && rel.AcquisitionFile.Project.PimsProjectPeople.Any(p => p.PersonId == effectivePersonId))))
                    || (!contractorPersonId.HasValue
                        && (userRegions.Contains(rel.AcquisitionFile.RegionCode) || rel.AcquisitionFile.RegionCode == 4))
                ) != null);
        }

        private static ExpressionStarter<PimsDocument> DispositionAccessPredicate(HashSet<short> userRegions, long? personId, long? contractorPersonId)
        {
            var effectivePersonId = contractorPersonId ?? personId;
            return PredicateBuilder.New<PimsDocument>(doc => doc.PimsDispositionFileDocuments.FirstOrDefault(rel =>
                (effectivePersonId.HasValue && rel.DispositionFile.PimsDispositionFileTeams.Any(t => t.PersonId == effectivePersonId))
                || (!contractorPersonId.HasValue
                    && (userRegions.Contains(rel.DispositionFile.RegionCode) || rel.DispositionFile.RegionCode == 4))
                )
                != null);
        }

        private static ExpressionStarter<PimsDocument> LeaseAccessPredicate(HashSet<short> userRegions, long? personId, long? contractorPersonId)
        {
            var effectivePersonId = contractorPersonId ?? personId;
            return PredicateBuilder.New<PimsDocument>(doc => doc.PimsLeaseDocuments.FirstOrDefault(rel =>
                (effectivePersonId.HasValue
                    && (rel.Lease.PimsLeaseLicenseTeams.Any(t => t.PersonId == effectivePersonId)
                        || (rel.Lease.Project != null
                            && rel.Lease.Project.PimsProjectPeople.Any(p => p.PersonId == effectivePersonId))))
                || (!contractorPersonId.HasValue
                    && (!rel.Lease.RegionCode.HasValue || userRegions.Contains(rel.Lease.RegionCode.Value) || rel.Lease.RegionCode == 4))
                
                ) != null);
        }

        private static ExpressionStarter<PimsDocument> ManagementAccessPredicate(HashSet<short> userRegions, long? personId, long? contractorPersonId)
        {
            var effectivePersonId = contractorPersonId ?? personId;
            return PredicateBuilder.New<PimsDocument>(doc =>
                doc.PimsManagementFileDocuments.FirstOrDefault(rel =>
                    (effectivePersonId.HasValue
                        && (rel.ManagementFile.PimsManagementFileTeams.Any(t => t.PersonId == effectivePersonId)
                            || (rel.ManagementFile.Project != null
                                && rel.ManagementFile.Project.PimsProjectPeople.Any(p => p.PersonId == effectivePersonId))))
                    || (!contractorPersonId.HasValue
                        && (!rel.ManagementFile.RegionCode.HasValue || userRegions.Contains(rel.ManagementFile.RegionCode.Value) || rel.ManagementFile.RegionCode == 4))
                    
                ) != null);
        }

        private static ExpressionStarter<PimsDocument> ResearchAccessPredicate()
        {
            return PredicateBuilder.New<PimsDocument>(doc => doc.PimsResearchFileDocuments.Any());
        }

        private static ExpressionStarter<PimsDocument> ProjectAccessPredicate(HashSet<short> userRegions, long? personId, long? contractorPersonId)
        {
            var effectivePersonId = contractorPersonId ?? personId;
            return PredicateBuilder.New<PimsDocument>(doc => doc.PimsProjectDocuments.FirstOrDefault(rel =>
                (effectivePersonId.HasValue && rel.Project.PimsProjectPeople.Any(p => p.PersonId == effectivePersonId))
                || (!contractorPersonId.HasValue
                    && (userRegions.Contains(rel.Project.RegionCode) || rel.Project.RegionCode == 4))
                ) != null);
        }

        private static ExpressionStarter<PimsDocument> PropertyAccessPredicate()
        {
            return PredicateBuilder.New<PimsDocument>(doc => doc.PimsPropertyDocuments.FirstOrDefault(rel => rel.Property != null) != null);
        }

        private static ExpressionStarter<PimsDocument> ManagementActivityAccessPredicate(HashSet<short> userRegions, long? personId, long? contractorPersonId)
        {
            var effectivePersonId = contractorPersonId ?? personId;
            return PredicateBuilder.New<PimsDocument>(doc => doc.PimsMgmtActivityDocuments.FirstOrDefault(rel =>
                rel.ManagementActivity != null
                && ((rel.ManagementActivity.ManagementFile != null
                        && ((effectivePersonId.HasValue
                                && (rel.ManagementActivity.ManagementFile.PimsManagementFileTeams.Any(t => t.PersonId == effectivePersonId)
                                    || (rel.ManagementActivity.ManagementFile.Project != null
                                        && rel.ManagementActivity.ManagementFile.Project.PimsProjectPeople.Any(p => p.PersonId == effectivePersonId))))
                            || (!contractorPersonId.HasValue
                                && (!rel.ManagementActivity.ManagementFile.RegionCode.HasValue
                                    || userRegions.Contains(rel.ManagementActivity.ManagementFile.RegionCode.Value)
                                    || rel.ManagementActivity.ManagementFile.RegionCode == 4))))
                    || (rel.ManagementActivity.ManagementFile == null
                        && rel.ManagementActivity.PimsManagementActivityProperties.Any(map => map.Property != null
                            && userRegions.Contains(map.Property.RegionCode))
                        && !contractorPersonId.HasValue))
                ) != null);
        }

        private static ExpressionStarter<PimsDocument> ApplyMayanIdFilter(ExpressionStarter<PimsDocument> predicate, DocumentSearchFilterModel filter)
        {
            if (filter.MayanDocumentIds?.Length > 0)
            {
                predicate = predicate.And(pd => pd.MayanId.HasValue && filter.MayanDocumentIds.Contains(pd.MayanId.Value));
            }

            return predicate;
        }

        private ExpressionStarter<PimsDocument> ApplyTemplateAndOrphanExclusions(ExpressionStarter<PimsDocument> predicate, bool excludeTemplates, bool excludeOrphans)
        {
            if (excludeTemplates)
            {
                var templateCodeType = Context.PimsDocumentTyps.FirstOrDefault(x => x.DocumentType == "CDOGTEMP");
                if (templateCodeType != null)
                {
                    predicate = predicate.And(x => x.DocumentTypeId != templateCodeType.DocumentTypeId);
                }
            }

            if (excludeOrphans)
            {
                predicate = predicate.And(x => x.PimsAcquisitionFileDocuments.Count > 0 || x.PimsDispositionFileDocuments.Count > 0 || x.PimsLeaseDocuments.Count > 0
                                            || x.PimsManagementFileDocuments.Count > 0 || x.PimsMgmtActivityDocuments.Count > 0 || x.PimsProjectDocuments.Count > 0
                                            || x.PimsPropertyDocuments.Count > 0 || x.PimsResearchFileDocuments.Count > 0);
            }

            return predicate;
        }

        #endregion
    }
}
