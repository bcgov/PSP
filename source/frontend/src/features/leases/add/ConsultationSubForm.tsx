import { Input, Select } from 'components/common/form';
import * as API from 'constants/API';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { FormikProps } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';

import { FormLeaseConsultation, LeaseFormModel } from '../models';

export interface IConsultationSubFormProps {
  formikProps: FormikProps<LeaseFormModel>;
}

const ConsultationSubForm: React.FunctionComponent<
  React.PropsWithChildren<IConsultationSubFormProps>
> = ({ formikProps }) => {
  const { values, setFieldValue } = formikProps;
  const { consultations } = values;
  const { getOptionsByType, getByType } = useLookupCodeHelpers();
  const consultationTypes = getByType(API.CONSULTATION_TYPES);
  const consultationStatusTypes = getOptionsByType(API.CONSULTATION_STATUS_TYPES);

  // Not all consultations might be comming from the backend. Add the ones missing.
  if (consultations.length !== consultationTypes.length) {
    const newConsultations: FormLeaseConsultation[] = [];

    consultationTypes.forEach(x => {
      const newConsultation = FormLeaseConsultation.fromApiLookup(values.id || 0, x);

      // If there is a consultation with the type, set the status to the existing one
      let existingConsultation = consultations.find(y => y.consultationType === x.id);
      if (existingConsultation !== undefined) {
        newConsultation.id = existingConsultation.id;
        newConsultation.consultationStatusType = existingConsultation.consultationStatusType;
        newConsultation.consultationStatusTypeDescription =
          existingConsultation.consultationStatusTypeDescription;
        newConsultation.rowVersion = existingConsultation.rowVersion;
      }
      newConsultations.push(newConsultation);
    });
    setFieldValue('consultations', newConsultations);
  }

  return (
    <Section header="Consultation" isCollapsable initiallyExpanded>
      {values.consultations.map((a, i) => (
        <SectionField
          key={`consultations.${i}`}
          label={a.consultationTypeDescription}
          labelWidth="4"
          contentWidth="8"
        >
          <Row>
            <Col>
              <Select
                field={`consultations.${i}.consultationStatusType`}
                options={consultationStatusTypes}
                required
              />
            </Col>
            <Col>
              {a.consultationType === 'OTHER' && (
                <Input
                  field={`consultations.${i}.consultationTypeOtherDescription`}
                  placeholder="Describe other"
                  required
                />
              )}
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
