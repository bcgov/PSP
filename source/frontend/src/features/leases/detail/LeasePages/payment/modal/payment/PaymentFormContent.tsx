import { useFormikContext } from 'formik';
import * as React from 'react';

import { FastCurrencyInput, FastDatePicker, Select } from '@/components/common/form';
import { InlineFlexDiv } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';

import { useCalculateActualGst } from '../../hooks/useCalculateActualGst';
import { FormLeasePayment } from '../../models';
import * as Styled from '../../styles';

interface IPaymentFormContentProps {
  isReceived: boolean;
  isGstEligible: boolean;
}

const PaymentFormContent: React.FunctionComponent<
  React.PropsWithChildren<IPaymentFormContentProps>
> = ({ isReceived, isGstEligible }) => {
  const formikProps = useFormikContext<FormLeasePayment>();
  const lookups = useLookupCodeHelpers();
  useCalculateActualGst(!!isGstEligible);
  const paymentMethodOptions = lookups.getOptionsByType(API.LEASE_PAYMENT_METHOD_TYPES);
  return (
    <Styled.StyledFormBody>
      <FastDatePicker
        required
        label={isReceived ? 'Received date:' : 'Sent date:'}
        field="receivedDate"
        formikProps={formikProps}
      />
      <Select label="Method:" field="leasePaymentMethodType.id" options={paymentMethodOptions} />
      <FastCurrencyInput formikProps={formikProps} label="Total received ($)" field="amountTotal" />
      <Styled.ActualPaymentBox>
        <Styled.FlexRight>
          <TooltipIcon
            toolTipId="actual-calculation-tooltip"
            toolTip="If left blank, these values are calculated based on the total received. Enter values here only to override the system calculation."
          />
        </Styled.FlexRight>
        <InlineFlexDiv>
          <FastCurrencyInput
            formikProps={formikProps}
            label="Expected payment ($)"
            field="amountPreTax"
            innerClassName="small"
          />
          <FastCurrencyInput
            formikProps={formikProps}
            label="GST ($)"
            field="amountGst"
            innerClassName="small"
          />
        </InlineFlexDiv>
      </Styled.ActualPaymentBox>
    </Styled.StyledFormBody>
  );
};

export default PaymentFormContent;
