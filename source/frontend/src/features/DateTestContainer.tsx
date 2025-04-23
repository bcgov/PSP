import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { Scrollable } from '@/components/common/Scrollable/Scrollable';
import { Section } from '@/components/common/Section/Section';
import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_CompReqAcqPayee } from '@/models/api/generated/ApiGen_Concepts_CompReqAcqPayee';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { exists, isValidId } from '@/utils';

import CompensationRequisitionDetailView from './mapSideBar/compensation/detail/CompensationRequisitionDetailView';

interface IDateTestContainerProps {
  something?: any;
}

const DateTestContainer: React.FC<React.PropsWithChildren<IDateTestContainerProps>> = () => {
  // TEST PARAMETERS -----------------------------------------
  const compReqId = 11;
  //const time = '2025-03-21T21:32:15.247';
  const time = '2025-03-21T21:33:00.247';
  const fileType = ApiGen_CodeTypes_FileTypes.Acquisition;
  // -----------------------------------------

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
  } = useCompensationRequisitionRepository();

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
      const compReqAcqPayees = await getCompensationAcqPayees(timedCompensation.id, time);
      setAcquisitionPayees(compReqAcqPayees);
    }
  }, [timedCompensation?.id, getCompensationAcqPayees]);

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
    };
  }, [
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
                  product={undefined}
                  project={undefined}
                  compensation={composedCompReq}
                  compensationProperties={compensationRequisitionProperties}
                  compensationAcqPayees={acquisitionPayees}
                  compensationLeasePayees={[]}
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
