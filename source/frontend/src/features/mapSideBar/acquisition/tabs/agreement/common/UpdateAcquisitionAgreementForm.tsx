import { Formik, FormikHelpers } from 'formik';
import styled from 'styled-components';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';

import AcquisitionAgreementForm from '../form/AcquisitionAgreementForm';
import { AcquisitionAgreementFormYupSchema } from '../form/AcquisitionAgreementFormYupSchema';
import { AcquisitionAgreementFormModel } from '../models/AcquisitionAgreementFormModel';

export interface IUpdateAcquisitionAgreementFormProps {
  isLoading: boolean;
  initialValues: AcquisitionAgreementFormModel | null;
  onSubmit: (
    values: AcquisitionAgreementFormModel,
    formikHelpers: FormikHelpers<AcquisitionAgreementFormModel>,
  ) => Promise<void>;
  onCancel: () => void;
}

const UpdateAcquisitionAgreementForm: React.FunctionComponent<
  React.PropsWithChildren<IUpdateAcquisitionAgreementFormProps>
> = ({ isLoading, initialValues, onSubmit, onCancel }) => {
  const { setModalContent, setDisplayModal } = useModalContext();

  const cancelFunc = (resetForm: () => void, dirty: boolean) => {
    if (!dirty) {
      resetForm();
      onCancel();
    } else {
      setModalContent({
        ...getCancelModalProps(),
        handleOk: () => {
          resetForm();
          setDisplayModal(false);
          onCancel();
        },
      });
      setDisplayModal(true);
    }
  };

  return (
    initialValues && (
      <StyledFormWrapper>
        <Formik<AcquisitionAgreementFormModel>
          enableReinitialize
          initialValues={initialValues}
          validationSchema={AcquisitionAgreementFormYupSchema}
          onSubmit={onSubmit}
        >
          {formikProps => {
            return (
              <>
                <LoadingBackdrop
                  show={formikProps.isSubmitting || isLoading}
                  parentScreen={true}
                ></LoadingBackdrop>
                <StyledContent>
                  <AcquisitionAgreementForm formikProps={formikProps}></AcquisitionAgreementForm>
                </StyledContent>
                <StyledFooter>
                  <SidebarFooter
                    onSave={() => formikProps.submitForm()}
                    isOkDisabled={formikProps.isSubmitting || !formikProps.dirty}
                    onCancel={() => cancelFunc(formikProps.resetForm, formikProps.dirty)}
                    displayRequiredFieldError={
                      formikProps.isValid === false && !!formikProps.submitCount
                    }
                  />
                </StyledFooter>
              </>
            );
          }}
        </Formik>
      </StyledFormWrapper>
    )
  );
};

export default UpdateAcquisitionAgreementForm;

const StyledContent = styled.div`
  background-color: ${props => props.theme.css.highlightBackgroundColor};
`;

const StyledFooter = styled.div`
  margin-right: 1rem;
  padding-bottom: 1rem;
  z-index: 0;
`;
