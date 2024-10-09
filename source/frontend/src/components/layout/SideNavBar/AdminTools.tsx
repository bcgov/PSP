import { Link } from 'react-router-dom';

import AdminIcon from '@/assets/images/admin-icon.svg?react';

import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';

/**
 * Admin Tools Tray page with links to admin functionality.
 * Intended for use within the left side tray.
 */
export const AdminTools = ({ onLinkClick }: ISideTrayPageProps) => {
  return (
    <>
      <Styled.TrayHeader>
        <span className="mr-2">
          <AdminIcon title="Admin Tools Icon" width="2.6rem" height="2.6rem" fill="currentColor" />
        </span>
        Admin Tools
      </Styled.TrayHeader>
      <Link to="/admin/users" onClick={onLinkClick} className="nav-item pl-9 pb-3">
        Manage Users
      </Link>
      <Link to="/admin/access/requests" onClick={onLinkClick} className="nav-item pl-9 pb-3">
        Manage Access Requests
      </Link>
      <Link to="/admin/document_generation" onClick={onLinkClick} className="nav-item pl-9 pb-3">
        Manage Form Document Templates
      </Link>
      <Link to="/admin/financial-code/list" onClick={onLinkClick} className="nav-item pl-9 pb-3">
        Manage Project and Financial Codes
      </Link>
    </>
  );
};
