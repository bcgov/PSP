import { useCallback, useEffect, useState } from 'react';

import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { exists, isValidId } from '@/utils';

import ContactLink from './ContactLink';
import { ISectionFieldProps, SectionField } from './Section/SectionField';

interface IContactFieldContainerProps extends ISectionFieldProps {
  personId: number | null;
  organizationId: number | null;
  primaryContact: number | null;
}

export const ContactFieldContainer: React.FunctionComponent<
  React.PropsWithChildren<IContactFieldContainerProps>
> = props => {
  const [person, setPerson] = useState<ApiGen_Concepts_Person | null>(null);
  const [organization, setOrganization] = useState<ApiGen_Concepts_Organization | null>(null);
  const [primaryContact, setPrimaryContact] = useState<ApiGen_Concepts_Person | null>(null);

  const {
    getPersonDetail: { execute: getPerson, loading: getPersonLoading },
  } = usePersonRepository();

  const {
    getOrganizationDetail: { execute: getOrganization, loading: getOrganizationLoading },
  } = useOrganizationRepository();

  const fetchData = useCallback(async () => {
    if (isValidId(props.personId)) {
      const returnedPerson = await getPerson(props.personId);
      if (exists(returnedPerson)) {
        setPerson(returnedPerson);
      }
    }
    if (isValidId(props.organizationId)) {
      const returnedOrganization = await getOrganization(props.organizationId);
      if (exists(returnedOrganization)) {
        setOrganization(returnedOrganization);
      }
    }

    if (isValidId(props.primaryContact)) {
      const returnedPrimaryContact = await getPerson(props.primaryContact);
      if (exists(returnedPrimaryContact)) {
        setPrimaryContact(returnedPrimaryContact);
      }
    }
  }, [getOrganization, getPerson, props.organizationId, props.personId, props.primaryContact]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);
  if (getPersonLoading || getOrganizationLoading) {
    return <></>;
  }

  return (
    <SectionField {...props}>
      {exists(person) && <ContactLink person={person} />}
      {exists(organization) && <ContactLink organization={organization} />}
      {exists(primaryContact) && <ContactLink person={primaryContact} />}
    </SectionField>
  );
};

export default ContactFieldContainer;
