import React from 'react';

import ExpandableTextList from '@/components/common/ExpandableTextList';
import { getAllNames } from '@/features/leases/leaseUtils';
import { Api_LeaseTenant } from '@/models/api/LeaseTenant';

export interface ILeaseHeaderTenantsProps {
  tenants?: Api_LeaseTenant[];
  delimiter?: React.ReactElement | string;
  maxCollapsedLength?: number;
}

export const LeaseHeaderTenants: React.FC<ILeaseHeaderTenantsProps> = ({
  tenants,
  delimiter = '; ',
  maxCollapsedLength = 2,
}) => {
  return (
    <ExpandableTextList<string>
      items={getAllNames(tenants ?? [])}
      keyFunction={(p: string, index: number) => `lease-tenant-${index}`}
      renderFunction={(p: string) => <>{p}</>}
      delimiter={delimiter}
      maxCollapsedLength={maxCollapsedLength}
    />
  );
};

export default LeaseHeaderTenants;
