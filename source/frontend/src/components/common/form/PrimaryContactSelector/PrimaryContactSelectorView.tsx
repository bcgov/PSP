import { FaExternalLinkAlt } from 'react-icons/fa';
import styled from 'styled-components';

import { SectionField } from '../../Section/SectionField';
import { StyledLink } from '../../styles';

interface PrimaryContactSelectorDetailsProps {
  label: string;
  teamMemberName: string;
  teamMemberUrl: string;
  primaryContactName: string;
  primaryContactUrl: string;
  showPrimaryContact: boolean;
  index?: number;
  tooltip?: string;
}

export const PrimaryContactSelectorDetails = ({
  label,
  teamMemberName,
  teamMemberUrl,
  primaryContactName,
  primaryContactUrl,
  showPrimaryContact,
  index,
  tooltip,
}: PrimaryContactSelectorDetailsProps) => (
  <>
    <StyledDiv>
      <SectionField label={label} valueTestId={`teamMember[${index}]`} tooltip={tooltip}>
        {teamMemberName && teamMemberUrl ? (
          <StyledLink target="_blank" rel="noopener noreferrer" to={teamMemberUrl}>
            <span>{teamMemberName}</span>
            <FaExternalLinkAlt className="ml-2" size="1rem" />
          </StyledLink>
        ) : (
          ''
        )}
      </SectionField>
    </StyledDiv>

    {showPrimaryContact && (
      <StyledDiv>
        <SectionField
          label="Primary contact"
          valueTestId={`primaryContact[${index}]`}
          className="pl-4 mb-3"
        >
          {primaryContactName && primaryContactUrl ? (
            <StyledLink target="_blank" rel="noopener noreferrer" to={primaryContactUrl}>
              <span>{primaryContactName}</span>
              <FaExternalLinkAlt className="ml-2" size="1rem" />
            </StyledLink>
          ) : (
            'No contacts available'
          )}
        </SectionField>
      </StyledDiv>
    )}
  </>
);

export const StyledDiv = styled.div`
  font-style: italic;
`;
