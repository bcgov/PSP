import ExpandableTextList from '@/components/common/ExpandableTextList';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';
import { exists, isValidString } from '@/utils';
import { getApiPropertyName } from '@/utils/mapPropertyUtils';

export interface ILeaseHeaderAddressesProps {
  propertyLeases?: ApiGen_Concepts_PropertyLease[];
  delimiter?: React.ReactElement | string;
  maxCollapsedLength?: number;
}

export const LeaseHeaderAddresses: React.FC<ILeaseHeaderAddressesProps> = ({
  propertyLeases,
  delimiter = '; ',
  maxCollapsedLength = 2,
}) => {
  return (
    <ExpandableTextList<ApiGen_Concepts_PropertyLease>
      items={propertyLeases ?? []}
      keyFunction={(item: ApiGen_Concepts_PropertyLease, index: number) =>
        `lease-property-${item.id}-address-${item?.property?.address?.id ?? index}`
      }
      renderFunction={(item: ApiGen_Concepts_PropertyLease) => (
        <>{getFormattedAddress(item?.property)}</>
      )}
      delimiter={delimiter}
      maxCollapsedLength={maxCollapsedLength}
    />
  );
};

const getFormattedAddress = (property: ApiGen_Concepts_Property | null | undefined) => {
  if (!exists(property)) {
    return '';
  }
  const address = property?.address;
  if (isValidString(address?.streetAddress1)) {
    return isValidString(address?.municipality)
      ? `${address!.streetAddress1}, ${address!.municipality}`
      : address!.streetAddress1;
  } else {
    return isValidString(address?.municipality)
      ? address!.municipality
      : `${getApiPropertyName(property).value} - Address not available in PIMS`;
  }
};

export default LeaseHeaderAddresses;
