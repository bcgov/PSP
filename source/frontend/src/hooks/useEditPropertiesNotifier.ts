import { FormikProps } from 'formik';
import { useEffect, useMemo } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { PropertyForm } from '@/features/mapSideBar/shared/models';
import { addPropertiesToCurrentFile } from '@/features/mapSideBar/shared/update/properties/UpdateProperties';
import { exists } from '@/utils/utils';

import { useEditPropertiesMode } from './useEditPropertiesMode';
import { useFeatureDatasetsWithAddresses } from './useFeatureDatasetsWithAddresses';

/**
 * Notifies the map state machine to enter/exit edit properties mode.
 * Use this hook in any component that needs to toggle edit properties mode on mount/unmount.
 */
export function useEditPropertiesNotifier<T extends { [key: string]: any }>(
  formikRef: React.RefObject<FormikProps<T>>,
  fieldName: keyof T,
  overrideFeatures?: SelectedFeatureDataset[],
) {
  const { selectedFeatures, processCreation } = useMapStateMachine();
  // Get PropertyForms with addresses for all selected features
  const { featuresWithAddresses, bcaLoading } = useFeatureDatasetsWithAddresses(
    overrideFeatures ?? selectedFeatures ?? [],
  );

  useEditPropertiesMode(); // if we are listening to property add notifications we must tell the state machine we are in edit mode.

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
  // This effect is used to update the file properties when "add to open file" is clicked in the worklist.
  useEffect(() => {
    if (exists(formikRef?.current) && propertyForms.length > 0) {
      addPropertiesToCurrentFile(formikRef, fieldName, propertyForms, processCreation);
    }
  }, [fieldName, formikRef, processCreation, propertyForms]);

  return { featuresWithAddresses, bcaLoading };
}
