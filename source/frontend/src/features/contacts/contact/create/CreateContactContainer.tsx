import { getIn } from 'formik';
import React from 'react';
import { MdContactMail } from 'react-icons/md';
import { useHistory, useLocation, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import { ContactTypeSelector } from '@/features/contacts';
import { ContactTypes } from '@/features/contacts/interfaces';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';

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

  const onClose = () => {
    history.push('/mapview');
  };

  return (
    <ContactLayout>
      <MapSideBarLayout
        showCloseButton
        title="Add Contact"
        icon={<MdContactMail className="mr-2 mb-2" size={32} />}
        onClose={onClose}
      >
        <ContactTypeSelector
          contactType={contactType}
          setContactType={onSelectorChange}
        ></ContactTypeSelector>

        <ContactRouter />
      </MapSideBarLayout>

      {/* <Styled.H1>Add a Contact</Styled.H1> */}
    </ContactLayout>
  );
};

const ContactLayout = styled.div`
  display: flex;
  flex-direction: column;
  text-align: left;
  height: 100%;
  width: 50%;
  min-width: 93rem;
  overflow: hidden;
  padding: 1rem;
  gap: 1.6rem;

  h1 {
    border-bottom: none;
    margin-bottom: 0.2rem;
  }
`;

const getContactTypeFromPath = (pathname: string) => {
  const pageName = getIn(pathname.match(/\/contact\/new\/(.*)/), '1');
  if (!pageName) {
    return ContactTypes.INDIVIDUAL;
  }
  return pageName;
};

export default CreateContactContainer;
