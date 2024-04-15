import { Formik, FormikHelpers, FormikProps } from 'formik';
import styled from 'styled-components';

import { IMapProperty } from '@/components/propertySelector/models';

import { FormLeaseProperty, getDefaultFormLease, LeaseFormModel } from '../models';
import LeasePropertySelector from '../shared/propertyPicker/LeasePropertySelector';
import { AddLeaseYupSchema } from './AddLeaseYupSchema';
import AdministrationSubForm from './AdministrationSubForm';
import ConsultationSubForm from './ConsultationSubForm';
import LeaseDetailSubForm from './LeaseDetailSubForm';
import DocumentationSubForm from './ReferenceSubForm';

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
  if (propertyInfo) {
    defaultFormLease.properties = [];
    defaultFormLease.properties.push(FormLeaseProperty.fromMapProperty(propertyInfo));
    defaultFormLease.regionId = propertyInfo.region ? propertyInfo.region.toString() : '';
  }

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
        initialValues={defaultFormLease}
        validationSchema={AddLeaseYupSchema}
        onSubmit={handleSubmit}
      >
        {formikProps => (
          <>
            <>
              <LeaseDetailSubForm formikProps={formikProps}></LeaseDetailSubForm>
              <LeasePropertySelector formikProps={formikProps} />
              <AdministrationSubForm formikProps={formikProps}></AdministrationSubForm>
              <ConsultationSubForm formikProps={formikProps}></ConsultationSubForm>
              <DocumentationSubForm />
            </>
          </>
        )}
      </Formik>
    </StyledFormWrapper>
  );
};

export default AddLeaseForm;

const StyledFormWrapper = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
`;
