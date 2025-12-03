import React from 'react';

import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import TooltipIcon from '@/components/common/TooltipIcon';
import Claims from '@/constants/claims';
import ManagementActivitiesList, {
  activityNavigationColumn,
  createActivityTableColumns,
} from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/list/ManagementActivitiesList';
import { IManagementActivitiesListViewProps } from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/list/ManagementActivitiesListView';

const AdHocSummaryActivitiesView: React.FunctionComponent<IManagementActivitiesListViewProps> = ({
  isLoading,
  propertyActivities,
  sort,
  getNavigationUrl,
  setSort,
}) => {
  return (
    <Section
      isCollapsable
      initiallyExpanded
      header={
        <div className="d-flex">
          <SectionListHeader
            claims={[Claims.MANAGEMENT_VIEW]}
            title="Ad-hoc Activities List"
            className="mr-2"
          />
          <TooltipIcon
            toolTipId="property-file-activity-summary"
            toolTip="These are all of the Ad-hoc activities that reference a property on this file."
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
        dataTestId="adhoc-activity-list-readonly"
      ></ManagementActivitiesList>
    </Section>
  );
};

export default AdHocSummaryActivitiesView;
