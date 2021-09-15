import { Link } from 'react-router-dom';

import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';

/**
 * Lease And Licenses Tray page with links to lis functionality.
 * Intended for use within the left side tray.
 */
export const LeaseAndLicenses = ({ onLinkClick }: ISideTrayPageProps) => {
  return (
    <>
      <Styled.TrayHeader>Management</Styled.TrayHeader>
      <Link onClick={onLinkClick} to="/lease/list">
        Leases & Licenses
      </Link>
    </>
  );
};
