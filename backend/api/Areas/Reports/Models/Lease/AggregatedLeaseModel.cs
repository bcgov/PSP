using System;
using System.Collections.Generic;
using System.Linq;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Api.Areas.Reports.Models.Lease
{
    public class AggregatedLeaseModel
    {
        public string Region { get; set; }

        public string Program { get; set; }

        public int AgreementCount { get; set; }

        public decimal ActualTotal { get; set; }

        public decimal ExpectedTotal { get; set; }

        public decimal[] ActualsByMonth { get; set; }

        public AggregatedLeaseModel(IEnumerable<PimsLease> leases, int fiscalYear, string region = null, string program = null)
        {

            DateTime fiscalYearStartDate = fiscalYear.ToFiscalYearDate();
            DateTime fiscalYearEndDate = fiscalYearStartDate.AddYears(1);
            var terms = leases.SelectMany(l => l.PimsLeaseTerms).Where(t => t.TermStartDate <= fiscalYearEndDate
                && (t.TermExpiryDate >= fiscalYearStartDate || t.TermExpiryDate == null));
            var payments = terms.SelectMany(t => t.PimsLeasePayments).Where(p => p.PaymentReceivedDate <= fiscalYearEndDate && (p.PaymentReceivedDate >= fiscalYearStartDate));

            this.Region = region;
            this.Program = program;
            AgreementCount = leases.Count(l => l.PimsLeaseTerms.Any(t => t.TermStartDate <= fiscalYearEndDate && (t.TermExpiryDate >= fiscalYearStartDate || t.TermExpiryDate == null)));
            ActualTotal = payments.Aggregate((decimal)0, (sum, payment) => sum + payment.PaymentAmountPreTax);
            ExpectedTotal = terms.Aggregate((decimal)0, (sum, term) => sum + term.PaymentAmount ?? 0);
            ActualsByMonth = new decimal[12];
            foreach (var (value, index) in ActualsByMonth.Select((value, index) => (value, index)))
            {
                ActualsByMonth[index] = payments.Where(p => p.PaymentReceivedDate.Month == (index + 1))
                    .Aggregate((decimal)0, (sum, payment) => sum + payment.PaymentAmountTotal);
            }
        }
    }
}
