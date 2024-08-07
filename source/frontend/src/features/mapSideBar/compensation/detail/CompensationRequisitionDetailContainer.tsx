import axios, { AxiosError } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { toast } from 'react-toastify';

import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import { useGenerateH120 } from '@/features/mapSideBar/acquisition/common/GenerateForm/hooks/useGenerateH120';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { exists, isValidId } from '@/utils';

import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';

export interface CompensationRequisitionDetailContainerProps {
  compensation: ApiGen_Concepts_CompensationRequisition;
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  clientConstant: string;
  loading: boolean;
  setEditMode: (editMode: boolean) => void;
  View: React.FunctionComponent<React.PropsWithChildren<CompensationRequisitionDetailViewProps>>;
}

export const CompensationRequisitionDetailContainer: React.FunctionComponent<
  React.PropsWithChildren<CompensationRequisitionDetailContainerProps>
> = ({ compensation, setEditMode, View, clientConstant, acquisitionFile, loading }) => {
  const onGenerate = useGenerateH120();

  const {
    getCompensationRequisitionProperties: {
      execute: getCompensationProperties,
      loading: loadingCompReqProperties,
    },
  } = useCompensationRequisitionRepository();
  const [payeePerson, setPayeePerson] = useState<ApiGen_Concepts_Person | undefined>();
  const [compensationRequisitionProperties, setCompensationRequisitionProperties] = useState<
    ApiGen_Concepts_FileProperty[]
  >([]);
  const [payeeOrganization, setPayeeOrganization] = useState<
    ApiGen_Concepts_Organization | undefined
  >();

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
        }
      } catch (e) {
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          if (axiosError.response?.status === 404) {
            setPayeePerson(undefined);
          } else {
            toast.error(axiosError.response?.data.error);
          }
        }
      }
    }
  }, [
    compensation.acquisitionFileTeam,
    compensation.id,
    compensation.interestHolder,
    getOrganization,
    getPerson,
  ]);

  const fetchCompensationProperties = useCallback(async () => {
    if (isValidId(compensation.id)) {
      const compReqProperties = await getCompensationProperties(compensation.id);
      setCompensationRequisitionProperties(compReqProperties);
    }
  }, [compensation.id, getCompensationProperties]);

  useEffect(() => {
    fetchCompensationPayee();
  }, [fetchCompensationPayee]);

  useEffect(() => {
    fetchCompensationProperties();
  }, [fetchCompensationProperties]);

  return compensation ? (
    <View
      loading={loading || loadingPerson || loadingOrganization || loadingCompReqProperties}
      acquisitionFile={acquisitionFile}
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
