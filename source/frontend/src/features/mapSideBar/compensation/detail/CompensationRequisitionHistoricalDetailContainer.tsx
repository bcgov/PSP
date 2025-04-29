import moment from 'moment';
import { useCallback, useEffect, useMemo, useState } from 'react';

import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import { useProductProvider } from '@/hooks/repositories/useProductProvider';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_CompReqAcqPayee } from '@/models/api/generated/ApiGen_Concepts_CompReqAcqPayee';
import { ApiGen_Concepts_CompReqLeasePayee } from '@/models/api/generated/ApiGen_Concepts_CompReqLeasePayee';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { isValidId } from '@/utils';

import { useGenerateH120 } from '../../acquisition/common/GenerateForm/hooks/useGenerateH120';
import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';

export interface ICompensationRequisitionHistoricalDetailContainerProps {
  fileType: ApiGen_CodeTypes_FileTypes;
  parentFileId: number;
  compensationRequisitionId: number;
  time: string;
  clientConstant: string;
  loading: boolean;
  setEditMode: (editMode: boolean) => void;
  View: React.FunctionComponent<CompensationRequisitionDetailViewProps>;
}

export const CompensationRequisitionHistoricalDetailContainer: React.FunctionComponent<
  React.PropsWithChildren<ICompensationRequisitionHistoricalDetailContainerProps>
> = ({
  fileType,
  parentFileId,
  compensationRequisitionId,
  time: almostTime,
  clientConstant,
  loading,
  setEditMode,
  View,
}) => {
  const onGenerate = useGenerateH120();

  const adjustedTime = useMemo(() => {
    const momentTime = moment(almostTime);
    return momentTime.add(1, 'seconds').toISOString();
  }, [almostTime]);

  const [parentFile, setParentFile] = useState<
    ApiGen_Concepts_Lease | ApiGen_Concepts_AcquisitionFile | null
  >(null);
  const [project, setProject] = useState<ApiGen_Concepts_Project | null>(null);
  const [alternateProject, setAlternateProject] = useState<ApiGen_Concepts_Project | null>(null);
  const [product, setProduct] = useState<ApiGen_Concepts_Product | null>(null);

  const [timedCompensation, setTimedCompensation] =
    useState<ApiGen_Concepts_CompensationRequisition | null>(null);

  const [financialActivityCodes, setFinancialActivityCodes] = useState<
    ApiGen_Concepts_FinancialCode[]
  >([]);
  const [chartOfAccountCodes, setChartOfAccountCodes] = useState<ApiGen_Concepts_FinancialCode[]>(
    [],
  );
  const [yearlyFinancialCodes, setYearlyFinancialCodes] = useState<ApiGen_Concepts_FinancialCode[]>(
    [],
  );
  const [responsibilityCentreCodes, setResponsibilityCentreCodes] = useState<
    ApiGen_Concepts_FinancialCode[]
  >([]);

  const [compensationRequisitionProperties, setCompensationRequisitionProperties] = useState<
    ApiGen_Concepts_FileProperty[]
  >([]);

  const [acquisitionPayees, setAcquisitionPayees] = useState<ApiGen_Concepts_CompReqAcqPayee[]>([]);
  const [leasePayees, setLeasePayees] = useState<ApiGen_Concepts_CompReqLeasePayee[]>([]);

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
    getCompensationRequisitionAtTime: {
      execute: getCompensationRequisition,
      loading: getCompensationRequisitionLoading,
    },
    getCompensationRequisitionAcqPropertiesAtTime: {
      execute: getCompensationAcqProperties,
      loading: loadingCompReqAcqProperties,
    },
    getCompensationRequisitionLeasePropertiesAtTime: {
      execute: getCompensationLeaseProperties,
      loading: loadingCompReqLeaseProperties,
    },
    getCompensationRequisitionAcqPayeesAtTime: {
      execute: getCompensationAcqPayees,
      loading: loadingCompReqAcqPayees,
    },
    getCompensationRequisitionLeasePayeesAtTime: {
      execute: getCompensationLeasePayees,
      loading: loadingCompReqLeasePayees,
    },
  } = useCompensationRequisitionRepository();

  const {
    getAcquisitionAtTime: { execute: getAcquisition },
  } = useAcquisitionProvider();

  const {
    getLeaseAtTime: { execute: getLease },
  } = useLeaseRepository();

  const {
    getProjectAtTime: { execute: getProject },
  } = useProjectProvider();

  const {
    getProductAtTime: { execute: getProduct },
  } = useProductProvider();

  const fetchParentFile = useCallback(async () => {
    if (fileType === ApiGen_CodeTypes_FileTypes.Acquisition) {
      const response = await getAcquisition(parentFileId, adjustedTime);
      setParentFile(response);
    } else if (fileType === ApiGen_CodeTypes_FileTypes.Lease) {
      const response = await getLease(parentFileId, adjustedTime);
      setParentFile(response);
    }
  }, [parentFileId, fileType, adjustedTime, getAcquisition, getLease]);

  const fetchProject = useCallback(
    async (
      projectId: number,
      setter: (value: React.SetStateAction<ApiGen_Concepts_Project>) => void,
    ) => {
      const response = await getProject(projectId, adjustedTime);
      setter(response);
    },
    [adjustedTime, getProject],
  );

  const fetchProduct = useCallback(
    async (product: number) => {
      setProduct(null);
      const response = await getProduct(product, adjustedTime);
      setProduct(response);
    },
    [adjustedTime, getProduct],
  );

  const fetchCompensationRequisition = useCallback(async () => {
    setTimedCompensation(null);
    const response = await getCompensationRequisition(compensationRequisitionId, adjustedTime);
    setTimedCompensation(response);
  }, [compensationRequisitionId, adjustedTime, getCompensationRequisition]);

  const fetchCompensationProperties = useCallback(async () => {
    if (fileType === ApiGen_CodeTypes_FileTypes.Acquisition) {
      const compReqProperties = await getCompensationAcqProperties(
        compensationRequisitionId,
        adjustedTime,
      );
      setCompensationRequisitionProperties(compReqProperties ?? []);
    }
    if (fileType === ApiGen_CodeTypes_FileTypes.Lease) {
      const compReqProperties = await getCompensationLeaseProperties(
        compensationRequisitionId,
        adjustedTime,
      );
      setCompensationRequisitionProperties(compReqProperties ?? []);
    }
  }, [
    compensationRequisitionId,
    fileType,
    getCompensationAcqProperties,
    adjustedTime,
    getCompensationLeaseProperties,
  ]);

  const fetchCompensationPayees = useCallback(async () => {
    if (fileType === ApiGen_CodeTypes_FileTypes.Acquisition) {
      const compReqAcqPayees = await getCompensationAcqPayees(
        compensationRequisitionId,
        adjustedTime,
      );
      setAcquisitionPayees(compReqAcqPayees);
    } else if (fileType === ApiGen_CodeTypes_FileTypes.Lease) {
      const compReqLeasePayees = await getCompensationLeasePayees(
        compensationRequisitionId,
        adjustedTime,
      );
      setLeasePayees(compReqLeasePayees);
    }
  }, [
    compensationRequisitionId,
    fileType,
    getCompensationAcqPayees,
    adjustedTime,
    getCompensationLeasePayees,
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
      setFinancialActivityCodes(activities);
      setChartOfAccountCodes(charts);
      setResponsibilityCentreCodes(responsibilities);
      setYearlyFinancialCodes(yearly);
    });
  }, [
    fetchChartOfAccounts,
    fetchFinancialActivities,
    fetchResponsibilityCodes,
    fetchYearlyFinancials,
  ]);

  useEffect(() => {
    fetchParentFile();
  }, [fetchParentFile]);

  useEffect(() => {
    if (isValidId(parentFile?.projectId)) {
      fetchProject(parentFile.projectId, setProject);
    }
  }, [fetchProject, parentFile?.projectId]);

  useEffect(() => {
    if (isValidId(parentFile?.productId)) {
      fetchProduct(parentFile.productId);
    }
  }, [fetchProduct, parentFile?.productId]);

  useEffect(() => {
    if (isValidId(timedCompensation?.alternateProjectId)) {
      fetchProject(timedCompensation.alternateProjectId, setAlternateProject);
    }
  }, [fetchProject, timedCompensation?.alternateProjectId]);

  useEffect(() => {
    fetchFinancialCodes();
  }, [fetchFinancialCodes]);

  useEffect(() => {
    fetchCompensationProperties();
  }, [fetchCompensationProperties]);

  useEffect(() => {
    fetchCompensationPayees();
  }, [fetchCompensationPayees]);

  useEffect(() => {
    fetchCompensationRequisition();
  }, [fetchCompensationRequisition]);

  const composedCompReq: ApiGen_Concepts_CompensationRequisition = useMemo(() => {
    return {
      ...timedCompensation,
      financials: timedCompensation?.financials.map<ApiGen_Concepts_CompensationFinancial>(x => ({
        ...x,
        financialActivityCode: financialActivityCodes.find(c => c.id === x.financialActivityCodeId),
      })),
      yearlyFinancial: yearlyFinancialCodes.find(
        x => x.id === timedCompensation?.yearlyFinancialId,
      ),
      chartOfAccounts: chartOfAccountCodes.find(x => x.id === timedCompensation?.chartOfAccountsId),
      responsibility: responsibilityCentreCodes.find(
        x => x.id === timedCompensation?.responsibilityId,
      ),
      alternateProject: alternateProject,
    };
  }, [
    alternateProject,
    chartOfAccountCodes,
    financialActivityCodes,
    responsibilityCentreCodes,
    timedCompensation,
    yearlyFinancialCodes,
  ]);

  const isLoading = useMemo(
    () =>
      loading ||
      loadingChartOfAccounts ||
      loadingFinancialActivities ||
      loadingResponsibilityCodes ||
      loadingYearlyFinancials ||
      getCompensationRequisitionLoading ||
      loadingCompReqAcqProperties ||
      loadingCompReqLeaseProperties ||
      loadingCompReqAcqPayees ||
      loadingCompReqLeasePayees,
    [
      getCompensationRequisitionLoading,
      loadingChartOfAccounts,
      loading,
      loadingCompReqAcqPayees,
      loadingCompReqAcqProperties,
      loadingCompReqLeasePayees,
      loadingCompReqLeaseProperties,
      loadingFinancialActivities,
      loadingResponsibilityCodes,
      loadingYearlyFinancials,
    ],
  );

  return (
    <View
      fileType={fileType}
      product={product}
      project={project}
      compensation={composedCompReq}
      compensationProperties={compensationRequisitionProperties}
      compensationAcqPayees={acquisitionPayees}
      compensationLeasePayees={leasePayees}
      clientConstant={clientConstant}
      loading={isLoading}
      setEditMode={setEditMode}
      onGenerate={onGenerate}
    />
  );
};
