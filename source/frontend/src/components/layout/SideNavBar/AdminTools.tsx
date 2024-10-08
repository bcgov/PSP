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
      <Link to="/admin/users" onClick={onLinkClick} className="nav-item">
        Manage Users
      </Link>
      <Link to="/admin/access/requests" onClick={onLinkClick} className="nav-item">
        Manage Access Requests
      </Link>
      <Link to="/admin/document_generation" onClick={onLinkClick} className="nav-item">
        Manage Form Document Templates
      </Link>
      <Link to="/admin/financial-code/list" onClick={onLinkClick} className="nav-item">
        Manage Project and Financial Codes
      </Link>
    </>
  );
};
