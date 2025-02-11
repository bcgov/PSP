import moment from 'moment';
import { useCallback, useEffect, useState } from 'react';

import { SelectOption } from '@/components/common/form';
import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { useLeaseStakeholderRepository } from '@/hooks/repositories/useLeaseStakeholderRepository';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_CompReqPayee } from '@/models/api/generated/ApiGen_Concepts_CompReqPayee';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { exists, isValidId } from '@/utils';

import { CompReqLeaseStakeholderModel } from '../../acquisition/models/LeaseStakeholderModel';
import { CompensationRequisitionFormModel } from '../models/CompensationRequisitionFormModel';
import { CompensationRequisitionFormProps } from './UpdateCompensationRequisitionForm';

export interface UpdateCompensationRequisitionContainerProps {
  compensation: ApiGen_Concepts_CompensationRequisition;
  fileType: ApiGen_CodeTypes_FileTypes;
  file: ApiGen_Concepts_AcquisitionFile | ApiGen_Concepts_Lease;
  onSuccess: () => void;
  onCancel: () => void;
  View: React.FC<CompensationRequisitionFormProps>;
}

const UpdateCompensationRequisitionContainer: React.FC<
  UpdateCompensationRequisitionContainerProps
> = ({ compensation, fileType, file, onSuccess, onCancel, View }) => {
  const [payeeOptions, setPayeeOptions] = useState<PayeeOption[]>(null);
  const [financialActivityOptions, setFinancialActivityOptions] = useState<SelectOption[]>([]);
  const [chartOfAccountOptions, setChartOfAccountOptions] = useState<SelectOption[]>([]);
  const [yearlyFinancialOptions, setyearlyFinancialOptions] = useState<SelectOption[]>([]);
  const [responsibilityCentreOptions, setResponsibilityCentreOptions] = useState<SelectOption[]>(
    [],
  );
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
      execute: fetchAcquisitionInterestHolders,
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

  const {
    getLeaseStakeholders: {
      execute: getLeaseStakeholders,
      loading: loadingLeaseStakeholders,
      response: leaseStakeholders,
    },
  } = useLeaseStakeholderRepository();

  const {
    getCompensationRequisitionPayees: {
      execute: getCompReqPayees,
      loading: loadingCompReqPayees,
      response: existingPayees,
    },
  } = useCompensationRequisitionRepository();

  const updateCompensation = async (compensation: CompensationRequisitionFormModel) => {
    const compensationApiModel = compensation.toApi();

    const result = await updateCompensationRequisition(fileType, compensationApiModel);
    if (result !== undefined) {
      onSuccess();
    }
    return result;
  };

  const fetchPayeeOptions = useCallback(async () => {
    if (file.id) {
      switch (fileType) {
        case ApiGen_CodeTypes_FileTypes.Acquisition:
          {
            const acquisitionOwnersCall = retrieveAcquisitionOwners(file.id);
            const interestHoldersCall = fetchAcquisitionInterestHolders(file.id);

            await Promise.all([acquisitionOwnersCall, interestHoldersCall]).then(
              ([acquisitionOwners, interestHolders]) => {
                const options = payeeOptions ?? [];

                if (acquisitionOwners !== undefined) {
                  const ownersOptions: PayeeOption[] = acquisitionOwners.map(x =>
                    PayeeOption.createOwner(x, getEmptyCompReqPayee(compensation.id)),
                  );
                  options.push(...ownersOptions);
                }

                if (interestHolders !== undefined) {
                  const interestHolderOptions: PayeeOption[] = interestHolders.map(x =>
                    PayeeOption.createInterestHolder(x, getEmptyCompReqPayee(compensation.id)),
                  );
                  options.push(...interestHolderOptions);
                }

                const teamMemberOptions: PayeeOption[] =
                  (file as ApiGen_Concepts_AcquisitionFile).acquisitionTeam
                    ?.filter(
                      (x): x is ApiGen_Concepts_AcquisitionFileTeam =>
                        exists(x) && x.teamProfileTypeCode === 'MOTILAWYER',
                    )
                    .map(x =>
                      PayeeOption.createTeamMember(x, getEmptyCompReqPayee(compensation.id)),
                    ) || [];
                options.push(...teamMemberOptions);

                if (compensation.legacyPayee) {
                  options.push(PayeeOption.createLegacyPayee(compensation, null, compensation.id));
                }

                setPayeeOptions(options);
              },
            );
          }
          break;
        case ApiGen_CodeTypes_FileTypes.Lease:
          {
            const leaseStakeHolders = await getLeaseStakeholders(file.id);
            const stakeHoldersOptions = leaseStakeHolders.map(x =>
              PayeeOption.createLeaseStakeholder(compensation.id, x),
            );

            setPayeeOptions(stakeHoldersOptions);
          }
          break;
      }
    }
  }, [
    file,
    fileType,
    retrieveAcquisitionOwners,
    fetchAcquisitionInterestHolders,
    payeeOptions,
    compensation,
    getLeaseStakeholders,
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

  const fetchCompensationPayees = useCallback(async () => {
    if (isValidId(compensation.id)) {
      await getCompReqPayees(compensation.id);
    }
  }, [compensation.id, getCompReqPayees]);

  const fetchLeaseStakeholders = useCallback(async () => {
    if (isValidId(file.id) && fileType === ApiGen_CodeTypes_FileTypes.Lease) {
      await getLeaseStakeholders(file.id);
    }
  }, [file.id, fileType, getLeaseStakeholders]);

  useEffect(() => {
    fetchCompensationPayees();
  }, [fetchCompensationPayees]);

  useEffect(() => {
    fetchLeaseStakeholders();
  }, [fetchLeaseStakeholders]);

  useEffect(() => {
    if (payeeOptions === null) {
      fetchPayeeOptions();
    }
  }, [fetchPayeeOptions, payeeOptions]);

  useEffect(() => {
    fetchFinancialCodes();
  }, [fetchFinancialCodes]);

  compensation.compReqPayees = existingPayees ?? [];
  return (
    <View
      isLoading={
        isUpdating ||
        loadingAcquisitionOwners ||
        loadingFinancialActivities ||
        loadingChartOfAccounts ||
        loadingResponsibilityCodes ||
        loadingYearlyFinancials ||
        loadingInterestHolders ||
        loadingLeaseStakeholders ||
        loadingCompReqPayees
      }
      initialValues={CompensationRequisitionFormModel.fromApi(
        compensation,
        yearlyFinancialOptions,
        chartOfAccountOptions,
        responsibilityCentreOptions,
        financialActivityOptions,
      )}
      payeeOptions={payeeOptions ?? []}
      leaseStakeholders={
        leaseStakeholders?.map(l => CompReqLeaseStakeholderModel.createFromStakeholder(l)) ?? []
      }
      gstConstant={gstDecimalPercentage ?? 0}
      financialActivityOptions={financialActivityOptions}
      chartOfAccountsOptions={chartOfAccountOptions}
      responsiblityCentreOptions={responsibilityCentreOptions}
      yearlyFinancialOptions={yearlyFinancialOptions}
      file={file}
      fileType={fileType}
      onSave={updateCompensation}
      onCancel={onCancel}
      showAltProjectError={showAltProjectError}
      setShowAltProjectError={setShowAltProjectError}
    />
  );
};

const getEmptyCompReqPayee = (compensationRequisitionId): ApiGen_Concepts_CompReqPayee => {
  return {
    compensationRequisitionId: compensationRequisitionId,
    acquisitionFileTeamId: null,
    interestHolderId: null,
    compReqPayeeId: null,
    acquisitionOwnerId: null,
    acquisitionOwner: null,
    interestHolder: null,
    acquisitionFileTeam: null,
    compensationRequisition: null,
    ...getEmptyBaseAudit(),
  };
};

export default UpdateCompensationRequisitionContainer;
