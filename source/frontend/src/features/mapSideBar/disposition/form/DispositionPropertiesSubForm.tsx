import { FieldArray, FormikProps } from 'formik';
import { geoJSON } from 'leaflet';
import { noop } from 'lodash';
import { useCallback, useEffect, useMemo, useRef } from 'react';
import { Col, Row } from 'react-bootstrap';
import { PiCornersOut } from 'react-icons/pi';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import { LinkButton } from '@/components/common/buttons/LinkButton';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { useFeatureDatasetsWithAddresses } from '@/hooks/useFeatureDatasetsWithAddresses';
import { exists, firstOrNull, latLngLiteralToGeometry } from '@/utils';
import { addPropertiesToCurrentFile } from '@/utils/propertyUtils';

import { PropertyForm } from '../../shared/models';
import AddPropertiesGuide from '../../shared/update/properties/AddPropertiesGuide';
import { DispositionFormModel } from '../models/DispositionFormModel';

export interface DispositionPropertiesSubFormProps {
  formikProps: FormikProps<DispositionFormModel>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

const DispositionPropertiesSubForm: React.FunctionComponent<DispositionPropertiesSubFormProps> = ({
  formikProps,
}) => {
  const localRef = useRef<FormikProps<DispositionFormModel>>();

  const {
    selectedFeatures,
    processCreation,
    mapLocationFeatureDataset,
    prepareForCreation,
    requestFlyToBounds,
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

  const fitBoundaries = () => {
    const fileProperties = formikProps.values.fileProperties;

    if (exists(fileProperties)) {
      const locations = fileProperties.map(
        p => p?.polygon ?? latLngLiteralToGeometry(p?.fileLocation),
      );
      const bounds = geoJSON(locations).getBounds();

      requestFlyToBounds(bounds);
    }
  };

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
        {({ remove }) => (
          <Section
            header={
              <Row>
                <Col xs="11">Selected Properties</Col>
                <Col>
                  <TooltipWrapper
                    tooltip="Fit map to the file properties"
                    tooltipId="property-selector-tooltip"
                  >
                    <LinkButton title="Fit boundaries button" onClick={fitBoundaries}>
                      <PiCornersOut size={18} className="mr-2" />
                    </LinkButton>
                  </TooltipWrapper>
                </Col>
              </Row>
            }
          >
            <AddPropertiesGuide />
            {exists(selectedFeatureDataset?.parcelFeature) && (
              <StyledButtonWrapper>
                <Button onClick={handleAddToSelection}>Add selected property</Button>
              </StyledButtonWrapper>
            )}

            <SelectedPropertyHeaderRow />
            {formikProps.values.fileProperties.map((property, index) => (
              <SelectedPropertyRow
                key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                onRemove={() => remove(index)}
                nameSpace={`fileProperties.${index}`}
                index={index}
                property={property.toFeatureDataset()}
              />
            ))}
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
