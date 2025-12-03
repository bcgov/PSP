import { FormikProps, getIn } from 'formik';
import { useEffect } from 'react';
import { toast } from 'react-toastify';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { PropertyForm } from '@/features/mapSideBar/shared/models';
import { exists } from '@/utils';
import { arePropertyFormsEqual } from '@/utils/mapPropertyUtils';

import { useEditPropertiesMode } from './useEditPropertiesMode';
import { useLocationFeatureDatasetsWithAddresses } from './useLocationFeatureDatasetsWithAddresses';

/**
 * Notifies the map state machine to enter/exit edit properties mode.
 * Use this hook in any component that needs to toggle edit properties mode on mount/unmount.
 */
export function usePropertyFormSyncronizer<T extends { [key: string]: any }>(
  formikRef: React.RefObject<FormikProps<T>>,
  fieldName: keyof T,
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

  // This effect is used to update the file properties when "add to open file" is clicked in the worklist.
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

      // If there are unique properties, add them to the formik values
      if (uniqueNewProperties.length > 0) {
        formikRef.current?.setFieldValue(fieldName as string, [
          ...existingProperties,
          ...uniqueNewProperties,
        ]);
        formikRef.current?.setFieldTouched(fieldName as string, true);
        toast.success(`Added ${uniqueNewProperties.length} new property(s) to the file.`);
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
  ]);

  return { featuresWithAddresses, isLoading: bcaLoading };
}
