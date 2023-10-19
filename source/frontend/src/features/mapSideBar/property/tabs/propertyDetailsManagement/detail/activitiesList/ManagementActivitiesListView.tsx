import { orderBy } from 'lodash';
import React from 'react';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { TableSort } from '@/components/Table/TableSort';
import { Api_PropertyManagementActivity } from '@/models/api/Property';

import ManagementActivitiesList from './ManagementActivitiesList';
import { PropertyActivityRow } from './models/PropertyActivityRow';

export interface IManagementActivitiesListViewProps {
  isLoading: boolean;
  propertyActivities: PropertyActivityRow[];
  onDelete: (managementActivityId: number) => void;
}

const ManagementActivitiesListView: React.FunctionComponent<IManagementActivitiesListViewProps> = ({
  isLoading,
  propertyActivities,
  onDelete,
}) => {
  const [sort, setSort] = React.useState<TableSort<Api_PropertyManagementActivity>>({});

  const mapSortField = (sortField: string) => {
    if (sortField === 'activityType') {
      return 'activityType.description';
    } else if (sortField === 'activitySubType') {
      return 'activitySubType.description';
    } else if (sortField === 'activityStatusType') {
      return 'activityStatusType.description';
    }
    return sortField;
  };

  const sortedActivities = React.useMemo(() => {
    if (propertyActivities?.length > 0) {
      let items: PropertyActivityRow[] = [...propertyActivities];

      if (sort) {
        const sortFields = Object.keys(sort);
        if (sortFields?.length > 0) {
          const keyName = (sort as any)[sortFields[0]];
          return orderBy(items, mapSortField(sortFields[0]), keyName);
        } else {
          items = orderBy(items, ['activityType'], ['asc']);
        }
      }
      return items;
    }
    return [];
  }, [propertyActivities, sort]);

  return (
    <Section isCollapsable initiallyExpanded header="Activities List">
      <LoadingBackdrop show={isLoading} />
      <ManagementActivitiesList
        propertyActivities={sortedActivities}
        handleDelete={onDelete}
        sort={sort}
        setSort={setSort}
      ></ManagementActivitiesList>
    </Section>
  );
};

export default ManagementActivitiesListView;
