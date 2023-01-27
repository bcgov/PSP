import {
  InlineCol,
  InlineFastDatePicker,
  InlineInput,
  InlineSelect,
} from 'components/common/form/styles';
import * as API from 'constants/API';
import { FormikProps } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { IAddFormLease } from 'interfaces';
import * as React from 'react';
import { useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';

import { LeaseH3 } from '../detail/styles';
import * as Styled from './styles';

export interface IAdministrationSubFormProps {
  formikProps: FormikProps<IAddFormLease>;
}

const AdministrationSubForm: React.FunctionComponent<IAdministrationSubFormProps> = ({
  formikProps,
}) => {
  const { values, setFieldValue } = formikProps;
  const { categoryType, type, purposeType, programType } = values;
  const { getOptionsByType } = useLookupCodeHelpers();
  const paymentReceivableTypes = getOptionsByType(API.LEASE_PAYMENT_RECEIVABLE_TYPES);
  const programTypes = getOptionsByType(API.LEASE_PROGRAM_TYPES);
  const types = getOptionsByType(API.LEASE_TYPES);
  const categoryTypes = getOptionsByType(API.LEASE_CATEGORY_TYPES);
  const purposeTypes = getOptionsByType(API.LEASE_PURPOSE_TYPES);
  const initiatorTypes = getOptionsByType(API.LEASE_INITIATOR_TYPES);
  const responsibilityTypes = getOptionsByType(API.LEASE_RESPONSIBILITY_TYPES);
  const regionTypes = getOptionsByType(API.REGION_TYPES);

  //clear the associated other fields if the corresponding type has its value changed from other to something else.
  useEffect(() => {
    if (!!categoryType && categoryType !== 'OTHER') {
      setFieldValue('otherCategoryType', '');
    }
    if (!!type && type !== 'OTHER') {
      setFieldValue('otherType', '');
    }
    if (!!type && !isLeaseCategoryVisible(type)) {
      setFieldValue('otherCategoryType', '');
      setFieldValue('categoryType', '');
    }
    if (!!purposeType && purposeType !== 'OTHER') {
      setFieldValue('otherPurposeType', '');
    }
    if (!!programType && programType !== 'OTHER') {
      setFieldValue('otherProgramType', '');
    }
  }, [categoryType, type, purposeType, programType, setFieldValue]);

  useEffect(() => {
    if (!!type && !isLeaseCategoryVisible(type)) {
      setFieldValue('categoryType', '');
    }
  }, [type, setFieldValue]);

  return (
    <>
      <Row>
        <Col>
          <LeaseH3>Administration</LeaseH3>
        </Col>
      </Row>
      <Row>
        <Col>
          <InlineSelect
            label="Receivable or Payable:"
            required
            field="paymentReceivableType"
            options={paymentReceivableTypes}
            placeholder="Select"
          />
        </Col>
      </Row>
      <Row>
        <Col>
          <Styled.LargeInlineInput label="MOTI Contact:" field="motiName" />
        </Col>
      </Row>
      <Row>
        <Col>
          <InlineSelect
            label="MOTI Region:"
            field="region"
            options={regionTypes}
            placeholder="Select region"
            required
          />
        </Col>
      </Row>
      <Row>
        <InlineCol>
          <InlineSelect
            label="Program:"
            field="programType"
            options={programTypes}
            placeholder="Select program"
            required
          />
          {values?.programType === 'OTHER' && (
            <InlineInput label="Other Program:" field="otherProgramType" required />
          )}
        </InlineCol>
      </Row>
      <Row>
        <InlineCol>
          <InlineSelect
            label="Type:"
            field="type"
            options={types}
            placeholder="Select type"
            required
          />
          {values?.type === 'OTHER' && (
            <InlineInput label="Describe other:" field="otherType" required />
          )}
        </InlineCol>
      </Row>
      {isLeaseCategoryVisible(values?.type) && (
        <Row>
          <InlineCol>
            <InlineSelect
              label="Category:"
              field="categoryType"
              options={categoryTypes}
              placeholder="Select category"
              required
            />
            {values?.categoryType === 'OTHER' && (
              <InlineInput label="Describe other:" field="otherCategoryType" required />
            )}
          </InlineCol>
        </Row>
      )}
      <Row>
        <InlineCol>
          <InlineSelect
            label="Purpose:"
            required
            field="purposeType"
            options={purposeTypes}
            placeholder="Select purpose"
          />
          {values?.purposeType === 'OTHER' && (
            <InlineInput label="Describe other:" field="otherPurposeType" required />
          )}
        </InlineCol>
      </Row>
      <Row>
        <Col>
          <InlineSelect
            label="Initiator:"
            field="initiatorType"
            placeholder="Select initiator"
            options={initiatorTypes}
            tooltip="Where did this lease/license initiate?"
          />
        </Col>
      </Row>
      <Row>
        <InlineCol>
          <InlineSelect
            label="Responsibility:"
            field="responsibilityType"
            placeholder="Select group responsible"
            options={responsibilityTypes}
            tooltip="Who is currently responsible?"
          />
          <InlineFastDatePicker
            formikProps={formikProps}
            label="Effective date of responsibility:"
            field="responsibilityEffectiveDate"
          />
        </InlineCol>
      </Row>
    </>
  );
};

export const isLeaseCategoryVisible = (typeId?: string) => {
  const visibleCategoryTypes = ['LSGRND', 'LSREG', 'LSUNREG'];
  return !!typeId && visibleCategoryTypes.includes(typeId);
};

export default AdministrationSubForm;
