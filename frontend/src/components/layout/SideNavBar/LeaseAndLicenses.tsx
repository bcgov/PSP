import { Link } from 'react-router-dom';

import * as Styled from './styles';

/**
 * Lease And Licenses Tray page with links to lis functionality.
 * Intended for use within the left side tray.
 */
export const LeaseAndLicenses = () => {
  return (
    <>
      <Styled.TrayHeader>Management</Styled.TrayHeader>
      <Link to=".">Leases & Licenses</Link>
    </>
  );
};
