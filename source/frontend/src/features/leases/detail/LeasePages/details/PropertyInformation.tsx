import { FieldArrayRenderProps, getIn, useFormikContext } from 'formik';
import styled from 'styled-components';

import { Input } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { formatNumber, isValidId, isValidString, pidFormatter } from '@/utils';
import { withNameSpace } from '@/utils/formUtils';

import AddressSubForm from '../AddressSubForm';

export interface IPropertyInformationProps {
  nameSpace: string;
  hideAddress?: boolean;
  disabled?: boolean;
}

/**
 * Sub-form displaying a property associated to the current lease.
 * @param {IPropertyInformationProps} param0
 */
export const PropertyInformation: React.FunctionComponent<
  React.PropsWithChildren<IPropertyInformationProps & Partial<FieldArrayRenderProps>>
> = ({ nameSpace, disabled, hideAddress }) => {
  const formikProps = useFormikContext<ApiGen_Concepts_Lease>();

  const landArea: number | null = getIn(formikProps.values, withNameSpace(nameSpace, 'leaseArea'));

  const areaUnitType: ApiGen_Base_CodeType<string> | null = getIn(
    formikProps.values,
    withNameSpace(nameSpace, 'areaUnitType'),
  );

  const legalDescription: string = getIn(
    formikProps.values,
    withNameSpace(nameSpace, 'property.landLegalDescription'),
  );
  const pid: number | null = getIn(formikProps.values, withNameSpace(nameSpace, 'property.pid'));

  const pidText = isValidId(pid) ? `PID: ${pidFormatter(pid.toString())}` : '';
  return (
    <StyledPropertyInfo>
      <SectionField label="PID" labelWidth="3">
        {pidText}
      </SectionField>
      <SectionField label="Descriptive name" labelWidth="3">
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'propertyName')} />
      </SectionField>
      <SectionField label="Area included" labelWidth="3">
        {formatNumber(landArea || 0, 2, 2)}{' '}
        {isValidString(areaUnitType?.description) ? (
          `${areaUnitType?.description}.`
        ) : (
          <>
            m<sup>2</sup>
          </>
        )}
      </SectionField>
      {!hideAddress ? (
        <SectionField label="Address" labelWidth="3">
          <AddressSubForm
            nameSpace={withNameSpace(nameSpace, 'property.address')}
            disabled={disabled}
          />
        </SectionField>
      ) : null}
      <SectionField label="Legal description" labelWidth="3">
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
