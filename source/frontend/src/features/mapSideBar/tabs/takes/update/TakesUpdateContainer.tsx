import { FormikProps } from 'formik';
import { Api_PropertyFile } from 'models/api/PropertyFile';
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
    if (!fileProperty?.id) {
      throw Error('File property must have id');
    }
    const {
      getTakesByFileId: { loading: takesByFileLoading, response: takes, execute: getTakesByFile },
      updateTakesByAcquisitionPropertyId: { execute: updateTakesByPropertyFile },
    } = useTakesRepository();

    React.useEffect(() => {
      fileProperty.fileId && getTakesByFile(fileProperty.fileId);
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
        takes={takes?.length ? takes?.map(t => new TakeModel(t)) : [new TakeModel(emptyTake)]}
        fileProperty={fileProperty}
        ref={ref}
      />
    );
  },
);

export default TakesUpdateContainer;
