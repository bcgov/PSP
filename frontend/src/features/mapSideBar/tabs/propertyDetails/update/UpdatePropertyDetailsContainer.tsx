import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Formik, FormikHelpers } from 'formik';
import useIsMounted from 'hooks/useIsMounted';
import { Api_Property } from 'models/api/Property';
import React, { useCallback, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { useGetProperty, useUpdateProperty } from '../hooks';
import { UpdatePropertyDetailsFormModel } from './models';
import { UpdatePropertyDetailsForm } from './UpdatePropertyDetailsForm';

export interface IUpdatePropertyDetailsContainerProps {
  pid?: string;
}

export const UpdatePropertyDetailsContainer: React.FC<IUpdatePropertyDetailsContainerProps> = props => {
  const isMounted = useIsMounted();
  const history = useHistory();
  const { retrieveProperty } = useGetProperty();
  const { updateProperty } = useUpdateProperty();

  const [initialForm, setForm] = useState<UpdatePropertyDetailsFormModel | undefined>(undefined);

  useEffect(() => {
    async function fetchProperty() {
      if (!!props.pid) {
        const retrieved = await retrieveProperty(props.pid);
        if (retrieved !== undefined && isMounted()) {
          setForm(UpdatePropertyDetailsFormModel.fromApi(retrieved));
        }
      }
    }
    fetchProperty();
  }, [isMounted, props.pid, retrieveProperty]);

  // save handler - sends updated property information to backend and redirects back to view screen
  const savePropertyInformation = async (
    values: UpdatePropertyDetailsFormModel,
    formikHelpers: FormikHelpers<UpdatePropertyDetailsFormModel>,
  ) => {
    const apiProperty: Api_Property = values.toApi();
    const response = await updateProperty(apiProperty);

    if (!!response?.pid) {
      formikHelpers.resetForm();
      history.replace(`/mapview/property/${apiProperty.pid}`);
    }

    formikHelpers.setSubmitting(false);
  };

  const onCancel = useCallback(() => {
    if (!!initialForm?.pid) {
      history.replace(`/mapview/property/${initialForm.pid}`);
    }
  }, [history, initialForm?.pid]);

  if (initialForm === undefined) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <Formik<UpdatePropertyDetailsFormModel>
      enableReinitialize
      initialValues={initialForm || new UpdatePropertyDetailsFormModel()}
      onSubmit={savePropertyInformation}
    >
      {formikProps => <UpdatePropertyDetailsForm {...formikProps} onCancel={onCancel} />}
    </Formik>
  );
};
