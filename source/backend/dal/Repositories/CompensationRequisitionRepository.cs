using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Security.Claims;
using LinqKit;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with compensation requisitions within the datasource.
    /// </summary>
    public class CompensationRequisitionRepository
        : BaseRepository<PimsCompensationRequisition>,
            ICompensationRequisitionRepository
    {
        private readonly IMapper _mapper;

        #region Constructors

        /// <summary>
        /// Creates a new instance of a CompensationRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public CompensationRequisitionRepository(
            PimsContext dbContext,
            ClaimsPrincipal user,
            ILogger<BaseRepository> logger,
            IMapper mapper
        )
            : base(dbContext, user, logger)
        {
            _mapper = mapper;
        }
        #endregion

        public IList<PimsCompensationRequisition> GetAllByAcquisitionFileId(long acquisitionFileId)
        {
            return Context
                .PimsCompensationRequisitions.Include(c => c.PimsCompReqFinancials)
                .Include(p => p.PimsCompReqAcqPayees)
                .AsNoTracking()
                .Where(c => c.AcquisitionFileId == acquisitionFileId)
                .ToList();
        }

        public IList<PimsCompensationRequisition> GetAllByLeaseFileId(long leaseFileId)
        {
            return Context
                .PimsCompensationRequisitions.Include(c => c.PimsCompReqFinancials)
                .Include(c => c.PimsCompReqLeasePayees)
                .AsNoTracking()
                .Where(c => c.LeaseId == leaseFileId)
                .ToList();
        }

        public PimsCompensationRequisition Add(PimsCompensationRequisition compensationRequisition)
        {
            Context.PimsCompensationRequisitions.Add(compensationRequisition);

            return compensationRequisition;
        }

        public PimsCompensationRequisition GetById(long compensationRequisitionId)
        {
            var entity =
                Context
                    .PimsCompensationRequisitions.Include(x => x.YearlyFinancial)
                    .Include(x => x.ChartOfAccounts)
                    .Include(x => x.Responsibility)
                    .Include(c => c.PimsCompReqFinancials)
                    .ThenInclude(y => y.FinancialActivityCode)
                    .Include(c => c.PimsCompReqAcqPayees)
                    .Include(x => x.AlternateProject)
                    .Include(x => x.PimsCompReqLeasePayees)
                    .ThenInclude(y => y.LeaseStakeholder)
                    .ThenInclude(z => z.LeaseStakeholderTypeCodeNavigation)
                    .Include(x => x.PimsCompReqLeasePayees)
                    .ThenInclude(y => y.LeaseStakeholder)
                    .ThenInclude(z => z.LessorTypeCodeNavigation)
                    .Include(x => x.PimsPropAcqFlCompReqs)
                    .ThenInclude(y => y.PropertyAcquisitionFile)
                    .ThenInclude(z => z.Property)
                    .Include(x => x.PimsPropLeaseCompReqs)
                    .ThenInclude(y => y.PropertyLease)
                    .ThenInclude(z => z.Property)
                    .AsNoTracking()
                    .FirstOrDefault(x =>
                        x.CompensationRequisitionId.Equals(compensationRequisitionId)
                    ) ?? throw new KeyNotFoundException();

            return entity;
        }

        public PimsCompensationRequisition Update(
            PimsCompensationRequisition compensationRequisition
        )
        {
            var existingCompensationRequisition =
                Context.PimsCompensationRequisitions.FirstOrDefault(x =>
                    x.CompensationRequisitionId.Equals(
                        compensationRequisition.CompensationRequisitionId
                    )
                ) ?? throw new KeyNotFoundException();

            Context
                .Entry(existingCompensationRequisition)
                .CurrentValues.SetValues(compensationRequisition);
            Context.UpdateChild<PimsCompensationRequisition, long, PimsCompReqFinancial, long>(
                a => a.PimsCompReqFinancials,
                compensationRequisition.CompensationRequisitionId,
                compensationRequisition.PimsCompReqFinancials.ToArray(),
                true
            );
            Context.UpdateChild<PimsCompensationRequisition, long, PimsPropAcqFlCompReq, long>(
                a => a.PimsPropAcqFlCompReqs,
                compensationRequisition.CompensationRequisitionId,
                compensationRequisition.PimsPropAcqFlCompReqs.ToArray(),
                true
            );
            Context.UpdateChild<PimsCompensationRequisition, long, PimsPropLeaseCompReq, long>(
                a => a.PimsPropLeaseCompReqs,
                compensationRequisition.CompensationRequisitionId,
                compensationRequisition.PimsPropLeaseCompReqs.ToArray(),
                true
            );
            Context.UpdateChild<PimsCompensationRequisition, long, PimsCompReqLeasePayee, long>(
                a => a.PimsCompReqLeasePayees,
                compensationRequisition.CompensationRequisitionId,
                compensationRequisition.PimsCompReqLeasePayees.ToArray(),
                true
            );
            Context.UpdateChild<PimsCompensationRequisition, long, PimsCompReqAcqPayee, long>(
                a => a.PimsCompReqAcqPayees,
                compensationRequisition.CompensationRequisitionId,
                compensationRequisition.PimsCompReqAcqPayees.ToArray(),
                true
            );

            return compensationRequisition;
        }

        public bool TryDelete(long compensationId)
        {
            var deletedEntity = Context
                .PimsCompensationRequisitions.Include(fa => fa.PimsCompReqFinancials)
                .Include(p => p.PimsPropAcqFlCompReqs)
                .Include(l => l.PimsPropLeaseCompReqs)
                .Include(s => s.PimsCompReqLeasePayees)
                .Include(ap => ap.PimsCompReqAcqPayees)
                .AsNoTracking()
                .FirstOrDefault(c => c.CompensationRequisitionId == compensationId);

            if (deletedEntity != null)
            {
                foreach (var financial in deletedEntity.PimsCompReqFinancials)
                {
                    Context.PimsCompReqFinancials.Remove(
                        new PimsCompReqFinancial()
                        {
                            CompReqFinancialId = financial.CompReqFinancialId,
                        }
                    );
                }

                foreach (var propAcqFile in deletedEntity.PimsPropAcqFlCompReqs)
                {
                    Context.PimsPropAcqFlCompReqs.Remove(
                        new PimsPropAcqFlCompReq()
                        {
                            PropAcqFlCompReqId = propAcqFile.PropAcqFlCompReqId,
                        }
                    );
                }

                foreach (var propLeaseFile in deletedEntity.PimsPropLeaseCompReqs)
                {
                    Context.PimsPropLeaseCompReqs.Remove(
                        new PimsPropLeaseCompReq()
                        {
                            PropLeaseCompReqId = propLeaseFile.PropLeaseCompReqId,
                        }
                    );
                }

                foreach (var compReqLeaseStakeholder in deletedEntity.PimsCompReqLeasePayees)
                {
                    Context.PimsCompReqLeasePayees.Remove(
                        new PimsCompReqLeasePayee()
                        {
                            CompReqLeasePayeeId = compReqLeaseStakeholder.CompReqLeasePayeeId,
                        }
                    );
                }

                foreach (var compReqAcqPayee in deletedEntity.PimsCompReqAcqPayees)
                {
                    Context.PimsCompReqAcqPayees.Remove(
                        new() { CompReqAcqPayeeId = compReqAcqPayee.CompReqAcqPayeeId }
                    );
                }

                Context.CommitTransaction(); // TODO: required to enforce delete order. Can be removed when cascade deletes are implemented.

                Context.PimsCompensationRequisitions.Remove(
                    new PimsCompensationRequisition()
                    {
                        CompensationRequisitionId = deletedEntity.CompensationRequisitionId,
                    }
                );
                return true;
            }
            return false;
        }

        public List<PimsPropertyAcquisitionFile> GetAcquisitionCompReqPropertiesById(
            long compensationRequisitionId
        )
        {
            return Context
                .PimsPropAcqFlCompReqs.Where(x =>
                    x.CompensationRequisitionId == compensationRequisitionId
                )
                .Include(pa => pa.PropertyAcquisitionFile)
                .ThenInclude(p => p.Property)
                .ThenInclude(rp => rp.RegionCodeNavigation)
                .Include(pa => pa.PropertyAcquisitionFile)
                .ThenInclude(p => p.Property)
                .ThenInclude(rp => rp.DistrictCodeNavigation)
                .Include(pa => pa.PropertyAcquisitionFile)
                .ThenInclude(p => p.Property)
                .ThenInclude(rp => rp.Address)
                .ThenInclude(a => a.Country)
                .Include(pa => pa.PropertyAcquisitionFile)
                .ThenInclude(p => p.Property)
                .ThenInclude(rp => rp.Address)
                .ThenInclude(a => a.Country)
                .AsNoTracking()
                .Select(x => x.PropertyAcquisitionFile)
                .ToList();
        }

        public List<PimsPropertyLease> GetLeaseCompReqPropertiesById(long compensationRequisitionId)
        {
            return Context
                .PimsPropLeaseCompReqs.Where(x =>
                    x.CompensationRequisitionId == compensationRequisitionId
                )
                .Include(l => l.PropertyLease)
                .ThenInclude(p => p.Property)
                .ThenInclude(rp => rp.RegionCodeNavigation)
                .Include(pa => pa.PropertyLease)
                .ThenInclude(p => p.Property)
                .ThenInclude(rp => rp.DistrictCodeNavigation)
                .Include(pa => pa.PropertyLease)
                .ThenInclude(p => p.Property)
                .ThenInclude(rp => rp.Address)
                .ThenInclude(a => a.Country)
                .Include(pa => pa.PropertyLease)
                .ThenInclude(p => p.Property)
                .ThenInclude(rp => rp.Address)
                .ThenInclude(a => a.Country)
                .AsNoTracking()
                .Select(x => x.PropertyLease)
                .ToList();
        }

        public IEnumerable<PimsCompReqFinancial> GetCompensationRequisitionFinancials(
            long compReqId
        )
        {
            return Context
                .PimsCompReqFinancials.AsNoTracking()
                .Include(y => y.FinancialActivityCode)
                .Where(x => x.CompensationRequisitionId == compReqId)
                .ToList();
        }

        public IEnumerable<PimsCompReqAcqPayee> GetCompensationRequisitionAcquisitionPayees(
            long compReqId
        )
        {
            return Context
                .PimsCompReqAcqPayees.AsNoTracking()
                .Include(x => x.AcquisitionOwner)
                .Include(x => x.AcquisitionFileTeam)
                .ThenInclude(y => y.Person)
                .Include(x => x.AcquisitionFileTeam)
                .ThenInclude(y => y.Organization)
                .Include(x => x.InterestHolder)
                .ThenInclude(y => y.InterestHolderTypeCodeNavigation)
                .Include(x => x.InterestHolder)
                .ThenInclude(y => y.Person)
                .Include(x => x.InterestHolder)
                .ThenInclude(y => y.Organization)
                .Where(x => x.CompensationRequisitionId == compReqId)
                .ToList();
        }

        public IEnumerable<PimsCompReqLeasePayee> GetCompensationRequisitionLeasePayees(
            long compReqId
        )
        {
            return Context
                .PimsCompReqLeasePayees.AsNoTracking()
                .Include(x => x.LeaseStakeholder)
                .ThenInclude(y => y.Person)
                .Include(x => x.LeaseStakeholder)
                .ThenInclude(y => y.Organization)
                .Include(x => x.LeaseStakeholder)
                .ThenInclude(y => y.LeaseStakeholderTypeCodeNavigation)
                .Include(x => x.LeaseStakeholder)
                .ThenInclude(y => y.LessorTypeCodeNavigation)
                .Where(x => x.CompensationRequisitionId == compReqId)
                .ToList();
        }

        public PimsCompensationRequisition GetCompensationRequisitionAtTime(
            long compReqId,
            DateTime time
        )
        {
            //TODO:
            Console.WriteLine("Here we are");
            Console.WriteLine($"compReqId: {compReqId} time:{time}");

            var compreqHist = Context
                .PimsCompensationRequisitionHists.AsNoTracking()
                .Where(crh => crh.CompensationRequisitionId == compReqId)
                .Where(crh => crh.EffectiveDateHist <= time)
                .OrderByDescending(a => a.EffectiveDateHist)
                .FirstOrDefault();

            var compreq = _mapper.Map<PimsCompensationRequisition>(compreqHist);

            // Retrieve financial information
            var financialsHist = Context
                .PimsCompReqFinancialHists.AsNoTracking()
                .Where(crfh => crfh.CompensationRequisitionId == compReqId)
                .Where(crfh => crfh.EffectiveDateHist <= time)
                .GroupBy(crfh => crfh.CompReqFinancialId)
                .Select(gcrfh => gcrfh.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault())
                .ToList();

            compreq.PimsCompReqFinancials = _mapper.Map<ICollection<PimsCompReqFinancial>>(
                financialsHist
            );

            return compreq;
        }

        public IEnumerable<PimsPropertyAcquisitionFile> GetCompensationRequisitionPropertiesAtTime(
            long compReqId,
            DateTime time
        )
        {
            var acqCompReqPropHist = Context
                .PimsPropAcqFlCompReqHists.AsNoTracking()
                .Where(pacr => pacr.CompensationRequisitionId == compReqId)
                .Where(pacr => pacr.EffectiveDateHist <= time)
                .GroupBy(pacr => pacr.PropertyAcquisitionFileId)
                .Select(gpacr => gpacr.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault())
                .ToList();

            List<PimsPropertyAcquisitionFile> acqfileProperties =
                new List<PimsPropertyAcquisitionFile>();

            foreach (var prop in acqCompReqPropHist)
            {
                var acqPropHist = Context
                    .PimsPropertyAcquisitionFileHists.AsNoTracking()
                    .Where(afp => afp.PropertyAcquisitionFileId == prop.PropertyAcquisitionFileId)
                    .Where(afp => afp.EffectiveDateHist <= time)
                    .OrderByDescending(a => a.EffectiveDateHist)
                    .FirstOrDefault();

                var propAcFile = _mapper.Map<PimsPropertyAcquisitionFile>(acqPropHist);

                var propHist = Context
                    .PimsPropertyHists.AsNoTracking()
                    .Where(ph => ph.PropertyId == acqPropHist.PropertyId)
                    .Where(ph => ph.EffectiveDateHist <= time)
                    .GroupBy(ph => ph.PropertyId)
                    .Select(gph => gph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault())
                    .ToList();

                propAcFile.Property = _mapper.Map<PimsProperty>(propHist);

                acqfileProperties.Add(propAcFile);
            }

            return acqfileProperties;
        }

        public IEnumerable<PimsCompReqAcqPayee> GetCompensationRequisitionAcquisitionPayeesAtTime(
            long compReqId,
            DateTime time
        )
        {
            var acqPayeeHist = Context
                .PimsCompReqAcqPayeeHists.AsNoTracking()
                .Where(pacr => pacr.CompensationRequisitionId == compReqId)
                .Where(pacr =>
                    pacr.EffectiveDateHist <= time
                    && (pacr.EndDateHist == null || pacr.EndDateHist > time)
                )
                .GroupBy(pacr => pacr.CompReqAcqPayeeId)
                .Select(gpacr => gpacr.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault())
                .ToList();

            var acqPayee = _mapper.Map<ICollection<PimsCompReqAcqPayee>>(acqPayeeHist);

            foreach (var payee in acqPayee)
            {
                if (payee.AcquisitionOwnerId != null)
                {
                    var acqOwnerHist = Context
                        .PimsAcquisitionOwnerHists.AsNoTracking()
                        .Where(aoh => aoh.AcquisitionOwnerId == payee.AcquisitionOwnerId)
                        .Where(aoh => aoh.EffectiveDateHist <= time)
                        .GroupBy(aoh => aoh.AcquisitionOwnerId)
                        .Select(gaoh =>
                            gaoh.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()
                        )
                        .FirstOrDefault();

                    payee.AcquisitionOwner = _mapper.Map<PimsAcquisitionOwner>(acqOwnerHist);
                }

                if (payee.InterestHolderId != null)
                {
                    var interestHolderHist = Context
                        .PimsInterestHolderHists.AsNoTracking()
                        .Where(ihh => ihh.InterestHolderId == payee.InterestHolderId)
                        .Where(ihh => ihh.EffectiveDateHist <= time)
                        .GroupBy(ihh => ihh.InterestHolderId)
                        .Select(gihh =>
                            gihh.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()
                        )
                        .FirstOrDefault();

                    var interestHolder = _mapper.Map<PimsInterestHolder>(interestHolderHist);
                    if (interestHolder.OrganizationId != null)
                    {
                        var organizationHist = Context
                            .PimsOrganizationHists.AsNoTracking()
                            .Where(aoh => aoh.OrganizationId == interestHolder.OrganizationId)
                            .Where(aoh => aoh.EffectiveDateHist <= time)
                            .GroupBy(aoh => aoh.OrganizationId)
                            .Select(gaoh =>
                                gaoh.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()
                            )
                            .FirstOrDefault();

                        interestHolder.Organization = _mapper.Map<PimsOrganization>(
                            organizationHist
                        );
                    }
                    if (interestHolder.PersonId != null)
                    {
                        var personHist = Context
                            .PimsPersonHists.AsNoTracking()
                            .Where(aoh => aoh.PersonId == interestHolder.PersonId)
                            .Where(aoh => aoh.EffectiveDateHist <= time)
                            .GroupBy(aoh => aoh.PersonId)
                            .Select(gaoh =>
                                gaoh.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()
                            )
                            .FirstOrDefault();

                        interestHolder.Person = _mapper.Map<PimsPerson>(personHist);
                    }
                    payee.InterestHolder = interestHolder;
                }
            }

            return acqPayee;

            //TODO...
            //retrieve relationship tables
            /*
                .Include(x => x.AcquisitionOwner)
                .Include(x => x.AcquisitionFileTeam)
                    .ThenInclude(y => y.Person)
                    .ThenInclude(y => y.Organization)
                .Include(x => x.InterestHolder)
                    .ThenInclude(y => y.InterestHolderTypeCodeNavigation) | This might not be necessary
                    .ThenInclude(y => y.Person)
                    .ThenInclude(y => y.Organization)
                    */

            List<PimsPropertyAcquisitionFile> acqfileProperties =
                new List<PimsPropertyAcquisitionFile>();

            return null;
        }
    }
}
