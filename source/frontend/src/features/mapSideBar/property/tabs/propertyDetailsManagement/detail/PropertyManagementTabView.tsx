import React from 'react';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { Api_Property } from '@/models/api/Property';

import { ManagementSummaryContainer } from './ManagementSummaryContainer';
import { ManagementSummaryView } from './ManagementSummaryView';
import { PropertyContactContainer } from './PropertyContactContainer';
import { PropertyContactView } from './PropertyContactView';

export interface IPropertyManagementTabView {
  property: Api_Property;
  loading: boolean;
  setEditMode: (isEditing: boolean) => void;
}

/**
 * Provides basic property information, as displayed under "Property Details" tab on the Property Information slide-out
 * @returns the rendered property details panel
 */
export const PropertyManagementTabView: React.FunctionComponent<IPropertyManagementTabView> = ({
  property,
  loading,
  setEditMode,
}) => {
  if (property.id !== undefined) {
    return (
      <StyledSummarySection>
        <LoadingBackdrop show={loading} parentScreen={true} />
        <ManagementSummaryContainer
          property={property}
          View={ManagementSummaryView}
          setEditMode={setEditMode}
        />
        <PropertyContactContainer
          propertyId={property.id}
          View={PropertyContactView}
          setEditMode={setEditMode}
        />
      </StyledSummarySection>
    );
  } else {
    return <></>;
  }
};
