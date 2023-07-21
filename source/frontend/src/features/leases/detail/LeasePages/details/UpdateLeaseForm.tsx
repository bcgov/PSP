import { Formik, FormikProps } from 'formik';
import * as React from 'react';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import { AddLeaseYupSchema } from '@/features/leases/add/AddLeaseYupSchema';
import AdministrationSubForm from '@/features/leases/add/AdministrationSubForm';
import ConsultationSubForm from '@/features/leases/add/ConsultationSubForm';
import LeaseDetailSubForm from '@/features/leases/add/LeaseDetailSubForm';
import ReferenceSubForm from '@/features/leases/add/ReferenceSubForm';
import { getDefaultFormLease, LeaseFormModel } from '@/features/leases/models';
import { LeasePropertySelector } from '@/features/leases/shared/propertyPicker/LeasePropertySelector';

export interface IUpdateLeaseFormProps {
  onSubmit: (lease: LeaseFormModel) => Promise<void>;
  initialValues?: LeaseFormModel;
  formikRef: React.Ref<FormikProps<LeaseFormModel>>;
}

export const UpdateLeaseForm: React.FunctionComponent<
  React.PropsWithChildren<IUpdateLeaseFormProps>
> = ({ onSubmit, initialValues, formikRef }) => {
  return (
    <StyledFormWrapper>
      <Formik<LeaseFormModel>
        validationSchema={AddLeaseYupSchema}
        onSubmit={values => onSubmit(values)}
        initialValues={getDefaultFormLease()}
        innerRef={formikRef}
      >
        {formikProps => (
          <>
            <Prompt
              when={formikProps.dirty}
              message="You have made changes on this form. Do you wish to leave without saving?"
            />
            <>
              <LeaseDetailSubForm formikProps={formikProps}></LeaseDetailSubForm>
              <LeasePropertySelector formikProps={formikProps} />
              <AdministrationSubForm formikProps={formikProps}></AdministrationSubForm>
              <ConsultationSubForm formikProps={formikProps}></ConsultationSubForm>
              <ReferenceSubForm />
            </>
          </>
        )}
      </Formik>
    </StyledFormWrapper>
  );
};

export default UpdateLeaseForm;

const StyledFormWrapper = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
`;
