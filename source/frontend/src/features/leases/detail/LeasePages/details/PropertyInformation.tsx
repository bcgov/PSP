import { Form, InputGroup } from 'components/common/form';
import * as Styled from 'features/leases/detail/styles';
import { getApiPropertyName } from 'features/properties/selector/utils';
import { FieldArrayRenderProps, getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import * as React from 'react';
import { withNameSpace } from 'utils/formUtils';

import AddressSubForm from '../AddressSubForm';

export interface IPropertyInformationProps {
  nameSpace: string;
  disabled?: boolean;
}

/**
 * Sub-form displaying a property associated to the current lease.
 * @param {IPropertyInformationProps} param0
 */
export const PropertyInformation: React.FunctionComponent<
  React.PropsWithChildren<IPropertyInformationProps & Partial<FieldArrayRenderProps>>
> = ({ nameSpace, disabled }) => {
  const formikProps = useFormikContext<IFormLease>();

  const areaUnitType = getIn(formikProps.values, withNameSpace(nameSpace, 'areaUnitType'));
  const landArea = getIn(formikProps.values, withNameSpace(nameSpace, 'landArea'));
  const address = getIn(formikProps.values, withNameSpace(nameSpace, 'address'));
  const property = getIn(formikProps.values, withNameSpace(nameSpace));
  const propertyName = getApiPropertyName(property);
  return (
    <li key={`property-${property.id}`}>
      <Styled.LeaseH3>Property Information</Styled.LeaseH3>
      <Styled.FormGrid>
        {!!address ? (
          <AddressSubForm nameSpace={withNameSpace(nameSpace, 'address')} disabled={disabled} />
        ) : (
          <>
            <Form.Label>{`${propertyName.label}:`}</Form.Label>
            <Styled.FormControl disabled={disabled} value={propertyName.value} />
          </>
        )}
        <br />
        {landArea !== undefined ? (
          <>
            <Form.Label>Lease Area</Form.Label>
            {!disabled ? (
              <InputGroup
                disabled={disabled}
                field={withNameSpace(nameSpace, 'landArea')}
                postText={areaUnitType?.description ?? ''}
              />
            ) : (
              <Styled.FormControl
                disabled={disabled}
                value={`${landArea} ${areaUnitType?.description ?? ''}`}
              />
            )}
          </>
        ) : null}
      </Styled.FormGrid>
    </li>
  );
};

export default PropertyInformation;
