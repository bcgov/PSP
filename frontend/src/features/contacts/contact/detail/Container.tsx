import { useContactDetail } from 'features/contacts/hooks/useContactDetail';
import * as React from 'react';
import styled from 'styled-components';

import { ContactBreadcrumb } from '../..';
import * as Styled from '../../styles';
import PersonView from './Person';

interface IContactViewContainerProps {
  match?: any;
}

const ContactViewContainer: React.FunctionComponent<IContactViewContainerProps> = props => {
  const { contact } = useContactDetail(props?.match?.params?.id);
  console.log(props?.match?.params);
  return (
    <ContactLayout>
      <ContactBreadcrumb />
      <Styled.H1>Contact</Styled.H1>
      {contact?.person && <PersonView person={contact?.person} />}
    </ContactLayout>
  );
};

const ContactLayout = styled.div`
  height: 100%;
  width: 50%;
  min-width: 30rem;
  overflow: auto;
  padding: 1rem;
`;

export default ContactViewContainer;
