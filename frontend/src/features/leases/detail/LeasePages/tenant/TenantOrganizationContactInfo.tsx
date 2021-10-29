import { Form, Input } from 'components/common/form';
import * as Styled from 'features/leases/detail/styles';
import { FieldArrayRenderProps } from 'formik';
import * as React from 'react';
import { phoneFormatter, withNameSpace } from 'utils/formUtils';

import AddressSubForm from '../AddressSubForm';

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
  return (
    <>
      <Styled.FormGrid>
        <Form.Label>Tenant organization:</Form.Label>
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'name')} />
        <Form.Label>Contact name:</Form.Label>
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'contactName')} />
        <Styled.LeaseH3>Contact Info</Styled.LeaseH3>
        <AddressSubForm nameSpace={withNameSpace(nameSpace, 'address')} disabled={disabled} />
        <br />
        <Form.Label>E-mail address:</Form.Label>
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'email')} />
        <Form.Label>Phone</Form.Label>
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

export default TenantOrganizationContactInfo;
