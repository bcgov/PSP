import { useCallback, useContext, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { TableSort } from '@/components/Table/TableSort';
import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import ManagementStatusUpdateSolver from '@/features/mapSideBar/management/tabs/fileDetails/detail/ManagementStatusUpdateSolver';
import { useManagementActivityPropertyRepository } from '@/hooks/repositories/useManagementActivityPropertyRepository';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { isValidId } from '@/utils';

import { IManagementActivitiesListViewProps } from './ManagementActivitiesListView';
import { PropertyActivityRow } from './models/PropertyActivityRow';

export interface IPropertyManagementActivitiesListContainerProps {
  statusSolver?: ManagementStatusUpdateSolver;
  propertyId: number;
  isAdHoc?: boolean;
  View: React.FC<IManagementActivitiesListViewProps>;
}

const PropertyManagementActivitiesListContainer: React.FunctionComponent<
  IPropertyManagementActivitiesListContainerProps
> = ({ statusSolver, propertyId, isAdHoc, View }) => {
  const history = useHistory();
  const isMounted = useIsMounted();
  const { setModalContent, setDisplayModal } = useModalContext();
  const [propertyActivities, setPropertyActivities] = useState<PropertyActivityRow[]>([]);
  const { staleLastUpdatedBy } = useContext(SideBarContext);
  const [sort, setSort] = useState<TableSort<ApiGen_Concepts_ManagementActivity>>({});

  const {
    getActivities: { execute: getActivities, loading },
    deleteActivity: { execute: deleteActivity, loading: deletingActivity },
  } = useManagementActivityPropertyRepository();

  const fetchPropertyActivities = useCallback(async () => {
    const response = await getActivities(propertyId);
    if (response && isMounted()) {
      setPropertyActivities([...response.map(x => PropertyActivityRow.fromApi(x))]);
    }
  }, [getActivities, isMounted, propertyId]);

  const onDelete = useCallback(
    async (activityId: number) => {
      const result = await deleteActivity(propertyId, activityId);
      if (result === true) {
        fetchPropertyActivities();
        history.push(`/mapview/sidebar/property/${propertyId}/management`);
      }
    },
    [deleteActivity, fetchPropertyActivities, history, propertyId],
  );

  useEffect(() => {
    fetchPropertyActivities();
  }, [fetchPropertyActivities, staleLastUpdatedBy]);
  //TODO: remove staleLastUpdatedBy when side bar context is refactored.

  const onCreate = () => {
    history.push(`/mapview/sidebar/property/${propertyId}/management/activity/new`);
  };

  const onView = (activityId: number) => {
    history.push(`/mapview/sidebar/property/${propertyId}/management/activity/${activityId}`);
  };

  const canEditActivities = !statusSolver || statusSolver?.canEditActivities();

  return (
    <View
      sort={sort}
      setSort={setSort}
      isLoading={loading || deletingActivity}
      propertyActivities={
        isAdHoc
          ? propertyActivities.filter(pa => !isValidId(pa.managementFileId))
          : propertyActivities.filter(pa => isValidId(pa.managementFileId))
      }
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
      getNavigationUrl={(row: PropertyActivityRow) => ({
        url: `/mapview/sidebar/management/${row.managementFileId}/activities/${row.activityId}`,
        title: `M-${row.managementFileId}`,
      })}
      canEditActivities={canEditActivities}
      addActivityButtonText="Add an Ad-hoc Activity"
      activitiesListTitle="Ad-hoc Activities List"
    />
  );
};

export default PropertyManagementActivitiesListContainer;
