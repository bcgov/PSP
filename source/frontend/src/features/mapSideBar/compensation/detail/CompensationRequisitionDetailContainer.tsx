import axios, { AxiosError } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { toast } from 'react-toastify';

import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import { useGenerateH120 } from '@/features/mapSideBar/acquisition/common/GenerateForm/hooks/useGenerateH120';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_CodeTypes_LessorTypes } from '@/models/api/generated/ApiGen_CodeTypes_LessorTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';
import { exists, isValidId } from '@/utils';

import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';

export interface CompensationRequisitionDetailContainerProps {
  compensation: ApiGen_Concepts_CompensationRequisition;
  fileType: ApiGen_CodeTypes_FileTypes;
  file: ApiGen_Concepts_AcquisitionFile | ApiGen_Concepts_Lease;
  clientConstant: string;
  loading: boolean;
  setEditMode: (editMode: boolean) => void;
  View: React.FunctionComponent<React.PropsWithChildren<CompensationRequisitionDetailViewProps>>;
}

export const CompensationRequisitionDetailContainer: React.FunctionComponent<
  React.PropsWithChildren<CompensationRequisitionDetailContainerProps>
> = ({ compensation, setEditMode, View, clientConstant, fileType, file, loading }) => {
  const onGenerate = useGenerateH120();
  const [compensationLeaseStakeHolders, setCompensationLeaseStakeHolders] = useState<
    ApiGen_Concepts_LeaseStakeholder[] | null
  >();

  const [compensationRequisitionProperties, setCompensationRequisitionProperties] = useState<
    ApiGen_Concepts_FileProperty[]
  >([]);

  const {
    getCompensationRequisitionProperties: {
      execute: getCompensationProperties,
      loading: loadingCompReqProperties,
    },
    getCompensationRequisitionPayees: {
      execute: getCompensationPayees,
      loading: loadingCompReqPayees,
      response: compReqPayees,
    },
  } = useCompensationRequisitionRepository();

  const {
    getPersonDetail: { execute: getPerson, loading: loadingPerson },
  } = usePersonRepository();

  const {
    getOrganizationDetail: { execute: getOrganization, loading: loadingOrganization },
  } = useOrganizationRepository();

  const fetchLeaseStakeholder = useCallback(async () => {
    if (isValidId(compensation.id)) {
      try {
        if (
          (!exists(compReqPayees) || compReqPayees.length === 0) &&
          compensation.compReqLeaseStakeholders?.length > 0
        ) {
          const stakeHolder = compensation.compReqLeaseStakeholders[0].leaseStakeholder;
          if (stakeHolder.lessorType.id === ApiGen_CodeTypes_LessorTypes.ORG) {
            const org = await getOrganization(stakeHolder.organizationId);
            setCompensationLeaseStakeHolders([{ ...stakeHolder, organization: org }]);
          } else if (stakeHolder.lessorType.id === ApiGen_CodeTypes_LessorTypes.PER) {
            const person = await getPerson(stakeHolder.personId);
            setCompensationLeaseStakeHolders([{ ...stakeHolder, person: person }]);
          }
        }
      } catch (e) {
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          if (axiosError.response?.status === 404) {
            setCompensationLeaseStakeHolders([]);
          } else {
            toast.error(axiosError.response?.data.error);
          }
        }
      }
    }
  }, [
    compReqPayees,
    compensation.compReqLeaseStakeholders,
    compensation.id,
    getOrganization,
    getPerson,
  ]);

  const fetchCompensationProperties = useCallback(async () => {
    if (isValidId(compensation.id)) {
      const compReqProperties = await getCompensationProperties(fileType, compensation.id);
      setCompensationRequisitionProperties(compReqProperties);
    }
  }, [compensation.id, fileType, getCompensationProperties]);

  const fetchCompensationPayees = useCallback(async () => {
    if (isValidId(compensation.id)) {
      await getCompensationPayees(compensation.id);
    }
  }, [compensation.id, getCompensationPayees]);

  useEffect(() => {
    fetchLeaseStakeholder();
  }, [fetchLeaseStakeholder]);

  useEffect(() => {
    fetchCompensationProperties();
  }, [fetchCompensationProperties]);

  useEffect(() => {
    fetchCompensationPayees();
  }, [fetchCompensationPayees]);

  return compensation ? (
    <View
      fileType={fileType}
      loading={
        loading ||
        loadingPerson ||
        loadingOrganization ||
        loadingCompReqProperties ||
        loadingCompReqPayees
      }
      file={file}
      compensation={compensation}
      compensationProperties={compensationRequisitionProperties}
      compensationPayees={compReqPayees}
      setEditMode={setEditMode}
      clientConstant={clientConstant}
      onGenerate={onGenerate}
      compensationLeaseStakeHolders={compensationLeaseStakeHolders}
    ></View>
  ) : null;
};
