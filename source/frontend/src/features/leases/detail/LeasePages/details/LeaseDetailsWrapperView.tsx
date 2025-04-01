import React from 'react';

import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists } from '@/utils';

import { FileTeamView } from '../../../../../components/common/FileTeamView';
import DetailAdministration from './DetailAdministration';
import { DetailFeeDetermination } from './DetailFeeDetermination';
import LeaseDetailView from './LeaseDetailView';
import { LeaseRenewalsView } from './LeaseRenewalsView';
import { PropertiesInformation } from './PropertiesInformation';

export interface ILeaseDetailsWrapperViewProps {
  lease?: ApiGen_Concepts_Lease;
}

export const LeaseDetailsWrapperView: React.FunctionComponent<ILeaseDetailsWrapperViewProps> = ({
  lease,
}) => {
  if (!exists(lease)) {
    return <></>;
  }

  return (
    <>
      <LeaseDetailView lease={lease} />
      <LeaseRenewalsView renewals={lease.renewals} />
      <PropertiesInformation lease={lease} />
      <DetailAdministration lease={lease} />
      <FileTeamView team={lease.leaseTeam} title="Lease Team" />
      <DetailFeeDetermination lease={lease} />
    </>
  );
};

export default LeaseDetailsWrapperView;
