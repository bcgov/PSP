import { uniq } from 'lodash';
import { useCallback, useContext, useEffect, useState } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import useActivityContactRetriever from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/hooks';
import { useManagementActivityRepository } from '@/hooks/repositories/useManagementActivityRepository';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { exists, isValidId } from '@/utils/utils';

import ManagementStatusUpdateSolver from '../../fileDetails/detail/ManagementStatusUpdateSolver';
import { IFileActivityDetailViewProps } from './FileActivityDetailView';

export interface IPropertyActivityDetailContainerProps {
  managementFileId: number;
  managementActivityId: number;
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
> = ({ managementFileId, managementActivityId, onClose, viewEnabled, View }) => {
  const [show, setShow] = useState(true);
  const { file } = useContext(SideBarContext);
  const castedFile = file as unknown as ApiGen_Concepts_ManagementFile;

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
    if (isValidId(managementFileId) && isValidId(managementActivityId)) {
      fetchActivity(managementFileId, managementActivityId);
    }
  }, [managementFileId, managementActivityId, fetchActivity]);

  const StatusSolver = new ManagementStatusUpdateSolver(castedFile);

  return (
    <View
      managementId={managementFileId}
      activity={loadedActivity}
      onClose={onClose}
      loading={getActivityLoading || isContactLoading || loadingProperties}
      show={show && viewEnabled}
      canEditActivity={StatusSolver.canEditActivities()}
      canEditDocuments={StatusSolver.canEditDocuments()}
      setShow={setShow}
    />
  );
};
