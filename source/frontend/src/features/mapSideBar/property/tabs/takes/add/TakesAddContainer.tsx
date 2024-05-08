import { FormikProps } from 'formik';
import * as React from 'react';

import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';

import { TakeModel } from '../models';
import { useTakesRepository } from '../repositories/useTakesRepository';
import { emptyTake, ITakesFormProps } from '../update/TakeForm';

export interface ITakesDetailContainerProps {
  fileProperty: ApiGen_Concepts_FileProperty;
  View: React.ForwardRefExoticComponent<ITakesFormProps & React.RefAttributes<FormikProps<any>>>;
  onSuccess: () => void;
}

export const TakesAddContainer = React.forwardRef<FormikProps<any>, ITakesDetailContainerProps>(
  ({ fileProperty, View, onSuccess }, ref) => {
    const {
      addTakeByAcquisitionPropertyId: { execute: addTakesByPropertyFile, loading: addTakesLoading },
    } = useTakesRepository();

    return (
      <View
        onSubmit={async (values, formikHelpers) => {
          formikHelpers.setSubmitting(true);
          try {
            const take = values.toApi();
            take.propertyAcquisitionFileId = fileProperty.id;
            await addTakesByPropertyFile(fileProperty.id, take);
            onSuccess();
          } finally {
            formikHelpers.setSubmitting(false);
          }
        }}
        loading={addTakesLoading}
        take={new TakeModel(emptyTake)}
        ref={ref}
      />
    );
  },
);

export default TakesAddContainer;
