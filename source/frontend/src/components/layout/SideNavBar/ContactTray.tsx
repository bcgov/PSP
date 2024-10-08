import { Link } from 'react-router-dom';

import ContactIcon from '@/assets/images/contact-icon.svg?react';
import { Claims } from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

import { ISideTrayPageProps } from './SideTray';
import * as Styled from './styles';

/**
 * Contacts page with links to contact functionality.
 * Intended for use within the left side tray.
 */
export const ContactTray = ({ onLinkClick }: ISideTrayPageProps) => {
  const { hasClaim } = useKeycloakWrapper();
  return (
    <>
      <Styled.TrayHeader>
        <span className="mr-2">
          <ContactIcon fill="currentColor" />
        </span>
        Contacts
      </Styled.TrayHeader>
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
