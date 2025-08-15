import React from 'react';

import ManagementActivitiesList, {
  activityActionColumn,
  createActivityTableColumns,
} from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/list/ManagementActivitiesList';
import { IManagementActivitiesListViewProps } from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/list/ManagementActivitiesListView';

export const ManagementFileActivitiesListView: React.FunctionComponent<
  IManagementActivitiesListViewProps
> = ({ isLoading, propertyActivities, sort, canEditActivities, onDelete, onView, setSort }) => {
  return (
    <ManagementActivitiesList
      propertyActivities={propertyActivities}
      sort={sort}
      setSort={setSort}
      loading={isLoading}
      columns={[
        ...createActivityTableColumns(),
        activityActionColumn(canEditActivities, onView, onDelete),
      ]}
      dataTestId="mgmt-activity-list"
    ></ManagementActivitiesList>
  );
};

export default ManagementFileActivitiesListView;
