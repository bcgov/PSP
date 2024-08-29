import axios, { AxiosError } from 'axios';
import { FormikHelpers } from 'formik';
import { useHistory, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';

import { useConsultationProvider } from '@/hooks/repositories/useConsultationProvider';
import { IApiError } from '@/interfaces/IApiError';

import { LeasePageNames } from '../../../LeaseContainer';
import { IConsultationEditFormProps } from './ConsultationEditForm';
import { ConsultationFormModel } from './models';

export interface IConsultationAddProps {
  leaseId: number;
  onSuccess: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IConsultationEditFormProps>>;
}

const ConsultationAddContainer: React.FunctionComponent<
  React.PropsWithChildren<IConsultationAddProps>
> = ({ onSuccess, View, leaseId }) => {
  const history = useHistory();
  const location = useLocation();

  const backUrl = location.pathname.split(`/${LeasePageNames.CONSULTATIONS}/add`)[0];

  const {
    addLeaseConsultation: { execute: addConsultation, loading: addConsultationLoading },
  } = useConsultationProvider();

  const onCreateError = (e: AxiosError<IApiError>) => {
    if (e?.response?.status === 400) {
      toast.error(e?.response.data.error);
    } else {
      toast.error('Unable to save. Please try again.');
    }
  };

  const handleSubmit = async (
    values: ConsultationFormModel,
    formikHelpers: FormikHelpers<ConsultationFormModel>,
  ) => {
    try {
      const consultationSaved = await addConsultation(leaseId, values.toApi());
      if (consultationSaved) {
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
    <View
      initialValues={new ConsultationFormModel(leaseId)}
      isLoading={addConsultationLoading}
      onSubmit={handleSubmit}
      onCancel={() => history.push(backUrl)}
    />
  );
};

export default ConsultationAddContainer;
