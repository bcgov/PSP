import ExpandableTextList from 'components/common/ExpandableTextList';
import { getPropertyName } from 'features/properties/selector/utils';
import { ILease, IProperty } from 'interfaces';
import * as React from 'react';

export interface ILeaseHeaderAddressesProps {
  lease?: ILease;
}

export const LeaseHeaderAddresses: React.FunctionComponent<ILeaseHeaderAddressesProps> = ({
  lease,
}) => {
  return (
    <ExpandableTextList<IProperty>
      items={lease?.properties ?? []}
      keyFunction={(item: IProperty, index: number) =>
        `lease-property-${item.id}-address-${item?.address?.id}`
      }
      renderFunction={(item: IProperty) => <>{getFormattedAddress(item)}</>}
      delimiter="; "
      maxCollapsedLength={2}
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
