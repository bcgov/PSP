import { Form, Input } from 'components/common/form';
import { FieldArrayRenderProps } from 'formik';
import * as React from 'react';
import styled from 'styled-components';
import { phoneFormatter, withNameSpace } from 'utils/formUtils';

import * as Styled from '../../styles';
import AddressSubForm from '../AddressSubForm';

export interface ITenantPersonContactInfoProps {
  nameSpace: string;
  disabled?: boolean;
}

/**
 * Sub-form displaying a person tenant associated to the current lease.
 * @param {ITenantPersonContactInfoProps} param0
 */
export const TenantPersonContactInfo: React.FunctionComponent<ITenantPersonContactInfoProps &
  Partial<FieldArrayRenderProps>> = ({ nameSpace, disabled }) => {
  return (
    <>
      <Styled.FormGrid>
        <Form.Label>Tenant Name:</Form.Label>
        <StyledLargeTextInput disabled={disabled} field={withNameSpace(nameSpace, 'summary')} />
        <Styled.LeaseH3>Contact Info</Styled.LeaseH3>
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

export default TenantPersonContactInfo;
