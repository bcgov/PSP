import { FastDatePicker, Input, Select } from 'components/common/form';
import { InlineInput } from 'components/common/form/styles';
import * as API from 'constants/API';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { FormikProps } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import * as React from 'react';
import { useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';

import { LeaseFormModel } from '../models';
import * as Styled from './styles';

export interface IAdministrationSubFormProps {
  formikProps: FormikProps<LeaseFormModel>;
}

const AdministrationSubForm: React.FunctionComponent<
  React.PropsWithChildren<IAdministrationSubFormProps>
> = ({ formikProps }) => {
  const { values, setFieldValue } = formikProps;
  const { categoryTypeCode, leaseTypeCode, purposeTypeCode, programTypeCode } = values;
  const { getOptionsByType } = useLookupCodeHelpers();
  const programTypes = getOptionsByType(API.LEASE_PROGRAM_TYPES);
  const types = getOptionsByType(API.LEASE_TYPES);
  const categoryTypes = getOptionsByType(API.LEASE_CATEGORY_TYPES);
  const purposeTypes = getOptionsByType(API.LEASE_PURPOSE_TYPES);
  const initiatorTypes = getOptionsByType(API.LEASE_INITIATOR_TYPES);
  const responsibilityTypes = getOptionsByType(API.LEASE_RESPONSIBILITY_TYPES);
  const regionTypes = getOptionsByType(API.REGION_TYPES);

  //clear the associated other fields if the corresponding type has its value changed from other to something else.
  useEffect(() => {
    if (!!categoryTypeCode && categoryTypeCode !== 'OTHER') {
      setFieldValue('otherCategoryTypeDescription', '');
    }
    if (!!leaseTypeCode && leaseTypeCode !== 'OTHER') {
      setFieldValue('otherLeaseTypeDescription', '');
    }
    if (!!leaseTypeCode && !isLeaseCategoryVisible(leaseTypeCode)) {
      setFieldValue('otherCategoryTypeDescription', '');
      setFieldValue('categoryTypeCode', '');
    }
    if (!!purposeTypeCode && purposeTypeCode !== 'OTHER') {
      setFieldValue('otherPurposeTypeDescription', '');
    }
    if (!!programTypeCode && programTypeCode !== 'OTHER') {
      setFieldValue('otherProgramTypeDescription', '');
    }
  }, [categoryTypeCode, leaseTypeCode, purposeTypeCode, programTypeCode, setFieldValue]);

  useEffect(() => {
    if (!!leaseTypeCode && !isLeaseCategoryVisible(leaseTypeCode)) {
      setFieldValue('categoryTypeCode', '');
    }
  }, [leaseTypeCode, setFieldValue]);

  return (
    <Section header="Administration">
      <SectionField label="MOTI contact" labelWidth="2" contentWidth="8">
        <InlineInput field="motiName" />
      </SectionField>

      <SectionField label="MOTI region" labelWidth="2" contentWidth="4" required>
        <Select field="regionId" options={regionTypes} placeholder="Select region" required />
      </SectionField>
      <Row>
        <Col>
          <SectionField label="Program" required>
            <Select
              field="programTypeCode"
              options={programTypes}
              placeholder="Select program"
              required
            />
          </SectionField>
        </Col>
        <Col>
          {values?.programTypeCode === 'OTHER' && (
            <SectionField label="Other Program" required>
              <Input field="otherProgramTypeDescription" required />
            </SectionField>
          )}
        </Col>
      </Row>
      <Row>
        <Col>
          <SectionField label="Type" required>
            <Select field="leaseTypeCode" options={types} placeholder="Select type" required />
          </SectionField>
        </Col>
        <Col>
          {values?.leaseTypeCode === 'OTHER' && (
            <SectionField label="Describe other" required>
              <Input field="otherLeaseTypeDescription" required />
            </SectionField>
          )}
        </Col>
      </Row>

      {isLeaseCategoryVisible(values?.leaseTypeCode) && (
        <Row>
          <Col>
            <SectionField label="Category" required>
              <Select
                field="categoryTypeCode"
                options={categoryTypes}
                placeholder="Select category"
                required
              />
            </SectionField>
          </Col>
          <Col>
            {values?.categoryTypeCode === 'OTHER' && (
              <SectionField label="Describe other" required>
                <Input field="otherCategoryTypeDescription" required />
              </SectionField>
            )}
          </Col>
        </Row>
      )}
      <Row>
        <Col>
          <SectionField label="Purpose" required>
            <Select
              required
              field="purposeTypeCode"
              options={purposeTypes}
              placeholder="Select purpose"
            />
          </SectionField>
        </Col>
        <Col>
          {values?.purposeTypeCode === 'OTHER' && (
            <SectionField label="Describe other" required>
              <Input field="otherPurposeTypeDescription" required />
            </SectionField>
          )}
        </Col>
      </Row>
      <SectionField
        label="Initiator"
        tooltip="Where did this lease/license initiate?"
        labelWidth="2"
        contentWidth="4"
      >
        <Select field="initiatorTypeCode" placeholder="Select initiator" options={initiatorTypes} />
      </SectionField>
      <Row>
        <Col>
          <SectionField label="Responsibility" tooltip="Who is currently responsible?">
            <Select
              field="responsibilityTypeCode"
              placeholder="Select group responsible"
              options={responsibilityTypes}
            />
          </SectionField>
        </Col>

        <Col>
          <SectionField label="Effective date">
            <FastDatePicker formikProps={formikProps} field="responsibilityEffectiveDate" />
          </SectionField>
        </Col>
      </Row>
      <SectionField label="Intended use">
        <Styled.MediumTextArea field="description" />
      </SectionField>
    </Section>
  );
};

export const isLeaseCategoryVisible = (typeId?: string) => {
  const visibleCategoryTypes = ['LSGRND', 'LSREG', 'LSUNREG'];
  return !!typeId && visibleCategoryTypes.includes(typeId);
};

export default AdministrationSubForm;
