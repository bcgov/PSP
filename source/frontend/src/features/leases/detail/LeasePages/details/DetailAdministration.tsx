import { getIn, useFormikContext } from 'formik';
import Multiselect from 'multiselect-react-dropdown';
import styled from 'styled-components';

import { Input, readOnlyMultiSelectStyle } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_CodeTypes_LeasePurposeTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePurposeTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeasePurpose } from '@/models/api/generated/ApiGen_Concepts_LeasePurpose';
import { exists, prettyFormatDate } from '@/utils';
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
> = ({ nameSpace, disabled }: IDetailAdministrationProps) => {
  const { values } = useFormikContext<ApiGen_Concepts_Lease>();
  const responsibilityDate = getIn(values, withNameSpace(nameSpace, 'responsibilityEffectiveDate'));
  const intendedUse = getIn(values, withNameSpace(nameSpace, 'description'));
  const note = getIn(values, withNameSpace(nameSpace, 'note'));

  const leasePurposes: ApiGen_Concepts_LeasePurpose[] = getIn(
    values,
    withNameSpace(nameSpace, 'leasePurposes'),
  );

  const selectedPurposes: ApiGen_Base_CodeType<string>[] =
    leasePurposes?.map(x => x.leasePurposeTypeCode).filter(exists) ?? [];

  const otherPurposeIndex = leasePurposes.findIndex(
    x => x.leasePurposeTypeCode.id === ApiGen_CodeTypes_LeasePurposeTypes.OTHER,
  );

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
        <SectionField label="Purpose(s)" labelWidth="3">
          <Multiselect
            disable
            disablePreSelectedValues
            hidePlaceholder
            placeholder=""
            selectedValues={selectedPurposes}
            displayValue="description"
            style={readOnlyMultiSelectStyle}
          />
          {leasePurposes.some(
            x => x.leasePurposeTypeCode.id === ApiGen_CodeTypes_LeasePurposeTypes.OTHER,
          ) && (
            <Input
              disabled={disabled}
              field={withNameSpace(
                nameSpace,
                `leasePurposes[${otherPurposeIndex}].purposeOtherDescription`,
              )}
            />
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
        <SectionField label="MOTI contact" labelWidth="3">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'motiName')} />
        </SectionField>
        <SectionField label="MOTI region" labelWidth="3">
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'region.description')} />
        </SectionField>
        <SectionField
          label="Intended use"
          labelWidth="3"
          tooltip="The purpose for which the license is issued, as per the agreement"
        >
          {intendedUse}
        </SectionField>
        <SectionField
          label="Primary arbitration city"
          labelWidth="3"
          valueTestId="primaryArbitrationCity"
        >
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'primaryArbitrationCity')} />
        </SectionField>
        <SectionField label="Lease comments" labelWidth="3">
          {note}
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
