using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "disposition")]
    [ExcludeFromCodeCoverage]
    public class DispositionFileRepositoryTest
    {
        private readonly TestHelper _helper;

        #region Constructors
        public DispositionFileRepositoryTest()
        {
            _helper = new TestHelper();
        }
        #endregion

        private DispositionFileRepository CreateRepositoryWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.CreateRepository<DispositionFileRepository>(user);
        }

        #region Tests

        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionAdd);
            var dispositionFile = EntityHelper.CreateDispositionFile();

            var repository = helper.CreateRepository<DispositionFileRepository>(user);

            var mockSequenceRepo = new Mock<ISequenceRepository>();
            mockSequenceRepo.Setup(x => x.GetNextSequenceValue(It.IsAny<string>())).Returns(100);

            // Act
            var result = repository.Add(dispositionFile);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionFile>();
            result.DispositionFileId.Should().Be(1);
            result.FileNumber.Equals("D-100");
        }

        [Fact]
        public void Add_WithRetiredProperty_Should_Fail()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionAdd);
            var dispositionFile = EntityHelper.CreateDispositionFile();

            dispositionFile.PimsDispositionFileProperties = new List<PimsDispositionFileProperty>()
            {
                new PimsDispositionFileProperty()
                {
                    PropertyId = 100,
                    Property = new PimsProperty()
                    {
                        IsRetired = true,
                    }
                },
            };

            var repository = helper.CreateRepository<DispositionFileRepository>(user);

            // Act
            Action act = () => repository.Add(dispositionFile);

            // Assert
            var ex = act.Should().Throw<BusinessRuleViolationException>();
            ex.WithMessage("Retired property can not be selected.");
        }
        #endregion

        #region Update

        [Fact]
        public void Update_Success()
        {
            // Arrange
            var dispositionFile = EntityHelper.CreateDispositionFile();

            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            _helper.AddAndSaveChanges(dispositionFile);

            // Act
            var result = repository.Update(dispositionFile.Internal_Id, dispositionFile);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionFile>();
            result.DispositionFileId.Should().Be(1);
        }

        [Fact]
        public void Update_KeyNotFound()
        {
            // Arrange
            var dispositionFile = EntityHelper.CreateDispositionFile();

            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);

            // Act
            Action act = () => repository.Update(dispositionFile.Internal_Id, dispositionFile);

            // Assert

            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionFile>();
            result.FileName.Should().Be("Test Disposition File");
            result.DispositionFileId.Should().Be(dispFile.DispositionFileId);
        }

        [Fact]
        public void GetById_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);

            // Act
            Action act = () => repository.GetById(1);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region GetRegion
        [Fact]
        public void GetRegion_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetRegion(1);

            // Assert
            result.Should().Be(1);
        }
        #endregion

        #region GetDispositionFileSale
        [Fact]
        public void GetDispositionFileSale_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionSales = new List<PimsDispositionSale>() { new PimsDispositionSale() };
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetDispositionFileSale(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionSale>();
        }
        #endregion

        #region GetTeamMembers
        [Fact]
        public void GetTeamMembers_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionFileTeams = new List<PimsDispositionFileTeam>() { new PimsDispositionFileTeam() { DspFlTeamProfileTypeCode = "PROPCOORD" } };
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetTeamMembers();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsDispositionFileTeam>>();
            result.Should().HaveCount(1);
        }
        #endregion

        #region GetDispositionOffers
        [Fact]
        public void GetDispositionOffers_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionOffers = new List<PimsDispositionOffer>() { new PimsDispositionOffer() { OfferName = "offer" } };
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetDispositionOffers(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsDispositionOffer>>();
            result.Should().HaveCount(1);
        }
        #endregion

        #region GetDispositionOfferById
        [Fact]
        public void GetDispositionOfferById_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionOffers = new List<PimsDispositionOffer>() { new PimsDispositionOffer() { OfferName = "offer" } };
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetDispositionOfferById(1, 1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionOffer>();
        }

        [Fact]
        public void GetDispositionOfferById_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);

            // Act
            Action act = () => repository.GetDispositionOfferById(1, 1);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region Appraisal

        [Fact]
        public void AddDisposition_Appraisal_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            var appraisal = new PimsDispositionAppraisal() { DispositionFileId = 1 };
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.AddDispositionFileAppraisal(appraisal);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionAppraisal>();
        }

        [Fact]
        public void UpdateDisposition_Appraisal_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionAppraisals = new List<PimsDispositionAppraisal>() { new PimsDispositionAppraisal() { DispositionAppraisalId = 1, ListPriceAmt = 200 } };

            var appraisal = dispFile.PimsDispositionAppraisals.FirstOrDefault();
            appraisal.ListPriceAmt = 300;

            // Act
            Action act = () => repository.UpdateDispositionFileAppraisal(2, appraisal);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void UpdateDisposition_Appraisal_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionAppraisals = new List<PimsDispositionAppraisal>() { new PimsDispositionAppraisal() { DispositionAppraisalId = 1, ListPriceAmt = 200 } };
            _helper.AddAndSaveChanges(dispFile);

            var appraisal = dispFile.PimsDispositionAppraisals.FirstOrDefault();
            appraisal.ListPriceAmt = 300;

            // Act
            var result = repository.UpdateDispositionFileAppraisal(1, appraisal);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionAppraisal>();
            result.ListPriceAmt.Should().Be(300);
        }

        #endregion

        #region DispositionSale

        [Fact]
        public void AddDisposition_Sale_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            _helper.AddAndSaveChanges(dispFile);

            var sale = new PimsDispositionSale() { DispositionFileId = 1 };

            // Act
            var result = repository.AddDispositionFileSale(sale);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionSale>();
        }

        [Fact]
        public void UpdateDisposition_Sale_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionSales = new List<PimsDispositionSale>() {
                new PimsDispositionSale()
                {
                    DispositionSaleId = 1,
                    SaleFinalAmt = 2000,
                }
            };
            _helper.AddAndSaveChanges(dispFile);

            var sale = new PimsDispositionSale();
            sale.DispositionSaleId = 2;

            // Act
            Action act = () => repository.UpdateDispositionFileSale(sale);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void UpdateDisposition_Sale_PurchaserAgent_Removed()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);

            var purchaserAgent = new PimsDspPurchAgent()
            {
                PersonId = 1000,
            };

            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionSales = new List<PimsDispositionSale>() {
                new PimsDispositionSale()
                {
                    DispositionSaleId = 1,
                    SaleFinalAmt = 2000,
                    DspPurchAgentId = purchaserAgent.DspPurchAgentId,
                    DspPurchAgent = purchaserAgent,
                }
            };
            _helper.AddAndSaveChanges(dispFile);

            var sale = dispFile.PimsDispositionSales.FirstOrDefault();
            sale.DspPurchAgent = null;
            sale.DspPurchAgentId = null;

            // Act
            var result = repository.UpdateDispositionFileSale(sale);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionSale>();
            Assert.Null(result.DspPurchAgentId);
            Assert.Null(result.DspPurchAgent);
        }

        [Fact]
        public void UpdateDisposition_Sale_PurchaserAgent_Added()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);

            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionSales = new List<PimsDispositionSale>() {
                new PimsDispositionSale()
                {
                    DispositionSaleId = 1,
                    SaleFinalAmt = 2000,
                    DspPurchAgentId = null,
                    DspPurchAgent = null,
                }
            };
            _helper.AddAndSaveChanges(dispFile);

            var sale = dispFile.PimsDispositionSales.FirstOrDefault();
            sale.DspPurchAgent = new()
            {
                DspPurchAgentId = 1000,
                PersonId = 100,
            };
            sale.DspPurchAgentId = 1000;

            // Act
            var result = repository.UpdateDispositionFileSale(sale);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionSale>();
            Assert.NotNull(result.DspPurchAgentId);
            Assert.NotNull(result.DspPurchAgent);
        }

        [Fact]
        public void UpdateDisposition_Sale_PurchaserAgent_Updated()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);

            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionSales = new List<PimsDispositionSale>() {
                new PimsDispositionSale()
                {
                    DispositionSaleId = 1,
                    SaleFinalAmt = 2000,
                    DspPurchAgentId = 10,
                    DspPurchAgent = new PimsDspPurchAgent()
                    {
                        DspPurchAgentId = 10,
                        PersonId = 1000,
                    }
                }
            };
            _helper.AddAndSaveChanges(dispFile);

            var updatedSale = new PimsDispositionSale(){
                DispositionSaleId = 1,
                SaleFinalAmt = 3000,
                DspPurchAgentId = 10,
                DspPurchAgent = new PimsDspPurchAgent()
                {
                    DspPurchAgentId = 10,
                    PersonId = 2000,
                }
            };

            // Act
            var result = repository.UpdateDispositionFileSale(updatedSale);
            var updatedAgent = result.DspPurchAgent;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionSale>();
            Assert.NotNull(result.DspPurchAgentId);
            Assert.NotNull(result.DspPurchAgent);
            Assert.Equal(2000, updatedAgent.PersonId);
        }

                [Fact]
        public void UpdateDisposition_Sale_PurchaserSolicitor_Removed()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);

            var purchaserSolicitor = new PimsDspPurchSolicitor()
            {
                PersonId = 1000,
            };

            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionSales = new List<PimsDispositionSale>() {
                new PimsDispositionSale()
                {
                    DispositionSaleId = 1,
                    SaleFinalAmt = 2000,
                    DspPurchSolicitorId = purchaserSolicitor.DspPurchSolicitorId,
                    DspPurchSolicitor = purchaserSolicitor,
                }
            };
            _helper.AddAndSaveChanges(dispFile);

            var sale = dispFile.PimsDispositionSales.FirstOrDefault();
            sale.DspPurchSolicitor = null;
            sale.DspPurchSolicitorId = null;

            // Act
            var result = repository.UpdateDispositionFileSale(sale);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionSale>();
            Assert.Null(result.DspPurchSolicitorId);
            Assert.Null(result.DspPurchSolicitor);
        }

        [Fact]
        public void UpdateDisposition_Sale_PurchaserSolicitor_Added()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);

            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionSales = new List<PimsDispositionSale>() {
                new PimsDispositionSale()
                {
                    DispositionSaleId = 1,
                    SaleFinalAmt = 2000,
                    DspPurchSolicitorId = null,
                    DspPurchSolicitor = null,
                }
            };
            _helper.AddAndSaveChanges(dispFile);

            var sale = dispFile.PimsDispositionSales.FirstOrDefault();
            sale.DspPurchSolicitor = new()
            {
                DspPurchSolicitorId = 1000,
                PersonId = 100,
            };
            sale.DspPurchSolicitorId = 1000;

            // Act
            var result = repository.UpdateDispositionFileSale(sale);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionSale>();
            Assert.NotNull(result.DspPurchSolicitorId);
            Assert.NotNull(result.DspPurchSolicitor);
        }

        [Fact]
        public void UpdateDisposition_Sale_PurchaserSolicitor_Updated()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);

            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionSales = new List<PimsDispositionSale>() {
                new PimsDispositionSale()
                {
                    DispositionSaleId = 1,
                    SaleFinalAmt = 2000,
                    DspPurchSolicitorId = 10,
                    DspPurchSolicitor = new PimsDspPurchSolicitor()
                    {
                        DspPurchSolicitorId = 10,
                        PersonId = 1000,
                    }
                }
            };
            _helper.AddAndSaveChanges(dispFile);

            var updatedSale = new PimsDispositionSale(){
                DispositionSaleId = 1,
                SaleFinalAmt = 3000,
                DspPurchSolicitorId = 10,
                DspPurchSolicitor = new PimsDspPurchSolicitor()
                {
                    DspPurchSolicitorId = 10,
                    PersonId = 2000,
                }
            };

            // Act
            var result = repository.UpdateDispositionFileSale(updatedSale);
            var updatedSolicitor = result.DspPurchSolicitor;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionSale>();
            Assert.NotNull(result.DspPurchSolicitorId);
            Assert.NotNull(result.DspPurchSolicitor);
            Assert.Equal(2000, updatedSolicitor.PersonId);
        }


        [Fact]
        public void UpdateDisposition_Sale_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionSales = new List<PimsDispositionSale>() {
                new PimsDispositionSale()
                {
                    DispositionSaleId = 1,
                    SaleFinalAmt = 2000,
                }
            };
            _helper.AddAndSaveChanges(dispFile);

            var sale = dispFile.PimsDispositionSales.FirstOrDefault();
            sale.SaleFinalAmt = 3000;

            // Act
            var result = repository.UpdateDispositionFileSale(sale);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionSale>();
            result.SaleFinalAmt.Should().Be(3000);
        }

        #endregion

        #region AddDispositionOffer
        [Fact]
        public void AddDispositionOffer_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            var offer = new PimsDispositionOffer() { OfferName = "offer" };
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.AddDispositionOffer(offer);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionOffer>();
        }
        #endregion

        #region UpdateDispositionOffer
        [Fact]
        public void UpdateDispositionOffer_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionOffers = new List<PimsDispositionOffer>() { new PimsDispositionOffer() { OfferName = "offer" } };
            _helper.AddAndSaveChanges(dispFile);

            var offer = dispFile.PimsDispositionOffers.FirstOrDefault();
            offer.OfferName = "updated";
            // Act
            var result = repository.UpdateDispositionOffer(offer);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionOffer>();
            result.OfferName.Should().Be("updated");
        }

        [Fact]
        public void UpdateDispositionOffer_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionOffers = new List<PimsDispositionOffer>() { new PimsDispositionOffer() { OfferName = "offer" } };

            var offer = dispFile.PimsDispositionOffers.FirstOrDefault();
            offer.OfferName = "updated";
            // Act
            Action act = () => repository.UpdateDispositionOffer(offer);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region TryDeleteDispositionOffer
        [Fact]
        public void TryDeleteDispositionOffer_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionOffers = new List<PimsDispositionOffer>() { new PimsDispositionOffer() { OfferName = "offer" } };
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.TryDeleteDispositionOffer(1, 1);

            // Assert
            result.Should().Be(true);
        }
        #endregion

        #region GetRowVersion
        [Fact]
        public void GetRowVersion_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetRowVersion(1);

            // Assert
            result.Should().Be(2);
        }

        [Fact]
        public void GetRowVersion_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);

            // Act
            Action act = () => repository.GetRowVersion(1);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        #endregion

        #region GetLastUpdateBy
        [Fact]
        public void GetLastUpdateBy_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.AppLastUpdateUserid = "test";
            dispFile.AppLastUpdateTimestamp = DateTime.Now;
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetLastUpdateBy(1);

            // Assert
            result.AppLastUpdateUserid.Should().Be("service");
            result.AppLastUpdateTimestamp.Should().BeSameDateAs(dispFile.AppLastUpdateTimestamp);
        }

        #endregion

        #region GetPageDeep
        [Fact]
        public void GetPageDeep_NoFilter_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter());

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_DispositionName_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.FileName = "fileName";
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { FileNameOrNumberOrReference = "fileName" });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_DispositionName_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.FileName = "fileName";
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { FileNameOrNumberOrReference = "notFound" });

            // Assert
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetPageDeep_DispositionNumber_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.FileNumber = "fileNumber";
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { FileNameOrNumberOrReference = "fileNumber" });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_DispositionHistoricalNumber_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.FileReference = "legacy";
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { FileNameOrNumberOrReference = "legacy" });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_DispositionTeamMemberPerson_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            dispFile.PimsDispositionFileTeams.Add(new PimsDispositionFileTeam() { DispositionFileId = dispFile.Internal_Id, PersonId = person.Internal_Id, Person = person, DspFlTeamProfileTypeCode = "COORD" });
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { TeamMemberPersonId = 1 });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_DispositionTeamMemberOrganization_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            var org = EntityHelper.CreateOrganization(1, "tester org");
            dispFile.PimsDispositionFileTeams.Add(new PimsDispositionFileTeam() { DispositionFileId = dispFile.Internal_Id, OrganizationId = org.Internal_Id, Organization = org, DspFlTeamProfileTypeCode = "COORD" });
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { TeamMemberOrganizationId = 1 });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_DispositionTeamMember_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            dispFile.PimsDispositionFileTeams.Add(new PimsDispositionFileTeam() { DispositionFileId = dispFile.Internal_Id, PersonId = person.Internal_Id, Person = person, DspFlTeamProfileTypeCode = "COORD" });
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { TeamMemberPersonId = 2 });

            // Assert
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetPageDeep_Pid_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionFileProperties = new List<PimsDispositionFileProperty>() { new PimsDispositionFileProperty() { Property = EntityHelper.CreateProperty(1, 2) } };
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { Pid = "1" });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_Pin_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionFileProperties = new List<PimsDispositionFileProperty>() { new PimsDispositionFileProperty() { Property = EntityHelper.CreateProperty(1, 2) } };
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { Pin = "2" });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_Address_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.PimsDispositionFileProperties = new List<PimsDispositionFileProperty>() { new PimsDispositionFileProperty() { Property = EntityHelper.CreateProperty(1, 2) } };
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { Address = "1234" });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_FileStatus_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { DispositionFileStatusCode = "ACTIVE" });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_Status_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.DispositionStatusTypeCode = "DRAFT";
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { DispositionStatusCode = "DRAFT" });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_Type_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.DispositionTypeCode = "SECTN3";

            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { DispositionTypeCode = "SECTN3" });

            // Assert
            result.Should().HaveCount(1);
        }

        #endregion

        #region Export

        [Fact]
        public void GetDispositionFileExport_Filter_DispositionName()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionAdd, Permissions.DispositionView);
            var dspFile = EntityHelper.CreateDispositionFile();
            dspFile.FileName = "fileName";
            var filter = new DispositionFilter() { FileNameOrNumberOrReference = "fileName" };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(dspFile);

            var repository = helper.CreateRepository<DispositionFileRepository>(user);

            // Act
            var result = repository.GetDispositionFileExportDeep(filter);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetDispositionFileExport_Filter_DispositionNumber()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionAdd);
            var dspFile = EntityHelper.CreateDispositionFile();
            dspFile.FileNumber = "100";
            var filter = new DispositionFilter() { FileNameOrNumberOrReference = "D-100" };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(dspFile);

            var repository = helper.CreateRepository<DispositionFileRepository>(user);

            // Act
            var result = repository.GetDispositionFileExportDeep(filter);

            // Assert
            result.Should().HaveCount(1);
        }

        #endregion

        #endregion
    }
}
