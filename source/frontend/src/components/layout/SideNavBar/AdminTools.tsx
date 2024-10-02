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
          <AdminIcon fill="currentColor" />
        </span>
        Admin Tools
      </Styled.TrayHeader>
      <Link className="pl-9 pb-3" to="/admin/users" onClick={onLinkClick}>
        Manage Users
      </Link>
      <Link className="pl-9 pb-3" to="/admin/access/requests" onClick={onLinkClick}>
        Manage Access Requests
      </Link>
      <Link className="pl-9 pb-3" to="/admin/document_generation" onClick={onLinkClick}>
        Manage Form Document Templates
      </Link>
      <Link className="pl-9 pb-3" to="/admin/financial-code/list" onClick={onLinkClick}>
        Manage Project and Financial Codes
      </Link>
    </>
  );
};
