import { useCallback, useEffect, useState } from 'react';

import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { Api_Property } from '@/models/api/Property';
import { Api_PropertyLease } from '@/models/api/PropertyLease';

import { IManagementSummaryViewProps } from './ManagementSummaryView';

interface IManagementSummaryContainerProps {
  property: Api_Property;
  setEditMode: (isEditing: boolean) => void;
  View: React.FC<IManagementSummaryViewProps>;
}

export const ManagementSummaryContainer: React.FunctionComponent<
  IManagementSummaryContainerProps
> = ({ property, setEditMode, View }) => {
  const [propertyLeases, setPropertyLeases] = useState<Api_PropertyLease[]>([]);

  const {
    getPropertyLeases: { execute: getPropertyLeases, loading },
  } = usePimsPropertyRepository();

  const fetchPropertyLeases = useCallback(async () => {
    if (!property.id) {
      return;
    }
    const propertyLeasesResponse = await getPropertyLeases(property.id);
    if (propertyLeasesResponse) {
      setPropertyLeases(propertyLeasesResponse);
    }
  }, [getPropertyLeases, property]);

  useEffect(() => {
    fetchPropertyLeases();
  }, [fetchPropertyLeases]);

  return (
    <View
      isLoading={loading}
      property={property}
      propertyLeases={propertyLeases}
      setEditMode={setEditMode}
    />
  );
};
