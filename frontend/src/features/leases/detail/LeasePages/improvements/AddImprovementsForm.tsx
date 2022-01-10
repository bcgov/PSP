import AddLeaseFormButtons from 'features/leases/add/AddLeaseFormButtons';
import { Formik, FormikProps } from 'formik';
import { defaultFormLease, IFormLease } from 'interfaces';
import * as React from 'react';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import { Improvements } from './Improvements';
export interface IAddImprovementsFormProps {
  onCancel: () => void;
  onSubmit: (lease: IFormLease) => Promise<void>;
  initialValues?: IFormLease;
  formikRef: React.Ref<FormikProps<IFormLease>>;
}

export const AddImprovementsForm: React.FunctionComponent<IAddImprovementsFormProps> = ({
  onCancel,
  onSubmit,
  initialValues,
  formikRef,
}) => {
  return (
    <>
      <Formik
        onSubmit={values => onSubmit(values)}
        innerRef={formikRef}
        enableReinitialize
        initialValues={{ ...defaultFormLease, ...initialValues }}
      >
        {formikProps => (
          <>
            <Prompt
              when={formikProps.dirty}
              message="You have made changes on this form. Do you wish to leave without saving?"
            />
            <StyledFormBody>
              <Improvements disabled={false} />
              <AddLeaseFormButtons formikProps={formikProps} onCancel={onCancel} />
            </StyledFormBody>
          </>
        )}
      </Formik>
    </>
  );
};

const StyledFormBody = styled.form`
  margin-left: 1rem;
  .form-group {
    flex-direction: column;
    input {
      border-left: 0;
      width: 70%;
    }
    textarea {
      width: 85%;
      resize: none;
    }
  }
  .improvements .formgrid {
    row-gap: 0.5rem;
    grid-template-columns: [controls] 1fr;
    & > .form-label {
      grid-column: controls;
      font-family: 'BcSans-Bold';
    }
    & > .input {
      border-left: 0;
    }
    .form-control {
      font-family: 'BcSans';
    }
    h5 {
      padding-top: 0;
    }
  }
`;

export default AddImprovementsForm;
