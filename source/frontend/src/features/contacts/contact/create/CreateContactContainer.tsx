import { getIn } from 'formik';
import React from 'react';
import { MdContactMail } from 'react-icons/md';
import { useHistory, useLocation, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import { FormTitleBar } from '@/components/common/form/FormTitleBar';
import { ContactTypeSelector, HalfWidthLayout } from '@/features/contacts';
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
    <HalfWidthLayout>
      <FormTitleBar
        showCloseButton
        title="Add Contact"
        icon={<MdContactMail className="mr-2 mb-2" size={28} />}
        onClose={onClose}
      ></FormTitleBar>

      <StyledFormWrapper>
        <ContactTypeSelector
          contactType={contactType}
          setContactType={onSelectorChange}
        ></ContactTypeSelector>

        <ContactRouter />
      </StyledFormWrapper>
    </HalfWidthLayout>
  );
};

const StyledFormWrapper = styled.div`
  background-color: ${props => props.theme.css.highlightBackgroundColor};
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
