import * as React from 'react';

import ExpandableTextList from '@/components/common/ExpandableTextList';
import { Api_Property } from '@/models/api/Property';
import { Api_PropertyLease } from '@/models/api/PropertyLease';
import { getPropertyName } from '@/utils/mapPropertyUtils';

export interface ILeaseHeaderAddressesProps {
  propertyLeases?: Api_PropertyLease[];
  delimiter?: React.ReactElement | string;
  maxCollapsedLength?: number;
}

export const LeaseHeaderAddresses: React.FC<ILeaseHeaderAddressesProps> = ({
  propertyLeases,
  delimiter = '; ',
  maxCollapsedLength = 2,
}) => {
  return (
    <ExpandableTextList<Api_PropertyLease>
      items={propertyLeases ?? []}
      keyFunction={(item: Api_PropertyLease, index: number) =>
        `lease-property-${item.id}-address-${item?.property?.address?.id ?? index}`
      }
      renderFunction={(item: Api_PropertyLease) => <>{getFormattedAddress(item?.property)}</>}
      delimiter={delimiter}
      maxCollapsedLength={maxCollapsedLength}
    />
  );
};

const getFormattedAddress = (property?: Api_Property) => {
  if (!property) {
    return '';
  }
  const address = property?.address;
  if (!!address?.streetAddress1) {
    return !!address?.municipality
      ? `${address.streetAddress1}, ${address.municipality}`
      : address.streetAddress1;
  } else {
    return !!address?.municipality
      ? address.municipality
      : `${
          getPropertyName({
            pid: property.pid?.toString(),
            pin: property.pin?.toString(),
            latitude: property.latitude,
            longitude: property.longitude,
          }).value
        } - Address not available in PIMS`;
  }
};

export default LeaseHeaderAddresses;
