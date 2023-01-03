import { AddLeaseYupSchema } from 'features/leases/add/AddLeaseYupSchema';
import AdministrationSubForm from 'features/leases/add/AdministrationSubForm';
import LeaseDetailSubForm from 'features/leases/add/LeaseDetailSubForm';
import ReferenceSubForm from 'features/leases/add/ReferenceSubForm';
import * as Styled from 'features/leases/add/styles';
import { getDefaultFormLease, LeaseFormModel } from 'features/leases/models';
import { Formik, FormikProps } from 'formik';
import * as React from 'react';
import { Prompt } from 'react-router-dom';

interface IUpdateLeaseFormProps {
  onSubmit: (lease: LeaseFormModel) => Promise<void>;
  initialValues?: LeaseFormModel;
  formikRef: React.Ref<FormikProps<LeaseFormModel>>;
}

export const UpdateLeaseForm: React.FunctionComponent<
  React.PropsWithChildren<IUpdateLeaseFormProps>
> = ({ onSubmit, initialValues, formikRef }) => {
  return (
    <Formik
      validationSchema={AddLeaseYupSchema}
      onSubmit={values => onSubmit(values)}
      initialValues={initialValues ?? getDefaultFormLease()}
      innerRef={formikRef}
    >
      {formikProps => (
        <>
          <Prompt
            when={formikProps.dirty}
            message="You have made changes on this form. Do you wish to leave without saving?"
          />
          <Styled.LeaseForm>
            <LeaseDetailSubForm formikProps={formikProps}></LeaseDetailSubForm>
            <AdministrationSubForm formikProps={formikProps}></AdministrationSubForm>
            <ReferenceSubForm />
          </Styled.LeaseForm>
        </>
      )}
    </Formik>
  );
};

export default UpdateLeaseForm;
