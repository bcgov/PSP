import { FaExternalLinkAlt } from 'react-icons/fa';

import { Api_Organization } from '@/models/api/Organization';
import { Api_Person } from '@/models/api/Person';
import { formatApiPersonNames } from '@/utils/personUtils';

import { StyledLink } from '../maps/leaflet/LayerPopup/styles';

type ContactPersonLink = {
  person: Api_Person;
  organization?: never;
};
type ContactOrganizationLink = {
  person?: never;
  organization: Api_Organization;
};

export type IContactLinkProps = ContactPersonLink | ContactOrganizationLink;

function isPersonLink(
  contactLink: ContactPersonLink | ContactOrganizationLink,
): contactLink is ContactPersonLink {
  return contactLink.person !== undefined;
}

export const ContactLink: React.FunctionComponent<
  React.PropsWithChildren<IContactLinkProps>
> = props => {
  let displayName = '';
  let contactLink = '';

  if (isPersonLink(props)) {
    displayName = formatApiPersonNames(props.person);
    contactLink = 'P' + props.person?.id;
  } else {
    displayName = props.organization?.name ?? '';
    contactLink = 'O' + props.organization.id;
  }

  return (
    <StyledLink target="_blank" rel="noopener noreferrer" to={`/contact/${contactLink}`}>
      <span>{displayName}</span>
      <FaExternalLinkAlt className="ml-2" size="1rem" />
    </StyledLink>
  );
};

export default ContactLink;
