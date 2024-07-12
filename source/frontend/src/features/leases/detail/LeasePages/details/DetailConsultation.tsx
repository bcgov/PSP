import { useFormikContext } from 'formik';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Concepts_ConsultationLease } from '@/models/api/generated/ApiGen_Concepts_ConsultationLease';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
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
  const { values } = useFormikContext<ApiGen_Concepts_Lease>();

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
