import { addFormLeaseToApiLease } from 'features/leases/leaseUtils';
import { Formik, FormikProps, useFormikContext } from 'formik';
import { defaultAddFormLease, IAddFormLease, IFormLease, ILease } from 'interfaces';
import * as React from 'react';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import AddLeaseFormButtons from '../AddLeaseFormButtons';
import { LeaseSchema } from '../AddLeaseYupSchema';
import AdministrationSubForm from '../AdministrationSubForm';
import LeaseDatesSubForm from '../LeaseDatesSubForm';
import PropertyInformationSubForm from '../PropertyInformationSubForm';
import ReferenceSubForm from '../ReferenceSubForm';
import * as Styled from '../styles';

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
