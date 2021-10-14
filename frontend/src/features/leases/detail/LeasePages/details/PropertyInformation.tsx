import { Form, InputGroup } from 'components/common/form';
import * as Styled from 'features/leases/detail/styles';
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
export const PropertyInformation: React.FunctionComponent<IPropertyInformationProps &
  Partial<FieldArrayRenderProps>> = ({ nameSpace, disabled }) => {
  const formikProps = useFormikContext<IFormLease>();

  const areaUnit = getIn(formikProps.values, withNameSpace(nameSpace, 'areaUnit'));
  const landArea = getIn(formikProps.values, withNameSpace(nameSpace, 'landArea'));
  return (
    <li>
      <Styled.LeaseH3>Property Information</Styled.LeaseH3>
      <Styled.FormGrid>
        <AddressSubForm nameSpace={withNameSpace(nameSpace, 'address')} disabled={disabled} />
        <br />
        <Form.Label>Area</Form.Label>
        {!disabled ? (
          <InputGroup
            disabled={disabled}
            formikProps={formikProps}
            field={withNameSpace(nameSpace, 'landArea')}
            postText={areaUnit}
          />
        ) : (
          <Styled.FormControl disabled={disabled} value={`${landArea} ${areaUnit}`} />
        )}
      </Styled.FormGrid>
    </li>
  );
};

export default PropertyInformation;
