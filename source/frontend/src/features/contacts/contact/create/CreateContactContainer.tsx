import { getIn } from 'formik';
import React from 'react';
import { useHistory, useLocation, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import { ContactBreadcrumb, ContactTypeSelector } from '@/features/contacts';
import { ContactTypes } from '@/features/contacts/interfaces';
import * as Styled from '@/features/contacts/styles';

import ContactRouter from './ContactRouter';

export const CreateContactContainer: React.FunctionComponent<
  React.PropsWithChildren<unknown>
> = () => {
  const history = useHistory();
  const { path } = useRouteMatch();
  const { pathname } = useLocation();

  const contactType = getContactTypeFromPath(pathname);

  const onSelectorChange = (newValue: ContactTypes) => {
    history.push(`${path}/${newValue}`);
  };

  return (
    <ContactLayout>
      <ContactBreadcrumb />
      <Styled.H1>Add a Contact</Styled.H1>

      <ContactTypeSelector
        contactType={contactType}
        setContactType={onSelectorChange}
      ></ContactTypeSelector>

      <ContactRouter />
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

const getContactTypeFromPath = (pathname: string) => {
  const pageName = getIn(pathname.match(/\/contact\/new\/(.*)/), '1');
  if (!pageName) {
    return ContactTypes.INDIVIDUAL;
  }
  return pageName;
};

export default CreateContactContainer;
