import { useCallback, useEffect } from 'react';

import { usePropertyManagementRepository } from '@/hooks/repositories/usePropertyManagementRepository';

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
  const {
    getPropertyManagement: {
      execute: getPropertyManagement,
      response: propertyManagement,
      loading,
    },
  } = usePropertyManagementRepository();

  const fetchPropertyManagement = useCallback(async () => {
    if (!propertyId) {
      return;
    }
    await getPropertyManagement(propertyId);
  }, [getPropertyManagement, propertyId]);

  useEffect(() => {
    fetchPropertyManagement();
  }, [fetchPropertyManagement]);

  return (
    <View
      isLoading={loading}
      propertyManagement={propertyManagement ?? null}
      setEditManagementState={setEditManagementState}
    />
  );
};
