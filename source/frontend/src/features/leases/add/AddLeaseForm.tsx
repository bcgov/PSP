import { Formik, FormikHelpers, FormikProps } from 'formik';
import styled from 'styled-components';

import { Section } from '@/components/common/Section/Section';

import { LeaseFormModel } from '../models';
import LeasePropertySelector from '../shared/propertyPicker/LeasePropertySelector';
import { AddLeaseTeamSubForm } from './AddLeaseTeamSubform';
import { AddLeaseTeamYupSchema } from './AddLeaseTeamYupSchema';
import { AddLeaseYupSchema } from './AddLeaseYupSchema';
import AdministrationSubForm from './AdministrationSubForm';
import LeaseDetailSubForm from './LeaseDetailSubForm';

export interface IAddLeaseFormProps {
  /** Submission handler */
  onSubmit: (
    values: LeaseFormModel,
    formikHelpers: FormikHelpers<LeaseFormModel>,
  ) => void | Promise<any>;
  formikRef: React.Ref<FormikProps<LeaseFormModel>>;
  /** Initial values of the form */
  initialValues: LeaseFormModel;
}

const AddLeaseForm: React.FunctionComponent<React.PropsWithChildren<IAddLeaseFormProps>> = ({
  onSubmit,
  formikRef,
  initialValues,
}) => {
  return (
    <StyledFormWrapper>
      <Formik<LeaseFormModel>
        enableReinitialize
        innerRef={formikRef}
        initialValues={initialValues}
        validationSchema={AddLeaseYupSchema.concat(AddLeaseTeamYupSchema)}
        onSubmit={onSubmit}
      >
        {formikProps => (
          <>
            <LeaseDetailSubForm formikProps={formikProps}></LeaseDetailSubForm>
            <LeasePropertySelector formikProps={formikProps} />
            <AdministrationSubForm formikProps={formikProps}></AdministrationSubForm>
            <Section header="Lease & Licence Team">
              <AddLeaseTeamSubForm />
              {formikProps.errors?.team && typeof formikProps.errors?.team === 'string' && (
                <div className="invalid-feedback" data-testid="team-profile-dup-error">
                  {formikProps.errors.team.toString()}
                </div>
              )}
            </Section>
          </>
        )}
      </Formik>
    </StyledFormWrapper>
  );
};

export default AddLeaseForm;

const StyledFormWrapper = styled.div`
  background-color: ${props => props.theme.css.highlightBackgroundColor};
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
`;
