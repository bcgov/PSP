import { uniq } from 'lodash';
import { useCallback, useEffect, useState } from 'react';

import useActivityContactRetriever from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/hooks';
import { useManagementActivityRepository } from '@/hooks/repositories/useManagementActivityRepository';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { exists, isValidId } from '@/utils/utils';

import { IFileActivityDetailViewProps } from './FileActivityDetailView';

export interface IPropertyActivityDetailContainerProps {
  managementFileId: number;
  propertyActivityId: number;
  onClose: () => void;
  viewEnabled: boolean;
  View: React.FunctionComponent<React.PropsWithChildren<IFileActivityDetailViewProps>>;
}

/**
 * Modal displaying form allowing add/update lease deposits. Save button triggers internal formik validation and submit.
 * @param viewEnabled defines the condition for the PopupTray to show based on the route exact match
 */
export const FileActivityDetailContainer: React.FunctionComponent<
  React.PropsWithChildren<IPropertyActivityDetailContainerProps>
> = ({ managementFileId, propertyActivityId, onClose, viewEnabled, View }) => {
  const [show, setShow] = useState(true);

  const [loadedActivity, setLoadedActivity] = useState<ApiGen_Concepts_PropertyActivity | null>(
    null,
  );

  const {
    fetchMinistryContacts,
    fetchPartiesContact,
    fetchProviderContact,
    isLoading: isContactLoading,
  } = useActivityContactRetriever();

  const {
    getManagementActivity: { execute: getManagementActivity, loading: getActivityLoading },
  } = useManagementActivityRepository();

  const {
    getAllPropertiesById: { execute: getAllPropertiesById, loading: loadingProperties },
  } = usePimsPropertyRepository();

  // Load the activity
  const fetchActivity = useCallback(
    async (managementFileId: number, activityId: number) => {
      const retrieved = await getManagementActivity(managementFileId, activityId);
      if (exists(retrieved)) {
        if (exists(retrieved.ministryContacts)) {
          for (const ministryContact of retrieved.ministryContacts) {
            await fetchMinistryContacts(ministryContact);
          }
        }
        if (exists(retrieved.involvedParties)) {
          for (const party of retrieved.involvedParties) {
            await fetchPartiesContact(party);
          }
        }
        await fetchProviderContact(retrieved);

        const propertyIds = uniq(retrieved.activityProperties?.flatMap(ap => ap.propertyId));
        const propertiesResponse = await getAllPropertiesById(propertyIds);
        retrieved.activityProperties.forEach(
          ap => (ap.property = propertiesResponse?.find(p => p.id === ap.propertyId)),
        );

        setLoadedActivity(retrieved);
      } else {
        setLoadedActivity(null);
      }
    },
    [
      fetchMinistryContacts,
      fetchPartiesContact,
      fetchProviderContact,
      getManagementActivity,
      getAllPropertiesById,
    ],
  );

  useEffect(() => {
    if (isValidId(managementFileId) && isValidId(propertyActivityId)) {
      fetchActivity(managementFileId, propertyActivityId);
    }
  }, [managementFileId, propertyActivityId, fetchActivity]);

  return (
    <View
      managementId={managementFileId}
      activity={loadedActivity}
      onClose={onClose}
      loading={getActivityLoading || isContactLoading || loadingProperties}
      show={show && viewEnabled}
      setShow={setShow}
    />
  );
};
