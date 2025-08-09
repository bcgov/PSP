import React from 'react';
import { Dropdown } from 'react-bootstrap';
import { FaEllipsisH } from 'react-icons/fa';
import { MdClose } from 'react-icons/md';
import styled from 'styled-components';

import AcquisitionIcon from '@/assets/images/acquisition-icon.svg?react';
import DispositionIcon from '@/assets/images/disposition-icon.svg?react';
import LeaseIcon from '@/assets/images/lease-icon.svg?react';
import ManagementIcon from '@/assets/images/management-icon.svg?react';
import ResearchIcon from '@/assets/images/research-icon.svg?react';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { Claims } from '@/constants';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

export interface IMoreOptionsDropdownProps {
  /** ARIA label for accessibility */
  ariaLabel?: string;
  /** Whether the "Clear All" action is enabled. When false, the option is grayed out and unclickable. */
  canClearAll?: boolean;
  /** Callback invoked when the "Clear All" option is clicked. */
  onClearAll: () => void;
  /** Callbacks for the various "Create File" options. */
  onCreateResearchFile: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateAcquisitionFile: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateDispositionFile: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateLeaseFile: (event: React.MouseEvent<HTMLElement>) => void;
  onCreateManagementFile: (event: React.MouseEvent<HTMLElement>) => void;
}

/**
 * A dropdown menu component with options to operate on the property worklist
 */
const MoreOptionsDropdown: React.FC<IMoreOptionsDropdownProps> = ({
  ariaLabel = 'More options',
  canClearAll = true,
  onClearAll,
  onCreateResearchFile,
  onCreateAcquisitionFile,
  onCreateDispositionFile,
  onCreateLeaseFile,
  onCreateManagementFile,
}) => {
  const keycloak = useKeycloakWrapper();

  return (
    <Dropdown alignRight>
      <TooltipWrapper tooltipId="more-options-tooltip" tooltip="More options...">
        <StyledToggleButton
          id="dropdown-ellipsis"
          variant="light"
          bsPrefix="btn"
          aria-label={ariaLabel}
        >
          <FaEllipsisH size={18} />
        </StyledToggleButton>
      </TooltipWrapper>

      <StyledDropdownMenu>
        <StyledDropdownItem
          aria-label="Clear list"
          disabled={!canClearAll}
          onClick={canClearAll ? onClearAll : undefined}
        >
          <MdClose size={20} className="mr-1" />
          Clear list
        </StyledDropdownItem>
        {keycloak.hasClaim(Claims.RESEARCH_ADD) && (
          <StyledDropdownItem aria-label="Create research file" onClick={onCreateResearchFile}>
            <ResearchIcon className="mr-1" width="2rem" height="2rem" />
            Create Research File
          </StyledDropdownItem>
        )}
        {keycloak.hasClaim(Claims.ACQUISITION_ADD) && (
          <StyledDropdownItem
            aria-label="Create acquisition file"
            onClick={onCreateAcquisitionFile}
          >
            <AcquisitionIcon className="mr-1" width="2rem" height="2rem" />
            Create Acquisition File
          </StyledDropdownItem>
        )}
        {keycloak.hasClaim(Claims.MANAGEMENT_ADD) && (
          <StyledDropdownItem aria-label="Create management file" onClick={onCreateManagementFile}>
            <ManagementIcon className="mr-1" width="2rem" height="2rem" />
            Create Management File
          </StyledDropdownItem>
        )}
        {keycloak.hasClaim(Claims.LEASE_ADD) && (
          <StyledDropdownItem aria-label="Create lease file" onClick={onCreateLeaseFile}>
            <LeaseIcon className="mr-1" width="2rem" height="2rem" />
            Create Lease/Licence File
          </StyledDropdownItem>
        )}
        {keycloak.hasClaim(Claims.DISPOSITION_ADD) && (
          <StyledDropdownItem
            aria-label="Create disposition file"
            onClick={onCreateDispositionFile}
          >
            <DispositionIcon className="mr-1" width="2rem" height="2rem" />
            Create Disposition File
          </StyledDropdownItem>
        )}
      </StyledDropdownMenu>
    </Dropdown>
  );
};

export default MoreOptionsDropdown;

const StyledToggleButton = styled(Dropdown.Toggle)`
  border: none;
  padding: 0;
  display: flex;
  align-items: center;
  background-color: transparent !important;
  box-shadow: none !important;
  outline: none !important;

  padding: 0.5rem; /* adds 5px padding on all sides to improve hit-area */
  min-width: 3rem;
  min-height: 3rem;

  &:focus,
  &:active,
  &:focus:active {
    background-color: transparent !important;
    box-shadow: none !important;
    outline: none !important;
  }
`;

const StyledDropdownMenu = styled(Dropdown.Menu)`
  padding-top: 1rem;
  padding-bottom: 1rem;
`;

const StyledDropdownItem = styled(Dropdown.Item)`
  display: flex;
  align-items: center;

  color: ${props => props.theme.css.pimsBlue200} !important;
  font-weight: 700;
  font-size: 1.4rem;

  padding-top: 0.5rem;
  padding-bottom: 0.5rem;

  /* Adds gap between items */
  & + & {
    margin-top: 0.25rem; /* adjust this value as needed */
  }

  &:hover,
  &:focus {
    // Adding a 38% opacity to the background color (to match the mockups)
    background-color: ${props => props.theme.css.pimsBlue10 + '38'} !important;
  }

  &:disabled {
    color: #6c757d; /* Bootstrap's default muted text */
    pointer-events: none;
  }
`;
