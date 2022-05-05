import { Form, Input } from 'components/common/form';
import { getPrimaryContact } from 'features/contacts/contactUtils';
import * as Styled from 'features/leases/detail/styles';
import { FieldArrayRenderProps, getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import * as React from 'react';
import { Link } from 'react-router-dom';
import styled from 'styled-components';
import { phoneFormatter, withNameSpace } from 'utils/formUtils';
import { formatApiPersonNames } from 'utils/personUtils';

import AddressSubForm from '../AddressSubForm';
import { FormTenant } from './Tenant';

export interface ITenantOrganizationContactInfoProps {
  nameSpace: string;
  disabled?: boolean;
}

/**
 * Sub-form displaying a organization tenant associated to the current lease.
 * @param {ITenantOrganizationContactInfoProps} param0
 */
export const TenantOrganizationContactInfo: React.FunctionComponent<ITenantOrganizationContactInfoProps &
  Partial<FieldArrayRenderProps>> = ({ nameSpace, disabled }) => {
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
    <>
      <Styled.FormGrid>
        <Form.Label>Tenant organization:</Form.Label>
        <StyledLargeTextInput disabled={disabled} field={withNameSpace(nameSpace, 'summary')} />
        <Form.Label>Primary Contact:</Form.Label>
        <Form.Group className="input">
          <StyledLink to={`/contact/P${primaryContact?.id}`}>{primaryContactName}</StyledLink>
        </Form.Group>
        <Styled.LeaseH3>Tenant information</Styled.LeaseH3>
        <AddressSubForm
          nameSpace={withNameSpace(nameSpace, 'mailingAddress')}
          disabled={disabled}
        />
        <br />
        <Form.Label>E-mail address:</Form.Label>
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'email')} />
        <Form.Label>Phone:</Form.Label>
        <Styled.NestedInlineField
          label="Landline:"
          disabled={disabled}
          field={withNameSpace(nameSpace, 'landline')}
          onBlurFormatter={phoneFormatter}
          pattern={/(\d\d\d)[\s-]?(\d\d\d)[\s-]?(\d\d\d\d)/}
        />
        <Styled.NestedInlineField
          label="Mobile:"
          disabled={disabled}
          field={withNameSpace(nameSpace, 'mobile')}
          onBlurFormatter={phoneFormatter}
          pattern={/(\d\d\d)[\s-]?(\d\d\d)[\s-]?(\d\d\d\d)/}
        />
      </Styled.FormGrid>
    </>
  );
};

const StyledLargeTextInput = styled(Input)`
  input {
    font-size: 1.8rem;
  }
`;

const StyledLink = styled(Link)`
  padding: 0.6rem 1.2rem;
`;
export default TenantOrganizationContactInfo;
