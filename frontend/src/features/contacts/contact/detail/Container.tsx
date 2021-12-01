import * as React from 'react';
import styled from 'styled-components';

import { ContactBreadcrumb } from '../..';
import * as Styled from '../../styles';

interface IContactViewContainerProps {
  match?: any;
}

//interface IContactContainerProps extends RouteComponentProps<MatchParams> {}

const ContactViewContainer: React.FunctionComponent<IContactViewContainerProps> = props => {
  //const [contactType, setContactType] = useState(getContactTypeFromId(id));
  const { contact } = useContactDetail(props?.match?.params?.contactId);
  return (
    <ContactLayout>
      <ContactBreadcrumb />
      <Styled.H1>Contact</Styled.H1>
      something
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
