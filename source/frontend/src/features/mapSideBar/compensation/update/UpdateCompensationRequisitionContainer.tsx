import moment from 'moment';
import { useCallback, useEffect, useState } from 'react';

import { SelectOption } from '@/components/common/form';
import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { useLeaseStakeholderRepository } from '@/hooks/repositories/useLeaseStakeholderRepository';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { usePrevious } from '@/hooks/usePrevious';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_CompReqAcqPayee } from '@/models/api/generated/ApiGen_Concepts_CompReqAcqPayee';
import { ApiGen_Concepts_CompReqLeasePayee } from '@/models/api/generated/ApiGen_Concepts_CompReqLeasePayee';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { exists, isValidId, isValidString } from '@/utils';

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
  const [yearlyFinancialOptions, setYearlyFinancialOptions] = useState<SelectOption[]>([]);
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
    getLeaseStakeholders: { execute: getLeaseStakeholders, loading: loadingLeaseStakeholders },
  } = useLeaseStakeholderRepository();

  const {
    getCompensationRequisitionAcqPayees: {
      execute: getCompReqAcqPayees,
      loading: loadingCompReqAcqPayees,
      response: existingAcqPayees,
    },
    getCompensationRequisitionLeasePayees: {
      execute: getCompReqLeasePayees,
      loading: loadingCompReqLeasePayees,
      response: existingLeasePayees,
    },
  } = useCompensationRequisitionRepository();

  // update payees when request to child endpoint returns
  const previousAcqPayees = usePrevious(existingAcqPayees);
  compensation.compReqAcqPayees = existingAcqPayees ?? [];

  const updateCompensation = async (compensation: CompensationRequisitionFormModel) => {
    const compensationApiModel = compensation.toApi();

    const result = await updateCompensationRequisition(fileType, compensationApiModel);
    if (result !== undefined) {
      onSuccess();
    }
    return result;
  };

  const fetchPayeeOptions = useCallback(
    async (
      fileType: ApiGen_CodeTypes_FileTypes,
      existingCompPayees: ApiGen_Concepts_CompReqAcqPayee[],
    ) => {
      if (file.id) {
        switch (fileType) {
          case ApiGen_CodeTypes_FileTypes.Acquisition:
            {
              const acquisitionOwnersCall = retrieveAcquisitionOwners(file.id);
              const interestHoldersCall = fetchAcquisitionInterestHolders(file.id);

              const [acquisitionOwners, interestHolders] = await Promise.all([
                acquisitionOwnersCall,
                interestHoldersCall,
              ]);

              const options = [];

              if (exists(acquisitionOwners)) {
                const ownersOptions: PayeeOption[] = acquisitionOwners.map(acquisitionOwner =>
                  PayeeOption.createOwner(
                    acquisitionOwner,
                    getEmptyCompReqAcqPayee(compensation.id),
                  ),
                );
                options.push(...ownersOptions);
              }

              if (exists(interestHolders)) {
                const interestHolderOptions: PayeeOption[] = interestHolders.map(interestHolder =>
                  PayeeOption.createInterestHolder(
                    interestHolder,
                    getEmptyCompReqAcqPayee(compensation.id),
                  ),
                );
                options.push(...interestHolderOptions);
              }

              const teamMemberOptions: PayeeOption[] =
                (file as ApiGen_Concepts_AcquisitionFile).acquisitionTeam
                  ?.filter(
                    (teamMember): teamMember is ApiGen_Concepts_AcquisitionFileTeam =>
                      exists(teamMember) && teamMember.teamProfileTypeCode === 'MOTILAWYER',
                  )
                  .map(teamMember =>
                    PayeeOption.createTeamMember(
                      teamMember,
                      getEmptyCompReqAcqPayee(compensation.id),
                    ),
                  ) || [];
              options.push(...teamMemberOptions);

              const legacy = (existingCompPayees ?? []).find(p => isValidString(p.legacyPayee));
              if (exists(legacy)) {
                options.push(
                  PayeeOption.createLegacyPayee(
                    legacy.legacyPayee,
                    getEmptyCompReqAcqPayee(compensation.id),
                  ),
                );
              }

              setPayeeOptions(options);
            }
            break;
          case ApiGen_CodeTypes_FileTypes.Lease:
            {
              const leaseStakeHolders = await getLeaseStakeholders(file.id);
              if (exists(leaseStakeHolders)) {
                const stakeHoldersOptions: PayeeOption[] = leaseStakeHolders.map(leaseStakeHolder =>
                  PayeeOption.createLeaseStakeholder(
                    compensation.id,
                    leaseStakeHolder,
                    getEmptyCompReqLeasePayee(compensation.id),
                  ),
                );

                setPayeeOptions(stakeHoldersOptions);
              }
            }
            break;
        }
      }
    },
    [
      file,
      retrieveAcquisitionOwners,
      fetchAcquisitionInterestHolders,
      compensation.id,
      getLeaseStakeholders,
    ],
  );

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
      setYearlyFinancialOptions(yearlyOptions);
    });
  }, [
    fetchChartOfAccounts,
    fetchFinancialActivities,
    fetchResponsibilityCodes,
    fetchYearlyFinancials,
  ]);

  const fetchCompensationPayees = useCallback(async () => {
    if (isValidId(compensation.id)) {
      fileType === ApiGen_CodeTypes_FileTypes.Acquisition
        ? await getCompReqAcqPayees(compensation.id)
        : await getCompReqLeasePayees(compensation.id);
    }
  }, [compensation.id, fileType, getCompReqAcqPayees, getCompReqLeasePayees]);

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
    if (payeeOptions === null || previousAcqPayees !== existingAcqPayees) {
      fetchPayeeOptions(fileType, existingAcqPayees);
    }
  }, [existingAcqPayees, fetchPayeeOptions, fileType, payeeOptions, previousAcqPayees]);

  useEffect(() => {
    if (payeeOptions === null) {
      fetchPayeeOptions(fileType, []);
    }
  }, [existingLeasePayees, fetchPayeeOptions, fileType, payeeOptions]);

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
        loadingInterestHolders ||
        loadingLeaseStakeholders ||
        loadingCompReqAcqPayees ||
        loadingCompReqLeasePayees
      }
      initialValues={CompensationRequisitionFormModel.fromApi(
        compensation,
        yearlyFinancialOptions,
        chartOfAccountOptions,
        responsibilityCentreOptions,
        financialActivityOptions,
      )}
      payeeOptions={payeeOptions ?? []}
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

const getEmptyCompReqAcqPayee = (compensationRequisitionId): ApiGen_Concepts_CompReqAcqPayee => {
  return {
    compensationRequisitionId: compensationRequisitionId,
    acquisitionFileTeamId: null,
    interestHolderId: null,
    compReqAcqPayeeId: null,
    acquisitionOwnerId: null,
    acquisitionOwner: null,
    interestHolder: null,
    acquisitionFileTeam: null,
    compensationRequisition: null,
    legacyPayee: null,
    ...getEmptyBaseAudit(),
  };
};

const getEmptyCompReqLeasePayee = (
  compensationRequisitionId,
): ApiGen_Concepts_CompReqLeasePayee => {
  return {
    compensationRequisitionId: compensationRequisitionId,
    leaseStakeholderId: null,
    compReqLeasePayeeId: null,
    leaseStakeholder: null,
    ...getEmptyBaseAudit(),
  };
};

export default UpdateCompensationRequisitionContainer;
