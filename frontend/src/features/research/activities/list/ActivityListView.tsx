import { SelectOption } from 'components/common/form/Select';
import { TableSort } from 'components/Table/TableSort';
import { useApiResearchFile } from 'hooks/pims-api/useApiResearchFile';
import useIsMounted from 'hooks/useIsMounted';
import { defaultActivityFilter, IActivityFilter } from 'interfaces/IActivityResults';
import { orderBy } from 'lodash';
import { Api_Activity, Api_ActivityTemplate } from 'models/api/Activity';
import React, { useCallback } from 'react';
import { useHistory } from 'react-router-dom';

import { ActivityFilterForm } from './ActivityFilter/ActivityFilterForm';
import { ActivityResults } from './ActivityResults/ActivityResults';
import { AddActivityForm } from './ActivityResults/AddActivityForm';
import * as Styled from './styles';

export interface IActivityListViewProps {
  fileId: number;
  defaultFilters?: IActivityFilter;
}
/**
 * Page that displays Activity information.
 */
export const ActivityListView: React.FunctionComponent<IActivityListViewProps> = (
  props: IActivityListViewProps,
) => {
  const { fileId, defaultFilters } = props;
  const history = useHistory();
  const isMounted = useIsMounted();
  const { getActivityTemplates, getResearchActivities, postActivity } = useApiResearchFile();
  const [sort, setSort] = React.useState<TableSort<Api_Activity>>({});
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  const [activityResults, setActivityResults] = React.useState<Api_Activity[]>([]);
  const [templateTypes, setTemplateTypes] = React.useState<SelectOption[]>([]);
  const [filters, setFilters] = React.useState<IActivityFilter>(
    defaultFilters ?? defaultActivityFilter,
  );

  const fetchData = useCallback(async () => {
    setIsLoading(true);
    try {
      await getResearchActivities(fileId || 0).then(response => {
        setActivityResults(response.data);
        if (response.data && isMounted()) {
          setActivityResults(response.data);
        }
      });
    } finally {
      setIsLoading(false);
    }
    await getActivityTemplates().then(response => {
      let options = response.data.map(
        (template: Api_ActivityTemplate) =>
          ({
            value: template.id,
            label: template.activityTemplateTypeCode?.description,
            code: template.activityTemplateTypeCode?.id,
          } as SelectOption),
      );
      setTemplateTypes(options);
      if (options && isMounted()) {
        setTemplateTypes(options);
      }
    });
  }, [getActivityTemplates, getResearchActivities, fileId, isMounted]);

  React.useEffect(() => {
    fetchData();
  }, [fetchData]);

  const sortedFilteredActivities = React.useMemo(() => {
    if (activityResults?.length > 0) {
      let activityItems = [...activityResults];

      if (filters) {
        activityItems = activityItems.filter(activity => {
          return (
            (!filters.activityTypeId ||
              activity.activityTemplateTypeCode?.id === filters.activityTypeId) &&
            (!filters.status || activity.activityStatusTypeCode?.id === filters.status)
          );
        });
      }
      if (sort) {
        const sortFields = Object.keys(sort);
        if (sortFields?.length > 0) {
          const keyName = sort[sortFields[0] as keyof Api_Activity];
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

  const saveActivity = (activityTypeId: number) => {
    setIsLoading(true);
    const activity: Api_Activity = { id: 0, activityTemplateId: activityTypeId, description: '' };
    postActivity(activity).then(response => {
      const newResults = [...activityResults, response.data];
      setActivityResults(newResults);
      setIsLoading(false);
    });
  };

  const getActivityUrl = (id: number): string => {
    const currentPath = history.location.pathname;
    if (currentPath.indexOf('activity') > -1) {
      const existing = currentPath.split('activity');
      return `${existing[0]}activity/${id}/view`;
    } else {
      return currentPath.charAt(currentPath.length - 1) === '/'
        ? `${currentPath}activity/${id}/view`
        : `${currentPath}/activity/${id}/view`;
    }
  };

  return (
    <Styled.ListPage>
      <Styled.Scrollable vertical={true}>
        <Styled.PageHeader>Activities</Styled.PageHeader>
        {
          <AddActivityForm
            onAddActivity={(activityTypeId: number) => {
              saveActivity(activityTypeId);
            }}
            templateTypes={templateTypes}
          ></AddActivityForm>
        }
        {<ActivityFilterForm onSetFilter={setFilters} activityFilter={filters} />}
        <ActivityResults
          results={sortedFilteredActivities}
          loading={isLoading}
          sort={sort}
          setSort={setSort}
          onShowActivity={(activity: Api_Activity) => {
            history.push(getActivityUrl(activity.id));
          }}
          onDelete={(activity: Api_Activity) => {}}
        />
      </Styled.Scrollable>
    </Styled.ListPage>
  );
};

export default ActivityListView;
