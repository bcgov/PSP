import React from 'react';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Concepts_PropertyActivityInvoice } from '@/models/api/generated/ApiGen_Concepts_PropertyActivityInvoice';
import { formatMoney, prettyFormatDate } from '@/utils';

export interface IInvoiceView {
  activityInvoice: ApiGen_Concepts_PropertyActivityInvoice;
  index: number;
}

export const InvoiceView: React.FunctionComponent<React.PropsWithChildren<IInvoiceView>> = ({
  activityInvoice,
  index,
}) => {
  return (
    <Section header={`Invoice ${index + 1}`} isCollapsable initiallyExpanded>
      <SectionField label="Invoice number" labelWidth="4" contentWidth="8">
        {activityInvoice.invoiceNum}
      </SectionField>

      <SectionField label="Invoice date" contentWidth="7">
        {prettyFormatDate(activityInvoice.invoiceDateTime)}
      </SectionField>

      <SectionField label="Description" contentWidth="7">
        {activityInvoice.description}
      </SectionField>

      <SectionField label="Amount (before tax)" contentWidth="7">
        {formatMoney(activityInvoice.pretaxAmount)}
      </SectionField>

      <SectionField label="GST amount" contentWidth="7">
        {formatMoney(activityInvoice.gstAmount)}
      </SectionField>

      <SectionField label="PST applicable?" contentWidth="7">
        {activityInvoice.isPstRequired === true ? 'Yes' : 'No'}
      </SectionField>

      <SectionField label="PST amount" contentWidth="7">
        {formatMoney(activityInvoice.pstAmount)}
      </SectionField>

      <SectionField label="Total amount" contentWidth="7">
        {formatMoney(activityInvoice.totalAmount)}
      </SectionField>
    </Section>
  );
};
