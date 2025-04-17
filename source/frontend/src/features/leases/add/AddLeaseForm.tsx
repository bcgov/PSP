import { Formik, FormikHelpers, FormikProps } from 'formik';
import styled from 'styled-components';

import { Section } from '@/components/common/Section/Section';
import { IMapProperty } from '@/components/propertySelector/models';

import { FormLeaseProperty, getDefaultFormLease, LeaseFormModel } from '../models';
import LeasePropertySelector from '../shared/propertyPicker/LeasePropertySelector';
import { AddLeaseTeamSubForm } from './AddLeaseTeamSubform';
import { AddLeaseTeamYupSchema } from './AddLeaseTeamYupSchema';
import { AddLeaseYupSchema } from './AddLeaseYupSchema';
import AdministrationSubForm from './AdministrationSubForm';
import LeaseDetailSubForm from './LeaseDetailSubForm';

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

  // support creating a new disposition file from the map popup
  if (propertyInfo) {
    defaultFormLease.properties = [];
    defaultFormLease.properties.push(FormLeaseProperty.fromMapProperty(propertyInfo));
    // auto-select file region based upon the location of the property
    defaultFormLease.regionId = propertyInfo.region ? propertyInfo.region.toString() : '';
  }

  const apiFormLease = LeaseFormModel.toApi(defaultFormLease);

  const handleSubmit = async (
    values: LeaseFormModel,
    formikHelpers: FormikHelpers<LeaseFormModel>,
  ) => {
    return await onSubmit(values, formikHelpers);
  };

  return (
    <StyledFormWrapper>
      <Formik<LeaseFormModel>
        enableReinitialize
        innerRef={formikRef}
        initialValues={LeaseFormModel.fromApi(apiFormLease)}
        validationSchema={AddLeaseYupSchema.concat(AddLeaseTeamYupSchema)}
        onSubmit={(values: LeaseFormModel, formikHelpers: FormikHelpers<LeaseFormModel>) => {
          handleSubmit(values, formikHelpers);
        }}
      >
        {formikProps => (
          <>
            <LeaseDetailSubForm formikProps={formikProps}></LeaseDetailSubForm>
            <LeasePropertySelector formikProps={formikProps} />
            <AdministrationSubForm formikProps={formikProps}></AdministrationSubForm>
            <Section header="Lease Team">
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
