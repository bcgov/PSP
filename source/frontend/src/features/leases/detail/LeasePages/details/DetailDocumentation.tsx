import { Input, TextArea } from 'components/common/form';
import { YesNoSelect } from 'components/common/form/YesNoSelect';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import * as React from 'react';
import styled from 'styled-components';
import { withNameSpace } from 'utils/formUtils';

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
          <TextAreaInput
            disabled={disabled}
            field={withNameSpace(nameSpace, 'documentationReference')}
          />
        </SectionField>
        <SectionField label="LIS #" labelWidth="3">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'tfaFileNumber')} />
        </SectionField>
        <SectionField label="PS #" labelWidth="3">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'psFileNo')} />
        </SectionField>
        <SectionField label="Lease Notes" labelWidth="3">
          <TextArea disabled={disabled} field={withNameSpace(nameSpace, 'note')} />
        </SectionField>
      </Section>
    </>
  );
};

const TextAreaInput = styled(TextArea)`
  padding: 0.6rem 1.2rem;
`;
export default DetailDocumentation;
