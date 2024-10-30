import { Link } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { ISideTrayPageProps } from './SideTray';

/**
 * Contacts page with links to contact functionality.
 * Intended for use within the left side tray.
 */
export const ContactTray = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      {hasClaim(Claims.CONTACT_VIEW) && (
        <Link onClick={onLinkClick} to="/contact/list" className="pl-9 pb-3 nav-item">
          Manage Contacts
        </Link>
      )}
      {hasClaim(Claims.CONTACT_ADD) && (
        <Link onClick={onLinkClick} to="/contact/new/P" className="pl-9 pb-3 nav-item">
          Add a Contact
        </Link>
      )}
    </>
  );
};
