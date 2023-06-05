import * as API from 'constants/API';
import { useAcquisitionProvider } from 'hooks/repositories/useAcquisitionProvider';
import { useCompensationRequisitionRepository } from 'hooks/repositories/useRequisitionCompensationRepository';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { Api_AcquisitionFile, Api_AcquisitionFilePerson } from 'models/api/AcquisitionFile';
import { Api_CompensationRequisition } from 'models/api/CompensationRequisition';
import { useCallback, useEffect, useState } from 'react';
import { SystemConstants, useSystemConstants } from 'store/slices/systemConstants';

import { CompensationRequisitionFormModel, PayeeOption } from '../models';
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

  const {
    getAcquisitionOwners: { execute: retrieveAcquisitionOwners, loading: loadingAcquisitionOwners },
    getAcquisitionFileSolicitors: {
      execute: retrieveAcquisitionFileSolicitors,
      loading: loadingAcquisitionFileSolicitors,
    },
    getAcquisitionFileRepresentatives: {
      execute: retrieveAcquisitionFileRepresentatives,
      loading: loadingAcquisitionFileRepresentatives,
    },
  } = useAcquisitionProvider();

  const [payeeOptions, setPayeeOptions] = useState<PayeeOption[]>([]);
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
    const compensationApiModel = compensation.toApi(payeeOptions);

    const result = await updateCompensationRequisition(compensationApiModel);
    if (result !== undefined) {
      onSuccess();
    }
    return result;
  };

  const fetchContacts = useCallback(async () => {
    if (acquisitionFile.id) {
      const acquisitionOwnersCall = retrieveAcquisitionOwners(acquisitionFile.id);
      const acquisitionSolicitorsCall = retrieveAcquisitionFileSolicitors(acquisitionFile.id);
      const acquisitionRepresentativesCall = retrieveAcquisitionFileRepresentatives(
        acquisitionFile.id,
      );

      await Promise.all([
        acquisitionOwnersCall,
        acquisitionSolicitorsCall,
        acquisitionRepresentativesCall,
      ]).then(([acquisitionOwners, acquisitionSolicitors, acquisitionRepresentatives]) => {
        const options = payeeOptions;

        if (acquisitionOwners !== undefined) {
          const ownersOptions: PayeeOption[] = acquisitionOwners.map(x =>
            PayeeOption.createOwner(x),
          );
          options.push(...ownersOptions);
        }

        if (acquisitionSolicitors !== undefined) {
          const acquisitionSolicitorOptions: PayeeOption[] = acquisitionSolicitors.map(x =>
            PayeeOption.createOwnerSolicitor(x),
          );
          options.push(...acquisitionSolicitorOptions);
        }

        if (acquisitionRepresentatives !== undefined) {
          const acquisitionSolicitorOptions: PayeeOption[] = acquisitionRepresentatives.map(x =>
            PayeeOption.createOwnerRepresentative(x),
          );
          options.push(...acquisitionSolicitorOptions);
        }

        const teamMemberOptions: PayeeOption[] =
          acquisitionFile.acquisitionTeam
            ?.filter((x): x is Api_AcquisitionFilePerson => !!x)
            .filter(x => x.personProfileTypeCode === 'MOTILAWYER')
            .map(x => PayeeOption.createTeamMember(x)) || [];
        options.push(...teamMemberOptions);

        setPayeeOptions(options);
      });
    }
  }, [
    payeeOptions,
    acquisitionFile.acquisitionTeam,
    acquisitionFile.id,
    retrieveAcquisitionOwners,
    retrieveAcquisitionFileSolicitors,
    retrieveAcquisitionFileRepresentatives,
  ]);

  useEffect(() => {
    fetchContacts();
  }, [fetchContacts]);

  return (
    <View
      isLoading={
        isUpdating ||
        loadingAcquisitionOwners ||
        loadingAcquisitionFileSolicitors ||
        loadingAcquisitionFileRepresentatives
      }
      initialValues={CompensationRequisitionFormModel.fromApi(compensation)}
      payeeOptions={payeeOptions}
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
