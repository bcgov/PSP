import axios, { AxiosError } from 'axios';
import { useCallback, useEffect, useState } from 'react';
import { toast } from 'react-toastify';

import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import { useGenerateH120 } from '@/features/mapSideBar/acquisition/common/GenerateForm/hooks/useGenerateH120';
import { IApiError } from '@/interfaces/IApiError';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { Api_Organization } from '@/models/api/Organization';
import { Api_Person } from '@/models/api/Person';

import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';

export interface CompensationRequisitionDetailContainerProps {
  compensation: Api_CompensationRequisition;
  acquisitionFile: Api_AcquisitionFile;
  clientConstant: string;
  loading: boolean;
  setEditMode: (editMode: boolean) => void;
  View: React.FunctionComponent<React.PropsWithChildren<CompensationRequisitionDetailViewProps>>;
}

export const CompensationRequisitionDetailContainer: React.FunctionComponent<
  React.PropsWithChildren<CompensationRequisitionDetailContainerProps>
> = ({ compensation, setEditMode, View, clientConstant, acquisitionFile, loading }) => {
  const onGenerate = useGenerateH120();

  const [contactPerson, setCompensationPayee] = useState<Api_Person | undefined>();
  const [contactOrganization, setContactOrganization] = useState<Api_Organization | undefined>();

  const {
    getPersonDetail: { execute: getPerson, loading: loadingPerson },
  } = usePersonRepository();

  const {
    getOrganizationDetail: { execute: getOrganization, loading: loadingOrganization },
  } = useOrganizationRepository();

  const fetchCompensationPayee = useCallback(async () => {
    if (compensation.id !== null) {
      try {
        if (!!compensation.acquisitionFilePerson && !!compensation.acquisitionFilePerson.personId) {
          const person = await getPerson(compensation.acquisitionFilePerson.personId);
          setCompensationPayee(person);
        } else if (!!compensation.interestHolder) {
          if (
            compensation.interestHolder.personId !== undefined &&
            compensation.interestHolder.personId !== null
          ) {
            const person = await getPerson(compensation.interestHolder.personId);
            setCompensationPayee(person);
          }
          if (!!compensation.interestHolder.organizationId) {
            const organization = await getOrganization(compensation.interestHolder.organizationId);
            setContactOrganization(organization);
          }
        }
      } catch (e) {
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          if (axiosError.response?.status === 404) {
            setCompensationPayee(undefined);
          } else {
            toast.error(axiosError.response?.data.error);
          }
        }
      }
    }
  }, [
    compensation.acquisitionFilePerson,
    compensation.id,
    compensation.interestHolder,
    getOrganization,
    getPerson,
  ]);

  useEffect(() => {
    fetchCompensationPayee();
  }, [fetchCompensationPayee]);

  return compensation ? (
    <View
      loading={loading || loadingPerson || loadingOrganization}
      compensation={compensation}
      compensationContactPerson={contactPerson}
      compensationContactOrganization={contactOrganization}
      acqFileProject={acquisitionFile?.project}
      acqFileProduct={acquisitionFile?.product}
      setEditMode={setEditMode}
      clientConstant={clientConstant}
      onGenerate={onGenerate}
    ></View>
  ) : null;
};
