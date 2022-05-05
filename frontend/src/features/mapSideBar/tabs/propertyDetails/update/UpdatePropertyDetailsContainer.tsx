import { Scrollable } from 'components/common/Scrollable/Scrollable';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Formik, FormikHelpers } from 'formik';
import useIsMounted from 'hooks/useIsMounted';
import { Api_Property } from 'models/api/Property';
import React, { useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import styled from 'styled-components';

import { useGetProperty, useUpdateProperty } from '../hooks';
import { defaultUpdateProperty, fromApi, toApi, UpdatePropertyDetailsFormModel } from './models';
import { UpdatePropertyDetailsForm } from './UpdatePropertyDetailsForm';

export interface IUpdatePropertyDetailsContainerProps {
  pid: string;
}

export const UpdatePropertyDetailsContainer: React.FC<IUpdatePropertyDetailsContainerProps> = props => {
  const isMounted = useIsMounted();
  const history = useHistory();
  const { retrieveProperty } = useGetProperty();
  const { updateProperty } = useUpdateProperty();

  const [initialForm, setForm] = useState<UpdatePropertyDetailsFormModel | undefined>(undefined);

  useEffect(() => {
    async function fetchProperty() {
      const retrieved = await retrieveProperty(props.pid);
      if (retrieved !== undefined && isMounted()) {
        setForm(fromApi(retrieved));
      }
    }
    fetchProperty();
  }, [isMounted, props.pid, retrieveProperty]);

  // save handler - sends updated property information to backend and redirects back to view screen
  const savePropertyInformation = async (
    values: UpdatePropertyDetailsFormModel,
    formikHelpers: FormikHelpers<UpdatePropertyDetailsFormModel>,
  ) => {
    const apiProperty: Api_Property = toApi(values);
    const response = await updateProperty(apiProperty);

    if (!!response?.pid) {
      formikHelpers.resetForm();
      history.replace(`/mapview/property${apiProperty.pid}`);
    }

    formikHelpers.setSubmitting(false);
  };

  if (initialForm === undefined) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <>
      <Content vertical>
        <Formik<UpdatePropertyDetailsFormModel>
          enableReinitialize
          initialValues={initialForm || defaultUpdateProperty}
          onSubmit={savePropertyInformation}
        >
          {formikProps => <UpdatePropertyDetailsForm {...formikProps} />}
        </Formik>
      </Content>
      <Footer></Footer>
    </>
  );
};

const Content = styled(Scrollable)`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;

const Footer = styled.div``;
