import { FormikProps } from 'formik';
import * as React from 'react';
import { useEffect } from 'react';
import { useParams } from 'react-router-dom';

import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { isValidId } from '@/utils/utils';

import { TakeModel } from '../models';
import { useTakesRepository } from '../repositories/useTakesRepository';
import { emptyTake, ITakesFormProps } from './TakeForm';

export interface ITakesDetailContainerProps {
  fileProperty: ApiGen_Concepts_FileProperty;
  View: React.ForwardRefExoticComponent<ITakesFormProps & React.RefAttributes<FormikProps<any>>>;
  onSuccess: () => void;
}

export const TakesUpdateContainer = React.forwardRef<FormikProps<any>, ITakesDetailContainerProps>(
  ({ fileProperty, View, onSuccess }, ref) => {
    const { takeId } = useParams<{ takeId: string }>();
    if (!isValidId(fileProperty?.id) || !isValidId(+takeId)) {
      throw Error('Unable to edit take with invalid ids');
    }
    const {
      updateTakeByAcquisitionPropertyId: {
        execute: updateTakesByPropertyFile,
        loading: updateTakesLoading,
      },
      getTakeById: { execute: getTakeById, loading: getTakeLoading, response: take },
    } = useTakesRepository();

    useEffect(() => {
      getTakeById(fileProperty.id, +takeId);
    }, [fileProperty.id, getTakeById, takeId]);

    return (
      <View
        onSubmit={async (values, formikHelpers) => {
          formikHelpers.setSubmitting(true);
          try {
            const take = values.toApi();
            await updateTakesByPropertyFile(fileProperty.id, take);
            onSuccess();
          } finally {
            formikHelpers.setSubmitting(false);
          }
        }}
        loading={updateTakesLoading || getTakeLoading}
        take={take ? new TakeModel(take) : new TakeModel(emptyTake)}
        ref={ref}
      />
    );
  },
);

export default TakesUpdateContainer;
