import React from 'react';
import styled from 'styled-components';

import OverflowTip from '@/components/common/OverflowTip';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { UserNameTooltip } from '@/components/common/UserNameTooltip';
import { ApiGen_Concepts_ManagementActivityInvoice } from '@/models/api/generated/ApiGen_Concepts_ManagementActivityInvoice';
import { formatMoney, prettyFormatDate, prettyFormatUTCDate } from '@/utils';
import { booleanToYesNoString } from '@/utils/formUtils';

export interface IInvoiceView {
  activityInvoice: ApiGen_Concepts_ManagementActivityInvoice;
  index: number;
}

export const InvoiceView: React.FunctionComponent<React.PropsWithChildren<IInvoiceView>> = ({
  activityInvoice,
  index,
}) => {
  const getInvoiceHeader = (): React.ReactNode => {
    const totalAmountString = formatMoney(activityInvoice.totalAmount);
    const paymentApprovedString = activityInvoice.isPaymentApproved ? 'Y' : 'N';
    const invoiceHeaderString = `Invoice ${
      index + 1
    } - ${totalAmountString} - Approved ${paymentApprovedString} - ${activityInvoice.description}`;
    return <OverflowTip fullText={invoiceHeaderString}></OverflowTip>;
  };

  return (
    <Section header={getInvoiceHeader()} isCollapsable initiallyExpanded={false} hideOverflow>
      <SectionField label="Invoice number" labelWidth={{ xs: 4 }} contentWidth={{ xs: 8 }}>
        {activityInvoice.invoiceNum}
      </SectionField>

      <SectionField label="Invoice date" contentWidth={{ xs: 7 }}>
        {prettyFormatDate(activityInvoice.invoiceDateTime)}
      </SectionField>

      <SectionField label="Description" contentWidth={{ xs: 7 }}>
        {activityInvoice.description}
      </SectionField>

      <SectionField label="Payment approved" contentWidth={{ xs: 3 }}>
        {booleanToYesNoString(activityInvoice.isPaymentApproved)}
        <StyledInvoiceAuditSpan>
          <UserNameTooltip
            userName={activityInvoice.appLastUpdateUserid}
            userGuid={activityInvoice.appLastUpdateUserGuid}
            tooltipId="invoice-audit-user"
          />
          <em> {prettyFormatUTCDate(activityInvoice.appLastUpdateTimestamp)}</em>
        </StyledInvoiceAuditSpan>
      </SectionField>

      <SectionField label="Payment forwarded" contentWidth={{ xs: 3 }}>
        {booleanToYesNoString(activityInvoice.isPaymentForwarded)}
      </SectionField>

      <SectionField label="Amount (before tax)" contentWidth={{ xs: 7 }}>
        {formatMoney(activityInvoice.pretaxAmount)}
      </SectionField>

      <SectionField label="GST amount" contentWidth={{ xs: 7 }}>
        {formatMoney(activityInvoice.gstAmount)}
      </SectionField>

      <SectionField label="PST applicable?" contentWidth={{ xs: 7 }}>
        {booleanToYesNoString(activityInvoice.isPstRequired)}
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

const StyledInvoiceAuditSpan = styled.span`
  margin-left: 0.3rem;
  min-width: 13rem;
  font-size: 1.1rem;
  font-style: italic;
  text-align: right;
  color: ${props => props.theme.css.activeActionColor};

  .tooltip-icon {
    color: ${props => props.theme.css.activeActionColor};
  }
`;
