using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Pims.Api.Models.CodeTypes;
using Pims.Core.Exceptions;
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "acquisition")]
    [ExcludeFromCodeCoverage]
    public class CompensationRequisitionRepositoryTest
    {
        // xUnit.net creates a new instance of the test class for every test that is run,
        // so any code which is placed into the constructor of the test class will be run for every single test.
        private readonly TestHelper _helper;

        public CompensationRequisitionRepositoryTest()
        {
            this._helper = new TestHelper();
        }

        private CompensationRequisitionRepository CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.CreateRepository<CompensationRequisitionRepository>(user);
        }

        [Fact]
        public void GetAllByAcquisitionFileId_Success()
        {
            // Arrange
            var compReq = new PimsCompensationRequisition()
            {
                IsDraft = true,
                AcquisitionFileId = 1,
            };

            var repository = CreateWithPermissions(Permissions.CompensationRequisitionView);
            _helper.AddAndSaveChanges(compReq);

            // Act
            var result = repository.GetAllByAcquisitionFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsCompensationRequisition>>();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetAllByLeaseFileId_Success()
        {
            // Arrange
            var compReq = new PimsCompensationRequisition()
            {
                IsDraft = true,
                LeaseId = 1,
            };

            var repository = CreateWithPermissions(Permissions.CompensationRequisitionView);
            _helper.AddAndSaveChanges(compReq);

            // Act
            var result = repository.GetAllByLeaseFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsCompensationRequisition>>();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void AddCompensationRequisition_Success()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.LeaseEdit);

            // Act
            var result = repository.Add(EntityHelper.CreateCompensationRequisition());

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsCompensationRequisition>();
        }

        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var compReq = EntityHelper.CreateCompensationRequisition(id: 1);
            compReq.AcquisitionFileId = 100;

            var repository = this.CreateWithPermissions(Permissions.CompensationRequisitionView);
            _helper.AddAndSaveChanges(compReq);

            // Act
            var result = repository.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsCompensationRequisition>();
            result.Internal_Id.Should().Be(1);
        }

        [Fact]
        public void GetById_KeyNotFoundException()
        {
            // Arrange
            var compReq = EntityHelper.CreateCompensationRequisition(id: 1);
            compReq.AcquisitionFileId = 100;

            var repository = CreateWithPermissions(Permissions.CompensationRequisitionView);
            _helper.AddAndSaveChanges(compReq);

            // Act
            Action act = () => repository.GetById(2);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void UpdateCompensationRequisitionAcq_Success()
        {
            // Arrange
            var compReq = EntityHelper.CreateCompensationRequisition(id: 1);
            compReq.AcquisitionFileId = 100;
            compReq.PimsCompReqAcqPayees = new List<PimsCompReqAcqPayee>
            {
                new()
                {
                    AcquisitionOwnerId = 1,
                    CompensationRequisitionId = 1,
                    Internal_Id = 1,

                }
            };

            var repository = CreateWithPermissions(Permissions.CompensationRequisitionEdit);
            _helper.AddAndSaveChanges(compReq);

            var updatedCompReq = EntityHelper.CreateCompensationRequisition(id: 1);
            updatedCompReq.AcquisitionFileId = compReq.AcquisitionFileId;
            updatedCompReq.FiscalYear = "2030/2031";
            updatedCompReq.PimsCompReqAcqPayees = new List<PimsCompReqAcqPayee>
            {
                new()
                {
                    AcquisitionOwnerId = 1,
                    CompensationRequisitionId = 1,
                    Internal_Id = 1,
                }
            };

            // Act
            var result = repository.Update(updatedCompReq);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsCompensationRequisition>();
            result.FiscalYear.Should().Be("2030/2031");
            result.PimsCompReqAcqPayees.First().AcquisitionOwnerId.Should().Be(1);
        }

        [Fact]
        public void UpdateCompensationRequisitionLease_Success()
        {
            // Arrange
            var compReq = EntityHelper.CreateCompensationRequisition(id: 1);
            compReq.AcquisitionFileId = 100;
            compReq.PimsCompReqLeasePayees = new List<PimsCompReqLeasePayee>
            {
                new()
                {
                    LeaseStakeholderId = 1,
                    CompensationRequisitionId = 1,
                    Internal_Id = 1,

                }
            };

            var repository = CreateWithPermissions(Permissions.CompensationRequisitionEdit);
            _helper.AddAndSaveChanges(compReq);

            var updatedCompReq = EntityHelper.CreateCompensationRequisition(id: 1);
            updatedCompReq.AcquisitionFileId = compReq.AcquisitionFileId;
            updatedCompReq.FiscalYear = "2030/2031";
            updatedCompReq.PimsCompReqLeasePayees = new List<PimsCompReqLeasePayee>
            {
                new()
                {
                    LeaseStakeholderId = 1,
                    CompensationRequisitionId = 1,
                    Internal_Id = 1,

                }
            };

            // Act
            var result = repository.Update(updatedCompReq);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsCompensationRequisition>();
            result.FiscalYear.Should().Be("2030/2031");
            result.PimsCompReqLeasePayees.First().LeaseStakeholderId.Should().Be(1);
        }

        [Fact]
        public void UpdateCompensationRequisition_KeyNotFoundException()
        {
            // Arrange
            var updatedCompReq = EntityHelper.CreateCompensationRequisition(id: 1);
            updatedCompReq.AcquisitionFileId = 100;
            updatedCompReq.FiscalYear = "2030/2031";

            var repository = CreateWithPermissions(Permissions.CompensationRequisitionEdit);

            // Act
            Action act = () => repository.Update(updatedCompReq);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void DeleteCompensationRequisition_Success()
        {
            // Arrange
            var compReq = EntityHelper.CreateCompensationRequisition(id: 1);
            compReq.AcquisitionFileId = 100;

            var repository = CreateWithPermissions(Permissions.CompensationRequisitionDelete);
            var context = _helper.AddAndSaveChanges(compReq);

            // Act
            context.ChangeTracker.Clear();
            var result = repository.TryDelete(compReq.Internal_Id);
            context.CommitTransaction();

            // Assert
            result.Should().BeTrue();
            context.PimsCompensationRequisitions.Should().BeEmpty();
        }

        [Fact]
        public void DeleteCompensationRequisition_NotFound()
        {
            // Arrange
            var repository = CreateWithPermissions(Permissions.CompensationRequisitionDelete);

            // Act
            var result = repository.TryDelete(1);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void DeleteCompensationRequisition_CascadeDelete_SubEntities_ACQUISITION()
        {
            // Arrange
            var compReq = EntityHelper.CreateCompensationRequisition(id: 1);
            compReq.AcquisitionFileId = 1;
            compReq.PimsCompReqFinancials.Add(
                new()
                {
                    Internal_Id = 1,
                    CompensationRequisitionId = 1,
                    FinancialActivityCodeId = 100,
                    FinancialActivityCode = new()
                    {
                        Id = 100,
                        Code = "TEST",
                        Description = "Lorem Ipsum"
                    }
                });
            compReq.PimsPropAcqFlCompReqs.Add(
                new()
                {
                    Internal_Id = 1,
                    CompensationRequisitionId = 1,
                    PropertyAcquisitionFileId = 1,
                    PropertyAcquisitionFile = new()
                    {
                        AcquisitionFileId = 1,
                        PropertyId = 12345,
                        Property = EntityHelper.CreateProperty(12345),
                    }
                });

            compReq.PimsCompReqAcqPayees.Add(
                new()
                {
                    Internal_Id = 1,
                    CompensationRequisitionId = 1,
                    AcquisitionOwnerId = 1
                });

            var repository = CreateWithPermissions(Permissions.CompensationRequisitionDelete);
            var context = _helper.AddAndSaveChanges(compReq);

            // Act
            context.ChangeTracker.Clear();
            var result = repository.TryDelete(compReq.Internal_Id);
            context.CommitTransaction();

            // Assert
            result.Should().BeTrue();
            context.PimsCompensationRequisitions.Should().BeEmpty();
            context.PimsCompReqFinancials.Should().BeEmpty();
            context.PimsPropAcqFlCompReqs.Should().BeEmpty();
            context.PimsCompReqAcqPayees.Should().BeEmpty();
        }

        [Fact]
        public void DeleteCompensationRequisition_CascadeDelete_SubEntities_LEASE()
        {
            // Arrange
            var compReq = EntityHelper.CreateCompensationRequisition(id: 1);
            compReq.LeaseId = 1;
            compReq.PimsCompReqFinancials.Add(
                new()
                {
                    Internal_Id = 1,
                    CompensationRequisitionId = 1,
                    FinancialActivityCodeId = 100,
                    FinancialActivityCode = new()
                    {
                        Id = 100,
                        Code = "TEST",
                        Description = "Lorem Ipsum"
                    }
                });
            compReq.PimsPropLeaseCompReqs.Add(
                new()
                {
                    Internal_Id = 1,
                    CompensationRequisitionId = 1,
                    PropertyLeaseId = 1,
                    PropertyLease = new()
                    {
                        LeaseId = 1,
                        PropertyId = 12345,
                        Property = EntityHelper.CreateProperty(12345),
                    }
                });

            compReq.PimsCompReqLeasePayees.Add(
                new()
                {
                    Internal_Id = 1,
                    CompensationRequisitionId = 1,
                    LeaseStakeholderId = 1
                });

            var repository = CreateWithPermissions(Permissions.CompensationRequisitionDelete);
            var context = _helper.AddAndSaveChanges(compReq);

            // Act
            context.ChangeTracker.Clear();
            var result = repository.TryDelete(compReq.Internal_Id);
            context.CommitTransaction();

            // Assert
            result.Should().BeTrue();
            context.PimsCompensationRequisitions.Should().BeEmpty();
            context.PimsCompReqFinancials.Should().BeEmpty();
            context.PimsPropLeaseCompReqs.Should().BeEmpty();
            context.PimsCompReqLeasePayees.Should().BeEmpty();
        }

        [Fact]
        public void GetAcquisitionCompReqPropertiesById_Success()
        {
            // Arrange
            var acquisitionCompReqProperty = new PimsPropAcqFlCompReq()
            {
                Internal_Id = 1,
                CompensationRequisitionId = 1,
                PropertyAcquisitionFileId = 1,
                PropertyAcquisitionFile = new()
                {
                    AcquisitionFileId = 1,
                    PropertyId = 12345,
                    Property = EntityHelper.CreateProperty(12345),
                }
            };

            var repository = CreateWithPermissions(Permissions.CompensationRequisitionView);
            _helper.AddAndSaveChanges(acquisitionCompReqProperty);

            // Act
            var result = repository.GetAcquisitionCompReqPropertiesById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsPropertyAcquisitionFile>>();
            result.Should().HaveCount(1);
            result.First().Property.Pid.Should().Be(12345);
            result.First().Property.Location.SRID.Should().Be(SpatialReference.BCALBERS); // repository should return property location in BCALBERS projection
        }

        [Fact]
        public void GetLeaseCompReqPropertiesById_Success()
        {
            // Arrange
            var leaseCompReqProperty = new PimsPropLeaseCompReq()
            {
                Internal_Id = 1,
                CompensationRequisitionId = 1,
                PropertyLeaseId = 1,
                PropertyLease = new()
                {
                    LeaseId = 1,
                    PropertyId = 12345,
                    Property = EntityHelper.CreateProperty(12345),
                }
            };

            var repository = CreateWithPermissions(Permissions.CompensationRequisitionView);
            _helper.AddAndSaveChanges(leaseCompReqProperty);

            // Act
            var result = repository.GetLeaseCompReqPropertiesById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsPropertyLease>>();
            result.Should().HaveCount(1);
            result.First().Property.Pid.Should().Be(12345);
            result.First().Property.Location.SRID.Should().Be(SpatialReference.BCALBERS); // repository should return property location in BCALBERS projection
        }

        [Fact]
        public void GetCompensationRequisitionFinancials_Success()
        {
            // Arrange
            PimsCompReqFinancial[] financials =
            {
                new ()
                {
                    Internal_Id = 1,
                    CompensationRequisitionId = 1,
                    FinancialActivityCodeId = 100,
                },
                new ()
                {
                    Internal_Id = 2,
                    CompensationRequisitionId = 1,
                    FinancialActivityCodeId = 100,
                },
            };

            var repository = CreateWithPermissions(Permissions.CompensationRequisitionView);
            _helper.AddAndSaveChanges<PimsCompReqFinancial>(financials);
            _helper.AddAndSaveChanges<PimsFinancialActivityCode>(
                new PimsFinancialActivityCode()
                {
                    Id = 100,
                    Code = "TEST",
                    Description = "lorem ipsum"
                });

            // Act
            var result = repository.GetCompensationRequisitionFinancials(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsCompReqFinancial>>();
            result.Should().HaveCount(2);
            result.First().CompensationRequisitionId.Should().Be(1);
        }

        [Fact]
        public void GetCompensationRequisitionPayees_Success()
        {
            // Arrange
            PimsCompReqAcqPayee[] payees =
            {
                new ()
                {
                    Internal_Id = 1,
                    CompensationRequisitionId = 1,
                },
                new ()
                {
                    Internal_Id = 2,
                    CompensationRequisitionId = 1,
                },
            };

            var repository = CreateWithPermissions(Permissions.CompensationRequisitionView);
            _helper.AddAndSaveChanges<PimsCompReqAcqPayee>(payees);

            // Act
            var result = repository.GetCompensationRequisitionAcquisitionPayees(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsCompReqAcqPayee>>();
            result.Should().HaveCount(2);
            result.First().CompensationRequisitionId.Should().Be(1);
        }

        [Fact]
        public void GetCompensationRequisitionLeasePayees_Success()
        {
            // Arrange
            PimsCompReqLeasePayee[] payees =
            {
                new ()
                {
                    Internal_Id = 1,
                    CompensationRequisitionId = 1,
                },
                new ()
                {
                    Internal_Id = 2,
                    CompensationRequisitionId = 1,
                },
            };

            var repository = CreateWithPermissions(Permissions.CompensationRequisitionView);
            _helper.AddAndSaveChanges<PimsCompReqLeasePayee>(payees);

            // Act
            var result = repository.GetCompensationRequisitionLeasePayees(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsCompReqLeasePayee>>();
            result.Should().HaveCount(2);
            result.First().CompensationRequisitionId.Should().Be(1);
        }
    }
}
