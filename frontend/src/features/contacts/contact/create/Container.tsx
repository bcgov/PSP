import { ContactBreadcrumb, ContactTypes } from 'features/contacts';
import { useQuery } from 'hooks/use-query';
import * as React from 'react';
import { useState } from 'react';
import styled from 'styled-components';

import * as Styled from '../../styles';
import ContactTypeSelector from '../typeSelector/ContactTypeSelector';
import CreatePersonForm from './Person/CreatePersonForm';

export interface IContactCreateContainerProps {}

export const ContactCreateContainer: React.FunctionComponent<IContactCreateContainerProps> = () => {
  // get default contact type from URL; e.g. contact/new?type=P
  const { type } = useQuery();
  const [contactType, setContactType] = useState(getContactType(type as string));

  return (
    <ContactLayout>
      <ContactBreadcrumb />
      <Styled.H1>Add a Contact</Styled.H1>

      <ContactTypeSelector
        contactType={contactType}
        setContactType={setContactType}
      ></ContactTypeSelector>

      {/* TODO: Render Person Form or Organization Form depending on type query param */}
      <CreatePersonForm />
    </ContactLayout>
  );
};

const ContactLayout = styled.div`
  display: flex;
  flex-direction: column;
  text-align: left;
  height: 100%;
  width: 50%;
  min-width: 30rem;
  overflow: hidden;
  padding: 1rem;
  gap: 1.6rem;
`;

const getContactType = (value?: string) => {
  if (value !== undefined) {
    return value as ContactTypes;
  }
  return ContactTypes.INDIVIDUAL;
};

export default ContactCreateContainer;
