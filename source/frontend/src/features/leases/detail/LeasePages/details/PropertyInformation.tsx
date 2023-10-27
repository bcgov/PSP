import { FieldArrayRenderProps, getIn, useFormikContext } from 'formik';
import * as React from 'react';
import styled from 'styled-components';

import { Input } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import { Api_Lease } from '@/models/api/Lease';
import { formatNumber, pidFormatter } from '@/utils';
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
  const formikProps = useFormikContext<Api_Lease>();
  const landArea = getIn(formikProps.values, withNameSpace(nameSpace, 'leaseArea'));
  const areaUnitType = getIn(formikProps.values, withNameSpace(nameSpace, 'areaUnitType'));
  const legalDescription = getIn(
    formikProps.values,
    withNameSpace(nameSpace, 'property.legalDescription'),
  );
  const pid = getIn(formikProps.values, withNameSpace(nameSpace, 'property.pid'));
  const pidText = pid ? `PID: ${pidFormatter(pid)}` : '';
  return (
    <StyledPropertyInfo>
      <SectionField label="PID" labelWidth="3">
        {pidText}
      </SectionField>
      <SectionField label="Descriptive name" labelWidth="3">
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'propertyName')} />
      </SectionField>
      <SectionField label="Area included" labelWidth="3">
        {formatNumber(landArea, 2, 2)}{' '}
        {areaUnitType?.description ? (
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
