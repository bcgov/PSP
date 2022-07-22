import { TableSort } from 'components/Table/TableSort';
import { useApiResearchFile } from 'hooks/pims-api/useApiResearchFile';
import useIsMounted from 'hooks/useIsMounted';
import { defaultActivityFilter, IActivityFilter } from 'interfaces/IActivityResults';
import { orderBy } from 'lodash';
import { Api_Activity } from 'models/api/Activity';
import React, { useCallback } from 'react';

import { ActivityFilterForm } from './ActivityFilter/ActivityFilterForm';
import { ActivityResults } from './ActivityResults/ActivityResults';
import { AddActivityForm } from './ActivityResults/AddActivityForm';
import * as Styled from './styles';

export interface IActivityListViewProps {
  researchFileId?: number;
  hideFilters?: boolean;
  defaultFilters?: IActivityFilter;
}
/**
 * Page that displays Activity information.
 */
export const ActivityListView: React.FunctionComponent<IActivityListViewProps> = (
  props: IActivityListViewProps,
) => {
  const { researchFileId, defaultFilters, hideFilters } = props;
  const isMounted = useIsMounted();
  const { getResearchActivities, postActivity } = useApiResearchFile();
  const [sort, setSort] = React.useState<TableSort<Api_Activity>>({});
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  const [activityResults, setActivityResults] = React.useState<Api_Activity[]>([]);

  const [filters, setFilters] = React.useState<IActivityFilter>(
    defaultFilters ?? defaultActivityFilter,
  );

  const fetchActivities = useCallback(async () => {
    setIsLoading(true);
    try {
      // const data = mockActivitiesResponse();
      // TODO Call actual api endpoint
      await getResearchActivities(researchFileId || 0).then(response => {
        setActivityResults(response.data);
        if (response.data && isMounted()) {
          setActivityResults(response.data);
        }
      });
    } finally {
      setIsLoading(false);
    }
  }, [getResearchActivities, researchFileId, isMounted]);

  React.useEffect(() => {
    fetchActivities();
  }, [fetchActivities]);

  const sortedFilteredActivities = React.useMemo(() => {
    if (activityResults?.length > 0) {
      let activityItems = [...activityResults];

      if (filters) {
        activityItems = activityItems.filter(activity => {
          return (
            (!filters.activityTypeId ||
              activity.activityTemplateTypeCode === filters.activityTypeId) &&
            (!filters.status || activity.activityStatusTypeCode?.id === filters.status)
          );
        });
      }
      if (sort) {
        const sortFields = Object.keys(sort);
        if (sortFields?.length > 0) {
          const keyName = (sort as any)[sortFields[0]];
          return orderBy(
            activityItems,
            sortFields[0] === 'activityTemplateTypeCode'
              ? 'activityTemplateTypeCode.description'
              : sortFields[0],
            keyName,
          );
        }
      }
      return activityItems;
    }
    return [];
  }, [activityResults, sort, filters]);

  const saveActivity = (activity: Api_Activity) => {
    setIsLoading(true);

    postActivity(activity).then(response => {
      const newResults = [...activityResults, response.data];
      setActivityResults(newResults);
      setIsLoading(false);
    });
  };

  return (
    <Styled.ListPage>
      <Styled.Scrollable vertical={true}>
        <Styled.PageHeader>Activities</Styled.PageHeader>
        {
          <AddActivityForm
            onAddActivity={(activity: Api_Activity) => {
              console.log(activity);
              saveActivity(activity);
            }}
          ></AddActivityForm>
        }
        {!hideFilters && <ActivityFilterForm onSetFilter={setFilters} activityFilter={filters} />}
        <ActivityResults
          results={sortedFilteredActivities}
          loading={isLoading}
          sort={sort}
          setSort={setSort}
          onShowActivity={(activity: Api_Activity) => {
            console.log('Trying to view activity', activity);
          }}
          onDelete={(activity: Api_Activity) => {
            console.log('Trying to delete activity', activity);
          }}
        />
      </Styled.Scrollable>
    </Styled.ListPage>
  );
};

export default ActivityListView;
