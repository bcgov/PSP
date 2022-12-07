import { getPrimaryContact } from 'features/contacts/contactUtils';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { FieldArrayRenderProps, getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import * as React from 'react';
import { FaExternalLinkAlt } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import styled from 'styled-components';
import { withNameSpace } from 'utils/formUtils';
import { formatApiPersonNames } from 'utils/personUtils';

import { FormTenant } from './Tenant';

export interface ITenantOrganizationContactInfoProps {
  nameSpace: string;
  disabled?: boolean;
}

/**
 * Sub-form displaying a organization tenant associated to the current lease.
 * @param {ITenantOrganizationContactInfoProps} param0
 */
export const TenantOrganizationContactInfo: React.FunctionComponent<
  React.PropsWithChildren<ITenantOrganizationContactInfoProps & Partial<FieldArrayRenderProps>>
> = ({ nameSpace, disabled }) => {
  const { values } = useFormikContext<IFormLease>();
  const tenant: FormTenant = getIn(values, nameSpace);
  let primaryContact = tenant?.initialPrimaryContact;
  if (primaryContact?.id !== tenant?.primaryContactId) {
    primaryContact = tenant?.primaryContactId
      ? getPrimaryContact(tenant?.primaryContactId, tenant)
      : undefined;
  }
  const primaryContactName = formatApiPersonNames(primaryContact);
  return (
    <StyledSectionWrapper>
      <SectionField labelWidth="2" contentWidth="4" label="Organization">
        <StyledLink to={`/contact/${tenant?.id}`}>
          {getIn(values, withNameSpace(nameSpace, 'summary'))}
        </StyledLink>
        <Link to={`/contact/${tenant?.id}`} target="_blank" rel="noopener noreferrer">
          <FaExternalLinkAlt />
        </Link>
      </SectionField>
      <SectionField labelWidth="2" contentWidth="4" label="Primary Contact">
        <StyledLink to={`/contact/P${primaryContact?.id}`}>{primaryContactName}</StyledLink>
        <Link to={`/contact/P${primaryContact?.id}`} target="_blank" rel="noopener noreferrer">
          <FaExternalLinkAlt />
        </Link>
      </SectionField>
    </StyledSectionWrapper>
  );
};

const StyledSectionWrapper = styled.div`
  border-bottom: 0.1rem gray solid;
  padding: 0.5rem;
`;

const StyledLink = styled(Link)`
  padding: 0.6rem 1.2rem;
`;
export default TenantOrganizationContactInfo;
