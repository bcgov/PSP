import React from 'react';

import ExpandableTextList from '@/components/common/ExpandableTextList';
import { getAllNames } from '@/features/leases/leaseUtils';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';

export interface ILeaseHeaderStakeholderProps {
  stakeholders?: ApiGen_Concepts_LeaseStakeholder[];
  delimiter?: React.ReactElement | string;
  maxCollapsedLength?: number;
}

export const LeaseHeaderStakeholders: React.FC<ILeaseHeaderStakeholderProps> = ({
  stakeholders,
  delimiter = '; ',
  maxCollapsedLength = 2,
}) => {
  return (
    <ExpandableTextList<string>
      items={getAllNames(stakeholders ?? [])}
      keyFunction={(p: string, index: number) => `lease-stakeholder-${index}`}
      renderFunction={(p: string) => <>{p}</>}
      delimiter={delimiter}
      maxCollapsedLength={maxCollapsedLength}
    />
  );
};

export default LeaseHeaderStakeholders;
