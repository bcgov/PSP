import { Link } from 'react-router-dom';

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
      <Styled.TrayHeader>Contacts</Styled.TrayHeader>
      {hasClaim(Claims.CONTACT_VIEW) && (
        <Link onClick={onLinkClick} to="/contact/list">
          Manage Contacts
        </Link>
      )}
      {hasClaim(Claims.CONTACT_ADD) && (
        <Link onClick={onLinkClick} to="/contact/new/P">
          Add a Contact
        </Link>
      )}
    </>
  );
};
