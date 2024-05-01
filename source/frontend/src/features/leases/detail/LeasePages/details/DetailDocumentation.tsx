import { getIn, useFormikContext } from 'formik';

import { Input } from '@/components/common/form';
import { YesNoSelect } from '@/components/common/form/YesNoSelect';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { withNameSpace } from '@/utils/formUtils';

export interface IDetailDocumentationProps {
  nameSpace?: string;
  disabled?: boolean;
}

/**
 * Sub-form containing lease detail administration fields
 * @param {IDetailDocumentationProps} param0
 */
export const DetailDocumentation: React.FunctionComponent<
  React.PropsWithChildren<IDetailDocumentationProps>
> = ({ nameSpace, disabled }) => {
  const formikProps = useFormikContext<ApiGen_Concepts_Lease>();
  const note = getIn(formikProps.values, withNameSpace(nameSpace, 'note'));
  const documentationReference = getIn(
    formikProps.values,
    withNameSpace(nameSpace, 'documentationReference'),
  );
  return (
    <>
      <Section initiallyExpanded={true} isCollapsable={true} header="Documentation">
        <SectionField label="Physical copy exists" labelWidth="3">
          <YesNoSelect disabled={disabled} field={withNameSpace(nameSpace, 'hasPhysicalLicense')} />
        </SectionField>
        <SectionField label="Digital copy exists" labelWidth="3">
          <YesNoSelect disabled={disabled} field={withNameSpace(nameSpace, 'hasDigitalLicense')} />
        </SectionField>
        <SectionField label="Document location" labelWidth="3">
          {documentationReference}
        </SectionField>
        <SectionField label="LIS #" labelWidth="3">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'tfaFileNumber')} />
        </SectionField>
        <SectionField label="PS #" labelWidth="3">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'psFileNo')} />
        </SectionField>
        <SectionField label="Lease notes" labelWidth="3">
          {note}
        </SectionField>
      </Section>
    </>
  );
};
export default DetailDocumentation;
