import axios, { AxiosError } from 'axios';
import { Formik } from 'formik';
import styled from 'styled-components';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';

import AcquisitionAgreementForm from '../form/AcquisitionAgreementForm';
import { AcquisitionAgreementFormYupSchema } from '../form/AcquisitionAgreementFormYupSchema';
import { AcquisitionAgreementFormModel } from '../models/AcquisitionAgreementFormModel';

export interface IUpdateAcquisitionAgreementViewProps {
  isLoading: boolean;
  initialValues: AcquisitionAgreementFormModel | null;
  onSave: (agreement: ApiGen_Concepts_Agreement) => Promise<ApiGen_Concepts_Agreement | undefined>;
  onSuccess: () => void;
  onError: (e: AxiosError<IApiError>) => void;
  onCancel: () => void;
}

const UpdateAcquisitionAgreementView: React.FunctionComponent<
  React.PropsWithChildren<IUpdateAcquisitionAgreementViewProps>
> = ({ isLoading, initialValues, onSave, onSuccess, onError, onCancel }) => {
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
          onSubmit={async (values, formikHelpers) => {
            try {
              const agreementSaved = await onSave(values.toApi());
              if (agreementSaved) {
                onSuccess();
              }
            } catch (e) {
              if (axios.isAxiosError(e)) {
                const axiosError = e as AxiosError<IApiError>;
                onError && onError(axiosError);
              }
            } finally {
              formikHelpers.setSubmitting(false);
            }
          }}
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

export default UpdateAcquisitionAgreementView;

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-bottom: 1rem;
`;

const StyledContent = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

const StyledFooter = styled.div`
  margin-right: 1rem;
  padding-bottom: 1rem;
  z-index: 0;
`;
