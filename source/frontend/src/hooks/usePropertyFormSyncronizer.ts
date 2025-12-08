import { FormikProps, getIn } from 'formik';
import { useCallback, useEffect } from 'react';
import { toast } from 'react-toastify';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { PropertyForm, WithFormProperties } from '@/features/mapSideBar/shared/models';
import { exists, isEmptyOrNull } from '@/utils';
import { arePropertyFormsEqual } from '@/utils/mapPropertyUtils';

import { useEditPropertiesMode } from './useEditPropertiesMode';
import { useLocationFeatureDatasetsWithAddresses } from './useLocationFeatureDatasetsWithAddresses';

/**
 * Notifies the map state machine to enter/exit edit properties mode.
 * Use this hook in any component that needs to toggle edit properties mode on mount/unmount.
 */
export function usePropertyFormSyncronizer<T extends WithFormProperties>(
  formikRef: React.RefObject<FormikProps<T>>,
  validateNewProperties: (
    newProperties: PropertyForm[],
    validateCallback: (isValid: boolean, newProperties: PropertyForm[]) => void,
  ) => void,
  overrideFeatures?: LocationFeatureDataset[],
) {
  const {
    locationFeaturesForAddition,
    pendingLocationFeaturesAddition,
    processLocationFeaturesAddition,
  } = useMapStateMachine();

  // Get PropertyForms with addresses for all selected features
  const { locationFeaturesWithAddresses: featuresWithAddresses, bcaLoading } =
    useLocationFeatureDatasetsWithAddresses(overrideFeatures ?? locationFeaturesForAddition);

  // if we are listening to property add notifications we must tell the state machine we are in edit mode.
  useEditPropertiesMode();

  const fieldName = 'properties';

  const validationCallback = useCallback(
    (isValid: boolean, newProperties: PropertyForm[]) => {
      if (isValid && !isEmptyOrNull(newProperties)) {
        const existingProperties = getIn(formikRef?.current?.values, fieldName) ?? [];
        formikRef.current?.setFieldValue(fieldName, [...existingProperties, ...newProperties]);
        formikRef.current?.setFieldTouched(fieldName, true);
        toast.success(`Added ${newProperties.length} new property(s) to the file.`);
      }
    },
    [fieldName, formikRef],
  );

  // This effect willbe called whenever there are new locations pending addition.
  useEffect(() => {
    if (
      exists(formikRef?.current) &&
      pendingLocationFeaturesAddition &&
      featuresWithAddresses.length > 0
    ) {
      // Convert LocationFeatureDataset to PropertyForm
      const newPropertyForms = featuresWithAddresses.map(obj => {
        const property = PropertyForm.fromLocationFeatureDataset(obj.feature);
        if (exists(obj.address)) {
          property.address = obj.address;
        }
        return property;
      });

      const existingProperties = getIn(formikRef?.current?.values, fieldName as string) ?? [];
      const uniqueNewProperties = newPropertyForms.filter(newProperty => {
        return !existingProperties.some((existingProperty: PropertyForm) =>
          arePropertyFormsEqual(existingProperty, newProperty),
        );
      });

      const duplicatesSkipped = newPropertyForms.length - uniqueNewProperties.length;

      // If there are unique properties request a confirmation
      if (uniqueNewProperties.length > 0) {
        validateNewProperties(uniqueNewProperties, validationCallback);
      }
      if (duplicatesSkipped > 0) {
        toast.warn(`Skipped ${duplicatesSkipped} duplicate property(s).`);
      }
      processLocationFeaturesAddition();
    }
  }, [
    featuresWithAddresses,
    fieldName,
    formikRef,
    pendingLocationFeaturesAddition,
    processLocationFeaturesAddition,
    validateNewProperties,
    validationCallback,
  ]);

  return { featuresWithAddresses, isLoading: bcaLoading };
}
