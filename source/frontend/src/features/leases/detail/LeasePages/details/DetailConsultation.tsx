import { useFormikContext } from 'formik';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_ConsultationLease } from '@/models/api/generated/ApiGen_Concepts_ConsultationLease';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { exists } from '@/utils/utils';

export interface IDetailConsultationProps {
  nameSpace?: string;
}

/**
 * Sub-form containing lease detail administration fields
 * @param {IDetailConsultationProps} param0
 */
export const DetailConsultation: React.FunctionComponent<
  React.PropsWithChildren<IDetailConsultationProps>
> = () => {
  const { values, setFieldValue } = useFormikContext<ApiGen_Concepts_Lease>();

  const { getByType } = useLookupCodeHelpers();
  const consultationTypes = getByType(API.CONSULTATION_TYPES);

  // Not all consultations might be coming from the backend. Add the ones missing.
  if (values.consultations?.length !== consultationTypes.length) {
    const newConsultations: ApiGen_Concepts_ConsultationLease[] = [];

    consultationTypes.forEach(consultationType => {
      const newConsultation: ApiGen_Concepts_ConsultationLease = {
        id: 0,
        parentLeaseId: values.id || 0,
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
      const existingConsultation = values.consultations?.find(
        consultation => consultation.consultationType?.id === consultationType.id,
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

  const generateLabel = (consultation: ApiGen_Concepts_ConsultationLease): string => {
    let label = consultation.consultationType?.description || '';
    if (exists(consultation.otherDescription)) {
      label += ' | ' + consultation.otherDescription;
    }

    return label;
  };

  return (
    <Section header="Consultation" initiallyExpanded isCollapsable>
      {values.consultations?.map(consultation => (
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
