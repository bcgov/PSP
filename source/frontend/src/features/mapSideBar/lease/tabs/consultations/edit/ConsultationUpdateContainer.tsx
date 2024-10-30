import axios, { AxiosError } from 'axios';
import { FormikHelpers } from 'formik';
import { useCallback, useEffect, useState } from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';

import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import { useConsultationProvider } from '@/hooks/repositories/useConsultationProvider';
import { IApiError } from '@/interfaces/IApiError';
import { exists, isValidId } from '@/utils';

import { LeasePageNames } from '../../../LeaseContainer';
import { IConsultationEditFormProps } from './ConsultationEditForm';
import { ConsultationFormModel } from './models';

export interface IConsultationUpdateContainerProps {
  leaseId: number;
  consultationId: number;
  onSuccess: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IConsultationEditFormProps>>;
}

const ConsultationUpdateContainer: React.FunctionComponent<
  React.PropsWithChildren<IConsultationUpdateContainerProps>
> = ({ leaseId, consultationId, onSuccess, View }) => {
  const history = useHistory();
  const location = useLocation();
  const [initialValues, setInitialValues] = useState<ConsultationFormModel>(
    new ConsultationFormModel(leaseId),
  );

  const backUrl = location.pathname.split(
    `/${LeasePageNames.CONSULTATIONS}/${consultationId}/edit`,
  )[0];

  const {
    getLeaseConsultationById: { execute: getConsultation, loading: getConsultationLoading },
    updateLeaseConsultation: { execute: updateConsultation, loading: updateConsultationLoading },
  } = useConsultationProvider();

  const {
    getPersonDetail: { execute: getPerson, loading: getPersonLoading },
  } = usePersonRepository();

  const {
    getOrganizationDetail: { execute: getOrganization, loading: getOrganizationLoading },
  } = useOrganizationRepository();

  const fetchData = useCallback(async () => {
    const consultation = await getConsultation(leaseId, consultationId);

    if (exists(consultation)) {
      let person = null;
      let organization = null;
      if (isValidId(consultation.personId)) {
        person = await getPerson(consultation.personId);
      }
      if (isValidId(consultation.organizationId)) {
        organization = await getOrganization(consultation.organizationId);
      }

      const consultationFormModel = ConsultationFormModel.fromApi(
        consultation,
        person,
        organization,
      );
      setInitialValues(consultationFormModel);
    }
  }, [leaseId, consultationId, getPerson, getOrganization, getConsultation]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

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
      const consultationSaved = await updateConsultation(leaseId, values.id, values.toApi());
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
      initialValues={initialValues}
      isLoading={
        updateConsultationLoading ||
        getConsultationLoading ||
        getPersonLoading ||
        getOrganizationLoading
      }
      onSubmit={handleSubmit}
      onCancel={() => history.push(backUrl)}
    />
  );
};

export default ConsultationUpdateContainer;
