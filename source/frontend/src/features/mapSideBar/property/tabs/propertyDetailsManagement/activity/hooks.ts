import { useCallback } from 'react';

import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import {
  Api_PropertyActivity,
  Api_PropertyActivityInvolvedParty,
  Api_PropertyMinistryContact,
} from '@/models/api/PropertyActivity';

const useActivityContactRetriever = () => {
  const {
    getOrganizationDetail: { execute: getOrganization, loading: loadingOrganization },
  } = useOrganizationRepository();

  const {
    getPersonDetail: { execute: getPerson, loading: loadingPerson },
  } = usePersonRepository();

  const fetchMinistryContacts = useCallback(
    async (c: Api_PropertyMinistryContact) => {
      if (c.personId !== undefined && c.personId !== null) {
        const person = await getPerson(c.personId);
        if (person !== undefined) {
          c.person = person;
        }
      }
    },
    [getPerson],
  );

  const fetchPartiesContact = useCallback(
    async (c: Api_PropertyActivityInvolvedParty) => {
      if (c.personId !== undefined && c.personId !== null) {
        const person = await getPerson(c.personId);
        if (person !== undefined) {
          c.person = person;
        }
      }
      if (c.organizationId !== null) {
        const organization = await getOrganization(c.organizationId);
        if (organization !== undefined) {
          c.organization = organization;
        }
      }
    },
    [getPerson, getOrganization],
  );

  const fetchProviderContact = useCallback(
    async (c: Api_PropertyActivity) => {
      if (c.serviceProviderPersonId !== undefined && c.serviceProviderPersonId !== null) {
        const person = await getPerson(c.serviceProviderPersonId);
        if (person !== undefined) {
          c.serviceProviderPerson = person;
        }
      }
      if (c.serviceProviderOrgId !== null) {
        const organization = await getOrganization(c.serviceProviderOrgId);
        if (organization !== undefined) {
          c.serviceProviderOrg = organization;
        }
      }
    },
    [getPerson, getOrganization],
  );

  return {
    fetchMinistryContacts,
    fetchPartiesContact,
    fetchProviderContact,
    isLoading: loadingOrganization || loadingPerson,
  };
};

export default useActivityContactRetriever;
