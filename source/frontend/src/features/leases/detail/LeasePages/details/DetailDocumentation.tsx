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
        <SectionField label="Physical copy exists">
          <YesNoSelect disabled={disabled} field={withNameSpace(nameSpace, 'hasPhysicalLicense')} />
        </SectionField>
        <SectionField label="Digital copy exists">
          <YesNoSelect disabled={disabled} field={withNameSpace(nameSpace, 'hasDigitalLicense')} />
        </SectionField>
        <SectionField label="Document location">
          <TextAreaInput
            disabled={disabled}
            field={withNameSpace(nameSpace, 'documentationReference')}
          />
        </SectionField>
        <SectionField label="LIS #">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'tfaFileNumber')} />
        </SectionField>
        <SectionField label="PS #">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'psFileNo')} />
        </SectionField>
        <SectionField label="Lease Notes">
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
