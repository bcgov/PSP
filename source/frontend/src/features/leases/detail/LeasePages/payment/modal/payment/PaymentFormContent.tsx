import { useFormikContext } from 'formik';
import { Col, Form, Row } from 'react-bootstrap';

import { FastCurrencyInput, FastDatePicker, Select } from '@/components/common/form';
import { InlineFlexDiv } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { round } from '@/utils';
import { getCurrencyCleanValue } from '@/utils/formUtils';

import { FormLeasePayment } from '../../models';
import * as Styled from '../../styles';

interface IPaymentFormContentProps {
  isReceived: boolean;
  isGstEligible: boolean;
  isVariable: boolean;
}

const PaymentFormContent: React.FunctionComponent<
  React.PropsWithChildren<IPaymentFormContentProps>
> = ({ isReceived, isGstEligible, isVariable }) => {
  const formikProps = useFormikContext<FormLeasePayment>();
  const lookups = useLookupCodeHelpers();
  const paymentMethodOptions = lookups.getOptionsByType(API.LEASE_PAYMENT_METHOD_TYPES);
  const categoryOptions = lookups.getOptionsByType(API.LEASE_PAYMENT_CATEGORY_TYPES);

  const { setFieldValue } = useFormikContext<FormLeasePayment>();
  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) : undefined;

  return (
    <Styled.StyledFormBody>
      <Row>
        <Col md={6}>
          <FastDatePicker
            required
            label={isReceived ? 'Received date:' : 'Sent date:'}
            field="receivedDate"
            formikProps={formikProps}
          />
        </Col>
      </Row>
      <Row>
        <Col md={6}>
          <Select
            label="Method:"
            field="leasePaymentMethodType.id"
            options={paymentMethodOptions}
          />
        </Col>
      </Row>
      {isVariable && (
        <Row>
          <Col md={6}>
            <Select
              label="Payment category:"
              field="leasePaymentCategoryTypeCode.id"
              options={categoryOptions}
            />
          </Col>
        </Row>
      )}
      <Row>
        <Col md={6}>
          <FastCurrencyInput
            formikProps={formikProps}
            label="Total received ($)"
            field="amountTotal"
            onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
              const cleanValue = getCurrencyCleanValue(e.target.value);
              if (gstDecimal && isGstEligible) {
                const calculatedPreTax = round(cleanValue / (gstDecimal / 100 + 1));
                const calculatedGst = round(cleanValue - calculatedPreTax);
                setFieldValue('amountGst', calculatedGst);
                setFieldValue('amountPreTax', calculatedPreTax);
              } else {
                setFieldValue('amountPreTax', cleanValue);
                setFieldValue('amountGst', '');
              }
              setFieldValue('amountTotal', cleanValue);
            }}
          />
        </Col>
      </Row>
      <Row>
        <Col>
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
        </Col>
      </Row>
      <Row>
        <Col>
          <Form.Control.Feedback type="invalid">
            {(formikProps.errors as any).form}
          </Form.Control.Feedback>
        </Col>
      </Row>
    </Styled.StyledFormBody>
  );
};

export default PaymentFormContent;
