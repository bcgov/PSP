import { Formik, FormikHelpers, FormikProps } from 'formik';
import styled from 'styled-components';

import { IMapProperty } from '@/components/propertySelector/models';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';

import { FormLeaseProperty, getDefaultFormLease, LeaseFormModel } from '../models';
import LeasePropertySelector from '../shared/propertyPicker/LeasePropertySelector';
import { AddLeaseYupSchema } from './AddLeaseYupSchema';
import AdministrationSubForm from './AdministrationSubForm';
import ConsultationSubForm, { getConsultations } from './ConsultationSubForm';
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

  const { getByType } = useLookupCodeHelpers();
  const consultationTypes = getByType(API.CONSULTATION_TYPES);

  // support creating a new disposition file from the map popup
  if (propertyInfo) {
    defaultFormLease.properties = [];
    defaultFormLease.properties.push(FormLeaseProperty.fromMapProperty(propertyInfo));
    // auto-select file region based upon the location of the property
    defaultFormLease.regionId = propertyInfo.region ? propertyInfo.region.toString() : '';
  }

  const apiFormLease = LeaseFormModel.toApi(defaultFormLease);
  apiFormLease.consultations = getConsultations(apiFormLease, consultationTypes);

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
        validationSchema={AddLeaseYupSchema}
        onSubmit={handleSubmit}
      >
        {formikProps => (
          <>
            <LeaseDetailSubForm formikProps={formikProps}></LeaseDetailSubForm>
            <LeasePropertySelector formikProps={formikProps} />
            <AdministrationSubForm formikProps={formikProps}></AdministrationSubForm>
            <ConsultationSubForm formikProps={formikProps}></ConsultationSubForm>
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
