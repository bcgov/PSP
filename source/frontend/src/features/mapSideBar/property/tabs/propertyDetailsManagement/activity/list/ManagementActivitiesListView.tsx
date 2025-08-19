import React from 'react';
import { BiListPlus } from 'react-icons/bi';

import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import { TableSort } from '@/components/Table/TableSort';
import Claims from '@/constants/claims';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';

import ManagementActivitiesList, {
  activityActionColumn,
  createActivityTableColumns,
} from './ManagementActivitiesList';
import { PropertyActivityRow } from './models/PropertyActivityRow';

export interface IManagementActivitiesListViewProps {
  isLoading: boolean;
  propertyActivities: PropertyActivityRow[];
  sort: TableSort<ApiGen_Concepts_ManagementActivity>;
  canEditActivities: boolean;
  addActivityButtonText?: string;
  activitiesListTitle?: string;
  getNavigationUrl?: (row: PropertyActivityRow) => { title: string; url: string };
  setSort: React.Dispatch<React.SetStateAction<TableSort<ApiGen_Concepts_ManagementActivity>>>;
  onCreate?: () => void;
  onView?: (activityId: number) => void;
  onDelete?: (activityId: number) => void;
}

const ManagementActivitiesListView: React.FunctionComponent<IManagementActivitiesListViewProps> = ({
  isLoading,
  propertyActivities,
  sort,
  canEditActivities,
  addActivityButtonText,
  activitiesListTitle,
  setSort,
  onCreate,
  onView,
  onDelete,
}) => {
  return (
    <Section
      isCollapsable
      initiallyExpanded
      header={
        canEditActivities ? (
          <SectionListHeader
            claims={[Claims.MANAGEMENT_EDIT]}
            title={activitiesListTitle ?? 'Activities List'}
            addButtonText={addActivityButtonText ?? 'Add an Activity'}
            addButtonIcon={<BiListPlus size={'2.5rem'} />}
            onButtonAction={onCreate}
            isAddEnabled={canEditActivities}
            button-data-testId="add-activity-button"
            title-data-testId="ad-hoc activities"
          />
        ) : (
          'Activities List'
        )
      }
    >
      <ManagementActivitiesList
        propertyActivities={propertyActivities}
        sort={sort}
        setSort={setSort}
        loading={isLoading}
        columns={[
          ...createActivityTableColumns(),
          activityActionColumn(canEditActivities, onView, onDelete),
        ]}
        dataTestId="adhoc-activity-list"
      ></ManagementActivitiesList>
    </Section>
  );
};

export default ManagementActivitiesListView;
