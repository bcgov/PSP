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
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
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
  const [payeePerson, setPayeePerson] = useState<ApiGen_Concepts_Person | null>();
  const [payeeOrganization, setPayeeOrganization] = useState<ApiGen_Concepts_Organization | null>();

  const [compensationRequisitionProperties, setCompensationRequisitionProperties] = useState<
    ApiGen_Concepts_FileProperty[]
  >([]);

  const {
    getCompensationRequisitionProperties: {
      execute: getCompensationProperties,
      loading: loadingCompReqProperties,
    },
  } = useCompensationRequisitionRepository();

  const {
    getPersonDetail: { execute: getPerson, loading: loadingPerson },
  } = usePersonRepository();

  const {
    getOrganizationDetail: { execute: getOrganization, loading: loadingOrganization },
  } = useOrganizationRepository();

  const fetchCompensationPayee = useCallback(async () => {
    if (isValidId(compensation.id)) {
      try {
        if (exists(compensation.acquisitionFileTeam)) {
          if (isValidId(compensation.acquisitionFileTeam.personId)) {
            const person = await getPerson(compensation.acquisitionFileTeam.personId);
            setPayeePerson(person);
          }
          if (isValidId(compensation.acquisitionFileTeam.organizationId)) {
            const organization = await getOrganization(
              compensation.acquisitionFileTeam.organizationId,
            );
            setPayeeOrganization(organization);
          }
        } else if (compensation.interestHolder) {
          if (isValidId(compensation.interestHolder.personId)) {
            const person = await getPerson(compensation.interestHolder.personId);
            setPayeePerson(person);
          }
          if (isValidId(compensation.interestHolder.organizationId)) {
            const organization = await getOrganization(compensation.interestHolder.organizationId);
            setPayeeOrganization(organization);
          }
        } else if (compensation.compReqLeaseStakeholder?.length > 0) {
          const stakeHolder = compensation.compReqLeaseStakeholder[0].leaseStakeholder;
          if (stakeHolder.lessorType.id === ApiGen_CodeTypes_LessorTypes.ORG) {
            const org = await getOrganization(stakeHolder.organizationId);
            setPayeeOrganization(org);
          } else if (stakeHolder.lessorType.id === ApiGen_CodeTypes_LessorTypes.PER) {
            const person = await getPerson(stakeHolder.personId);
            setPayeePerson(person);
          }
        }
      } catch (e) {
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          if (axiosError.response?.status === 404) {
            setPayeePerson(null);
            setPayeeOrganization(null);
          } else {
            toast.error(axiosError.response?.data.error);
          }
        }
      }
    }
  }, [
    compensation.acquisitionFileTeam,
    compensation.compReqLeaseStakeholder,
    compensation.id,
    compensation.interestHolder,
    getOrganization,
    getPerson,
  ]);

  const fetchCompensationProperties = useCallback(async () => {
    if (isValidId(compensation.id)) {
      const compReqProperties = await getCompensationProperties(fileType, compensation.id);
      setCompensationRequisitionProperties(compReqProperties);
    }
  }, [compensation.id, fileType, getCompensationProperties]);

  useEffect(() => {
    fetchCompensationPayee();
  }, [fetchCompensationPayee]);

  useEffect(() => {
    fetchCompensationProperties();
  }, [fetchCompensationProperties]);

  return compensation ? (
    <View
      fileType={fileType}
      loading={loading || loadingPerson || loadingOrganization || loadingCompReqProperties}
      file={file}
      compensation={compensation}
      compensationProperties={compensationRequisitionProperties}
      compensationContactPerson={payeePerson}
      compensationContactOrganization={payeeOrganization}
      setEditMode={setEditMode}
      clientConstant={clientConstant}
      onGenerate={onGenerate}
    ></View>
  ) : null;
};
