import { FormikProps } from 'formik';
import { useCompensationRequisitionRepository } from 'hooks/repositories/useRequisitionCompensationRepository';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_Compensation } from 'models/api/Compensation';

import { CompensationRequisitionFormModel } from '../models';
import { CompensationRequisitionFormProps } from './UpdateCompensationRequisitionForm';

export interface UpdateCompensationRequisitionContainerProps {
  compensation: Api_Compensation;
  acquisitionFile: Api_AcquisitionFile;
  formikRef: React.Ref<FormikProps<CompensationRequisitionFormModel>>;
  onSuccess: () => void;
  onCancel: () => void;
  View: React.FC<CompensationRequisitionFormProps>;
}

const UpdateCompensationRequisitionContainer: React.FC<
  UpdateCompensationRequisitionContainerProps
> = ({ compensation, acquisitionFile, formikRef, onSuccess, onCancel, View }) => {
  const {
    updateCompensationRequisition: { execute: updateCompensationRequisition, loading: isUpdating },
  } = useCompensationRequisitionRepository();

  const updateCompensation = async (compensation: CompensationRequisitionFormModel) => {
    const result = await updateCompensationRequisition(compensation.toApi());
    if (result !== undefined) {
      onSuccess();
    }
    return result;
  };

  return (
    <View
      isLoading={isUpdating}
      formikRef={formikRef}
      initialValues={CompensationRequisitionFormModel.fromApi(compensation)}
      acquisitionFile={acquisitionFile}
      onSave={updateCompensation}
      onCancel={onCancel}
    />
  );
};

export default UpdateCompensationRequisitionContainer;
