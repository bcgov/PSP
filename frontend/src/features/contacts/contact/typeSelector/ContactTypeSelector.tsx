import { InlineFlexDiv } from 'components/common/styles';
import * as React from 'react';
import { Form } from 'react-bootstrap';
import styled from 'styled-components';

import { ContactTypes } from '../..';

interface IContactTypeSelectorProps {
  disabled?: boolean;
  contactType: ContactTypes;
  setContactType: (contactType: ContactTypes) => void;
}

export const ContactTypeSelector: React.FunctionComponent<IContactTypeSelectorProps> = ({
  contactType,
  setContactType,
  disabled,
}) => {
  return (
    <StyledInlineFlex>
      <Form.Check
        id="contact-individual"
        type="radio"
        label="Individual"
        disabled={disabled}
        checked={contactType === ContactTypes.INDIVIDUAL}
        onChange={(e: React.SyntheticEvent) => setContactType(ContactTypes.INDIVIDUAL)}
      ></Form.Check>
      <Form.Check
        id="contact-organization"
        label="Organization"
        type="radio"
        disabled={disabled}
        checked={contactType === ContactTypes.ORGANIZATION}
        onChange={(e: React.SyntheticEvent) => setContactType(ContactTypes.ORGANIZATION)}
      ></Form.Check>
    </StyledInlineFlex>
  );
};

const StyledInlineFlex = styled(InlineFlexDiv)`
  gap: 5rem;
  background-color: ${props => props.theme.css.filterBackgroundColor};
  padding: 0.5rem 3rem;
  .form-check {
    display: flex;
    align-items: center;
    padding-left: 2rem;
  }
`;

export default ContactTypeSelector;
