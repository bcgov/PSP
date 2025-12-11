import * as React from 'react';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Concepts_ManagementActivityInvoice } from '@/models/api/generated/ApiGen_Concepts_ManagementActivityInvoice';
import { formatMoney } from '@/utils';

interface IActivityDetailInvoiceTotalsViewProps {
  invoices: ApiGen_Concepts_ManagementActivityInvoice[];
}

const ActivityDetailInvoiceTotalsView: React.FunctionComponent<
  IActivityDetailInvoiceTotalsViewProps
> = props => {
  const invoices: ApiGen_Concepts_ManagementActivityInvoice[] = props.invoices ?? [];

  let pretaxAmount = 0;
  let gstAmount = 0;
  let pstAmount = 0;
  let totalAmount = 0;

  for (let i = 0; i < invoices.length; i++) {
    pretaxAmount += invoices[i].pretaxAmount ?? 0;
    gstAmount += invoices[i].gstAmount ?? 0;
    pstAmount += invoices[i].pstAmount ?? 0;
    totalAmount += invoices[i].totalAmount ?? 0;
  }
  return (
    <Section header="Invoices Total">
      <SectionField label="Total (before tax)" contentWidth={{ xs: 7 }}>
        {formatMoney(pretaxAmount)}
      </SectionField>
      <SectionField label="GST amount" contentWidth={{ xs: 7 }}>
        {formatMoney(gstAmount)}
      </SectionField>
      <SectionField label="PST amount" contentWidth={{ xs: 7 }}>
        {formatMoney(pstAmount)}
      </SectionField>
      <SectionField label="Total amount" contentWidth={{ xs: 7 }}>
        {formatMoney(totalAmount)}
      </SectionField>
    </Section>
  );
};

export default ActivityDetailInvoiceTotalsView;
