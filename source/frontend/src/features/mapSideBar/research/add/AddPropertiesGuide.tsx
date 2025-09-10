import { FunctionComponent, PropsWithChildren, ReactNode, useCallback, useMemo } from 'react';
import styled from 'styled-components';

import { Button, LinkButton } from '@/components/common/buttons';
import FormGuideContainer from '@/components/common/form/FormGuide/FormGuideContainer';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { exists, firstOrNull } from '@/utils';

export const AddPropertiesGuide: FunctionComponent<PropsWithChildren<unknown>> = () => {
  const {
    mapLocationFeatureDataset,
    prepareForCreation,
    toggleMapSearchControl,
    isShowingMapSearch,
  } = useMapStateMachine();

  const selectedFeatureDataset = useMemo<SelectedFeatureDataset>(() => {
    return {
      selectingComponentId: mapLocationFeatureDataset?.selectingComponentId ?? null,
      location: mapLocationFeatureDataset?.location,
      fileLocation: mapLocationFeatureDataset?.fileLocation ?? null,
      parcelFeature: firstOrNull(mapLocationFeatureDataset?.parcelFeatures),
      pimsFeature: firstOrNull(mapLocationFeatureDataset?.pimsFeatures),
      regionFeature: mapLocationFeatureDataset?.regionFeature ?? null,
      districtFeature: mapLocationFeatureDataset?.districtFeature ?? null,
      municipalityFeature: firstOrNull(mapLocationFeatureDataset?.municipalityFeatures),
      isActive: true,
      displayOrder: 0,
    };
  }, [
    mapLocationFeatureDataset?.selectingComponentId,
    mapLocationFeatureDataset?.location,
    mapLocationFeatureDataset?.fileLocation,
    mapLocationFeatureDataset?.parcelFeatures,
    mapLocationFeatureDataset?.pimsFeatures,
    mapLocationFeatureDataset?.regionFeature,
    mapLocationFeatureDataset?.districtFeature,
    mapLocationFeatureDataset?.municipalityFeatures,
  ]);

  const handleOpenSearch = useCallback(() => {
    if (!isShowingMapSearch) {
      toggleMapSearchControl();
    }
  }, [isShowingMapSearch, toggleMapSearchControl]);

  const handleAddToSelection = useCallback(() => {
    prepareForCreation([selectedFeatureDataset]);
  }, [prepareForCreation, selectedFeatureDataset]);

  const guideBodyContent = (): ReactNode => {
    return (
      <ol>
        <StyledBoldLi>
          <div>Find a Property</div>
          <StyledNormalText>
            Navigate to an area of the map OR use{' '}
            <LinkButton className="p-0 d-inline-block" onClick={handleOpenSearch}>
              Search
            </LinkButton>
          </StyledNormalText>
        </StyledBoldLi>
        <StyledBoldLi>
          <div>Select a property</div>
          <StyledNormalText>Click on the map and the selection will be highlighed</StyledNormalText>
        </StyledBoldLi>
        <StyledBoldLi>
          <div>Add it to this file</div>
          <StyledNormalText>
            Click &quot;Add selected&quot; property button when it appears below
          </StyledNormalText>
        </StyledBoldLi>
      </ol>
    );
  };

  return (
    <>
      <FormGuideContainer title="New workflow" guideBody={guideBodyContent()} />
      {exists(selectedFeatureDataset?.parcelFeature) && (
        <Button onClick={handleAddToSelection}>Add selected property</Button>
      )}
    </>
  );
};

const StyledNormalText = styled.div`
  font-weight: normal;
`;
const StyledBoldLi = styled.li`
  font-weight: bold;
`;
