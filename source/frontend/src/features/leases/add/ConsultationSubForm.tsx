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

export interface IConsultationSubFormProps {
  formikProps: FormikProps<LeaseFormModel>;
}

const ConsultationSubForm: React.FunctionComponent<
  React.PropsWithChildren<IConsultationSubFormProps>
> = ({ formikProps }) => {
  const { values, setFieldValue } = formikProps;
  const { categoryTypeCode, leaseTypeCode, purposeTypeCode, programTypeCode } = values;
  const { getOptionsByType, getByType } = useLookupCodeHelpers();
  const programTypes = getOptionsByType(API.LEASE_PROGRAM_TYPES);
  const types = getOptionsByType(API.LEASE_TYPES);
  const consultationTypes = getByType(API.CONSULTATION_TYPES);
  const consultationStatusTypes = getOptionsByType(API.CONSULTATION_STATUS_TYPES);

  //clear the associated other fields if the corresponding type has its value changed from other to something else.
  useEffect(() => {
    if (!!programTypeCode && programTypeCode !== 'OTHER') {
      setFieldValue('otherProgramTypeDescription', '');
    }
  }, [programTypeCode, setFieldValue]);

  useEffect(() => {
    if (!!leaseTypeCode && !isLeaseCategoryVisible(leaseTypeCode)) {
      setFieldValue('categoryTypeCode', '');
    }
  }, [leaseTypeCode, setFieldValue]);

  return (
    <Section header="Consultation">
      {consultationTypes.map((a, i) => (
        <SectionField key={`consultation-${a.name}`} label={a.name} labelWidth="4" contentWidth="8">
          <Row>
            <Col>
              <Select
                field="consultationa"
                options={consultationStatusTypes}
                placeholder="Unknown"
                required
              />
            </Col>
            <Col>
              {a.id === 'OTHER' && <Input field="regionId" placeholder="Describe other" required />}
            </Col>
          </Row>
        </SectionField>
      ))}
    </Section>
  );
};

export const isLeaseCategoryVisible = (typeId?: string) => {
  const visibleCategoryTypes = ['LSGRND', 'LSREG', 'LSUNREG'];
  return !!typeId && visibleCategoryTypes.includes(typeId);
};

export default ConsultationSubForm;
