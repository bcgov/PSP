import { Formik, FormikHelpers, FormikProps } from 'formik';
import styled from 'styled-components';

import { AddLeaseYupSchema } from '@/features/leases/add/AddLeaseYupSchema';
import AdministrationSubForm from '@/features/leases/add/AdministrationSubForm';
import FeeDeterminationSubForm from '@/features/leases/add/FeeDeterminationSubForm';
import LeaseDetailSubForm from '@/features/leases/add/LeaseDetailSubForm';
import RenewalSubForm from '@/features/leases/add/RenewalSubForm';
import { getDefaultFormLease, LeaseFormModel } from '@/features/leases/models';
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
        title: 'Acknowledgement',
        variant: 'warning',
        message: `Selecting the Duplicate file status will hide the file within the Property Information, PIMS Files tab. The file will remain searchable in the Manage Lease/Licence Files advanced search.
        Please ensure that all related documents and notes are moved to the active file.

        Do you want to proceed?`,
        okButtonText: 'Yes',
        cancelButtonText: 'No',
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
            <LeaseDetailSubForm formikProps={formikProps} />
            <RenewalSubForm formikProps={formikProps} />
            <AdministrationSubForm formikProps={formikProps} />
            <FeeDeterminationSubForm formikProps={formikProps} />
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
