import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { Scrollable } from '@/components/common/Scrollable/Scrollable';
import { Section } from '@/components/common/Section/Section';
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
import { exists, isValidId } from '@/utils';

import CompensationRequisitionDetailView from './mapSideBar/compensation/detail/CompensationRequisitionDetailView';

interface IDateTestContainerProps {
  something?: any;
}

const DateTestContainer: React.FC<React.PropsWithChildren<IDateTestContainerProps>> = () => {
  // TEST PARAMETERS -----------------------------------------
  const compReqId = 52;
  //const time = '2025-03-21T21:32:15.247';
  const time = '2025-04-26T21:33:00.247';
  const fileType: ApiGen_CodeTypes_FileTypes = ApiGen_CodeTypes_FileTypes.Lease;
  const parentFileId = 18;
  // -----------------------------------------

  const [parentFile, setParentFile] = useState<
    ApiGen_Concepts_Lease | ApiGen_Concepts_AcquisitionFile | null
  >(null);
  const [project, setProject] = useState<ApiGen_Concepts_Project | null>(null);
  const [alternateProject, setAlternateProject] = useState<ApiGen_Concepts_Project | null>(null);
  const [product, setProduct] = useState<ApiGen_Concepts_Product | null>(null);

  const [timedCompensation, setTimedCompensation] =
    useState<ApiGen_Concepts_CompensationRequisition | null>(null);

  const [latestCompensation, setLatestCompensation] =
    useState<ApiGen_Concepts_CompensationRequisition | null>(null);

  const [latestAcqPayees, setLatestAcqPayees] = useState<ApiGen_Concepts_CompReqAcqPayee[]>([]);

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
    getCompensationRequisition: { execute: getCurrentCompReq },
    getCompensationRequisitionAcqPayees: { execute: getCurrentAcqPayees },
    getCompensationRequisitionAtTime: {
      execute: getCompensationRequisition,
      loading: getCompensationRequisitionLoading,
    },
    getCompensationRequisitionPropertiesAtTime: {
      execute: getCompensationProperties,
      loading: loadingCompReqProperties,
    },
    getCompensationRequisitionAcqPayeesAtTime: {
      execute: getCompensationAcqPayees,
      loading: loadingCompReqAcqPayees,
      response: compReqAcqPayees,
    },
    getCompensationRequisitionLeasePayeesAtTime: {
      execute: getCompensationLeasePayees,
      loading: loadingCompReqLeasePayees,
      response: compReqLeasePayees,
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
      const response = await getAcquisition(parentFileId, time);
      setParentFile(response);
    } else if (fileType === ApiGen_CodeTypes_FileTypes.Lease) {
      const response = await getLease(parentFileId, time);
      setParentFile(response);
    }
  }, [fileType, getAcquisition, getLease]);

  const fetchProject = useCallback(
    async (
      projectId: number,
      setter: (value: React.SetStateAction<ApiGen_Concepts_Project>) => void,
    ) => {
      const response = await getProject(projectId, time);
      setter(response);
    },
    [getProject],
  );

  const fetchProduct = useCallback(
    async (product: number) => {
      setProduct(null);
      const response = await getProduct(product, time);
      setProduct(response);
    },
    [getProduct],
  );

  const fetchCurrentCompensationRequisition = useCallback(async () => {
    setTimedCompensation(null);
    const response = await getCurrentCompReq(compReqId);
    setLatestCompensation(response);
  }, [getCurrentCompReq]);

  const fetchCurrentAcquisitionPayees = useCallback(async () => {
    setTimedCompensation(null);
    const response = await getCurrentAcqPayees(compReqId);
    setLatestAcqPayees(response);
  }, [getCurrentAcqPayees]);

  const fetchCompensationRequisition = useCallback(async () => {
    setTimedCompensation(null);
    const response = await getCompensationRequisition(compReqId, time);
    setTimedCompensation(response);
  }, [getCompensationRequisition]);

  const fetchCompensationProperties = useCallback(async () => {
    if (isValidId(timedCompensation?.id)) {
      const compReqProperties = await getCompensationProperties(timedCompensation.id, time);
      setCompensationRequisitionProperties(compReqProperties);
    }
  }, [timedCompensation?.id, getCompensationProperties]);

  const fetchCompensationPayees = useCallback(async () => {
    if (isValidId(timedCompensation?.id)) {
      if (fileType === ApiGen_CodeTypes_FileTypes.Acquisition) {
        const compReqAcqPayees = await getCompensationAcqPayees(timedCompensation.id, time);
        setAcquisitionPayees(compReqAcqPayees);
      } else if (fileType === ApiGen_CodeTypes_FileTypes.Lease) {
        const compReqLeasePayees = await getCompensationLeasePayees(timedCompensation.id, time);
        setLeasePayees(compReqLeasePayees);
      }
    }
  }, [timedCompensation?.id, fileType, getCompensationAcqPayees]);

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
    fetchCurrentAcquisitionPayees();
  }, [fetchCurrentAcquisitionPayees]);

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

  useEffect(() => {
    fetchCurrentCompensationRequisition();
  }, [fetchCurrentCompensationRequisition]);

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

  return (
    <Scrollable>
      <Row>
        <Col>
          <SectionWrapper>
            <Section header="Date Test Container">
              <div>
                <Button onClick={() => fetchCompensationRequisition()}>Fetch Data</Button>
              </div>
              {exists(composedCompReq) && (
                <CompensationRequisitionDetailView
                  fileType={fileType}
                  product={product}
                  project={project}
                  compensation={composedCompReq}
                  compensationProperties={compensationRequisitionProperties}
                  compensationAcqPayees={acquisitionPayees}
                  compensationLeasePayees={leasePayees}
                  clientConstant={''}
                  loading={false}
                  setEditMode={() => {
                    console.log('clicked');
                  }}
                  onGenerate={() => {
                    console.log('clicked');
                  }}
                />
              )}
            </Section>
          </SectionWrapper>
        </Col>
        <Col>
          <Row>
            <Col>
              Timed Comp Req
              <JsonWrapper>{JSON.stringify(composedCompReq, null, 4)}</JsonWrapper>
            </Col>
            <Col>
              Latest Comp Req
              <JsonWrapper>{JSON.stringify(latestCompensation, null, 4)}</JsonWrapper>
            </Col>
          </Row>
          <Row>
            <Col>
              Timed Acq Payee
              <JsonWrapper>{JSON.stringify(acquisitionPayees, null, 4)}</JsonWrapper>
            </Col>
            <Col>
              Latest Acq Payee
              <JsonWrapper>{JSON.stringify(latestAcqPayees, null, 4)}</JsonWrapper>
            </Col>
          </Row>
        </Col>
      </Row>
    </Scrollable>
  );
};

export default DateTestContainer;

const SectionWrapper = styled.div`
  border: 1px solid red;
  width: 800px;
`;
const JsonWrapper = styled.div`
  border: 1px solid green;
  white-space: pre;
  text-align: left;
  max-width: 200px;
`;
