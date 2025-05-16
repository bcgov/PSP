import React, { useCallback, useContext, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { IManagementActivitiesListViewProps } from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/list/ManagementActivitiesListView';
import { PropertyActivityRow } from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/list/models/PropertyActivityRow';
import { useManagementActivityRepository } from '@/hooks/repositories/useManagementActivityRepository';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import useIsMounted from '@/hooks/util/useIsMounted';

export interface IPropertyManagementActivitiesListContainerProps {
  managementFileId: number;
  View: React.FC<IManagementActivitiesListViewProps>;
}

const ManagementFileActivitiesListContainer: React.FunctionComponent<
  IPropertyManagementActivitiesListContainerProps
> = ({ managementFileId, View }) => {
  const history = useHistory();
  const isMounted = useIsMounted();
  const { setModalContent, setDisplayModal } = useModalContext();
  const [propertyActivities, setPropertyActivities] = useState<PropertyActivityRow[]>([]);
  const { staleLastUpdatedBy } = useContext(SideBarContext);

  const {
    getManagementActivities: { execute: getActivities, loading },
    deleteManagementActivity: { execute: deleteActivity, loading: deletingActivity },
  } = useManagementActivityRepository();

  const fetchPropertyActivities = useCallback(async () => {
    const response = await getActivities(managementFileId);
    if (response && isMounted()) {
      setPropertyActivities([...response.map(x => PropertyActivityRow.fromApi(x))]);
    }
  }, [getActivities, isMounted, managementFileId]);

  const onDelete = useCallback(
    async (activityId: number) => {
      const result = await deleteActivity(managementFileId, activityId);
      if (result === true) {
        fetchPropertyActivities();
        history.push(`/mapview/sidebar/management/${managementFileId}/activities`);
      }
    },
    [deleteActivity, fetchPropertyActivities, history, managementFileId],
  );

  useEffect(() => {
    fetchPropertyActivities();
  }, [fetchPropertyActivities, staleLastUpdatedBy]);
  //TODO: remove staleLastUpdatedBy when side bar context is refactored.

  const onCreate = () => {
    history.push(`/mapview/sidebar/management/${managementFileId}/activities/new`);
  };

  const onView = (activityId: number) => {
    history.push(`/mapview/sidebar/management/${managementFileId}/activities/${activityId}`);
  };

  return (
    <View
      isEmbedded={true}
      isLoading={loading || deletingActivity}
      propertyActivities={propertyActivities}
      onCreate={onCreate}
      onView={onView}
      onDelete={async (activityId: number) => {
        setModalContent({
          ...getDeleteModalProps(),
          handleOk: async () => {
            await onDelete(activityId);
            setDisplayModal(false);
          },
          handleCancel: () => {
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      }}
    />
  );
};

export default ManagementFileActivitiesListContainer;
