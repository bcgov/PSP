import { getIn, useFormikContext } from 'formik';

import { Form, Input } from '@/components/common/form';
import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists, isValidString } from '@/utils';
import { withNameSpace } from '@/utils/formUtils';

import { FieldValue } from '../styles';

export interface IAddressSubFormProps {
  disabled?: boolean;
  nameSpace?: string;
}

export const AddressSubForm: React.FunctionComponent<
  React.PropsWithChildren<IAddressSubFormProps>
> = ({ disabled, nameSpace }) => {
  const formikProps = useFormikContext<ApiGen_Concepts_Lease>();

  const address: ApiGen_Concepts_Address | null = getIn(
    formikProps.values,
    withNameSpace(nameSpace),
  );

  if (!exists(address)) {
    return (
      <>
        <Form.Label>Address:</Form.Label>
        <FieldValue>Address not available in PIMS</FieldValue>
      </>
    );
  }

  const municipality = address.municipality;
  const postal = address.postal;
  const country = address.country?.description;
  const province = address.province?.description;
  const streetAddress1 = address.streetAddress1;
  const streetAddress2 = address.streetAddress2;
  const streetAddress3 = address.streetAddress3;

  return (
    <>
      {isValidString(streetAddress1) && (
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'streetAddress1')} />
      )}
      {isValidString(streetAddress2) && (
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'streetAddress2')} />
      )}
      {isValidString(streetAddress3) && (
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'streetAddress3')} />
      )}
      {isValidString(municipality) && (
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'municipality')} />
      )}
      {isValidString(postal) && (
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'postal')} />
      )}
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
