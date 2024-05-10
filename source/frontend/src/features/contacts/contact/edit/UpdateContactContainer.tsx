import React from 'react';
import { MdContactMail } from 'react-icons/md';
import { useHistory, useParams } from 'react-router-dom';
import styled from 'styled-components';

import { FormTitleBar } from '@/components/common/form/FormTitleBar';
import { ContactTypes } from '@/features/contacts/interfaces';
import { HalfWidthLayout } from '@/features/contacts/styles';

import UpdateOrganizationForm from './Organization/UpdateOrganizationForm';
import UpdatePersonForm from './Person/UpdatePersonForm';

export const UpdateContactContainer: React.FC<React.PropsWithChildren<unknown>> = () => {
  const history = useHistory();
  // get id from route params -> /contact/{id}/edit
  const { id } = useParams<{ id: string }>();
  const showPerson = id && id.startsWith(ContactTypes.INDIVIDUAL);
  const showOrganization = id && id.startsWith(ContactTypes.ORGANIZATION);

  // route parameter ID values have a prefix eg P31, O12
  // we want to ignore the prefix when getting the contact Id
  const idNumber = parseInt(id.substring(1));

  const onClose = () => {
    history.push('/mapview');
  };

  return (
    <HalfWidthLayout>
      <FormTitleBar
        showCloseButton
        title="Update Contact"
        icon={<MdContactMail className="mr-2 mb-2" size={32} />}
        onClose={onClose}
      ></FormTitleBar>

      <StyledFormWrapper>
        {showPerson && <UpdatePersonForm id={idNumber} />}
        {showOrganization && <UpdateOrganizationForm id={idNumber} />}
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

export default UpdateContactContainer;
