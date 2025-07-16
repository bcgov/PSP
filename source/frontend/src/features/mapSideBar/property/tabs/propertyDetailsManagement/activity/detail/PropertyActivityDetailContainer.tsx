import { useCallback, useEffect, useState } from 'react';

import { useManagementActivityPropertyRepository } from '@/hooks/repositories/useManagementActivityPropertyRepository';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { exists, isValidId } from '@/utils/utils';

import useActivityContactRetriever from '../hooks';
import { IPropertyActivityDetailViewProps } from './PropertyActivityDetailView';

export interface IPropertyActivityDetailContainerProps {
  propertyId: number;
  managementActivityId: number;
  onClose: () => void;
  viewEnabled: boolean;
  View: React.FunctionComponent<React.PropsWithChildren<IPropertyActivityDetailViewProps>>;
}

/**
 * Modal displaying form allowing add/update lease deposits. Save button triggers internal formik validation and submit.
 * @param viewEnabled defines the condition for the PopupTray to show based on the route exact match
 */
export const PropertyActivityDetailContainer: React.FunctionComponent<
  React.PropsWithChildren<IPropertyActivityDetailContainerProps>
> = ({ propertyId, managementActivityId, onClose, viewEnabled, View }) => {
  const [show, setShow] = useState(true);

  const [loadedActivity, setLoadedActivity] = useState<ApiGen_Concepts_ManagementActivity | null>(
    null,
  );

  const {
    fetchMinistryContacts,
    fetchPartiesContact,
    fetchProviderContact,
    isLoading: isContactLoading,
  } = useActivityContactRetriever();

  const {
    getActivity: { execute: getActivity, loading: getActivityLoading },
  } = useManagementActivityPropertyRepository();

  // Load the activity
  const fetchActivity = useCallback(
    async (propertyId: number, activityId: number) => {
      const retrieved = await getActivity(propertyId, activityId);
      if (exists(retrieved)) {
        if (exists(retrieved.ministryContacts)) {
          for (let i = 0; i < retrieved.ministryContacts.length; i++) {
            await fetchMinistryContacts(retrieved.ministryContacts[i]);
          }
        }
        if (exists(retrieved.involvedParties)) {
          for (let i = 0; i < retrieved.involvedParties.length; i++) {
            await fetchPartiesContact(retrieved.involvedParties[i]);
          }
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
    if (isValidId(propertyId) && isValidId(managementActivityId)) {
      fetchActivity(propertyId, managementActivityId);
    }
  }, [propertyId, managementActivityId, fetchActivity]);

  return (
    <View
      propertyId={propertyId}
      activity={loadedActivity}
      onClose={onClose}
      loading={getActivityLoading || isContactLoading}
      show={show && viewEnabled}
      setShow={setShow}
    />
  );
};
