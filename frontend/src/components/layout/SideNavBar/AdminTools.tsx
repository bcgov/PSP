import { Link } from 'react-router-dom';

import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';

/**
 * Admin Tools Tray page with links to admin functionality.
 * Intended for use within the left side tray.
 */
export const AdminTools = ({ onLinkClick }: ISideTrayPageProps) => {
  return (
    <>
      <Styled.TrayHeader>Admin Tools</Styled.TrayHeader>
      <Link to="/admin/users" onClick={onLinkClick}>
        Manage Users
      </Link>
      <Link to="/admin/access/requests" onClick={onLinkClick}>
        Manage Access Requests
      </Link>
    </>
  );
};
