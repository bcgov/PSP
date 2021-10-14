import { Form, Input } from 'components/common/form';
import { getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import * as React from 'react';
import { withNameSpace } from 'utils/formUtils';

export interface IAddressSubFormProps {
  disabled?: boolean;
  nameSpace?: string;
}

export const AddressSubForm: React.FunctionComponent<IAddressSubFormProps> = ({
  disabled,
  nameSpace,
}) => {
  const formikProps = useFormikContext<IFormLease>();
  const streetAddress2 = getIn(formikProps.values, withNameSpace(nameSpace, 'streetAddress2'));
  const streetAddress3 = getIn(formikProps.values, withNameSpace(nameSpace, 'streetAddress3'));

  return (
    <>
      <Form.Label>Address</Form.Label>
      <Input disabled={disabled} field={withNameSpace(nameSpace, 'streetAddress1')} />
      {streetAddress2 && (
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'streetAddress2')} />
      )}
      {streetAddress3 && (
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'streetAddress3')} />
      )}
      <Input disabled={disabled} field={withNameSpace(nameSpace, 'municipality')} />
      <Input disabled={disabled} field={withNameSpace(nameSpace, 'postal')} />
      <Input disabled={disabled} field={withNameSpace(nameSpace, 'province')} />
      <Input disabled={disabled} field={withNameSpace(nameSpace, 'country')} />
    </>
  );
};

export default AddressSubForm;
