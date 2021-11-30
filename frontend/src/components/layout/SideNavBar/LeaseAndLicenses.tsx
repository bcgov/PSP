import { Claims } from 'constants/claims';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Link } from 'react-router-dom';

import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';

/**
 * Lease And Licenses Tray page with links to lis functionality.
 * Intended for use within the left side tray.
 */
export const LeaseAndLicenses = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      <Styled.TrayHeader>Management</Styled.TrayHeader>
      {hasClaim(Claims.LEASE_VIEW) && (
        <Link onClick={onLinkClick} to="/lease/list">
          Search for a Lease or License
        </Link>
      )}
      {hasClaim(Claims.LEASE_ADD) && (
        <Link onClick={onLinkClick} to="/lease/new">
          Add a lease or license
        </Link>
      )}
    </>
  );
};
