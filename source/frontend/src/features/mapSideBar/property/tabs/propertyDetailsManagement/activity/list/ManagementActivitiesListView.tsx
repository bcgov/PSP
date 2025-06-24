import React from 'react';
import { BiListPlus } from 'react-icons/bi';

import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import { TableSort } from '@/components/Table/TableSort';
import Claims from '@/constants/claims';
import ManagementStatusUpdateSolver from '@/features/mapSideBar/management/tabs/fileDetails/detail/ManagementStatusUpdateSolver';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';

import ManagementActivitiesList, {
  activityActionColumn,
  createActivityTableColumns,
} from './ManagementActivitiesList';
import { PropertyActivityRow } from './models/PropertyActivityRow';

export interface IManagementActivitiesListViewProps {
  isLoading: boolean;
  propertyActivities: PropertyActivityRow[];
  sort: TableSort<ApiGen_Concepts_PropertyActivity>;
  statusSolver?: ManagementStatusUpdateSolver;
  getNavigationUrl?: (row: PropertyActivityRow) => { title: string; url: string };
  setSort: React.Dispatch<React.SetStateAction<TableSort<ApiGen_Concepts_PropertyActivity>>>;
  onCreate?: () => void;
  onView?: (activityId: number) => void;
  onDelete?: (activityId: number) => void;
}

const ManagementActivitiesListView: React.FunctionComponent<IManagementActivitiesListViewProps> = ({
  isLoading,
  propertyActivities,
  sort,
  statusSolver,
  setSort,
  onCreate,
  onView,
  onDelete,
}) => {
  const canEditActivities = !statusSolver || (statusSolver && statusSolver.canEditActivities());

  return (
    <Section
      isCollapsable
      initiallyExpanded
      header={
        canEditActivities ? (
          <SectionListHeader
            claims={[Claims.MANAGEMENT_EDIT]}
            title="Activities List"
            addButtonText="Add an Activity"
            addButtonIcon={<BiListPlus size={'2.5rem'} />}
            onAdd={onCreate}
            isAddEnabled={canEditActivities}
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
      ></ManagementActivitiesList>
    </Section>
  );
};

export default ManagementActivitiesListView;
