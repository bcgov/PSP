import { FieldArray, FormikProps } from 'formik';
import noop from 'lodash/noop';
import { useCallback, useEffect, useMemo, useRef } from 'react';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { useFeatureDatasetsWithAddresses } from '@/hooks/useFeatureDatasetsWithAddresses';
import { exists, firstOrNull } from '@/utils';
import { addPropertiesToCurrentFile } from '@/utils/propertyUtils';

import { PropertyForm } from '../../shared/models';
import AddPropertiesGuide from '../../shared/update/properties/AddPropertiesGuide';
import { AcquisitionForm } from './models';

export interface IAcquisitionPropertiesProps {
  formikProps: FormikProps<AcquisitionForm>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

export const AcquisitionPropertiesSubForm: React.FunctionComponent<IAcquisitionPropertiesProps> = ({
  formikProps,
}) => {
  const localRef = useRef<FormikProps<AcquisitionForm>>();

  const { selectedFeatures, processCreation, mapLocationFeatureDataset, prepareForCreation } =
    useMapStateMachine();

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

  const handleAddToSelection = useCallback(() => {
    prepareForCreation([selectedFeatureDataset]);
  }, [prepareForCreation, selectedFeatureDataset]);

  useEffect(() => {
    if (exists(localRef.current) && propertyForms.length > 0) {
      addPropertiesToCurrentFile(localRef, 'properties', propertyForms, noop);
      processCreation();
    }
  }, [localRef, processCreation, propertyForms]);

  return (
    <StyledComponentWrapper>
      <div className="py-2">
        Select one or more properties that you want to include in this acquisition. You can choose a
        location from the map, or search by other criteria.
      </div>

      <FieldArray name="properties">
        {({ remove }) => (
          <Section header="Selected Properties">
            <AddPropertiesGuide />
            {exists(selectedFeatureDataset?.parcelFeature) && (
              <StyledButtonWrapper>
                <Button onClick={handleAddToSelection}>Add selected property</Button>
              </StyledButtonWrapper>
            )}

            <SelectedPropertyHeaderRow />
            {formikProps.values.properties.map((property, index) => (
              <SelectedPropertyRow
                key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                onRemove={() => remove(index)}
                nameSpace={`properties.${index}`}
                index={index}
                property={property.toFeatureDataset()}
              />
            ))}
            {formikProps.values.properties.length === 0 && <span>No Properties selected</span>}
          </Section>
        )}
      </FieldArray>
    </StyledComponentWrapper>
  );
};

export default AcquisitionPropertiesSubForm;

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
