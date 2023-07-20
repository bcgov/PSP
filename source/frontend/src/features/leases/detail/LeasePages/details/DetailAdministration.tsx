import { getIn, useFormikContext } from 'formik';
import * as React from 'react';
import styled from 'styled-components';

import { Input } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { Api_Lease } from '@/models/api/Lease';
import { prettyFormatDate } from '@/utils';
import { withNameSpace } from '@/utils/formUtils';
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
  const { values } = useFormikContext<Api_Lease>();
  const responsibilityDate = getIn(values, withNameSpace(nameSpace, 'responsibilityEffectiveDate'));
  return (
    <>
      <Section initiallyExpanded={true} isCollapsable={true} header="Administration">
        <SectionField label="Program" labelWidth="3">
          <LargeTextInput disabled={disabled} field={withNameSpace(nameSpace, 'programName')} />
          {values.otherProgramType && values?.programType?.id === 'OTHER' && (
            <LargeTextInput
              disabled={disabled}
              field={withNameSpace(nameSpace, 'otherProgramType')}
            />
          )}
        </SectionField>
        <SectionField label="Account type" labelWidth="3">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'type.description')} />
          {values.otherType && values?.type?.id === 'OTHER' && (
            <Input disabled={disabled} field={withNameSpace(nameSpace, 'otherType')} />
          )}
        </SectionField>
        <SectionField label="Receivable to" labelWidth="3">
          <Input
            disabled={disabled}
            field={withNameSpace(nameSpace, 'paymentReceivableType.description')}
          />
        </SectionField>
        <SectionField label="Category" labelWidth="3">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'categoryType.description')} />
          {values?.categoryType?.id === 'OTHER' && values.otherCategoryType && (
            <Input disabled={disabled} field={withNameSpace(nameSpace, 'otherCategoryType')} />
          )}
        </SectionField>
        <SectionField label="Purpose" labelWidth="3">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'purposeType.description')} />
          {values?.purposeType?.id === 'OTHER' && values.otherPurposeType && (
            <Input disabled={disabled} field={withNameSpace(nameSpace, 'otherPurposeType')} />
          )}
        </SectionField>
        <SectionField label="Initiator" labelWidth="3">
          <Input
            disabled={disabled}
            field={withNameSpace(nameSpace, 'initiatorType.description')}
          />
        </SectionField>
        <SectionField label="Responsibility" labelWidth="3">
          <Input
            disabled={disabled}
            field={withNameSpace(nameSpace, 'responsibilityType.description')}
          />
        </SectionField>
        <SectionField label="Effective date" labelWidth="3">
          {prettyFormatDate(responsibilityDate)}
        </SectionField>
        <SectionField label="MoTI contact" labelWidth="3">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'motiName')} />
        </SectionField>
        <SectionField label="Intended use" labelWidth="3">
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
