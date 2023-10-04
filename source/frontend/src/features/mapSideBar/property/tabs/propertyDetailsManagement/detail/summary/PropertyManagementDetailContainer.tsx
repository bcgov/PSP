import { useCallback, useEffect, useState } from 'react';

import { usePropertyManagementRepository } from '@/hooks/repositories/usePropertyManagementRepository';
import { Api_PropertyManagement } from '@/models/api/Property';

import { EditManagementState } from '../../../../PropertyViewSelector';
import { IPropertyManagementDetailViewProps } from './PropertyManagementDetailView';

export interface IPropertyManagementDetailContainerProps {
  propertyId: number;
  setEditManagementState: (state: EditManagementState | null) => void;
  View: React.FC<IPropertyManagementDetailViewProps>;
}

export const PropertyManagementDetailContainer: React.FunctionComponent<
  IPropertyManagementDetailContainerProps
> = ({ propertyId, setEditManagementState, View }) => {
  const [propertyManagement, setPropertyManagement] = useState<Api_PropertyManagement>({
    id: propertyId ?? 0,
    rowVersion: null,
    managementPurposes: [],
    additionalDetails: null,
    isUtilitiesPayable: null,
    isTaxesPayable: null,
    isLeaseActive: false,
    isLeaseExpired: false,
    leaseExpiryDate: null,
  });

  const {
    getPropertyManagement: { execute: getPropertyManagement, loading },
  } = usePropertyManagementRepository();

  const fetchPropertyManagement = useCallback(async () => {
    if (!propertyId) {
      return;
    }
    const response = await getPropertyManagement(propertyId);
    if (response) {
      setPropertyManagement(response);
    }
  }, [getPropertyManagement, propertyId]);

  useEffect(() => {
    fetchPropertyManagement();
  }, [fetchPropertyManagement]);

  return (
    <View
      isLoading={loading}
      propertyManagement={propertyManagement}
      setEditManagementState={setEditManagementState}
    />
  );
};
