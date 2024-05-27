import axios, { AxiosError } from 'axios';
import { Formik } from 'formik';
import styled from 'styled-components';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { DispositionSaleFormModel } from '@/features/mapSideBar/disposition/models/DispositionSaleFormModel';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_DispositionFileSale } from '@/models/api/generated/ApiGen_Concepts_DispositionFileSale';

import DispositionSaleForm from '../form/DispositionSaleForm';
import { DispositionSaleFormYupSchema } from '../form/DispositionSaleFormYupSchema';

export interface IUpdateDispositionSaleViewProps {
  initialValues: DispositionSaleFormModel;
  loading: boolean;
  onSave: (
    sale: ApiGen_Concepts_DispositionFileSale,
  ) => Promise<ApiGen_Concepts_DispositionFileSale | undefined>;
  onCancel: () => void;
  onSuccess: () => void;
  onError: (e: AxiosError<IApiError>) => void;
}

const UpdateDispositionSaleView: React.FC<IUpdateDispositionSaleViewProps> = ({
  initialValues,
  loading,
  onSave,
  onCancel,
  onSuccess,
  onError,
}) => {
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
    <StyledFormWrapper>
      <Formik<DispositionSaleFormModel>
        enableReinitialize
        validationSchema={DispositionSaleFormYupSchema}
        initialValues={initialValues}
        onSubmit={async (values: DispositionSaleFormModel, formikHelpers) => {
          try {
            const sale = await onSave(values.toApi());
            if (sale) {
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
                show={formikProps.isSubmitting || loading}
                parentScreen={true}
              ></LoadingBackdrop>
              <StyledContent>
                <DispositionSaleForm dispositionSaleId={initialValues.id} />
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
  );
};

export default UpdateDispositionSaleView;

const StyledContent = styled.div`
  background-color: ${props => props.theme.css.highlightBackgroundColor};
`;

const StyledFooter = styled.div`
  margin-right: 1rem;
  padding-bottom: 1rem;
  z-index: 0;
`;
