import ExpandableTextList from 'components/common/ExpandableTextList';
import { ILease } from 'interfaces';
import { Api_Person } from 'models/api/Person';
import * as React from 'react';
import { formatNames } from 'utils/personUtils';

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
    <ExpandableTextList<Api_Person>
      items={lease?.persons ?? []}
      keyFunction={(p: Api_Person, index: number) => `lease-tenant-${p?.id ?? index}`}
      renderFunction={(p: Api_Person) => (
        <>{formatNames([p?.firstName, p?.middleNames, p?.surname])}</>
      )}
      delimiter={delimiter}
      maxCollapsedLength={maxCollapsedLength}
    />
  );
};

export default LeaseHeaderTenants;
