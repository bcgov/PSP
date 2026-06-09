import React from 'react';

import { PrimaryContactSelectorView } from '@/components/common/form/PrimaryContactSelector/PrimaryContactSelectorView';
import { Section } from '@/components/common/Section/Section';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { formatApiPersonNames } from '@/utils/personUtils';

export interface ILeaseTeamProps {
  lease: ApiGen_Concepts_Lease;
}
export const LeaseTeamView: React.FunctionComponent<React.PropsWithChildren<ILeaseTeamProps>> = ({
  lease,
}: ILeaseTeamProps) => {
  return (
    <Section header="Lease & Licence Team">
      {lease.leaseTeam.map((teamMember, index) => (
        <React.Fragment key={`lease-team-${teamMember?.id ?? index}`}>
          <PrimaryContactSelectorView
            label={teamMember?.teamProfileType.description || ''}
            teamMemberName={
              teamMember?.person
                ? formatApiPersonNames(teamMember.person)
                : teamMember?.organization?.name ?? ''
            }
            teamMemberUrl={
              teamMember?.personId
                ? `/contact/P${teamMember?.personId}`
                : `/contact/O${teamMember?.organizationId}`
            }
            primaryContactName={
              teamMember?.primaryContact ? formatApiPersonNames(teamMember.primaryContact) : ''
            }
            primaryContactUrl={
              teamMember?.primaryContactId ? `/contact/P${teamMember?.primaryContactId}` : undefined
            }
            showPrimaryContact={!!teamMember?.organizationId}
          />
        </React.Fragment>
      ))}
    </Section>
  );
};
