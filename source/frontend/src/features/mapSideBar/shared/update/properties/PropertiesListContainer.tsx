import { FieldArray } from 'formik';
import { useCallback, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import { ZoomIconType, ZoomToLocation } from '@/components/maps/ZoomToLocation';
import MapClickMonitor from '@/components/propertySelector/MapClickMonitor';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { UploadResponseModel } from '@/features/properties/shapeUpload/models';
import useDraftMarkerSynchronizer from '@/hooks/useDraftMarkerSynchronizer';
import { exists, isEmptyOrNull, isLatLngInFeatureSetBoundary, isNumber } from '@/utils';

import { PropertyForm } from '../../models';
import AddPropertiesGuide from './AddPropertiesGuide';

export interface IPropertiesListContainerProps {
  properties: PropertyForm[];
  verifyCanRemove: (propertyId: number, removeCallback: () => void) => void;
  needsConfirmationBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
  confirmBeforeAddMessage?: React.ReactNode;
  showDisabledProperties?: boolean;
  canUploadShapefiles?: boolean;
  canReposition?: boolean;
}

export const PropertiesListContainer: React.FunctionComponent<
  IPropertiesListContainerProps
> = props => {
  const [repositionPropertyIndex, setRepositionPropertyIndex] = useState<number | null>(null);

  useDraftMarkerSynchronizer(props.properties);

  const {
    mapLocationFeatureDataset,
    requestLocationFeatureAddition,
    isRepositioning,
    startReposition,
    finishReposition,
  } = useMapStateMachine();

  const handleAddToSelection = useCallback(() => {
    requestLocationFeatureAddition([mapLocationFeatureDataset]);
  }, [requestLocationFeatureAddition, mapLocationFeatureDataset]);

  const onRepositionClick = useCallback(
    (propertyIndex: number) => {
      setRepositionPropertyIndex(propertyIndex);
      const formProperty = props.properties[propertyIndex];
      startReposition(formProperty.toFeature());
    },
    [props.properties, startReposition],
  );

  return (
    <>
      <AddPropertiesGuide />
      {!isEmptyOrNull(mapLocationFeatureDataset?.parcelFeatures) && (
        <StyledButtonWrapper>
          <Button onClick={handleAddToSelection}>Add selected property</Button>
        </StyledButtonWrapper>
      )}

      <FieldArray name="properties">
        {({ remove, replace }) => (
          <Section
            header={
              <Row>
                <Col xs="11">Selected Properties</Col>
                <Col>
                  <ZoomToLocation formProperties={props.properties} icon={ZoomIconType.area} />
                </Col>
              </Row>
            }
          >
            <MapClickMonitor
              onNewLocation={(
                locationDataSet: LocationFeatureDataset,
                hasMultipleProperties: boolean,
              ) => {
                if (
                  props.canReposition &&
                  isRepositioning &&
                  isNumber(repositionPropertyIndex) &&
                  repositionPropertyIndex >= 0 &&
                  !hasMultipleProperties
                ) {
                  debugger;
                  // As long as the marker is repositioned within the boundary of the originally selected property simply reposition the marker without further notification.
                  const formProperty = props.properties[repositionPropertyIndex];

                  if (
                    isLatLngInFeatureSetBoundary(
                      locationDataSet.location,
                      formProperty.toLocationFeatureDataset(),
                    )
                  ) {
                    const updatedFormProperty = new PropertyForm(formProperty);
                    updatedFormProperty.fileLocation = locationDataSet.location;

                    // Find property within formik values and reposition it based on incoming file marker position
                    replace(repositionPropertyIndex, updatedFormProperty);

                    // Reset the reposition state
                    finishReposition();
                    setRepositionPropertyIndex(null);
                  } else {
                    toast.warn(
                      'Please choose a location that is within the (highlighted) boundary of this property.',
                    );
                  }
                }
              }}
            />
            <SelectedPropertyHeaderRow />
            {props.properties?.map((property, index) => (
              <SelectedPropertyRow
                key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                onRemove={async () => {
                  const removeCallback = () => {
                    remove(index);
                  };
                  await props.verifyCanRemove(property.apiId, removeCallback);
                }}
                canReposition={props.canReposition}
                onReposition={onRepositionClick}
                nameSpace={`properties.${index}`}
                index={index}
                property={property}
                showDisable={props.showDisabledProperties}
                canUploadShapefile={props.canUploadShapefiles}
                onUploadShapefile={(result: UploadResponseModel | null) => {
                  // Update the property boundary based on the uploaded shapefile
                  if (exists(result)) {
                    if (result.isSuccess && exists(result.boundary)) {
                      const updatedFormProperty = new PropertyForm(property);
                      updatedFormProperty.fileBoundary = result.boundary;
                      replace(index, updatedFormProperty);
                    }
                  }
                }}
              />
            ))}
            {props.properties?.length === 0 && <span>No Properties selected</span>}
          </Section>
        )}
      </FieldArray>
    </>
  );
};

export default PropertiesListContainer;

const StyledButtonWrapper = styled.div`
  margin: 0 1.6rem;
  padding-left: 1.6rem;
  text-align: left;
  text-underline-offset: 2px;

  button {
    font-size: 14px;
  }
`;
