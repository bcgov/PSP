import { Formik, FormikHelpers, FormikProps } from 'formik';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import { AddLeaseYupSchema } from '@/features/leases/add/AddLeaseYupSchema';
import AdministrationSubForm from '@/features/leases/add/AdministrationSubForm';
import FeeDeterminationSubForm from '@/features/leases/add/FeeDeterminationSubForm';
import LeaseDetailSubForm from '@/features/leases/add/LeaseDetailSubForm';
import RenewalSubForm from '@/features/leases/add/RenewalSubForm';
import { getDefaultFormLease, LeaseFormModel } from '@/features/leases/models';
import { LeasePropertySelector } from '@/features/leases/shared/propertyPicker/LeasePropertySelector';
import { useModalContext } from '@/hooks/useModalContext';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';

export interface IUpdateLeaseFormProps {
  onSubmit: (lease: LeaseFormModel) => Promise<void>;
  initialValues?: LeaseFormModel;
  formikRef: React.Ref<FormikProps<LeaseFormModel>>;
}

export const UpdateLeaseForm: React.FunctionComponent<IUpdateLeaseFormProps> = ({
  onSubmit,
  initialValues,
  formikRef,
}) => {
  const { setModalContent, setDisplayModal } = useModalContext();

  const handleSubmit = async (
    values: LeaseFormModel,
    formikHelpers: FormikHelpers<LeaseFormModel>,
  ): Promise<void> => {
    if (
      initialValues.statusTypeCode !== ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE &&
      values.statusTypeCode === ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE
    ) {
      setModalContent({
        variant: 'warning',
        okButtonText: 'Yes',
        cancelButtonText: 'No',
        message: `You've marked this status file as a duplicate. If you save it, you'll still see it in the management table.

        Please ensure that all related documents and notes are moved to the main file.

        Do you want to acknowledge and proceed?`,
        title: 'Warning',
        handleCancel: () => {
          formikHelpers.setFieldValue('statusTypeCode', initialValues.statusTypeCode);
          setDisplayModal(false);
        },
        handleOk: () => {
          onSubmit(values);
          setDisplayModal(false);
        },
      });
      setDisplayModal(true);
    } else {
      await onSubmit(values);
    }
  };

  return (
    <StyledFormWrapper>
      <Formik<LeaseFormModel>
        validationSchema={AddLeaseYupSchema}
        onSubmit={handleSubmit}
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
