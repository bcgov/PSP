import React from 'react';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { isValidId } from '@/utils';

import PropertyManagementActivitiesListContainer from '../activity/list/ManagementActivitiesListContainer';
import ManagementActivitiesListView from '../activity/list/ManagementActivitiesListView';
import { PropertyContactListContainer } from './PropertyContactListContainer';
import { PropertyContactListView } from './PropertyContactListView';
import { PropertyManagementDetailContainer } from './summary/PropertyManagementDetailContainer';
import { PropertyManagementDetailView } from './summary/PropertyManagementDetailView';

export interface IPropertyManagementTabView {
  property: ApiGen_Concepts_Property;
  loading: boolean;
}

/**
 * Provides basic property information, as displayed under "Property Details" tab on the Property Information slide-out
 * @returns the rendered property details panel
 */
export const PropertyManagementTabView: React.FunctionComponent<IPropertyManagementTabView> = ({
  property,
  loading,
}) => {
  if (isValidId(property.id)) {
    return (
      <StyledSummarySection>
        <LoadingBackdrop show={loading} parentScreen={true} />
        <PropertyManagementDetailContainer
          propertyId={property.id}
          View={PropertyManagementDetailView}
        />
        <PropertyContactListContainer propertyId={property.id} View={PropertyContactListView} />

        <PropertyManagementActivitiesListContainer
          View={ManagementActivitiesListView}
          propertyId={property.id}
        ></PropertyManagementActivitiesListContainer>
      </StyledSummarySection>
    );
  } else {
    return <></>;
  }
};
