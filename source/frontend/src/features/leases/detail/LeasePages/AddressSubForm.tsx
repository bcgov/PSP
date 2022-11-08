import { Form, Input } from 'components/common/form';
import { getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import * as React from 'react';
import { withNameSpace } from 'utils/formUtils';

import { FieldValue } from '../styles';

export interface IAddressSubFormProps {
  disabled?: boolean;
  nameSpace?: string;
}

export const AddressSubForm: React.FunctionComponent<IAddressSubFormProps> = ({
  disabled,
  nameSpace,
}) => {
  const formikProps = useFormikContext<IFormLease>();
  const address = getIn(formikProps.values, withNameSpace(nameSpace));
  const municipality = getIn(formikProps.values, withNameSpace(nameSpace, 'municipality'));
  const postal = getIn(formikProps.values, withNameSpace(nameSpace, 'postal'));
  const country = getIn(formikProps.values, withNameSpace(nameSpace, 'country'));
  const streetAddress1 = getIn(formikProps.values, withNameSpace(nameSpace, 'streetAddress1'));
  const streetAddress2 = getIn(formikProps.values, withNameSpace(nameSpace, 'streetAddress2'));
  const streetAddress3 = getIn(formikProps.values, withNameSpace(nameSpace, 'streetAddress3'));

  if (!address) {
    return (
      <>
        <Form.Label>Address:</Form.Label>
        <FieldValue>Address not available in PIMS</FieldValue>
      </>
    );
  }

  return (
    <>
      <Form.Label>Address:</Form.Label>
      {streetAddress1 && (
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'streetAddress1')} />
      )}
      {streetAddress2 && (
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'streetAddress2')} />
      )}
      {streetAddress3 && (
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'streetAddress3')} />
      )}
      {municipality && (
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'municipality')} />
      )}
      {postal && <Input disabled={disabled} field={withNameSpace(nameSpace, 'postal')} />}
      <Input disabled={disabled} field={withNameSpace(nameSpace, 'province')} />
      {country && <Input disabled={disabled} field={withNameSpace(nameSpace, 'country')} />}
    </>
  );
};

export default AddressSubForm;
