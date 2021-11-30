import * as React from 'react';
import { useState } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import styled from 'styled-components';

import { ContactBreadcrumb, ContactTypeSelector } from '../..';
import * as Styled from '../../styles';

interface MatchParams {
  id?: string;
}

export enum ContactTypes {
  ORGANIZATION = 'O',
  INDIVIDUAL = 'P',
}

export interface IContactContainerProps extends RouteComponentProps<MatchParams> {
  type: ContactTypes;
}

export const ContactContainer: React.FunctionComponent<IContactContainerProps> = ({
  match: {
    params: { id },
  },
}) => {
  const [contactType, setContactType] = useState(getContactTypeFromId(id));
  return (
    <ContactLayout>
      <ContactBreadcrumb />
      <Styled.H1>Add a Contact</Styled.H1>

      {!id && (
        <ContactTypeSelector
          contactType={contactType}
          setContactType={setContactType}
        ></ContactTypeSelector>
      )}
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

const getContactTypeFromId = (id?: string) => {
  if (id?.length !== undefined && id.length > 0) {
    return id[0] as ContactTypes;
  }
  return ContactTypes.INDIVIDUAL;
};

export default ContactContainer;
