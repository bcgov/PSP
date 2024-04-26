import { FormikProps } from 'formik/dist/types';
import { Col, Row } from 'react-bootstrap';

import { Input, Select } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';

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

  useDeepCompareEffect(() => {
    // Not all consultations might be coming from the backend. Add the ones missing.
    if (consultations.length !== consultationTypes.length) {
      const newConsultations: FormLeaseConsultation[] = [];

      consultationTypes.forEach(consultationType => {
        const newConsultation = FormLeaseConsultation.fromApiLookup(
          values.id || 0,
          consultationType,
        );

        // If there is a consultation with the type, set the status to the existing one
        const existingConsultation = consultations.find(
          consultation => consultation.consultationType === consultationType.id,
        );
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
  }, [consultationTypes, consultations, setFieldValue, values.id]);

  return (
    <Section header="Consultation" isCollapsable initiallyExpanded>
      {values.consultations.map((consultation, i) => (
        <SectionField
          key={`consultations-${consultation.consultationTypeDescription}`}
          label={consultation.consultationTypeDescription}
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
              {consultation.consultationType === 'OTHER' && (
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

export default ConsultationSubForm;
