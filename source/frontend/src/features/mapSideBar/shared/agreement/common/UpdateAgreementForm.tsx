import { Formik, FormikHelpers } from 'formik';
import styled from 'styled-components';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';

import AgreementForm from '../form/AgreementForm';
import { AgreementFormYupSchema } from '../form/AgreementFormYupSchema';
import { AgreementFormModel } from '../models/AgreementFormModel';

export interface IUpdateAgreementFormProps {
  isLoading: boolean;
  fileType: string;
  isNew?: boolean;
  initialValues: AgreementFormModel | null;
  onSubmit: (
    values: AgreementFormModel,
    formikHelpers: FormikHelpers<AgreementFormModel>,
  ) => Promise<void>;
  onCancel: () => void;
}

const UpdateAgreementForm: React.FunctionComponent<
  React.PropsWithChildren<IUpdateAgreementFormProps>
> = ({ isLoading, fileType, isNew, initialValues, onSubmit, onCancel }) => {
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
        <Formik<AgreementFormModel>
          enableReinitialize
          initialValues={initialValues}
          validationSchema={AgreementFormYupSchema}
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
                  <AgreementForm
                    formikProps={formikProps}
                    fileType={fileType}
                    isNew={isNew}
                  ></AgreementForm>
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

export default UpdateAgreementForm;

const StyledContent = styled.div`
  background-color: ${props => props.theme.css.highlightBackgroundColor};
`;

const StyledFooter = styled.div`
  margin-right: 1rem;
  padding-bottom: 1rem;
  z-index: 0;
`;
