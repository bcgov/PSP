import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';
import { exists, isValidString } from '@/utils';

export interface IAddressViewProps {
  address: ApiGen_Concepts_Address | null | undefined;
}

export const AddressView: React.FunctionComponent<React.PropsWithChildren<IAddressViewProps>> = ({
  address,
}) => {
  if (!exists(address)) {
    return (
      <>
        <div>Address not available in PIMS</div>
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
      {isValidString(streetAddress1) && <div>{streetAddress1}</div>}
      {isValidString(streetAddress2) && <div>{streetAddress2} </div>}
      {isValidString(streetAddress3) && <div>{streetAddress3} </div>}
      {isValidString(municipality) && <div>{municipality}</div>}
      {isValidString(postal) && <div>{postal}</div>}
      {province && <div> {province} </div>}
      {country && <div>{country} </div>}
    </>
  );
};

export default AddressView;
