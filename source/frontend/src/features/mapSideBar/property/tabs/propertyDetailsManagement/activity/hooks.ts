import { useCallback } from 'react';

import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { ApiGen_Concepts_ManagementActivityInvolvedParty } from '@/models/api/generated/ApiGen_Concepts_ManagementActivityInvolvedParty';
import { ApiGen_Concepts_PropertyMinistryContact } from '@/models/api/generated/ApiGen_Concepts_PropertyMinistryContact';
import { isValidId } from '@/utils';

const useActivityContactRetriever = () => {
  const {
    getOrganizationDetail: { execute: getOrganization, loading: loadingOrganization },
  } = useOrganizationRepository();

  const {
    getPersonDetail: { execute: getPerson, loading: loadingPerson },
  } = usePersonRepository();

  const fetchMinistryContacts = useCallback(
    async (c: ApiGen_Concepts_PropertyMinistryContact) => {
      if (isValidId(c.personId)) {
        const person = await getPerson(c.personId);
        if (person !== undefined) {
          c.person = person;
        }
      }
    },
    [getPerson],
  );

  const fetchPartiesContact = useCallback(
    async (c: ApiGen_Concepts_ManagementActivityInvolvedParty) => {
      if (isValidId(c.personId)) {
        const person = await getPerson(c.personId);
        if (person !== undefined) {
          c.person = person;
        }
      }
      if (isValidId(c.organizationId)) {
        const organization = await getOrganization(c.organizationId);
        if (organization !== undefined) {
          c.organization = organization;
        }
      }
    },
    [getPerson, getOrganization],
  );

  const fetchProviderContact = useCallback(
    async (c: ApiGen_Concepts_ManagementActivity) => {
      if (isValidId(c.serviceProviderPersonId)) {
        const person = await getPerson(c.serviceProviderPersonId);
        if (person !== undefined) {
          c.serviceProviderPerson = person;
        }
      }
      if (isValidId(c.serviceProviderOrgId)) {
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
