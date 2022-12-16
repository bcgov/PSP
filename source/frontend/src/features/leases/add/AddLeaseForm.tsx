import { IMapProperty } from 'components/propertySelector/models';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import * as React from 'react';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import { FormLeaseProperty, getDefaultFormLease, LeaseFormModel } from '../models';
import LeasePropertySelector from '../shared/propertyPicker/LeasePropertySelector';
import { AddLeaseYupSchema } from './AddLeaseYupSchema';
import AdministrationSubForm from './AdministrationSubForm';
import LeaseDetailSubForm from './LeaseDetailSubForm';
import DocumentationSubForm from './ReferenceSubForm';
import * as Styled from './styles';

interface IAddLeaseFormProps {
  onSubmit: (
    values: LeaseFormModel,
    formikHelpers: FormikHelpers<LeaseFormModel>,
  ) => void | Promise<any>;
  formikRef: React.Ref<FormikProps<LeaseFormModel>>;
  propertyInfo: IMapProperty | null;
}

const AddLeaseForm: React.FunctionComponent<React.PropsWithChildren<IAddLeaseFormProps>> = ({
  onSubmit,
  formikRef,
  propertyInfo,
}) => {
  const defaultFormLease = getDefaultFormLease();

  if (propertyInfo) {
    defaultFormLease.properties = [];
    defaultFormLease.properties.push(FormLeaseProperty.fromMapProperty(propertyInfo));
    defaultFormLease.regionId = propertyInfo.region ? propertyInfo.region.toString() : '';
  }

  const handleSubmit = async (
    values: LeaseFormModel,
    formikHelpers: FormikHelpers<LeaseFormModel>,
  ) => {
    formikHelpers.setSubmitting(false);
    await onSubmit(values, formikHelpers);
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
              <LeasePropertySelector formikProps={formikProps} />
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
