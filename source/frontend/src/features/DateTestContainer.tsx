import React, { useCallback, useEffect, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { Scrollable } from '@/components/common/Scrollable/Scrollable';
import { Section } from '@/components/common/Section/Section';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
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

  const [compensationRequisitionProperties, setCompensationRequisitionProperties] = useState<
    ApiGen_Concepts_FileProperty[]
  >([]);
  const {
    getCompensationRequisition: { execute: getCurrentCompReq },
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
      await getCompensationAcqPayees(timedCompensation.id, time);
    }
  }, [timedCompensation?.id, getCompensationAcqPayees]);

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

  return (
    <Scrollable>
      <Row>
        <Col>
          <SectionWrapper>
            <Section header="Date Test Container">
              <div>
                <Button onClick={() => fetchCompensationRequisition()}>Fetch Data</Button>
              </div>
              {exists(timedCompensation) && (
                <CompensationRequisitionDetailView
                  fileType={fileType}
                  product={undefined}
                  project={undefined}
                  compensation={timedCompensation}
                  compensationProperties={compensationRequisitionProperties}
                  compensationAcqPayees={[]}
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
          Timed Comp Req
          <JsonWrapper>{JSON.stringify(timedCompensation, null, 4)}</JsonWrapper>
        </Col>
        <Col>
          Latest Comp Req
          <JsonWrapper>{JSON.stringify(latestCompensation, null, 4)}</JsonWrapper>
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
  max-width: 500px;
`;
