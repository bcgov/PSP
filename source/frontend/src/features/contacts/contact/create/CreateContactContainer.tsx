import { getIn } from 'formik';
import React from 'react';
import { MdContactMail } from 'react-icons/md';
import { useHistory, useLocation, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import { FormTitleBar } from '@/components/common/form/FormTitleBar';
import { ContactTypeSelector } from '@/features/contacts';
import { ContactTypes } from '@/features/contacts/interfaces';

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
      <FormTitleBar
        showCloseButton
        title="Add Contact"
        icon={<MdContactMail className="mr-2 mb-2" size={32} />}
        onClose={onClose}
      ></FormTitleBar>

      <StyledFormWrapper>
        <ContactTypeSelector
          contactType={contactType}
          setContactType={onSelectorChange}
        ></ContactTypeSelector>

        <ContactRouter />
      </StyledFormWrapper>
    </ContactLayout>
  );
};

const ContactLayout = styled.div`
  position: relative;
  display: flex;
  flex-direction: column;
  height: 100%;
  width: 50%;
  min-width: 93rem;
  overflow: hidden;
  padding: 1.4rem 1.6rem;
  padding-bottom: 0;
  /* text-align: left; */
  /* gap: 1.6rem; */
`;

const StyledFormWrapper = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
  overflow: inherit;
  display: flex;
  flex-direction: column;
  flex: 1;
`;

const getContactTypeFromPath = (pathname: string) => {
  const pageName = getIn(pathname.match(/\/contact\/new\/(.*)/), '1');
  if (!pageName) {
    return ContactTypes.INDIVIDUAL;
  }
  return pageName;
};

export default CreateContactContainer;
