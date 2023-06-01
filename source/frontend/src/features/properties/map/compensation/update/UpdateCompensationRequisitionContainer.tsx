import * as API from 'constants/API';
import { useCompensationRequisitionRepository } from 'hooks/repositories/useRequisitionCompensationRepository';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_CompensationRequisition } from 'models/api/CompensationRequisition';
import { SystemConstants, useSystemConstants } from 'store/slices/systemConstants';

import { CompensationRequisitionFormModel } from '../models';
import { CompensationRequisitionFormProps } from './UpdateCompensationRequisitionForm';

export interface UpdateCompensationRequisitionContainerProps {
  compensation: Api_CompensationRequisition;
  acquisitionFile: Api_AcquisitionFile;
  onSuccess: () => void;
  onCancel: () => void;
  View: React.FC<CompensationRequisitionFormProps>;
}

const UpdateCompensationRequisitionContainer: React.FC<
  UpdateCompensationRequisitionContainerProps
> = ({ compensation, acquisitionFile, onSuccess, onCancel, View }) => {
  const lookups = useLookupCodeHelpers();
  const {
    updateCompensationRequisition: { execute: updateCompensationRequisition, loading: isUpdating },
  } = useCompensationRequisitionRepository();

  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimalPercentage =
    gstConstant !== undefined ? parseFloat(gstConstant.value) / 100 : undefined;
  const chartOfAccountsOptions = lookups
    .getOptionsByType(API.CHART_OF_ACCOUNTS_CODE_TYPES, false)
    .map(item => {
      return {
        ...item,
        label: `${item.code} - ${item.label}`,
      };
    });
  const responsiblityCentreOptions = lookups
    .getOptionsByType(API.RESPONSIBILITY_CODE_TYPES, false)
    .map(item => {
      return {
        ...item,
        label: `${item.code} - ${item.label}`,
      };
    });
  const yearlyFinancialOptions = lookups
    .getOptionsByType(API.YEARLY_FINANCIAL_CODE_TYPES, false)
    .map(item => {
      return {
        ...item,
        label: `${item.code}`,
      };
    });
  const financialActivityOptions = lookups
    .getOptionsByType(API.FINANCIAL_ACTIVITY_CODE_TYPES, false)
    .map(item => {
      return {
        ...item,
        label: `${item.code} - ${item.label}`,
      };
    });

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
      initialValues={CompensationRequisitionFormModel.fromApi(compensation)}
      gstConstant={gstDecimalPercentage ?? 0}
      financialActivityOptions={financialActivityOptions}
      chartOfAccountsOptions={chartOfAccountsOptions}
      responsiblityCentreOptions={responsiblityCentreOptions}
      yearlyFinancialOptions={yearlyFinancialOptions}
      acquisitionFile={acquisitionFile}
      onSave={updateCompensation}
      onCancel={onCancel}
    />
  );
};

export default UpdateCompensationRequisitionContainer;
