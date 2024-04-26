import { Form } from 'react-bootstrap';
import styled from 'styled-components';

import { InlineFlexDiv } from '@/components/common/styles';
import { ContactTypes } from '@/features/contacts/interfaces';

interface IContactTypeSelectorProps {
  disabled?: boolean;
  contactType: ContactTypes;
  setContactType: (contactType: ContactTypes) => void;
}

export const ContactTypeSelector: React.FunctionComponent<
  React.PropsWithChildren<IContactTypeSelectorProps>
> = ({ contactType, setContactType, disabled }) => {
  return (
    <StyledInlineFlex>
      <Form.Check
        id="contact-individual"
        type="radio"
        label="Individual"
        disabled={disabled}
        checked={contactType === ContactTypes.INDIVIDUAL}
        onChange={() => setContactType(ContactTypes.INDIVIDUAL)}
      ></Form.Check>
      <Form.Check
        id="contact-organization"
        label="Organization"
        type="radio"
        disabled={disabled}
        checked={contactType === ContactTypes.ORGANIZATION}
        onChange={() => setContactType(ContactTypes.ORGANIZATION)}
      ></Form.Check>
    </StyledInlineFlex>
  );
};

const StyledInlineFlex = styled(InlineFlexDiv)`
  gap: 5rem;
  background-color: white;
  border-radius: 0.5rem;
  margin: 1.5rem;
  margin-right: 3rem;
  padding: 1.5rem;
  .form-check {
    display: flex;
    align-items: center;
    padding-left: 2rem;
  }
`;

export default ContactTypeSelector;
