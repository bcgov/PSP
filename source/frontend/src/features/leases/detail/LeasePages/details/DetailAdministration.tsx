import Multiselect from 'multiselect-react-dropdown';

import { readOnlyMultiSelectStyle } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_CodeTypes_LeasePurposeTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePurposeTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists, prettyFormatDate } from '@/utils';

export interface IDetailAdministrationProps {
  lease: ApiGen_Concepts_Lease;
}

/**
 * Sub-form containing lease detail administration fields
 * @param {IDetailAdministrationProps} param0
 */
export const DetailAdministration: React.FunctionComponent<
  React.PropsWithChildren<IDetailAdministrationProps>
> = ({ lease }: IDetailAdministrationProps) => {
  const responsibilityDate = lease.responsibilityEffectiveDate;

  const intendedUse = lease?.description;
  const leasePurposes = lease.leasePurposes;

  const selectedPurposes: ApiGen_Base_CodeType<string>[] =
    leasePurposes?.map(x => x.leasePurposeTypeCode).filter(exists) ?? [];

  const otherPurposeIndex = leasePurposes.findIndex(
    x => x.leasePurposeTypeCode.id === ApiGen_CodeTypes_LeasePurposeTypes.OTHER,
  );

  return (
    <>
      <Section initiallyExpanded={true} isCollapsable={true} header="Administration">
        <SectionField label="Program" labelWidth={{ xs: 3 }}>
          {lease.programName}
          {lease.otherProgramType && lease?.programType?.id === 'OTHER' && (
            <>{lease.otherProgramType}</>
          )}
        </SectionField>
        <SectionField label="Account type" labelWidth={{ xs: 3 }}>
          {lease.type?.description}
          {lease.otherType && lease?.type?.id === 'OTHER' && <>{lease.otherType}</>}
        </SectionField>
        <SectionField label="Receivable to" labelWidth={{ xs: 3 }}>
          {lease.paymentReceivableType?.description}
        </SectionField>
        <SectionField label="Purpose(s)" labelWidth={{ xs: 3 }}>
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
          ) && <>{lease.leasePurposes[otherPurposeIndex].purposeOtherDescription}</>}
        </SectionField>
        <SectionField label="Initiator" labelWidth={{ xs: 3 }}>
          {lease.initiatorType?.description}
        </SectionField>
        <SectionField label="Responsibility" labelWidth={{ xs: 3 }}>
          {lease.responsibilityType?.description}
        </SectionField>
        <SectionField label="Effective date" labelWidth={{ xs: 3 }}>
          {prettyFormatDate(responsibilityDate)}
        </SectionField>
        <SectionField label="MOTI contact" labelWidth={{ xs: 3 }}>
          {lease.motiName}
        </SectionField>
        <SectionField label="MOTI region" labelWidth={{ xs: 3 }}>
          {lease.region?.description}
        </SectionField>
        <SectionField
          label="Intended use"
          labelWidth={{ xs: 3 }}
          tooltip="The purpose for which the license is issued, as per the agreement"
        >
          {intendedUse}
        </SectionField>
        <SectionField
          label="Primary arbitration city"
          labelWidth={{ xs: 3 }}
          valueTestId="primaryArbitrationCity"
        >
          {lease.primaryArbitrationCity}
        </SectionField>
      </Section>
    </>
  );
};

export default DetailAdministration;
