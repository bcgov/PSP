import React from 'react';
import { useHistory } from 'react-router-dom';

import { Button } from '@/components/common/buttons';
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
  const history = useHistory();

  const createActivity = () => {
    history.push(`/mapview/sidebar/property/${property.id}/activity/new`);
  };

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

        <Button onClick={createActivity}>New Activity</Button>
      </StyledSummarySection>
    );
  } else {
    return <></>;
  }
};
