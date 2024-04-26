import orderBy from 'lodash/orderBy';
import React from 'react';
import { BiListPlus } from 'react-icons/bi';

import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import { TableSort } from '@/components/Table/TableSort';
import Claims from '@/constants/claims';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';

import ManagementActivitiesList from './ManagementActivitiesList';
import { PropertyActivityRow } from './models/PropertyActivityRow';

export interface IManagementActivitiesListViewProps {
  isLoading: boolean;
  propertyActivities: PropertyActivityRow[];
  onCreate: () => void;
  onView: (activityId: number) => void;
  onDelete: (activityId: number) => void;
}

const ManagementActivitiesListView: React.FunctionComponent<IManagementActivitiesListViewProps> = ({
  isLoading,
  propertyActivities,
  onCreate,
  onView,
  onDelete,
}) => {
  const [sort, setSort] = React.useState<TableSort<ApiGen_Concepts_PropertyActivity>>({});

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
    <Section
      isCollapsable
      initiallyExpanded
      header={
        <SectionListHeader
          claims={[Claims.MANAGEMENT_EDIT]}
          title="Activities List"
          addButtonText="Add an Activity"
          addButtonIcon={<BiListPlus size={'2.5rem'} />}
          onAdd={onCreate}
        />
      }
    >
      <ManagementActivitiesList
        propertyActivities={sortedActivities}
        handleView={onView}
        handleDelete={onDelete}
        sort={sort}
        setSort={setSort}
        loading={isLoading}
      ></ManagementActivitiesList>
    </Section>
  );
};

export default ManagementActivitiesListView;
