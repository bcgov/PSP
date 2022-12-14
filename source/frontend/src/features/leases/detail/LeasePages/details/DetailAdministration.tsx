import { Input } from 'components/common/form';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import * as React from 'react';
import styled from 'styled-components';
import { prettyFormatDate } from 'utils';
import { withNameSpace } from 'utils/formUtils';

export interface IDetailAdministrationProps {
  nameSpace?: string;
  disabled?: boolean;
}

/**
 * Sub-form containing lease detail administration fields
 * @param {IDetailAdministrationProps} param0
 */
export const DetailAdministration: React.FunctionComponent<
  React.PropsWithChildren<IDetailAdministrationProps>
> = ({ nameSpace, disabled }) => {
  const { values } = useFormikContext<IFormLease>();
  return (
    <>
      <Section initiallyExpanded={true} isCollapsable={true} header="Administration">
        <SectionField label="Program">
          <LargeTextInput disabled={disabled} field={withNameSpace(nameSpace, 'programName')} />
          {values.otherProgramType && values?.programType?.id === 'OTHER' && (
            <LargeTextInput
              disabled={disabled}
              field={withNameSpace(nameSpace, 'otherProgramType')}
            />
          )}
        </SectionField>
        <SectionField label="Account Type">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'type.description')} />
          {values.otherType && values?.type?.id === 'OTHER' && (
            <Input disabled={disabled} field={withNameSpace(nameSpace, 'otherType')} />
          )}
        </SectionField>
        <SectionField label="Receivable To">
          <Input
            disabled={disabled}
            field={withNameSpace(nameSpace, 'paymentReceivableType.description')}
          />
        </SectionField>
        <SectionField label="Category">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'categoryType.description')} />
          {values?.categoryType?.id === 'OTHER' && values.otherCategoryType && (
            <Input disabled={disabled} field={withNameSpace(nameSpace, 'otherCategoryType')} />
          )}
        </SectionField>
        <SectionField label="Purpose">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'purposeType.description')} />
          {values?.purposeType?.id === 'OTHER' && values.otherPurposeType && (
            <Input disabled={disabled} field={withNameSpace(nameSpace, 'otherPurposeType')} />
          )}
        </SectionField>
        <SectionField label="Initiator">
          <Input
            disabled={disabled}
            field={withNameSpace(nameSpace, 'initiatorType.description')}
          />
        </SectionField>
        <SectionField label="Responsibility">
          <Input
            disabled={disabled}
            field={withNameSpace(nameSpace, 'responsibilityType.description')}
          />
        </SectionField>
        <SectionField label="Effective Date">
          {prettyFormatDate(getIn(values, withNameSpace(nameSpace, 'responsibilityEffectiveDate')))}
        </SectionField>
        <SectionField label="MoTI contact">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'motiName')} />
        </SectionField>
        <SectionField label="Intended Use">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'description')} />
        </SectionField>
      </Section>
    </>
  );
};

const LargeTextInput = styled(Input)`
  input.form-control {
    font-size: 1.8rem;
  }
`;
export default DetailAdministration;
