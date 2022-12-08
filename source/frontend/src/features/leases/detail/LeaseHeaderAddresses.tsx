import ExpandableTextList from 'components/common/ExpandableTextList';
import { ILease, IProperty } from 'interfaces';
import * as React from 'react';
import { getPropertyName } from 'utils/mapPropertyUtils';

export interface ILeaseHeaderAddressesProps {
  lease?: ILease;
  delimiter?: React.ReactElement | string;
  maxCollapsedLength?: number;
}

export const LeaseHeaderAddresses: React.FC<ILeaseHeaderAddressesProps> = ({
  lease,
  delimiter = '; ',
  maxCollapsedLength = 2,
}) => {
  return (
    <ExpandableTextList<IProperty>
      items={lease?.properties ?? []}
      keyFunction={(item: IProperty, index: number) =>
        `lease-property-${item.id}-address-${item?.address?.id ?? index}`
      }
      renderFunction={(item: IProperty) => <>{getFormattedAddress(item)}</>}
      delimiter={delimiter}
      maxCollapsedLength={maxCollapsedLength}
    />
  );
};

const getFormattedAddress = (property: IProperty) => {
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
            pid: property.pid,
            pin: property.pin?.toString(),
            latitude: property.latitude,
            longitude: property.longitude,
          }).value
        } - Address not available in PIMS`;
  }
};

export default LeaseHeaderAddresses;
