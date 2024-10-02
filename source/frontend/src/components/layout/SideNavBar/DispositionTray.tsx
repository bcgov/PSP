import { MdAirlineStops } from 'react-icons/md';
import { Link } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';

/**
 * Disposition Tray page.
 * Intended for use within the left side tray.
 */
export const DispositionTray = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      <Styled.TrayHeader>
        <span className="mr-2">
          <MdAirlineStops size={26} />
        </span>
        Disposition Files
      </Styled.TrayHeader>
      {hasClaim(Claims.DISPOSITION_VIEW) && (
        <Link className="pl-9 pb-3" onClick={onLinkClick} to="/disposition/list">
          Manage Disposition Files
        </Link>
      )}
      {hasClaim(Claims.DISPOSITION_ADD) && (
        <Link className="pl-9 pb-3" onClick={onLinkClick} to="/mapview/sidebar/disposition/new">
          Create a Disposition File
        </Link>
      )}
    </>
  );
};
