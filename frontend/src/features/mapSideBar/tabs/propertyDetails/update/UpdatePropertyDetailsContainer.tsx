import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import useIsMounted from 'hooks/useIsMounted';
import { useQueryMapLayersByLocation } from 'hooks/useQueryMapLayersByLocation';
import isNumber from 'lodash/isNumber';
import { Api_Property } from 'models/api/Property';
import React, { useEffect, useRef, useState } from 'react';
import styled from 'styled-components';

import { useGetProperty, useUpdateProperty } from '../hooks';
import { UpdatePropertyDetailsFormModel } from './models';
import { UpdatePropertyDetailsForm } from './UpdatePropertyDetailsForm';
import { UpdatePropertyDetailsYupSchema } from './validation';

export interface IUpdatePropertyDetailsContainerProps {
  pid: string;
  setFormikRef: (ref: React.RefObject<FormikProps<any>> | undefined) => void;
  onSuccess: () => void;
}

export const UpdatePropertyDetailsContainer: React.FC<IUpdatePropertyDetailsContainerProps> = props => {
  const isMounted = useIsMounted();
  const { retrieveProperty, retrievePropertyLoading } = useGetProperty();
  const { updateProperty } = useUpdateProperty();
  const { queryAll } = useQueryMapLayersByLocation();

  const formikRef = useRef<FormikProps<UpdatePropertyDetailsFormModel>>(null);

  props.setFormikRef(formikRef);

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

  // save handler - sends updated property information to backend
  const savePropertyInformation = async (
    values: UpdatePropertyDetailsFormModel,
    formikHelpers: FormikHelpers<UpdatePropertyDetailsFormModel>,
  ) => {
    const apiProperty: Api_Property = values.toApi();
    const response = await updateProperty(apiProperty);

    formikHelpers.setSubmitting(false);

    if (!!response?.pid) {
      formikHelpers.resetForm();
      props.onSuccess();
    }
  };

  if (retrievePropertyLoading) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <Formik<UpdatePropertyDetailsFormModel>
      enableReinitialize
      innerRef={formikRef}
      validationSchema={UpdatePropertyDetailsYupSchema}
      initialValues={initialForm || new UpdatePropertyDetailsFormModel()}
      onSubmit={savePropertyInformation}
    >
      {formikProps => (
        <StyledFormWrapper>
          <UpdatePropertyDetailsForm formikProps={formikProps} />
        </StyledFormWrapper>
      )}
    </Formik>
  );
};

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;
