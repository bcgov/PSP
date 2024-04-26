import axios, { AxiosError } from 'axios';
import { FormikHelpers } from 'formik/dist/types';
import { useHistory, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';

import { useAgreementProvider } from '@/hooks/repositories/useAgreementProvider';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';

import { IUpdateAcquisitionAgreementViewProps } from '../common/UpdateAcquisitionAgreementForm';
import { AcquisitionAgreementFormModel } from '../models/AcquisitionAgreementFormModel';

export interface IAddAcquisitionAgreementContainerProps {
  acquisitionFileId: number;
  View: React.FC<IUpdateAcquisitionAgreementViewProps>;
  onSuccess: () => void;
}

const AddAcquisitionAgreementContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddAcquisitionAgreementContainerProps>
> = ({ acquisitionFileId, View, onSuccess }) => {
  const history = useHistory();
  const location = useLocation();
  const { setModalContent, setDisplayModal } = useModalContext();
  const initialValues = new AcquisitionAgreementFormModel(acquisitionFileId);

  const backUrl = location.pathname.split('/add')[0];

  const {
    addAcquisitionAgreement: { execute: postAcquisitionAgreement, loading },
  } = useAgreementProvider();

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
    values: AcquisitionAgreementFormModel,
    formikHelpers: FormikHelpers<AcquisitionAgreementFormModel>,
  ) => {
    try {
      const agreementSaved = await postAcquisitionAgreement(acquisitionFileId, values.toApi());
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
        isLoading={loading}
        onSubmit={handleSubmit}
        onCancel={() => history.push(backUrl)}
      ></View>
    )
  );
};

export default AddAcquisitionAgreementContainer;
