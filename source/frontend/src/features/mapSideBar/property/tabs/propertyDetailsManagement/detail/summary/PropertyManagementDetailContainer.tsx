import { useCallback, useEffect, useState } from 'react';

import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import { usePropertyManagementRepository } from '@/hooks/repositories/usePropertyManagementRepository';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { exists, isValidId } from '@/utils';

import { IPropertyManagementDetailViewProps } from './PropertyManagementDetailView';

export interface IPropertyManagementDetailContainerProps {
  propertyId: number;
  View: React.FC<IPropertyManagementDetailViewProps>;
}

export const PropertyManagementDetailContainer: React.FunctionComponent<
  IPropertyManagementDetailContainerProps
> = ({ propertyId, View }) => {
  const {
    getPropertyManagement: {
      execute: getPropertyManagement,
      response: propertyManagement,
      loading,
    },
  } = usePropertyManagementRepository();

  const [person, setPerson] = useState<ApiGen_Concepts_Person | null>(null);
  const [organization, setOrganization] = useState<ApiGen_Concepts_Organization | null>(null);
  const [primaryContact, setPrimaryContact] = useState<ApiGen_Concepts_Person | null>(null);
  const {
    getPersonDetail: { execute: getPerson, loading: getPersonLoading },
  } = usePersonRepository();

  const {
    getOrganizationDetail: { execute: getOrganization, loading: getOrganizationLoading },
  } = useOrganizationRepository();

  const fetchPropertyManagement = useCallback(async () => {
    if (!propertyId) {
      return;
    }
    await getPropertyManagement(propertyId);
  }, [getPropertyManagement, propertyId]);

  const fetchData = useCallback(async () => {
    if (isValidId(propertyManagement?.responsiblePayerPersonId)) {
      const returnedPerson = await getPerson(propertyManagement?.responsiblePayerPersonId);
      if (exists(returnedPerson)) {
        setPerson(returnedPerson);
      }
    }
    if (isValidId(propertyManagement?.responsiblePayerOrganizationId)) {
      const returnedOrganization = await getOrganization(
        propertyManagement?.responsiblePayerOrganizationId,
      );
      if (exists(returnedOrganization)) {
        setOrganization(returnedOrganization);
      }
    }

    if (isValidId(propertyManagement?.responsiblePayerPrimaryContactId)) {
      const returnedPrimaryContact = await getPerson(
        propertyManagement?.responsiblePayerPrimaryContactId,
      );
      if (exists(returnedPrimaryContact)) {
        setPrimaryContact(returnedPrimaryContact);
      }
    }
  }, [
    getOrganization,
    getPerson,
    propertyManagement?.responsiblePayerOrganizationId,
    propertyManagement?.responsiblePayerPersonId,
    propertyManagement?.responsiblePayerPrimaryContactId,
  ]);

  useEffect(() => {
    fetchPropertyManagement();
  }, [fetchPropertyManagement]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);
  if (getPersonLoading || getOrganizationLoading) {
    return <></>;
  }

  return (
    <View
      isLoading={loading}
      propertyManagement={propertyManagement ?? null}
      responsiblePayerPerson={person}
      responsiblePayerOrganization={organization}
      primaryContact={primaryContact}
    />
  );
};
