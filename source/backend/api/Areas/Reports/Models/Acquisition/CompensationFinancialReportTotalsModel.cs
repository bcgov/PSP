using System.Collections.Generic;
using System.ComponentModel;
using Pims.Dal.Entities;

namespace Pims.Api.Areas.Reports.Models.Acquisition
{
    public class CompensationFinancialReportTotalsModel
    {
        [DisplayName("Project Total (Pre Tax) - Draft Requisitions")]
        public Dictionary<long, decimal> ProjectDraftPreTaxAmount { get; set; } = new Dictionary<long, decimal>();

        [DisplayName("Project Total (Tax) - Draft Requisitions")]
        public Dictionary<long, decimal> ProjectDraftTaxAmount { get; set; } = new Dictionary<long, decimal>();

        [DisplayName("Project Total (Inc. Tax) - Draft Requisitions")]
        public Dictionary<long, decimal> ProjectDraftTotalAmount { get; set; } = new Dictionary<long, decimal>();

        [DisplayName("Project Total (Pre Tax) - Final Requisitions")]
        public Dictionary<long, decimal> ProjectFinalPreTaxAmount { get; set; } = new Dictionary<long, decimal>();

        [DisplayName("Project Total (Tax) - Final Requisitions")]
        public Dictionary<long, decimal> ProjectFinalTaxAmount { get; set; } = new Dictionary<long, decimal>();

        [DisplayName("Project Total (Inc. Tax) - Final Requisitions")]
        public Dictionary<long, decimal> ProjectFinalTotalAmount { get; set; } = new Dictionary<long, decimal>();

        [DisplayName("Total Expenditures shown in this report (Pre Tax) - draft")]
        public decimal AllExpendituresDraftPreTaxAmount { get; set; } = 0;

        [DisplayName("Total Expenditures shown in this report (Tax) - draft")]
        public decimal AllExpendituresDraftTaxAmount { get; set; } = 0;

        [DisplayName("Total Expenditures shown in this report (Inc. Tax) - draft")]
        public decimal AllExpendituresDraftTotalAmount { get; set; } = 0;

        [DisplayName("Total Expenditures shown in this report (Pre Tax) - final")]
        public decimal AllExpendituresFinalPreTaxAmount { get; set; } = 0;

        [DisplayName("Total Expenditures shown in this report (Tax) - final")]
        public decimal AllExpendituresFinalTaxAmount { get; set; } = 0;

        [DisplayName("Total Expenditures shown in this report (Inc. Tax) - final")]
        public decimal AllExpendituresFinalTotalAmount { get; set; } = 0;

        public CompensationFinancialReportTotalsModel(IEnumerable<PimsCompReqFinancial> financials)
        {
            foreach (var financial in financials)
            {
                if (IsDraftRequisition(financial))
                {
                    AllExpendituresDraftPreTaxAmount += GetNullableDecimal(financial.PretaxAmt);
                    AllExpendituresDraftTaxAmount += GetNullableDecimal(financial.TaxAmt);
                    AllExpendituresDraftTotalAmount += GetNullableDecimal(financial.TotalAmt);

                    var project = GetProject(financial);
                    if (project is not null)
                    {
                        AddToProjectTotal(ProjectDraftPreTaxAmount, project.Id, GetNullableDecimal(financial.PretaxAmt));
                        AddToProjectTotal(ProjectDraftTaxAmount, project.Id, GetNullableDecimal(financial.TaxAmt));
                        AddToProjectTotal(ProjectDraftTotalAmount, project.Id, GetNullableDecimal(financial.TotalAmt));
                    }
                }
                else
                {
                    AllExpendituresFinalPreTaxAmount += GetNullableDecimal(financial.PretaxAmt);
                    AllExpendituresFinalTaxAmount += GetNullableDecimal(financial.TaxAmt);
                    AllExpendituresFinalTotalAmount += GetNullableDecimal(financial.TotalAmt);

                    var project = GetProject(financial);
                    if (project is not null)
                    {
                        AddToProjectTotal(ProjectFinalPreTaxAmount, project.Id, GetNullableDecimal(financial.PretaxAmt));
                        AddToProjectTotal(ProjectFinalTaxAmount, project.Id, GetNullableDecimal(financial.TaxAmt));
                        AddToProjectTotal(ProjectFinalTotalAmount, project.Id, GetNullableDecimal(financial.TotalAmt));
                    }
                }
            }
        }

        private static bool IsDraftRequisition(PimsCompReqFinancial financial)
        {
            if (financial == null)
            {
                return true;
            }
            else
            {
                return financial.CompensationRequisition == null || !financial.CompensationRequisition.IsDraft.HasValue || financial.CompensationRequisition.IsDraft.Value;
            }
        }

        private static decimal GetNullableDecimal(decimal? number)
        {
            return number.HasValue ? number.Value : 0;
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

        private static void AddToProjectTotal(Dictionary<long, decimal> dict, long projectId, decimal financialValue)
        {
            if (dict.ContainsKey(projectId))
            {
                dict[projectId] += financialValue;
            }
            else
            {
                dict.Add(projectId, financialValue);
            }
        }
    }
}
