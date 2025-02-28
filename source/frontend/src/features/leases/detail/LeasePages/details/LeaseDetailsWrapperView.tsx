import React from 'react';

import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists } from '@/utils';

import DetailAdministration from './DetailAdministration';
import { DetailFeeDetermination } from './DetailFeeDetermination';
import LeaseDetailView from './LeaseDetailView';
import { LeaseRenewalsView } from './LeaseRenewalsView';
import { PropertiesInformation } from './PropertiesInformation';

export interface ILeaseDetailsWrapperViewProps {
  lease?: ApiGen_Concepts_Lease;
  onGenerate: (lease?: ApiGen_Concepts_Lease) => void;
}

export const LeaseDetailsWrapperView: React.FunctionComponent<ILeaseDetailsWrapperViewProps> = ({
  lease,
  onGenerate,
}) => {
  if (!exists(lease)) {
    return <></>;
  }

  return (
    <>
      <LeaseDetailView lease={lease} onGenerate={onGenerate} />
      <LeaseRenewalsView renewals={lease.renewals} />
      <PropertiesInformation lease={lease} />
      <DetailAdministration lease={lease} />
      <DetailFeeDetermination lease={lease} />
    </>
  );
};

export default LeaseDetailsWrapperView;
