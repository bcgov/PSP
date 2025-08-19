import React from 'react';

import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import TooltipIcon from '@/components/common/TooltipIcon';
import Claims from '@/constants/claims';

import ManagementActivitiesList, {
  activityNavigationColumn,
  createActivityTableColumns,
} from './ManagementActivitiesList';
import { IManagementActivitiesListViewProps } from './ManagementActivitiesListView';

const ManagementSummaryActivitiesListView: React.FunctionComponent<
  IManagementActivitiesListViewProps
> = ({ isLoading, propertyActivities, sort, getNavigationUrl, setSort }) => {
  return (
    <Section
      isCollapsable
      initiallyExpanded
      header={
        <div className="d-flex">
          <SectionListHeader
            claims={[Claims.MANAGEMENT_VIEW]}
            title="Property File Activity Summary"
            className="mr-2"
          />
          <TooltipIcon
            toolTipId="property-file-activity-summary"
            toolTip="These are all of the activities at the management file level that reference this property."
            className="align-self-end"
          />
        </div>
      }
    >
      <ManagementActivitiesList
        propertyActivities={propertyActivities}
        sort={sort}
        setSort={setSort}
        loading={isLoading}
        columns={[...createActivityTableColumns(), activityNavigationColumn(getNavigationUrl)]}
        dataTestId="mgmt-activity-list-readonly"
      ></ManagementActivitiesList>
    </Section>
  );
};

export default ManagementSummaryActivitiesListView;
