import { Formik, FormikHelpers, FormikProps } from 'formik';
import styled from 'styled-components';

import { Section } from '@/components/common/Section/Section';
import PropertiesListContainer from '@/features/mapSideBar/shared/update/properties/PropertiesListContainer';

import { LeaseFormModel } from '../models';
import { AddLeaseTeamSubForm } from './AddLeaseTeamSubform';
import { AddLeaseTeamYupSchema } from './AddLeaseTeamYupSchema';
import { AddLeaseYupSchema } from './AddLeaseYupSchema';
import AdministrationSubForm from './AdministrationSubForm';
import FeeDeterminationSubForm from './FeeDeterminationSubForm';
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
            <LeaseDetailSubForm formikProps={formikProps} />
            <Section header="Properties to include in this file:">
              <div className="py-2">
                Select one or more properties that you want to include in this lease/licence file.
                You can choose a location from the map, or search by other criteria.
              </div>
              <PropertiesListContainer
                properties={formikProps.values.properties}
                verifyCanRemove={(_, callback) => callback()}
                showArea
              />
            </Section>
            <AdministrationSubForm formikProps={formikProps} />
            <Section header="Lease & Licence Team">
              <AddLeaseTeamSubForm />
              {formikProps.errors?.team && typeof formikProps.errors?.team === 'string' && (
                <div className="invalid-feedback" data-testid="team-profile-dup-error">
                  {formikProps.errors.team.toString()}
                </div>
              )}
            </Section>
            <FeeDeterminationSubForm formikProps={formikProps} />
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
