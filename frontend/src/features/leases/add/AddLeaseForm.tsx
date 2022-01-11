import { Formik, FormikProps } from 'formik';
import { defaultAddFormLease, IAddFormLease, ILease } from 'interfaces';
import * as React from 'react';
import { Prompt } from 'react-router-dom';

import { addFormLeaseToApiLease } from '../leaseUtils';
import AddLeaseFormButtons from './AddLeaseFormButtons';
import { LeaseSchema } from './AddLeaseYupSchema';
import AdministrationSubForm from './AdministrationSubForm';
import LeaseDatesSubForm from './LeaseDatesSubForm';
import PropertyInformationSubForm from './PropertyInformationSubForm';
import ReferenceSubForm from './ReferenceSubForm';
import * as Styled from './styles';

interface IAddLeaseFormProps {
  onCancel: () => void;
  onSubmit: (lease: ILease) => void;
  formikRef: React.Ref<FormikProps<IAddFormLease>>;
  initialValues?: IAddFormLease;
}

const AddLeaseForm: React.FunctionComponent<IAddLeaseFormProps> = ({
  onCancel,
  onSubmit,
  formikRef,
  initialValues,
}) => {
  return (
    <Formik<IAddFormLease>
      initialValues={defaultAddFormLease ?? initialValues}
      onSubmit={async (values: IAddFormLease, formikHelpers) => {
        const apiLease = addFormLeaseToApiLease(values);
        formikHelpers.setSubmitting(false);
        onSubmit(apiLease);
      }}
      validationSchema={LeaseSchema}
      innerRef={formikRef}
    >
      {formikProps => (
        <>
          <Prompt
            when={formikProps.dirty && formikProps.submitCount === 0}
            message="You have made changes on this form. Do you wish to leave without saving?"
          />
          <Styled.LeaseForm>
            <LeaseDatesSubForm formikProps={formikProps}></LeaseDatesSubForm>
            <AdministrationSubForm formikProps={formikProps}></AdministrationSubForm>
            <ReferenceSubForm />
            <PropertyInformationSubForm />
            <AddLeaseFormButtons formikProps={formikProps} onCancel={onCancel} />
          </Styled.LeaseForm>
        </>
      )}
    </Formik>
  );
};

export default AddLeaseForm;
