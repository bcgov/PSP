import React from 'react';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { Api_Property } from '@/models/api/Property';

import { EditManagementState } from '../../../PropertyViewSelector';
import { PropertyContactListContainer } from './PropertyContactListContainer';
import { PropertyContactListView } from './PropertyContactListView';
import { PropertyManagementDetailContainer } from './summary/PropertyManagementDetailContainer';
import { PropertyManagementDetailView } from './summary/PropertyManagementDetailView';

export interface IPropertyManagementTabView {
  property: Api_Property;
  loading: boolean;
  setEditManagementState: (state: EditManagementState | null) => void;
}

/**
 * Provides basic property information, as displayed under "Property Details" tab on the Property Information slide-out
 * @returns the rendered property details panel
 */
export const PropertyManagementTabView: React.FunctionComponent<IPropertyManagementTabView> = ({
  property,
  loading,
  setEditManagementState,
}) => {
  if (property.id !== undefined) {
    return (
      <StyledSummarySection>
        <LoadingBackdrop show={loading} parentScreen={true} />

        <PropertyManagementDetailContainer
          propertyId={property.id}
          View={PropertyManagementDetailView}
          setEditManagementState={setEditManagementState}
        />
        <PropertyContactListContainer
          propertyId={property.id}
          View={PropertyContactListView}
          setEditManagementState={setEditManagementState}
        />
      </StyledSummarySection>
    );
  } else {
    return <></>;
  }
};
