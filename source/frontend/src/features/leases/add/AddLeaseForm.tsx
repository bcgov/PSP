import { Formik, FormikHelpers, FormikProps } from 'formik';
import { IProperty } from 'interfaces';
import { Api_Lease } from 'models/api/Lease';
import * as React from 'react';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import { FormLeaseProperty, getDefaultFormLease, LeaseFormModel } from '../models';
import { AddLeaseYupSchema } from './AddLeaseYupSchema';
import AdministrationSubForm from './AdministrationSubForm';
import LeaseDetailSubForm from './LeaseDetailSubForm';
import DocumentationSubForm from './ReferenceSubForm';
import * as Styled from './styles';

interface IAddLeaseFormProps {
  onSubmit: (lease: Api_Lease) => void;
  formikRef: React.Ref<FormikProps<LeaseFormModel>>;
  propertyInfo: IProperty | null;
}

const AddLeaseForm: React.FunctionComponent<React.PropsWithChildren<IAddLeaseFormProps>> = ({
  onSubmit,
  formikRef,
  propertyInfo,
}) => {
  const defaultFormLease = getDefaultFormLease();

  if (propertyInfo) {
    defaultFormLease.properties = [];
    defaultFormLease.properties.push(
      FormLeaseProperty.fromApi({
        property: {
          pid: propertyInfo.pid ? +propertyInfo.pid : undefined,
          pin: propertyInfo?.pin ? +propertyInfo.pin : undefined,
          latitude: propertyInfo.latitude,
          longitude: propertyInfo.longitude,
          location: { coordinate: { x: propertyInfo.longitude, y: propertyInfo.latitude } },
        },
        leaseArea: propertyInfo.landArea,
        areaUnitType: { id: propertyInfo.areaUnit },
      }),
    );
    defaultFormLease.regionId = propertyInfo.regionId ? propertyInfo.regionId : 0;
  }

  const handleSubmit = (values: LeaseFormModel, formikHelpers: FormikHelpers<LeaseFormModel>) => {
    const apiLease = values.toApi();
    formikHelpers.setSubmitting(false);
    onSubmit(apiLease);
  };

  return (
    <StyledFormWrapper>
      <Formik<LeaseFormModel>
        enableReinitialize
        innerRef={formikRef}
        initialValues={defaultFormLease}
        validationSchema={AddLeaseYupSchema}
        onSubmit={handleSubmit}
      >
        {formikProps => (
          <>
            <Prompt
              when={formikProps.dirty && formikProps.submitCount === 0}
              message="You have made changes on this form. Do you wish to leave without saving?"
            />
            <Styled.LeaseForm>
              <LeaseDetailSubForm formikProps={formikProps}></LeaseDetailSubForm>
              <AdministrationSubForm formikProps={formikProps}></AdministrationSubForm>
              <DocumentationSubForm />
            </Styled.LeaseForm>
          </>
        )}
      </Formik>
    </StyledFormWrapper>
  );
};

export default AddLeaseForm;

const StyledFormWrapper = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
`;
