import { useCallback, useEffect, useState } from 'react';

import { usePropertyActivityRepository } from '@/hooks/repositories/usePropertyActivityRepository';
import { Api_PropertyActivity } from '@/models/api/PropertyActivity';

import useActivityContactRetriever from '../hooks';
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
    fetchMinistryContacts,
    fetchPartiesContact,
    fetchProviderContact,
    isLoading: isContactLoading,
  } = useActivityContactRetriever();

  const {
    getActivity: { execute: getActivity, loading: getActivityLoading },
  } = usePropertyActivityRepository();

  // Load the activity
  const fetchActivity = useCallback(
    async (propertyId: number, activityId: number) => {
      const retrieved = await getActivity(propertyId, activityId);
      if (retrieved !== undefined) {
        for (let i = 0; i < retrieved.ministryContacts.length; i++) {
          await fetchMinistryContacts(retrieved.ministryContacts[i]);
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
    [fetchMinistryContacts, fetchPartiesContact, fetchProviderContact, getActivity],
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
      loading={getActivityLoading || isContactLoading}
      show={show}
      setShow={setShow}
    />
  );
};
