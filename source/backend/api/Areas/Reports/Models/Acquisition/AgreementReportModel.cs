using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Api.Areas.Reports.Models.Agreement
{
    public class AgreementReportModel
    {
        [DisplayName("Export Date")]
        public string ExportDate { get; set; }

        [DisplayName("Export By")]
        public string ExportBy { get; set; }

        [DisplayName("Ministry Project")]
        public string MinistryProject { get; set; }

        [DisplayName("Product")]
        public string Product { get; set; }

        [DisplayName("Acquisition # and Name")]
        public string AcquisitionNumberAndName { get; set; }

        [DisplayName("File Created Date")]
        public string FileCreatedDate { get; set; }

        [DisplayName("File Status")]
        public string FileStatus { get; set; }

        [DisplayName("Agreement Status")]
        public string AgreementStatus { get; set; }

        [DisplayName("Legal Survey Plan")]
        public string LegalSurveyPlan { get; set; }

        [DisplayName("Agreement Type")]
        public string AgreementType { get; set; }

        [DisplayName("Agreement Date")]
        public string AgreementDate { get; set; }

        [DisplayName("Completion Date")]
        public string CompletionDate { get; set; }

        [DisplayName("Commencement Date")]
        public string CommencementDate { get; set; }

        [DisplayName("Termination Date")]
        public string TerminationDate { get; set; }

        [DisplayName("Purchase Price")]
        public decimal PurchasePrice { get; set; }

        [DisplayName("Deposit Amount")]
        public decimal DepositAmount { get; set; }

        [DisplayName("Property Agent")]
        public string PropertyAgent { get; set; }

        [DisplayName("Property Analyst")]
        public string PropertyAnalyst { get; set; }

        [DisplayName("Property Coordinator")]
        public string PropertyCoordinator { get; set; }

        [DisplayName("Expropriation Agent")]
        public string ExpropriationAgent { get; set; }

        [DisplayName("MoTI Solicitor")]
        public string MotiSolicitor { get; set; }

        [DisplayName("Negotiation Agent")]
        public string NegotiationAgent { get; set; }

        public AgreementReportModel(PimsAgreement agreement, ClaimsPrincipal user)
        {
            MinistryProject = GetMinistryProjectName(agreement.AcquisitionFile?.Project);
            Product = GetMinistryProductName(agreement.AcquisitionFile?.Product);
            AcquisitionNumberAndName = $"{agreement.AcquisitionFile?.FileNumber} - {agreement.AcquisitionFile?.FileName}";
            FileCreatedDate = GetNullableDate(agreement.AcquisitionFile?.AppCreateTimestamp);
            FileStatus = agreement.AcquisitionFile?.AcquisitionFileStatusTypeCodeNavigation?.Description;
            AgreementStatus = !agreement.IsDraft.HasValue || agreement.IsDraft.Value ? "Draft" : "Final";
            LegalSurveyPlan = agreement.LegalSurveyPlanNum;
            AgreementType = agreement.AgreementTypeCodeNavigation?.Description ?? string.Empty;
            AgreementDate = GetNullableDate(agreement.AgreementDate);
            CompletionDate = GetNullableDate(agreement.CompletionDate);
            CommencementDate = GetNullableDate(agreement.CommencementDate);
            TerminationDate = GetNullableDate(agreement.TerminationDate);
            PurchasePrice = agreement.PurchasePrice.HasValue ? agreement.PurchasePrice.Value : 0;
            DepositAmount = agreement.DepositAmount.HasValue ? agreement.DepositAmount.Value : 0;
            PropertyAgent = GetTeamMemberName(agreement.AcquisitionFile, "PROPAGENT");
            PropertyAnalyst = GetTeamMemberName(agreement.AcquisitionFile, "PROPANLYS");
            PropertyCoordinator = GetTeamMemberName(agreement.AcquisitionFile, "PROPCOORD");
            ExpropriationAgent = GetTeamMemberName(agreement.AcquisitionFile, "EXPRAGENT");
            MotiSolicitor = GetTeamMemberName(agreement.AcquisitionFile, "MOTILAWYER");
            NegotiationAgent = GetTeamMemberName(agreement.AcquisitionFile, "NEGOTAGENT");
            ExportBy = user.GetDisplayName();
            ExportDate = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private static string GetTeamMemberName(PimsAcquisitionFile file, string personProfileTypeCode)
        {
            PimsPerson matchingPerson = file?.PimsAcquisitionFilePeople?.FirstOrDefault(x => x.AcqFlPersonProfileTypeCode == personProfileTypeCode)?.Person;
            return matchingPerson?.GetFullName() ?? string.Empty;
        }

        private static string GetNullableDate(DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : string.Empty;
        }

        private static string GetMinistryProjectName(PimsProject project)
        {
            if (project == null)
            {
                return string.Empty;
            }
            else
            {
                return $"{project.Code} - {project.Description}";
            }
        }

        private static string GetMinistryProductName(PimsProduct product)
        {
            if (product == null)
            {
                return string.Empty;
            }
            else
            {
                return $"{product.Code} - {product.Description}";
            }
        }
    }
}
