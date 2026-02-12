import axios, { AxiosError } from 'axios';
import { FormikHelpers } from 'formik/dist/types';
import { useHistory, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';

import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';

import { IUpdateAgreementFormProps } from '../common/UpdateAgreementForm';
import { AgreementFormModel } from '../models/AgreementFormModel';

export interface IAddAcquisitionAgreementContainerProps {
  fileId: number;
  fileType: string;
  isNew?: boolean;
  View: React.FC<IUpdateAgreementFormProps>;
  onSuccess: () => void;
  onCreateAgreement: (fileId: number, data: any) => Promise<any>;
  isCreatingAgreement: boolean;
}

const AddAgreementContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddAcquisitionAgreementContainerProps>
> = ({ fileId, fileType, isNew, View, onSuccess, onCreateAgreement, isCreatingAgreement }) => {
  const history = useHistory();
  const location = useLocation();
  const { setModalContent, setDisplayModal } = useModalContext();
  const initialValues = new AgreementFormModel(fileId);

  const backUrl = location.pathname.split('/add')[0];

  const onCreateError = (e: AxiosError<IApiError>) => {
    if (e?.response?.status === 409) {
      setModalContent({
        variant: 'error',
        title: 'Error',
        message: 'You can only have one offer with status of "Accepted".',
        okButtonText: 'Close',
        handleOk: () => setDisplayModal(false),
      });
      setDisplayModal(true);
    } else {
      if (e?.response?.status === 400) {
        toast.error(e?.response.data.error);
      } else {
        toast.error('Unable to save. Please try again.');
      }
    }
  };

  const handleSubmit = async (
    values: AgreementFormModel,
    formikHelpers: FormikHelpers<AgreementFormModel>,
  ) => {
    try {
      const agreementSaved = await onCreateAgreement(fileId, values.toApi());
      if (agreementSaved) {
        onSuccess();
        history.push(backUrl);
      }
    } catch (e) {
      if (axios.isAxiosError(e)) {
        const axiosError = e as AxiosError<IApiError>;
        onCreateError && onCreateError(axiosError);
      }
    } finally {
      formikHelpers.setSubmitting(false);
    }
  };

  return (
    initialValues && (
      <View
        initialValues={initialValues}
        fileType={fileType}
        isNew={isNew}
        isLoading={isCreatingAgreement}
        onSubmit={handleSubmit}
        onCancel={() => history.push(backUrl)}
      />
    )
  );
};

export default AddAgreementContainer;
