import { FormikProps } from 'formik';
import { useCompensationRequisitionRepository } from 'hooks/repositories/useRequisitionCompensationRepository';
import { Api_Compensation } from 'models/api/Compensation';

import { CompensationRequisitionFormProps } from '../CompensationRequisitionForm';
import { CompensationRequisitionFormModel } from '../models';

export interface UpdateCompensationRequisitionContainerProps {
  compensation: Api_Compensation;
  formikRef: React.Ref<FormikProps<CompensationRequisitionFormModel>>;
  onSuccess: () => void;
  View: React.FC<CompensationRequisitionFormProps>;
}

const UpdateCompensationRequisitionContainer: React.FC<
  UpdateCompensationRequisitionContainerProps
> = ({ compensation, formikRef, onSuccess, View }) => {
  const {
    updateCompensationRequisition: { execute: updateCompensationRequisition, loading: isUpdating },
  } = useCompensationRequisitionRepository();

  const udpateCompensation = async (compensation: CompensationRequisitionFormModel) => {
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
      onSave={udpateCompensation}
    />
  );
};

export default UpdateCompensationRequisitionContainer;
