import { FaExternalLinkAlt } from 'react-icons/fa';

import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { exists } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { StyledLink } from '../maps/leaflet/LayerPopup/styles';

type ContactPersonLink = {
  person: ApiGen_Concepts_Person;
  organization?: never;
};
type ContactOrganizationLink = {
  person?: never;
  organization: ApiGen_Concepts_Organization;
};

export type IContactLinkProps = ContactPersonLink | ContactOrganizationLink;

function isPersonLink(
  contactLink: ContactPersonLink | ContactOrganizationLink,
): contactLink is ContactPersonLink {
  return exists(contactLink.person);
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
