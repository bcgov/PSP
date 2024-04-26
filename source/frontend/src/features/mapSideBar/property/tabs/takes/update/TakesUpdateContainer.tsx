import { FormikProps } from 'formik';
import orderBy from 'lodash/orderBy';
import { forwardRef, useEffect, useState } from 'react';

import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Take } from '@/models/api/generated/ApiGen_Concepts_Take';

import { useTakesRepository } from '../repositories/useTakesRepository';
import { TakeModel } from './models';
import { emptyTake, ITakesUpdateFormProps } from './TakesUpdateForm';

export interface ITakesDetailContainerProps {
  fileProperty: ApiGen_Concepts_FileProperty;
  View: React.ForwardRefExoticComponent<
    ITakesUpdateFormProps & React.RefAttributes<FormikProps<any>>
  >;
  onSuccess: () => void;
}

export const TakesUpdateContainer = forwardRef<FormikProps<any>, ITakesDetailContainerProps>(
  ({ fileProperty, View, onSuccess }, ref) => {
    const [propertyTakes, setPropertyTakes] = useState<ApiGen_Concepts_Take[]>([]);

    if (!fileProperty?.id) {
      throw Error('File property must have id');
    }
    const {
      getTakesByFileId: { loading: takesByFileLoading, execute: getTakesByFile },
      updateTakesByAcquisitionPropertyId: { execute: updateTakesByPropertyFile },
    } = useTakesRepository();

    useEffect(() => {
      const fetchTakes = async () => {
        if (fileProperty.fileId) {
          const fileTakes = await getTakesByFile(fileProperty.fileId);
          if (fileTakes !== undefined) {
            const propertyTakesRetrieved = fileTakes.filter(
              x => x.propertyAcquisitionFileId === fileProperty.id,
            );
            setPropertyTakes(propertyTakesRetrieved);
          } else {
            setPropertyTakes([]);
          }
        }
      };
      fetchTakes();
    }, [getTakesByFile, fileProperty]);

    return (
      <View
        onSubmit={async (values, formikHelpers) => {
          formikHelpers.setSubmitting(true);
          try {
            const takeModels = values.takes;
            const takes = takeModels.map(t => {
              t.propertyAcquisitionFileId = fileProperty.id ?? 0;
              return t.toApi();
            });
            if (fileProperty.id) {
              await updateTakesByPropertyFile(fileProperty.id, takes);
              onSuccess();
            }
          } finally {
            formikHelpers.setSubmitting(false);
          }
        }}
        loading={takesByFileLoading}
        takes={
          propertyTakes?.length
            ? orderBy(
                propertyTakes?.map(t => new TakeModel(t)),
                t => t.id,
                'desc',
              )
            : [new TakeModel(emptyTake)]
        }
        fileProperty={fileProperty}
        ref={ref}
      />
    );
  },
);

export default TakesUpdateContainer;
