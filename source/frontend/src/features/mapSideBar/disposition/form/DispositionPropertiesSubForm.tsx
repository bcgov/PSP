import { FieldArray, FormikProps } from 'formik';
import { LatLngLiteral } from 'leaflet';
import { noop } from 'lodash';
import { useCallback, useEffect, useMemo, useRef } from 'react';
import { Col, Row } from 'react-bootstrap';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import { ZoomIconType, ZoomToLocation } from '@/components/maps/ZoomToLocation';
import MapClickMonitor from '@/components/propertySelector/MapClickMonitor';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { UploadResponseModel } from '@/features/properties/shapeUpload/models';
import { useFeatureDatasetsWithAddresses } from '@/hooks/useFeatureDatasetsWithAddresses';
import { useModalContext } from '@/hooks/useModalContext';
import { exists, firstOrNull, isLatLngInFeatureSetBoundary, isNumber } from '@/utils';
import {
  addPropertiesToCurrentFile,
  addShapeToProperty,
  removeShapeFromPropertyWithConfirmation,
} from '@/utils/propertyUtils';

import { PropertyForm } from '../../shared/models';
import AddPropertiesGuide from '../../shared/update/properties/AddPropertiesGuide';
import { DispositionFormModel } from '../models/DispositionFormModel';

export interface DispositionPropertiesSubFormProps {
  formikProps: FormikProps<DispositionFormModel>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

const getPropertyIndex = (property: PropertyForm, properties: PropertyForm[]): number | null => {
  if (
    !(
      exists(property.fileLocation) ||
      exists(property.fileBoundary) ||
      exists(property.latitude) ||
      exists(property.longitude)
    )
  ) {
    return null;
  }
  let index = 0;
  for (const p of properties) {
    if (
      exists(p.fileLocation) ||
      exists(p.fileBoundary) ||
      exists(p.latitude) ||
      exists(p.longitude)
    ) {
      if (p === property) {
        return index;
      }
      index++;
    }
  }
  return null;
};

const DispositionPropertiesSubForm: React.FunctionComponent<DispositionPropertiesSubFormProps> = ({
  formikProps,
}) => {
  const localRef = useRef<FormikProps<DispositionFormModel>>();

  const { setModalContent, setDisplayModal } = useModalContext();
  const { selectedFeatures, processCreation, mapLocationFeatureDataset, prepareForCreation } =
    useMapStateMachine();

  const selectedFeatureDataset = useMemo<SelectedFeatureDataset>(() => {
    return {
      selectingComponentId: mapLocationFeatureDataset?.selectingComponentId ?? null,
      location: mapLocationFeatureDataset?.location,
      fileLocation: mapLocationFeatureDataset?.fileLocation ?? null,
      fileBoundary: null,
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

  // Get PropertyForms with addresses for all selected features
  const { featuresWithAddresses } = useFeatureDatasetsWithAddresses(selectedFeatures ?? []);

  // Convert SelectedFeatureDataset to PropertyForm
  const propertyForms = useMemo(
    () =>
      featuresWithAddresses.map(obj => {
        const property = PropertyForm.fromFeatureDataset(obj.feature);
        if (exists(obj.address)) {
          property.address = obj.address;
        }
        return property;
      }),
    [featuresWithAddresses],
  );

  const handleAddToSelection = useCallback(() => {
    prepareForCreation([selectedFeatureDataset]);
  }, [prepareForCreation, selectedFeatureDataset]);

  useEffect(() => {
    if (exists(localRef.current) && propertyForms.length > 0) {
      addPropertiesToCurrentFile(localRef, 'fileProperties', propertyForms, noop);
      processCreation();
    }
  }, [localRef, processCreation, propertyForms]);

  return (
    <StyledComponentWrapper>
      <div className="py-2">
        Select one or more properties that you want to include in this disposition. You can choose a
        location from the map, or search by other criteria.
      </div>

      <FieldArray name="fileProperties">
        {({ remove, replace }) => (
          <Section
            header={
              <Row>
                <Col xs="11">Selected Properties</Col>
                <Col>
                  <ZoomToLocation
                    icon={ZoomIconType.area}
                    formProperties={formikProps?.values?.fileProperties}
                  />
                </Col>
              </Row>
            }
          >
            <AddPropertiesGuide />
            {exists(selectedFeatureDataset?.parcelFeature) ||
            exists(selectedFeatureDataset?.pimsFeature) ||
            exists(selectedFeatureDataset?.location) ? (
              <StyledButtonWrapper>
                <Button onClick={handleAddToSelection}>Add selected property</Button>
              </StyledButtonWrapper>
            ) : null}

            <Row className="py-3 no-gutters">
              <Col>
                <MapClickMonitor
                  selectedComponentId={null}
                  addProperty={noop}
                  repositionProperty={(
                    featureset: SelectedFeatureDataset,
                    latLng: LatLngLiteral,
                    index: number | null,
                  ) => {
                    if (
                      isNumber(index) &&
                      index >= 0 &&
                      isLatLngInFeatureSetBoundary(latLng, featureset)
                    ) {
                      const formProperty = formikProps.values.fileProperties[index];
                      const updatedFormProperty = new PropertyForm(formProperty);
                      updatedFormProperty.fileLocation = latLng;
                      replace(index, updatedFormProperty);
                    } else if (!isLatLngInFeatureSetBoundary(latLng, featureset)) {
                      toast.warn(
                        'Please choose a location that is within the (highlighted) boundary of this property.',
                      );
                    }
                  }}
                  modifiedProperties={formikProps.values.fileProperties.map(p =>
                    p.toFeatureDataset(),
                  )}
                />
              </Col>
            </Row>

            <SelectedPropertyHeaderRow />
            {formikProps.values.fileProperties.map((property, index) => {
              const propertyIndex = getPropertyIndex(property, formikProps.values.fileProperties);
              return (
                <SelectedPropertyRow
                  key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                  onRemove={() => remove(index)}
                  nameSpace={`fileProperties.${index}`}
                  index={propertyIndex}
                  property={property}
                  canUploadShapefile={true}
                  onUploadShapefile={(result: UploadResponseModel | null) => {
                    const updatedFormProperty = addShapeToProperty(property, result);
                    replace(index, updatedFormProperty);
                  }}
                  onRemoveShapefile={() => {
                    removeShapeFromPropertyWithConfirmation(
                      property,
                      setModalContent,
                      setDisplayModal,
                      updatedProperty => {
                        replace(index, updatedProperty);
                      },
                    );
                  }}
                />
              );
            })}
            {formikProps.values.fileProperties.length === 0 && <span>No Properties selected</span>}
          </Section>
        )}
      </FieldArray>
    </StyledComponentWrapper>
  );
};

export default DispositionPropertiesSubForm;

const StyledComponentWrapper = styled.div`
  margin: 0 1.6rem 0 1.6rem;
  padding: 0 1.6rem 0 1.6rem;
  text-align: left;
  text-underline-offset: 2px;

  button {
    font-size: 14px;
  }
`;

const StyledButtonWrapper = styled.div`
  margin: 0 1.6rem;
  padding-left: 1.6rem;
  text-align: left;
  text-underline-offset: 2px;

  button {
    font-size: 14px;
  }
`;
