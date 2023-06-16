import { FormikProps } from 'formik';
import React, { useEffect, useState } from 'react';

import * as API from '@/constants/API';
import { useAgreementProvider } from '@/hooks/repositories/useAgreementProvider';
import { useLookupCodeHelpers } from '@/hooks/useLookupCodeHelpers';

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
  }, [acquisitionFileId, getAgreements]);

  const saveAgreements = async (apiAcquisitionFile: AgreementsFormModel) => {
    const result = await updateAcquisitionAgreements(acquisitionFileId, apiAcquisitionFile.toApi());
    if (result !== undefined) {
      onSuccess();
    }
    return result;
  };

  return (
    <View
      isLoading={isUpdating || loadingAgeements}
      formikRef={formikRef}
      initialValues={initialValues}
      agreementTypes={sectionTypes}
      onSave={saveAgreements}
    />
  );
};
