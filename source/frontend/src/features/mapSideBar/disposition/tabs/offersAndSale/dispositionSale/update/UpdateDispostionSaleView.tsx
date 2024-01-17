import axios, { AxiosError } from 'axios';
import { Formik } from 'formik';
import styled from 'styled-components';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { DispositionSaleFormModel } from '@/features/mapSideBar/disposition/models/DispositionSaleFormModel';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { Api_DispositionFileSale } from '@/models/api/DispositionFile';

import DispositionSaleForm from '../form/DispositionSaleForm';
import { DispositionSaleFormYupSchema } from '../form/DispositionSaleFormYupSchema';

export interface IUpdateDispositionSaleViewProps {
  initialValues: DispositionSaleFormModel;
  loading: boolean;
  onSave: (sale: Api_DispositionFileSale) => Promise<Api_DispositionFileSale | undefined>;
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
            if (sale && sale.id) {
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
                <DispositionSaleForm dispostionSaleId={initialValues.id} />
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
