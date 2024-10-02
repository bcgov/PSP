import { Link } from 'react-router-dom';

import AcquisitionFileIcon from '@/assets/images/acquisition-icon.svg?react';
import { Claims } from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';

/**
 * Acquisitions page with links to acquisition functionality.
 * Intended for use within the left side tray.
 */
export const AcquisitionTray = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      <Styled.TrayHeader>
        <span className="mr-2">
          <AcquisitionFileIcon
            title="Acquisition file Icon"
            width="2.6rem"
            height="2.6rem"
            fill="currentColor"
          />
        </span>
        Acquisition Files
      </Styled.TrayHeader>
      {hasClaim(Claims.ACQUISITION_VIEW) && (
        <Link className="pl-9 pb-3" onClick={onLinkClick} to="/acquisition/list">
          Manage Acquisition Files
        </Link>
      )}
      {hasClaim(Claims.ACQUISITION_ADD) && (
        <Link className="pl-9 pb-3" onClick={onLinkClick} to="/mapview/sidebar/acquisition/new">
          Create an Acquisition File
        </Link>
      )}
    </>
  );
};
