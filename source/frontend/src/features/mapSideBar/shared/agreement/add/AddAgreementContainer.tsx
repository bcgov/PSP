import axios, { AxiosError } from 'axios';
import { FormikHelpers } from 'formik/dist/types';
import { useHistory, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';

import { useAgreementProvider } from '@/hooks/repositories/useAgreementProvider';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';

import { IUpdateAgreementFormProps } from '../common/UpdateAgreementForm';
import { AgreementFormModel } from '../models/AgreementFormModel';

export interface IAddAcquisitionAgreementContainerProps {
  acquisitionFileId: number;
  fileType: string;
  isNew?: boolean;
  View: React.FC<IUpdateAgreementFormProps>;
  onSuccess: () => void;
}

const AddAgreementContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddAcquisitionAgreementContainerProps>
> = ({ acquisitionFileId, fileType, isNew, View, onSuccess }) => {
  const history = useHistory();
  const location = useLocation();
  const { setModalContent, setDisplayModal } = useModalContext();
  const initialValues = new AgreementFormModel(acquisitionFileId);

  const backUrl = location.pathname.split('/add')[0];

  const {
    addAcquisitionAgreement: {
      execute: postAcquisitionAgreement,
      loading: loadingAcquisitionAgreement,
    },
    addDispositionAgreement: {
      execute: postDispositionAgreement,
      loading: loadingDispositionAgreement,
    },
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
    values: AgreementFormModel,
    formikHelpers: FormikHelpers<AgreementFormModel>,
  ) => {
    try {
      let agreementSaved;
      if (fileType === 'acquisition') {
        agreementSaved = await postAcquisitionAgreement(acquisitionFileId, values.toApi());
      } else if (fileType === 'disposition') {
        agreementSaved = await postDispositionAgreement(acquisitionFileId, values.toApi());
      }
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
        isLoading={loadingAcquisitionAgreement || loadingDispositionAgreement}
        onSubmit={handleSubmit}
        onCancel={() => history.push(backUrl)}
      />
    )
  );
};

export default AddAgreementContainer;
