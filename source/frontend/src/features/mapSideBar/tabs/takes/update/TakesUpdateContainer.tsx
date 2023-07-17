import { FormikProps } from 'formik';
import { Api_PropertyFile } from 'models/api/PropertyFile';
import { Api_Take } from 'models/api/Take';
import * as React from 'react';

import { useTakesRepository } from '../repositories/useTakesRepository';
import { TakeModel } from './models';
import { emptyTake, ITakesUpdateFormProps } from './TakesUpdateForm';

export interface ITakesDetailContainerProps {
  fileProperty: Api_PropertyFile;
  View: React.ForwardRefExoticComponent<
    ITakesUpdateFormProps & React.RefAttributes<FormikProps<any>>
  >;
  onSuccess: () => void;
}

export const TakesUpdateContainer = React.forwardRef<FormikProps<any>, ITakesDetailContainerProps>(
  ({ fileProperty, View, onSuccess }, ref) => {
    const [propertyTakes, setPropertyTakes] = React.useState<Api_Take[]>([]);

    if (!fileProperty?.id) {
      throw Error('File property must have id');
    }
    const {
      getTakesByFileId: { loading: takesByFileLoading, execute: getTakesByFile },
      updateTakesByAcquisitionPropertyId: { execute: updateTakesByPropertyFile },
    } = useTakesRepository();

    React.useEffect(() => {
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
            fileProperty.id && (await updateTakesByPropertyFile(fileProperty.id, takes));
            onSuccess();
          } finally {
            formikHelpers.setSubmitting(false);
          }
        }}
        loading={takesByFileLoading}
        takes={
          propertyTakes?.length
            ? propertyTakes?.map(t => new TakeModel(t))
            : [new TakeModel(emptyTake)]
        }
        fileProperty={fileProperty}
        ref={ref}
      />
    );
  },
);

export default TakesUpdateContainer;
