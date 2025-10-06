import { FieldArray, FormikProps, useFormikContext } from 'formik';
import noop from 'lodash/noop';
import { useCallback, useEffect, useMemo, useRef } from 'react';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { useEditPropertiesMode } from '@/hooks/useEditPropertiesMode';
import { useFeatureDatasetsWithAddresses } from '@/hooks/useFeatureDatasetsWithAddresses';
import { addPropertiesToCurrentFile } from '@/utils/propertyUtils';
import { exists, firstOrNull } from '@/utils/utils';

import { PropertyForm } from '../../shared/models';
import { AddPropertiesGuide } from '../../shared/update/properties/AddPropertiesGuide';
import { ResearchForm } from './models';

export interface IResearchPropertiesProps {
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

const ResearchProperties: React.FC<IResearchPropertiesProps> = () => {
  const localRef = useRef<FormikProps<ResearchForm>>();
  const { values } = useFormikContext<ResearchForm>();
  useEditPropertiesMode();

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
    <Section header="Properties to include in this file:">
      <AddPropertiesGuide />
      {exists(selectedFeatureDataset?.parcelFeature) && (
        <StyledButtonWrapper>
          <Button onClick={handleAddToSelection}>Add selected property</Button>
        </StyledButtonWrapper>
      )}
      <FieldArray name="properties">
        {({ remove }) => (
          <Section header="Selected Properties">
            <SelectedPropertyHeaderRow />
            {values.properties.map((property, index) => (
              <SelectedPropertyRow
                key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                onRemove={() => remove(index)}
                nameSpace={`properties.${index}`}
                index={index}
                property={property.toFeatureDataset()}
              />
            ))}
            {values.properties.length === 0 && <span>No Properties selected</span>}
          </Section>
        )}
      </FieldArray>
    </Section>
  );
};

export default ResearchProperties;

const StyledButtonWrapper = styled.div`
  margin: 0 1.6rem;
  padding-left: 1.6rem;
  text-align: left;
  text-underline-offset: 2px;

  button {
    font-size: 14px;
  }
`;
