import { AxiosError } from 'axios';
import * as API from 'constants/API';
import { FormikProps } from 'formik';
import { useAgreementProvider } from 'hooks/repositories/useAgreemetProvider';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import { IApiError } from 'interfaces/IApiError';
import { Api_Agreement } from 'models/api/Agreement';
import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';

import { AgreementsFormModel } from './models';
import { IUpdateAgreementsFormProps } from './UpdateAgreementsForm';

export interface IUpdateAgreementsContainerProps {
  formikRef: React.Ref<FormikProps<AgreementsFormModel>>;
  acquisitionFileId: number;
  onSuccess: () => void;
  View: React.FC<IUpdateAgreementsFormProps>;
}

export const UpdateAgreementsContainer: React.FC<IUpdateAgreementsContainerProps> = ({
  formikRef,
  acquisitionFileId,
  onSuccess,
  View,
}) => {
  const { getByType } = useLookupCodeHelpers();
  const {
    updateAcquisitionAgreements: { execute: updateAcquisitionAgreements, loading: isUpdating },
    getAcquisitionAgreements: { execute: getAgreements, loading: loadingAgeements },
  } = useAgreementProvider();

  const sectionTypes = getByType(API.AGREEMENT_TYPES);
  const [initialValues, setInitialValues] = useState<AgreementsFormModel>(
    new AgreementsFormModel(acquisitionFileId),
  );

  useEffect(() => {
    const fetchData = async () => {
      const agreements = (await getAgreements(acquisitionFileId)) || [];
      setInitialValues(AgreementsFormModel.fromApi(acquisitionFileId, agreements));
    };
    fetchData();
  }, [acquisitionFileId]);

  const saveChecklist = async (apiAcquisitionFile: AgreementsFormModel) => {
    return updateAcquisitionAgreements(acquisitionFileId, apiAcquisitionFile.toApi());
  };

  const onUpdateSuccess = async (apiAcquisitionFile: Api_Agreement[]) => {
    onSuccess && onSuccess();
  };

  // generic error handler.
  const onError = (e: AxiosError<IApiError>) => {
    if (e?.response?.status === 400) {
      toast.error(e?.response.data.error);
    } else {
      toast.error('Unable to save. Please try again.');
    }
  };

  return (
    <View
      isLoading={isUpdating || loadingAgeements}
      formikRef={formikRef}
      initialValues={initialValues}
      agreementTypes={sectionTypes}
      onSave={saveChecklist}
      onSuccess={onUpdateSuccess}
      onError={onError}
    />
  );
};
