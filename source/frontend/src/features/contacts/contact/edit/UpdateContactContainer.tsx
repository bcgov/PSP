import React from 'react';
import { useParams } from 'react-router-dom';

import { ContactBreadcrumb } from '@/features/contacts';
import { ContactTypes } from '@/features/contacts/interfaces';
import * as Styled from '@/features/contacts/styles';

import { HalfWidthLayout } from '../create/styles';
import UpdateOrganizationForm from './Organization/UpdateOrganizationForm';
import UpdatePersonForm from './Person/UpdatePersonForm';

export const UpdateContactContainer: React.FC<React.PropsWithChildren<unknown>> = () => {
  // get id from route params -> /contact/{id}/edit
  const { id } = useParams<{ id: string }>();
  const showPerson = id && id.startsWith(ContactTypes.INDIVIDUAL);
  const showOrganization = id && id.startsWith(ContactTypes.ORGANIZATION);

  // route parameter ID values have a prefix eg P31, O12
  // we want to ignore the prefix when getting the contact Id
  const idNumber = parseInt(id.substring(1));

  return (
    <HalfWidthLayout>
      <ContactBreadcrumb />
      <Styled.H1>Update Contact</Styled.H1>
      {showPerson && <UpdatePersonForm id={idNumber} />}
      {showOrganization && <UpdateOrganizationForm id={idNumber} />}
    </HalfWidthLayout>
  );
};

export default UpdateContactContainer;
