import { FormikProps } from 'formik';
import React, { useEffect } from 'react';

import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { isValidId } from '@/utils';

import { PropertyNetBookFormModel } from './models';
import { IPropertyNetBookUpdateFormProps } from './PropertyNetBookUpdateForm';

export interface IPropertyNetBookUpdateContainerProps {
  propertyId: number;
  onSuccess: () => void;
  View: React.ForwardRefExoticComponent<
    IPropertyNetBookUpdateFormProps & React.RefAttributes<FormikProps<PropertyNetBookFormModel>>
  >;
}

export const PropertyNetBookUpdateContainer = React.forwardRef<
  FormikProps<PropertyNetBookFormModel>,
  IPropertyNetBookUpdateContainerProps
>(({ propertyId, View, onSuccess }, formikRef) => {
  const {
    getPropertyWrapper: {
      execute: executeGetProperty,
      loading: loadingGetProperty,
      response: property,
    },
    updatePropertyNetBookWrapper: { execute: executeUpdatePropertyNetBook },
  } = usePimsPropertyRepository();

  // API save handler
  const savePropertyNetBook = async (values: ApiGen_Concepts_Property) => {
    const result = await executeUpdatePropertyNetBook(values);
    if (isValidId(result?.id) && typeof onSuccess === 'function') {
      onSuccess();
    }
  };

  // Initial load
  useEffect(() => {
    if (isValidId(propertyId)) {
      executeGetProperty(propertyId);
    }
  }, [executeGetProperty, propertyId]);

  return (
    <View
      isLoading={loadingGetProperty}
      property={property ?? null}
      onSave={savePropertyNetBook}
      ref={formikRef}
    />
  );
});
