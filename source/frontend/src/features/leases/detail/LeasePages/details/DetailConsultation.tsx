import { useFormikContext } from 'formik';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { Api_Lease, Api_LeaseConsultation } from '@/models/api/Lease';

export interface IDetailConsultationProps {
  nameSpace?: string;
}

/**
 * Sub-form containing lease detail administration fields
 * @param {IDetailConsultationProps} param0
 */
export const DetailConsultation: React.FunctionComponent<
  React.PropsWithChildren<IDetailConsultationProps>
> = ({ nameSpace }) => {
  const { values, setFieldValue } = useFormikContext<Api_Lease>();

  const { getByType } = useLookupCodeHelpers();
  const consultationTypes = getByType(API.CONSULTATION_TYPES);

  // Not all consultations might be coming from the backend. Add the ones missing.
  if (values.consultations.length !== consultationTypes.length) {
    const newConsultations: Api_LeaseConsultation[] = [];

    consultationTypes.forEach(consultationType => {
      const newConsultation: Api_LeaseConsultation = {
        id: 0,
        parentLeaseId: values.id || 0,
        consultationType: {
          id: consultationType.id.toString(),
          description: consultationType.name,
        },
        consultationStatusType: { id: 'UNKNOWN', description: 'Unknown' },
        otherDescription: null,
        rowVersion: 0,
      };

      // If there is a consultation with the type, set the status to the existing one
      let existingConsultation = values.consultations.find(
        consultation => consultation.consultationType === consultationType.id,
      );
      if (existingConsultation !== undefined) {
        newConsultation.id = existingConsultation.id;
        newConsultation.consultationStatusType = existingConsultation.consultationStatusType;
        newConsultation.rowVersion = existingConsultation.rowVersion;
      }
      newConsultations.push(newConsultation);
    });
    setFieldValue('consultations', newConsultations);
  }

  const generateLabel = (consultation: Api_LeaseConsultation): string => {
    var label = consultation.consultationType?.description || '';
    if (consultation.otherDescription !== undefined && consultation.otherDescription !== null) {
      label += ' | ' + consultation.otherDescription;
    }

    return label;
  };

  return (
    <Section header="Consultation" initiallyExpanded isCollapsable>
      {values.consultations.map((consultation, index) => (
        <SectionField
          key={`consultations-${consultation.consultationType?.id}`}
          label={generateLabel(consultation)}
          labelWidth="4"
          contentWidth="8"
        >
          {consultation.consultationStatusType?.description}
        </SectionField>
      ))}
    </Section>
  );
};

export default DetailConsultation;
