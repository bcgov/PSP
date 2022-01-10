import AddLeaseFormButtons from 'features/leases/add/AddLeaseFormButtons';
import { LeaseSchema } from 'features/leases/add/AddLeaseYupSchema';
import AdministrationSubForm from 'features/leases/add/AdministrationSubForm';
import LeaseDatesSubForm from 'features/leases/add/LeaseDatesSubForm';
import PropertyInformationSubForm from 'features/leases/add/PropertyInformationSubForm';
import ReferenceSubForm from 'features/leases/add/ReferenceSubForm';
import * as Styled from 'features/leases/add/styles';
import { Formik, FormikProps } from 'formik';
import { defaultAddFormLease, IAddFormLease } from 'interfaces';
import * as React from 'react';
import { Prompt } from 'react-router-dom';

interface IUpdateLeaseFormProps {
  onCancel: () => void;
  onSubmit: (lease: IAddFormLease) => Promise<void>;
  initialValues?: IAddFormLease;
  formikRef: React.Ref<FormikProps<IAddFormLease>>;
}

export const UpdateLeaseForm: React.FunctionComponent<IUpdateLeaseFormProps> = ({
  onCancel,
  onSubmit,
  initialValues,
  formikRef,
}) => {
  return (
    <Formik
      validationSchema={LeaseSchema}
      onSubmit={values => onSubmit(values)}
      initialValues={
        !!initialValues ? { ...defaultAddFormLease, ...initialValues } : defaultAddFormLease
      }
      innerRef={formikRef}
      enableReinitialize
    >
      {formikProps => (
        <>
          <Prompt
            when={formikProps.dirty}
            message="You have made changes on this form. Do you wish to leave without saving?"
          />
          <Styled.LeaseForm>
            <LeaseDatesSubForm formikProps={formikProps}></LeaseDatesSubForm>
            <AdministrationSubForm formikProps={formikProps}></AdministrationSubForm>
            <ReferenceSubForm />
            <PropertyInformationSubForm />
          </Styled.LeaseForm>
          <AddLeaseFormButtons formikProps={formikProps} onCancel={onCancel} />
        </>
      )}
    </Formik>
  );
};

export default UpdateLeaseForm;
