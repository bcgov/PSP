import ExpandableTextList from 'components/common/ExpandableTextList';
import { getAllNames } from 'features/leases/leaseUtils';
import { ILease } from 'interfaces';
import React from 'react';

export interface ILeaseHeaderTenantsProps {
  lease?: ILease;
  delimiter?: React.ReactElement | string;
  maxCollapsedLength?: number;
}

export const LeaseHeaderTenants: React.FC<ILeaseHeaderTenantsProps> = ({
  lease,
  delimiter = '; ',
  maxCollapsedLength = 2,
}) => {
  return (
    <ExpandableTextList<string>
      items={getAllNames(lease) ?? []}
      keyFunction={(p: string, index: number) => `lease-tenant-${index}`}
      renderFunction={(p: string) => <>{p}</>}
      delimiter={delimiter}
      maxCollapsedLength={maxCollapsedLength}
    />
  );
};

export default LeaseHeaderTenants;
