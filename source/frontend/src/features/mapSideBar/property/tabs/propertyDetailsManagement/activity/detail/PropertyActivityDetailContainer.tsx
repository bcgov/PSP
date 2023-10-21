import { useCallback, useEffect, useState } from 'react';

import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import { usePropertyActivityRepository } from '@/hooks/repositories/usePropertyActivityRepository';
import {
  Api_PropertyActivity,
  Api_PropertyActivityInvolvedParty,
  Api_PropertyMinistryContact,
} from '@/models/api/PropertyActivity';

import { IPropertyActivityDetailViewProps } from './PropertyActivityDetailView';

export interface IPropertyActivityDetailContainerProps {
  propertyId: number;
  propertyActivityId: number;
  onClose: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IPropertyActivityDetailViewProps>>;
}

export const PropertyActivityDetailContainer: React.FunctionComponent<
  React.PropsWithChildren<IPropertyActivityDetailContainerProps>
> = ({ propertyId, propertyActivityId, onClose, View }) => {
  const [show, setShow] = useState(true);

  const [loadedActivity, setLoadedActivity] = useState<Api_PropertyActivity | null>(null);

  const {
    getActivity: { execute: getActivity, loading: getActivityLoading },
  } = usePropertyActivityRepository();

  const {
    getPersonDetail: { execute: getPerson, loading: loadingPerson },
  } = usePersonRepository();

  const {
    getOrganizationDetail: { execute: getOrganization, loading: loadingOrganization },
  } = useOrganizationRepository();

  const fetchMinistryContacs = useCallback(
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
      if (!!c.organizationId) {
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
      if (!!c.serviceProviderOrgId) {
        const organization = await getOrganization(c.serviceProviderOrgId);
        if (organization !== undefined) {
          c.serviceProviderOrg = organization;
        }
      }
    },
    [getPerson, getOrganization],
  );

  // Load the activity
  const fetchActivity = useCallback(
    async (propertyId: number, activityId: number) => {
      const retrieved = await getActivity(propertyId, activityId);
      if (retrieved !== undefined) {
        for (let i = 0; i < retrieved.ministryContacts.length; i++) {
          await fetchMinistryContacs(retrieved.ministryContacts[i]);
        }
        for (let i = 0; i < retrieved.involvedParties.length; i++) {
          await fetchPartiesContact(retrieved.involvedParties[i]);
        }
        await fetchProviderContact(retrieved);

        setLoadedActivity(retrieved);
      } else {
        setLoadedActivity(null);
      }
    },
    [getActivity, fetchProviderContact, fetchMinistryContacs, fetchPartiesContact],
  );

  useEffect(() => {
    if (propertyId !== undefined && propertyActivityId !== undefined) {
      fetchActivity(propertyId, propertyActivityId);
    }
  }, [propertyId, propertyActivityId, fetchActivity]);

  return (
    <View
      propertyId={propertyId}
      activity={loadedActivity}
      onClose={onClose}
      loading={getActivityLoading || loadingOrganization || loadingPerson}
      show={show}
      setShow={setShow}
    />
  );
};
