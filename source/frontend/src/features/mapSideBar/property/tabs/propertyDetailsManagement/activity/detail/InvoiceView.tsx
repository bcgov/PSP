import React from 'react';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Concepts_ManagementActivityInvoice } from '@/models/api/generated/ApiGen_Concepts_ManagementActivityInvoice';
import { formatMoney, prettyFormatDate } from '@/utils';

export interface IInvoiceView {
  activityInvoice: ApiGen_Concepts_ManagementActivityInvoice;
  index: number;
}

export const InvoiceView: React.FunctionComponent<React.PropsWithChildren<IInvoiceView>> = ({
  activityInvoice,
  index,
}) => {
  return (
    <Section header={`Invoice ${index + 1}`} isCollapsable initiallyExpanded>
      <SectionField label="Invoice number" labelWidth={{ xs: 4 }} contentWidth={{ xs: 8 }}>
        {activityInvoice.invoiceNum}
      </SectionField>

      <SectionField label="Invoice date" contentWidth={{ xs: 7 }}>
        {prettyFormatDate(activityInvoice.invoiceDateTime)}
      </SectionField>

      <SectionField label="Description" contentWidth={{ xs: 7 }}>
        {activityInvoice.description}
      </SectionField>

      <SectionField label="Amount (before tax)" contentWidth={{ xs: 7 }}>
        {formatMoney(activityInvoice.pretaxAmount)}
      </SectionField>

      <SectionField label="GST amount" contentWidth={{ xs: 7 }}>
        {formatMoney(activityInvoice.gstAmount)}
      </SectionField>

      <SectionField label="PST applicable?" contentWidth={{ xs: 7 }}>
        {activityInvoice.isPstRequired === true ? 'Yes' : 'No'}
      </SectionField>

      <SectionField label="PST amount" contentWidth={{ xs: 7 }}>
        {formatMoney(activityInvoice.pstAmount)}
      </SectionField>

      <SectionField label="Total amount" contentWidth={{ xs: 7 }}>
        {formatMoney(activityInvoice.totalAmount)}
      </SectionField>
    </Section>
  );
};
