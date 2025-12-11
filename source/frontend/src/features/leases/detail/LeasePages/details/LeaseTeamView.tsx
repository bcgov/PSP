import React from 'react';
import { FaExternalLinkAlt } from 'react-icons/fa';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledLink } from '@/components/common/styles';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists } from '@/utils';
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
          <SectionField
            label={teamMember?.teamProfileType.description || ''}
            valueTestId={`teamMember[${index}]`}
          >
            <StyledLink
              target="_blank"
              rel="noopener noreferrer"
              to={
                teamMember?.personId
                  ? `/contact/P${teamMember?.personId}`
                  : `/contact/O${teamMember?.organizationId}`
              }
            >
              <span>
                {teamMember?.person
                  ? formatApiPersonNames(teamMember?.person)
                  : teamMember?.organization?.name ?? ''}
              </span>
              <FaExternalLinkAlt className="ml-2" size="1rem" />
            </StyledLink>
          </SectionField>
          {exists(teamMember?.organizationId) && (
            <SectionField label="Primary contact" valueTestId={`primaryContact[${index}]`}>
              {teamMember?.primaryContactId ? (
                <StyledLink
                  target="_blank"
                  rel="noopener noreferrer"
                  to={`/contact/P${teamMember?.primaryContactId}`}
                >
                  <span>
                    {teamMember?.primaryContact
                      ? formatApiPersonNames(teamMember.primaryContact)
                      : ''}
                  </span>
                  <FaExternalLinkAlt className="m1-2" size="1rem" />
                </StyledLink>
              ) : (
                'No contacts available'
              )}
            </SectionField>
          )}
        </React.Fragment>
      ))}
    </Section>
  );
};
