import { FieldArrayRenderProps, getIn, useFormikContext } from 'formik';
import { FaExternalLinkAlt } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import { SectionField } from '@/components/common/Section/SectionField';
import { getPrimaryContact } from '@/features/contacts/contactUtils';
import { LeaseFormModel } from '@/features/leases/models';
import { withNameSpace } from '@/utils/formUtils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { FormTenant } from './models';

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
  const { values } = useFormikContext<LeaseFormModel>();
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
      <SectionField labelWidth="2" contentWidth="10" label="Organization">
        {getIn(values, withNameSpace(nameSpace, 'summary')) && (
          <>
            <StyledLink to={`/contact/${tenant?.id}`} target="_blank" rel="noopener noreferrer">
              {getIn(values, withNameSpace(nameSpace, 'summary'))} <FaExternalLinkAlt />
            </StyledLink>
          </>
        )}
      </SectionField>
      <SectionField labelWidth="2" contentWidth="10" label="Primary Contact">
        {primaryContact && (
          <>
            <StyledLink
              to={`/contact/P${primaryContact?.id}`}
              target="_blank"
              rel="noopener noreferrer"
            >
              {primaryContactName} <FaExternalLinkAlt />
            </StyledLink>
          </>
        )}
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
