import ExpandableTextList from 'components/common/ExpandableTextList';
import { ILease, IProperty } from 'interfaces';
import * as React from 'react';
import { pidFormatter } from 'utils';

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
        `lease-property-${item.id}-address-${item.addressId}`
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
    return !!address.municipality
      ? `${address.streetAddress1}, ${address.municipality}`
      : address.streetAddress1;
  } else {
    return !!address.municipality ? address.municipality : pidFormatter(property.pid);
  }
};

export default LeaseHeaderAddresses;
