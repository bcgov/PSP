import { Formik, FormikProps } from 'formik';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import { AddLeaseYupSchema } from '@/features/leases/add/AddLeaseYupSchema';
import AdministrationSubForm from '@/features/leases/add/AdministrationSubForm';
import FeeDeterminationSubForm from '@/features/leases/add/FeeDeterminationSubForm';
import LeaseDetailSubForm from '@/features/leases/add/LeaseDetailSubForm';
import RenewalSubForm from '@/features/leases/add/RenewalSubForm';
import { getDefaultFormLease, LeaseFormModel } from '@/features/leases/models';
import { LeasePropertySelector } from '@/features/leases/shared/propertyPicker/LeasePropertySelector';

export interface IUpdateLeaseFormProps {
  onSubmit: (lease: LeaseFormModel) => Promise<void>;
  initialValues?: LeaseFormModel;
  formikRef: React.Ref<FormikProps<LeaseFormModel>>;
}

export const UpdateLeaseForm: React.FunctionComponent<IUpdateLeaseFormProps> = ({
  onSubmit,
  formikRef,
}) => {
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
              <RenewalSubForm formikProps={formikProps} />
              <LeasePropertySelector formikProps={formikProps} />
              <AdministrationSubForm formikProps={formikProps}></AdministrationSubForm>
              <FeeDeterminationSubForm formikProps={formikProps}></FeeDeterminationSubForm>
            </>
          </>
        )}
      </Formik>
    </StyledFormWrapper>
  );
};

export default UpdateLeaseForm;

const StyledFormWrapper = styled.div`
  background-color: ${props => props.theme.css.highlightBackgroundColor};
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
`;
