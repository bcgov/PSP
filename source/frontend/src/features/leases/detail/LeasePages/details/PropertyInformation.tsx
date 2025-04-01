import { FieldArrayRenderProps } from 'formik';
import styled from 'styled-components';

import { SectionField } from '@/components/common/Section/SectionField';
import AreaContainer from '@/components/measurements/AreaContainer';
import { AreaUnitTypes } from '@/constants';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';
import { isValidId, pidFormatter } from '@/utils';

import AddressView from '../AddressView';

export interface IPropertyInformationProps {
  hideAddress?: boolean;
  property: ApiGen_Concepts_PropertyLease;
}

/**
 * Sub-form displaying a property associated to the current lease.
 * @param {IPropertyInformationProps} param0
 */
export const PropertyInformation: React.FunctionComponent<
  React.PropsWithChildren<IPropertyInformationProps & Partial<FieldArrayRenderProps>>
> = ({ property, hideAddress }) => {
  const landArea: number | null = property.leaseArea;

  const areaUnitType: ApiGen_Base_CodeType<string> | null = property?.areaUnitType;

  const legalDescription: string = property.property.landLegalDescription;
  const pid: number | null = property.property.pid;

  const pidText = isValidId(pid) ? `PID: ${pidFormatter(pid.toString())}` : '';
  return (
    <StyledPropertyInfo>
      <SectionField label="PID" labelWidth={{ xs: 3 }}>
        {pidText}
      </SectionField>
      <SectionField label="Descriptive name" labelWidth={{ xs: 3 }}>
        {property.propertyName}
      </SectionField>
      <SectionField label="Area included" labelWidth={{ xs: 3 }} className="py-4">
        <AreaContainer
          landArea={landArea}
          unitCode={areaUnitType?.id ?? AreaUnitTypes.SquareMeters}
        />
      </SectionField>
      {!hideAddress ? (
        <SectionField label="Address" labelWidth={{ xs: 3 }} className="py-2">
          <AddressView address={property.property.address} />
        </SectionField>
      ) : null}
      <SectionField label="Legal description" labelWidth={{ xs: 3 }}>
        {legalDescription}
      </SectionField>

      <hr />
    </StyledPropertyInfo>
  );
};

export default PropertyInformation;
const StyledPropertyInfo = styled.div`
  margin-top: 4rem;
`;
