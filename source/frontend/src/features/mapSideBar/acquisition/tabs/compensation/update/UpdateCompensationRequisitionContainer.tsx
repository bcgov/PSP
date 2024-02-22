import moment from 'moment';
import { useCallback, useEffect, useState } from 'react';

import { SelectOption } from '@/components/common/form';
import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { exists } from '@/utils';

import { CompensationRequisitionFormModel } from './models';
import { CompensationRequisitionFormProps } from './UpdateCompensationRequisitionForm';

export interface UpdateCompensationRequisitionContainerProps {
  compensation: ApiGen_Concepts_CompensationRequisition;
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  onSuccess: () => void;
  onCancel: () => void;
  View: React.FC<CompensationRequisitionFormProps>;
}

const UpdateCompensationRequisitionContainer: React.FC<
  UpdateCompensationRequisitionContainerProps
> = ({ compensation, acquisitionFile, onSuccess, onCancel, View }) => {
  const [payeeOptions, setPayeeOptions] = useState<PayeeOption[]>([]);
  const [financialActivityOptions, setFinancialActivityOptions] = useState<SelectOption[]>([]);
  const [chartOfAccountOptions, setChartOfAccountOptions] = useState<SelectOption[]>([]);
  const [responsibilityCentreOptions, setResponsibilityCentreOptions] = useState<SelectOption[]>(
    [],
  );
  const [yearlyFinancialOptions, setyearlyFinancialOptions] = useState<SelectOption[]>([]);
  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimalPercentage =
    gstConstant !== undefined ? parseFloat(gstConstant.value) / 100 : undefined;
  const {
    updateCompensationRequisition: { execute: updateCompensationRequisition, loading: isUpdating },
  } = useCompensationRequisitionRepository();
  const [showAltProjectError, setShowAltProjectError] = useState<boolean>(false);

  const {
    getAcquisitionOwners: { execute: retrieveAcquisitionOwners, loading: loadingAcquisitionOwners },
  } = useAcquisitionProvider();

  const {
    getAcquisitionInterestHolders: {
      execute: fetchInterestHolders,
      loading: loadingInterestHolders,
    },
  } = useInterestHolderRepository();

  const {
    getFinancialActivityCodeTypes: {
      execute: fetchFinancialActivities,
      loading: loadingFinancialActivities,
    },
    getChartOfAccountsCodeTypes: { execute: fetchChartOfAccounts, loading: loadingChartOfAccounts },
    getResponsibilityCodeTypes: {
      execute: fetchResponsibilityCodes,
      loading: loadingResponsibilityCodes,
    },
    getYearlyFinancialsCodeTypes: {
      execute: fetchYearlyFinancials,
      loading: loadingYearlyFinancials,
    },
  } = useFinancialCodeRepository();

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
      const interestHoldersCall = fetchInterestHolders(acquisitionFile.id);

      await Promise.all([acquisitionOwnersCall, interestHoldersCall]).then(
        ([acquisitionOwners, interestHolders]) => {
          const matchedInterestHolder =
            interestHolders?.find(ih => ih.interestHolderId === compensation.interestHolderId) ??
            null;
          compensation.interestHolder = matchedInterestHolder;

          const options = payeeOptions;

          if (acquisitionOwners !== undefined) {
            const ownersOptions: PayeeOption[] = acquisitionOwners.map(x =>
              PayeeOption.createOwner(x),
            );
            options.push(...ownersOptions);
          }

          if (interestHolders !== undefined) {
            const interestHolderOptions: PayeeOption[] = interestHolders.map(x =>
              PayeeOption.createInterestHolder(x),
            );
            options.push(...interestHolderOptions);
          }

          const teamMemberOptions: PayeeOption[] =
            acquisitionFile.acquisitionTeam
              ?.filter(
                (x): x is ApiGen_Concepts_AcquisitionFileTeam =>
                  exists(x) && x.teamProfileTypeCode === 'MOTILAWYER',
              )
              .map(x => PayeeOption.createTeamMember(x)) || [];
          options.push(...teamMemberOptions);

          if (compensation.legacyPayee) {
            options.push(PayeeOption.createLegacyPayee(compensation));
          }

          setPayeeOptions(options);
        },
      );
    }
  }, [
    compensation,
    payeeOptions,
    acquisitionFile.acquisitionTeam,
    acquisitionFile.id,
    retrieveAcquisitionOwners,
    fetchInterestHolders,
  ]);

  const fetchFinancialCodes = useCallback(async () => {
    const fetchFinancialActivitiesCall = fetchFinancialActivities();
    const fetchChartOfAccountsCall = fetchChartOfAccounts();
    const fetchResponsibilityCodesCall = fetchResponsibilityCodes();
    const fetchYearlyFinancialsCall = fetchYearlyFinancials();

    await Promise.all([
      fetchFinancialActivitiesCall,
      fetchChartOfAccountsCall,
      fetchResponsibilityCodesCall,
      fetchYearlyFinancialsCall,
    ]).then(([activities, charts, responsibilities, yearly]) => {
      const currentDate = moment();
      const activityOptions: SelectOption[] =
        activities
          ?.filter(
            a =>
              currentDate >= moment(a.effectiveDate) &&
              (!a.expiryDate || currentDate <= moment(a.expiryDate)),
          )
          ?.map(item => {
            return {
              label: `${item.code} - ${item.description}`,
              value: item.id ?? 0,
            };
          }) ?? [];

      const chartsOptions: SelectOption[] =
        charts
          ?.filter(
            c =>
              currentDate >= moment(c.effectiveDate) &&
              (!c.expiryDate || currentDate <= moment(c.expiryDate)),
          )
          ?.map(item => {
            return {
              label: `${item.code} - ${item.description}`,
              value: item.id ?? 0,
            };
          }) ?? [];

      const responsibilitiesOptions: SelectOption[] =
        responsibilities
          ?.filter(
            r =>
              currentDate >= moment(r.effectiveDate) &&
              (!r.expiryDate || currentDate <= moment(r.expiryDate)),
          )
          ?.map(item => {
            return {
              label: `${item.code} - ${item.description}`,
              value: item.id ?? 0,
            };
          }) ?? [];

      const yearlyOptions: SelectOption[] =
        yearly
          ?.filter(
            y =>
              currentDate >= moment(y.effectiveDate) &&
              (!y.expiryDate || currentDate <= moment(y.expiryDate)),
          )
          ?.map(item => {
            return {
              label: `${item.code} - ${item.description}`,
              value: item.id ?? 0,
            };
          }) ?? [];

      setFinancialActivityOptions(activityOptions);
      setChartOfAccountOptions(chartsOptions);
      setResponsibilityCentreOptions(responsibilitiesOptions);
      setyearlyFinancialOptions(yearlyOptions);
    });
  }, [
    fetchChartOfAccounts,
    fetchFinancialActivities,
    fetchResponsibilityCodes,
    fetchYearlyFinancials,
  ]);

  useEffect(() => {
    fetchContacts();
  }, [fetchContacts]);

  useEffect(() => {
    fetchFinancialCodes();
  }, [fetchFinancialCodes]);

  return (
    <View
      isLoading={
        isUpdating ||
        loadingAcquisitionOwners ||
        loadingFinancialActivities ||
        loadingChartOfAccounts ||
        loadingResponsibilityCodes ||
        loadingYearlyFinancials ||
        loadingInterestHolders
      }
      initialValues={CompensationRequisitionFormModel.fromApi(
        compensation,
        yearlyFinancialOptions,
        chartOfAccountOptions,
        responsibilityCentreOptions,
        financialActivityOptions,
      )}
      payeeOptions={payeeOptions}
      gstConstant={gstDecimalPercentage ?? 0}
      financialActivityOptions={financialActivityOptions}
      chartOfAccountsOptions={chartOfAccountOptions}
      responsiblityCentreOptions={responsibilityCentreOptions}
      yearlyFinancialOptions={yearlyFinancialOptions}
      acquisitionFile={acquisitionFile}
      onSave={updateCompensation}
      onCancel={onCancel}
      showAltProjectError={showAltProjectError}
      setShowAltProjectError={setShowAltProjectError}
    />
  );
};

export default UpdateCompensationRequisitionContainer;
