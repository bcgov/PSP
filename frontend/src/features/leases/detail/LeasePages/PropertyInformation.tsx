import { Form, Input, InputGroup } from 'components/common/form';
import { FieldArrayRenderProps, getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import * as React from 'react';
import { withNameSpace } from 'utils/formUtils';

import * as Styled from '../styles';

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
  const streetAddress2 = getIn(
    formikProps.values,
    withNameSpace(nameSpace, 'address.streetAddress2'),
  );
  const streetAddress3 = getIn(
    formikProps.values,
    withNameSpace(nameSpace, 'address.streetAddress3'),
  );

  const areaUnit = getIn(formikProps.values, withNameSpace(nameSpace, 'areaUnit'));
  const landArea = getIn(formikProps.values, withNameSpace(nameSpace, 'landArea'));
  return (
    <li>
      <Styled.LeaseH3>Property Information</Styled.LeaseH3>
      <Styled.FormGrid>
        <Form.Label>Address</Form.Label>
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'address.streetAddress1')} />
        {streetAddress2 && (
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'address.streetAddress2')} />
        )}
        {streetAddress3 && (
          <Input disabled={disabled} field={withNameSpace(nameSpace, 'address.streetAddress3')} />
        )}
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'address.municipality')} />
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'address.postal')} />
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'address.province')} />
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'address.country')} />

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
