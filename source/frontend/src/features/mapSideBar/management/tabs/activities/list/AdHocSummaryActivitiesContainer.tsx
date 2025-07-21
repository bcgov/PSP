import React, { useCallback, useContext, useEffect, useState } from 'react';

import { TableSort } from '@/components/Table/TableSort';
import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { IManagementActivitiesListViewProps } from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/list/ManagementActivitiesListView';
import { PropertyActivityRow } from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/list/models/PropertyActivityRow';
import { useManagementActivityRepository } from '@/hooks/repositories/useManagementActivityRepository';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { isValidId } from '@/utils/utils';

export interface IPropertyManagementActivitiesListContainerProps {
  managementFileId: number;
  View: React.FC<IManagementActivitiesListViewProps>;
}

const AdHocFileActivitiesSummaryContainer: React.FunctionComponent<
  IPropertyManagementActivitiesListContainerProps
> = ({ managementFileId, View }) => {
  const isMounted = useIsMounted();
  const [propertyActivities, setPropertyActivities] = useState<PropertyActivityRow[]>([]);
  const { staleLastUpdatedBy } = useContext(SideBarContext);
  const [sort, setSort] = useState<TableSort<ApiGen_Concepts_ManagementActivity>>({});

  const {
    getManagementFileActivities: { execute: getActivities, loading },
  } = useManagementActivityRepository();

  const fetchPropertyActivities = useCallback(async () => {
    const response = await getActivities(managementFileId);
    if (response && isMounted()) {
      setPropertyActivities([...response.map(x => PropertyActivityRow.fromApi(x))]);
    }
  }, [getActivities, isMounted, managementFileId]);

  useEffect(() => {
    fetchPropertyActivities();
  }, [fetchPropertyActivities, staleLastUpdatedBy]);
  //TODO: remove staleLastUpdatedBy when side bar context is refactored.

  return (
    <View
      isLoading={loading}
      propertyActivities={propertyActivities.filter(pa => !isValidId(pa.managementFileId))}
      setSort={setSort}
      sort={sort}
      getNavigationUrl={(row: PropertyActivityRow) => ({
        url: `/mapview/sidebar/property/${row.adHocPropertyId}/management/activity/${row.activityId}`,
        title: row.adHocPropertyName,
      })}
      canEditActivities={false}
    />
  );
};

export default AdHocFileActivitiesSummaryContainer;
