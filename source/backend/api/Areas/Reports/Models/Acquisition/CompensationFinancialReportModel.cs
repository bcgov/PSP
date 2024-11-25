using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Claims;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Api.Areas.Reports.Models.Acquisition
{
    public class CompensationFinancialReportModel
    {
        [DisplayName("Export Date")]
        public string ExportDate { get; set; }

        [DisplayName("Export By")]
        public string ExportBy { get; set; }

        [DisplayName("Ministry Project")]
        public string MinistryProject { get; set; }

        [DisplayName("Product")]
        public string Product { get; set; }

        [DisplayName("File Type")]
        public string FileType { get; set; }

        [DisplayName("File # and Name")]
        public string FileNumberAndName { get; set; }

        [DisplayName("Fiscal Year")]
        public string FiscalYear { get; set; }

        [DisplayName("Alternate Project")]
        public string AlternateProject { get; set; }

        [DisplayName("Requisition Number")]
        public long RequisitionNumber { get; set; }

        [DisplayName("Requisition State")]
        public string RequisitionState { get; set; }

        [DisplayName("Final Date")]
        public string FinalDate { get; set; }

        [DisplayName("Financial Activity Name")]
        public string FinancialActivityName { get; set; }

        [DisplayName("Financial Activity Amount (Before Tax)")]
        public decimal PreTaxAmount { get; set; }

        [DisplayName("Financial Activity Tax")]
        public decimal TaxAmount { get; set; }

        [DisplayName("Financial Activity Amount (Inc. Tax)")]
        public decimal TotalAmount { get; set; }

        [DisplayName("Project Total (Pre Tax) - Draft Requisitions")]
        public decimal ProjectDraftPreTaxAmount { get; set; }

        [DisplayName("Project Total (Tax) - Draft Requisitions")]
        public decimal ProjectDraftTaxAmount { get; set; }

        [DisplayName("Project Total (Inc. Tax) - Draft Requisitions")]
        public decimal ProjectDraftTotalAmount { get; set; }

        [DisplayName("Project Total (Pre Tax) - Final Requisitions")]
        public decimal ProjectFinalPreTaxAmount { get; set; }

        [DisplayName("Project Total (Tax) - Final Requisitions")]
        public decimal ProjectFinalTaxAmount { get; set; }

        [DisplayName("Project Total (Inc. Tax) - Final Requisitions")]
        public decimal ProjectFinalTotalAmount { get; set; }

        [DisplayName("Total Expenditures shown in this report (Pre Tax) - draft")]
        public decimal AllExpendituresDraftPreTaxAmount { get; set; }

        [DisplayName("Total Expenditures shown in this report (Tax) - draft")]
        public decimal AllExpendituresDraftTaxAmount { get; set; }

        [DisplayName("Total Expenditures shown in this report (Inc. Tax) - draft")]
        public decimal AllExpendituresDraftTotalAmount { get; set; }

        [DisplayName("Total Expenditures shown in this report (Pre Tax) - final")]
        public decimal AllExpendituresFinalPreTaxAmount { get; set; }

        [DisplayName("Total Expenditures shown in this report (Tax) - final")]
        public decimal AllExpendituresFinalTaxAmount { get; set; }

        [DisplayName("Total Expenditures shown in this report (Inc. Tax) - final")]
        public decimal AllExpendituresFinalTotalAmount { get; set; }

        public CompensationFinancialReportModel(PimsCompReqFinancial financial, CompensationFinancialReportTotalsModel reportTotals, ClaimsPrincipal user)
        {
            ExportBy = user.GetDisplayName();
            ExportDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            MinistryProject = GetMinistryProjectName(financial.CompensationRequisition?.AcquisitionFile?.Project ?? financial.CompensationRequisition?.Lease?.Project);
            Product = GetMinistryProductName(financial.CompensationRequisition?.AcquisitionFile?.Product ?? financial.CompensationRequisition?.Lease?.Product);
            FileType = GetFileType(financial.CompensationRequisition);
            FileNumberAndName = GetFileNumberAndName(financial.CompensationRequisition);
            FiscalYear = financial.CompensationRequisition?.FiscalYear ?? string.Empty;
            AlternateProject = GetMinistryProjectName(financial.CompensationRequisition?.AlternateProject);
            RequisitionNumber = financial.CompensationRequisition?.Internal_Id ?? 0;
            bool isDraftCompReq = financial.CompensationRequisition == null || !financial.CompensationRequisition.IsDraft.HasValue || financial.CompensationRequisition.IsDraft.Value;
            RequisitionState = isDraftCompReq ? "Draft" : "Final";
            FinalDate = GetNullableDate(financial.CompensationRequisition?.FinalizedDate);
            FinancialActivityName = GetFinancialActivityName(financial.FinancialActivityCode);
            PreTaxAmount = financial.PretaxAmt.HasValue ? financial.PretaxAmt.Value : 0;
            TaxAmount = financial.TaxAmt.HasValue ? financial.TaxAmt.Value : 0;
            TotalAmount = financial.TotalAmt.HasValue ? financial.TotalAmt.Value : 0;

            var project = GetProject(financial);

            // Draft requisition totals per project
            ProjectDraftPreTaxAmount = project is not null && reportTotals.ProjectDraftPreTaxAmount.TryGetValue(project.Id, out decimal draftPreTax) ? draftPreTax : 0;
            ProjectDraftTaxAmount = project is not null && reportTotals.ProjectDraftTaxAmount.TryGetValue(project.Id, out decimal draftTax) ? draftTax : 0;
            ProjectDraftTotalAmount = project is not null && reportTotals.ProjectDraftTotalAmount.TryGetValue(project.Id, out decimal draftTotal) ? draftTotal : 0;

            // Final requisition total per project
            ProjectFinalPreTaxAmount = project is not null && reportTotals.ProjectFinalPreTaxAmount.TryGetValue(project.Id, out decimal finalPreTax) ? finalPreTax : 0;
            ProjectFinalTaxAmount = project is not null && reportTotals.ProjectFinalTaxAmount.TryGetValue(project.Id, out decimal finalTax) ? finalTax : 0;
            ProjectFinalTotalAmount = project is not null && reportTotals.ProjectFinalTotalAmount.TryGetValue(project.Id, out decimal finalTotal) ? finalTotal : 0;

            // Total expenditures - draft
            AllExpendituresDraftPreTaxAmount = reportTotals.AllExpendituresDraftPreTaxAmount;
            AllExpendituresDraftTaxAmount = reportTotals.AllExpendituresDraftTaxAmount;
            AllExpendituresDraftTotalAmount = reportTotals.AllExpendituresDraftTotalAmount;

            // Total expenditures - final
            AllExpendituresFinalPreTaxAmount = reportTotals.AllExpendituresFinalPreTaxAmount;
            AllExpendituresFinalTaxAmount = reportTotals.AllExpendituresFinalTaxAmount;
            AllExpendituresFinalTotalAmount = reportTotals.AllExpendituresFinalTotalAmount;
        }

        private static PimsProject GetProject(PimsCompReqFinancial financial)
        {
            // If compensation requisition has alternate project selected, then all project information should be sourced from 'alternate project' rather than 'file project'.
            if (financial?.CompensationRequisition?.AlternateProject is not null)
            {
                return financial.CompensationRequisition.AlternateProject;
            }
            else if (financial?.CompensationRequisition?.AcquisitionFile?.Project is not null)
            {
                return financial.CompensationRequisition.AcquisitionFile.Project;
            }
            else if (financial?.CompensationRequisition?.Lease?.Project is not null)
            {
                return financial.CompensationRequisition.Lease.Project;
            }
            else
            {
                return null;
            }
        }

        private static string GetNullableDate(DateOnly? date)
        {
            return date.HasValue ? date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : string.Empty;
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

        private static string GetFinancialActivityName(PimsFinancialActivityCode financialActivity)
        {
            if (financialActivity == null)
            {
                return string.Empty;
            }
            else
            {
                return $"{financialActivity.Code} - {financialActivity.Description}";
            }
        }

        private static string GetFileNumberAndName(PimsCompensationRequisition compensationRequisition)
        {
            if (compensationRequisition?.AcquisitionFile is not null)
            {
                //TODO: Fix Mapings
                return "fix me please";
                //return $"{compensationRequisition.AcquisitionFile.FileNumber} - {compensationRequisition.AcquisitionFile.FileName}";
            }
            else if (compensationRequisition?.Lease is not null)
            {
                return compensationRequisition.Lease.LFileNo ?? string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

        private static string GetFileType(PimsCompensationRequisition compensationRequisition)
        {
            if (compensationRequisition?.AcquisitionFile is not null)
            {
                return "Acquisition";
            }
            else if (compensationRequisition?.Lease is not null)
            {
                return "Lease/Licence";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
