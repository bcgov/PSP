import { ColProps } from 'react-bootstrap';
import { FaExternalLinkAlt } from 'react-icons/fa';

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
  labelWidth?: ColProps;
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
  labelWidth,
}: PrimaryContactSelectorDetailsProps) => (
  <>
    <SectionField
      label={label}
      className="pb-1"
      valueTestId={`teamMember[${index}]`}
      labelWidth={labelWidth}
      tooltip={tooltip}
    >
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
        labelWidth={labelWidth}
        className="mb-4"
        labelClassName="pl-10"
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
