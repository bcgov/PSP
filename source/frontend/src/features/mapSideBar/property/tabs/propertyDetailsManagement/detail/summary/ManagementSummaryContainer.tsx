import { useCallback, useEffect, useState } from 'react';

import { usePropertyManagementRepository } from '@/hooks/repositories/usePropertyManagementRepository';
import { Api_Property, Api_PropertyManagement } from '@/models/api/Property';

import { EditManagementState } from '../../../../PropertyViewSelector';
import { IManagementSummaryViewProps } from './ManagementSummaryView';

interface IManagementSummaryContainerProps {
  property: Api_Property;
  setEditManagementState: (state: EditManagementState | null) => void;
  View: React.FC<IManagementSummaryViewProps>;
}

export const ManagementSummaryContainer: React.FunctionComponent<
  IManagementSummaryContainerProps
> = ({ property, setEditManagementState, View }) => {
  const [propertyManagement, setPropertyManagement] = useState<Api_PropertyManagement>({
    id: property.id ?? 0,
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
    if (!property.id) {
      return;
    }
    const response = await getPropertyManagement(property.id);
    if (response) {
      setPropertyManagement(response);
    }
  }, [getPropertyManagement, property.id]);

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
