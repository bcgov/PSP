import React, { useCallback, useContext, useEffect, useState } from 'react';

import { TableSort } from '@/components/Table/TableSort';
import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { IManagementActivitiesListViewProps } from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/list/ManagementActivitiesListView';
import { PropertyActivityRow } from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/list/models/PropertyActivityRow';
import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import { useManagementActivityRepository } from '@/hooks/repositories/useManagementActivityRepository';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';

import ManagementStatusUpdateSolver from '../../fileDetails/detail/ManagementStatusUpdateSolver';

export interface IPropertyManagementActivitiesListContainerProps {
  managementFileId: number;
  statusSolver: ManagementStatusUpdateSolver;
  View: React.FC<IManagementActivitiesListViewProps>;
}

const ManagementFileActivitiesListContainer: React.FunctionComponent<
  IPropertyManagementActivitiesListContainerProps
> = ({ managementFileId, statusSolver, View }) => {
  const isMounted = useIsMounted();
  const { setModalContent, setDisplayModal } = useModalContext();
  const [propertyActivities, setPropertyActivities] = useState<PropertyActivityRow[]>([]);
  const { staleLastUpdatedBy } = useContext(SideBarContext);
  const [sort, setSort] = useState<TableSort<ApiGen_Concepts_ManagementActivity>>({});

  const pathGenerator = usePathGenerator();

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
        pathGenerator.showDetails('management', managementFileId, 'activities', true);
      }
    },
    [deleteActivity, fetchPropertyActivities, managementFileId, pathGenerator],
  );

  useEffect(() => {
    fetchPropertyActivities();
  }, [fetchPropertyActivities, staleLastUpdatedBy]);
  //TODO: remove staleLastUpdatedBy when side bar context is refactored.

  const onCreate = () => {
    pathGenerator.addDetail('management', managementFileId, 'activities');
  };

  const onView = (activityId: number) => {
    pathGenerator.showDetail('management', managementFileId, 'activities', activityId, false);
  };

  const canEditActivities: boolean = statusSolver?.canEditActivities();

  return (
    <View
      isLoading={loading || deletingActivity}
      propertyActivities={propertyActivities}
      canEditActivities={canEditActivities}
      addActivityButtonText="Add an Activity"
      setSort={setSort}
      sort={sort}
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
