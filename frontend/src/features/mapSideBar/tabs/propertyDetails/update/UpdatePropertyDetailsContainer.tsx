import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Formik, FormikHelpers } from 'formik';
import useIsMounted from 'hooks/useIsMounted';
import { useQueryMapLayersByLocation } from 'hooks/useQueryMapLayersByLocation';
import isNumber from 'lodash/isNumber';
import { Api_Property } from 'models/api/Property';
import React, { useCallback, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { useGetProperty, useUpdateProperty } from '../hooks';
import { UpdatePropertyDetailsFormModel } from './models';
import { UpdatePropertyDetailsForm } from './UpdatePropertyDetailsForm';
import { UpdatePropertyDetailsYupSchema } from './validation';

export interface IUpdatePropertyDetailsContainerProps {
  pid: string;
}

export const UpdatePropertyDetailsContainer: React.FC<IUpdatePropertyDetailsContainerProps> = props => {
  const isMounted = useIsMounted();
  const history = useHistory();
  const { retrieveProperty, retrievePropertyLoading } = useGetProperty();
  const { updateProperty } = useUpdateProperty();
  const { queryAll } = useQueryMapLayersByLocation();

  const [initialForm, setForm] = useState<UpdatePropertyDetailsFormModel | undefined>(undefined);

  useEffect(() => {
    async function fetchProperty() {
      if (!!props.pid) {
        const retrieved = await retrieveProperty(props.pid);
        if (retrieved !== undefined && isMounted()) {
          const formValues = UpdatePropertyDetailsFormModel.fromApi(retrieved);

          // This triggers API calls to DataBC map layers
          if (isNumber(retrieved.latitude) && isNumber(retrieved.longitude)) {
            const layers = await queryAll({ lat: retrieved.latitude, lng: retrieved.longitude });
            formValues.isALR = !!layers.isALR;
            formValues.motiRegion = layers.motiRegion;
            formValues.highwaysDistrict = layers.highwaysDistrict;
            formValues.electoralDistrict = layers.electoralDistrict;
            formValues.firstNations = layers.firstNations;
          }

          setForm(formValues);
        }
      }
    }
    fetchProperty();
  }, [isMounted, props.pid, queryAll, retrieveProperty]);

  // save handler - sends updated property information to backend and redirects back to view screen
  const savePropertyInformation = async (
    values: UpdatePropertyDetailsFormModel,
    formikHelpers: FormikHelpers<UpdatePropertyDetailsFormModel>,
  ) => {
    const apiProperty: Api_Property = values.toApi();
    const response = await updateProperty(apiProperty);

    if (!!response?.pid) {
      formikHelpers.resetForm();
      history.push(`/mapview/property/${apiProperty.pid}`);
    }

    formikHelpers.setSubmitting(false);
  };

  const onCancel = useCallback(() => {
    if (!!initialForm?.pid) {
      history.push(`/mapview/property/${initialForm.pid}`);
    }
  }, [history, initialForm?.pid]);

  if (retrievePropertyLoading) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <Formik<UpdatePropertyDetailsFormModel>
      enableReinitialize
      validationSchema={UpdatePropertyDetailsYupSchema}
      initialValues={initialForm || new UpdatePropertyDetailsFormModel()}
      onSubmit={savePropertyInformation}
    >
      {formikProps => <UpdatePropertyDetailsForm {...formikProps} onCancel={onCancel} />}
    </Formik>
  );
};
