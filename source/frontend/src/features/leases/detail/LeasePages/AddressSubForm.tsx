import { getIn, useFormikContext } from 'formik';
import * as React from 'react';

import { Form, Input } from '@/components/common/form';
import { Api_Address } from '@/models/api/Address';
import { Api_Lease } from '@/models/api/Lease';
import { withNameSpace } from '@/utils/formUtils';

import { FieldValue } from '../styles';

export interface IAddressSubFormProps {
  disabled?: boolean;
  nameSpace?: string;
}

export const AddressSubForm: React.FunctionComponent<
  React.PropsWithChildren<IAddressSubFormProps>
> = ({ disabled, nameSpace }) => {
  const formikProps = useFormikContext<Api_Lease>();
  const address = getIn(formikProps.values, withNameSpace(nameSpace)) as Api_Address;
  const municipality = address.municipality;
  const postal = address.postal;
  const country = address.country?.description;
  const province = address.province?.description;
  const streetAddress1 = address.streetAddress1;
  const streetAddress2 = address.streetAddress2;
  const streetAddress3 = address.streetAddress3;

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
      {province && (
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'province.description')} />
      )}
      {country && (
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'country.description')} />
      )}
    </>
  );
};

export default AddressSubForm;
