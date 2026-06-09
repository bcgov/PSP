import { FaExternalLinkAlt } from 'react-icons/fa';

import { SectionField } from '../../Section/SectionField';
import { StyledLink } from '../../styles';

interface PrimaryContactSelectorViewProps {
  label: string;
  teamMemberName: string;
  teamMemberUrl: string;
  primaryContactName: string;
  primaryContactUrl: string;
  showPrimaryContact: boolean;
  index?: number;
  tooltip?: string;
}

export const PrimaryContactSelectorView = ({
  label,
  teamMemberName,
  teamMemberUrl,
  primaryContactName,
  primaryContactUrl,
  showPrimaryContact = false,
  index = 0,
  tooltip,
}: PrimaryContactSelectorViewProps) => (
  <>
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

    {showPrimaryContact && (
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
    )}
  </>
);
