import { FormikProps } from 'formik/dist/types';
import { Col, Row } from 'react-bootstrap';

import { Input, Select } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_ConsultationLease } from '@/models/api/generated/ApiGen_Concepts_ConsultationLease';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes';

import { LeaseFormModel } from '../models';

export interface IConsultationSubFormProps {
  formikProps: FormikProps<LeaseFormModel>;
}

const ConsultationSubForm: React.FunctionComponent<
  React.PropsWithChildren<IConsultationSubFormProps>
> = ({ formikProps }) => {
  const { values } = formikProps;
  const { getOptionsByType } = useLookupCodeHelpers();
  const consultationStatusTypes = getOptionsByType(API.CONSULTATION_STATUS_TYPES);

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

export const getConsultations = (
  lease: ApiGen_Concepts_Lease,
  consultationTypes: ILookupCode[],
) => {
  if (lease.consultations?.length !== consultationTypes.length) {
    const newConsultations: ApiGen_Concepts_ConsultationLease[] = [];

    consultationTypes.forEach(consultationType => {
      const newConsultation: ApiGen_Concepts_ConsultationLease = {
        id: 0,
        parentLeaseId: lease.id || 0,
        consultationType: {
          id: consultationType.id.toString(),
          description: consultationType.name,
          displayOrder: null,
          isDisabled: false,
        },
        consultationStatusType: {
          id: 'UNKNOWN',
          description: 'Unknown',
          displayOrder: null,
          isDisabled: false,
        },
        otherDescription: null,
        ...getEmptyBaseAudit(0),
      };

      // If there is a consultation with the type, set the status to the existing one
      const existingConsultation = lease.consultations?.find(
        consultation => consultation.consultationType?.id === consultationType.id,
      );
      if (existingConsultation !== undefined) {
        newConsultation.id = existingConsultation.id;
        newConsultation.consultationStatusType = existingConsultation.consultationStatusType;
        newConsultation.rowVersion = existingConsultation.rowVersion;
      }
      newConsultations.push(newConsultation);
    });
    return newConsultations;
  }
  return lease.consultations;
};

export default ConsultationSubForm;
