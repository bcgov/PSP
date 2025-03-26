import React from 'react';
import { FaExternalLinkAlt } from 'react-icons/fa';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledLink } from '@/components/maps/leaflet/LayerPopup/styles';
import { ApiGen_Concepts_FileTeam } from '@/models/api/generated/ApiGen_Concepts_FileTeam';
import { formatApiPersonNames } from '@/utils/personUtils';

export interface IFileTeamProps {
  title: string;
  team: ApiGen_Concepts_FileTeam[];
}
export const FileTeamView: React.FunctionComponent<React.PropsWithChildren<IFileTeamProps>> = ({
  title,
  team,
}: IFileTeamProps) => {
  return (
    <Section header={title}>
      {team?.map((teamMember, index) => (
        <React.Fragment key={`file-team-${teamMember?.id ?? index}`}>
          <SectionField label={teamMember?.teamProfileType.description || ''}>
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
          {teamMember?.organizationId && (
            <SectionField label="Primary contact">
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
      )) ?? null}
    </Section>
  );
};
