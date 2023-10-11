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

        [DisplayName("Acquisition File # and Name")]
        public string AcquisitionNumberAndName { get; set; }

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
            MinistryProject = GetMinistryProjectName(financial.CompensationRequisition?.AcquisitionFile?.Project);
            Product = GetMinistryProductName(financial.CompensationRequisition?.AcquisitionFile?.Product);
            AcquisitionNumberAndName = GetAcquisitionFileName(financial.CompensationRequisition?.AcquisitionFile);
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
            ProjectDraftPreTaxAmount = project is not null && reportTotals.ProjectDraftPreTaxAmount.ContainsKey(project.Id) ? reportTotals.ProjectDraftPreTaxAmount[project.Id] : 0;
            ProjectDraftTaxAmount = project is not null && reportTotals.ProjectDraftTaxAmount.ContainsKey(project.Id) ? reportTotals.ProjectDraftTaxAmount[project.Id] : 0;
            ProjectDraftTotalAmount = project is not null && reportTotals.ProjectDraftTotalAmount.ContainsKey(project.Id) ? reportTotals.ProjectDraftTotalAmount[project.Id] : 0;

            // Final requisition total per project
            ProjectFinalPreTaxAmount = project is not null && reportTotals.ProjectFinalPreTaxAmount.ContainsKey(project.Id) ? reportTotals.ProjectFinalPreTaxAmount[project.Id] : 0;
            ProjectFinalTaxAmount = project is not null && reportTotals.ProjectFinalTaxAmount.ContainsKey(project.Id) ? reportTotals.ProjectFinalTaxAmount[project.Id] : 0;
            ProjectFinalTotalAmount = project is not null && reportTotals.ProjectFinalTotalAmount.ContainsKey(project.Id) ? reportTotals.ProjectFinalTotalAmount[project.Id] : 0;

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
            else
            {
                return null;
            }
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

        private static string GetAcquisitionFileName(PimsAcquisitionFile file)
        {
            if (file == null)
            {
                return string.Empty;
            }
            else
            {
                return $"{file.FileNumber} - {file.FileName}";
            }
        }
    }
}
