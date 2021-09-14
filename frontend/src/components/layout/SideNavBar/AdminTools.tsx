import { Link } from 'react-router-dom';

import * as Styled from './styles';

/**
 * Admin Tools Tray page with links to admin functionality.
 * Intended for use within the left side tray.
 */
export const AdminTools = () => {
  return (
    <>
      <Styled.TrayHeader>Admin Tools</Styled.TrayHeader>
      <Link to="/admin/users">Manage Users</Link>
      <Link to="/admin/access/requests">Manage Access Requests</Link>
    </>
  );
};
