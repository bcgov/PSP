import { useCallback, useEffect, useState } from 'react';

import { usePropertyImprovementRepository } from '@/hooks/repositories/usePropertyImprovementRepository';
import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';
import { isValidId } from '@/utils';

import { IPropertyImprovementsListViewProps } from './PropertyImprovementsListView';

export interface IPropertyImprovementsListContainerProps {
  propertyId: number;
  View: React.FunctionComponent<IPropertyImprovementsListViewProps>;
}

export const PropertyImprovementsListContainer: React.FunctionComponent<
  IPropertyImprovementsListContainerProps
> = ({ propertyId, View }) => {
  const [propertyImprovements, setPropertyImprovements] =
    useState<ApiGen_Concepts_PropertyImprovement[]>(null);

  const {
    getPropertyImprovements: { execute: getPropertyImprovementsApi, loading: loadingImprovements },
    deletePropertyImprovement: {
      execute: deletePropertyImprovementApi,
      loading: deletingImprovement,
    },
  } = usePropertyImprovementRepository();

  const fetchData = useCallback(async () => {
    const improvements = await getPropertyImprovementsApi(propertyId);
    if (improvements) {
      setPropertyImprovements(improvements);
    }
  }, [getPropertyImprovementsApi, propertyId]);

  const handleImprovementDeleted = async (propertyImprovementId: number) => {
    if (isValidId(propertyImprovementId)) {
      await deletePropertyImprovementApi(propertyId, propertyImprovementId);
      const improvements = await getPropertyImprovementsApi(propertyId);
      setPropertyImprovements(improvements);
    }
  };

  useEffect(() => {
    if (isValidId(propertyId) && propertyImprovements === null) {
      fetchData();
    }
  }, [fetchData, propertyId, propertyImprovements]);

  return isValidId(propertyId) ? (
    <View
      loading={loadingImprovements || deletingImprovement}
      propertyImprovements={propertyImprovements ?? []}
      onDelete={handleImprovementDeleted}
    ></View>
  ) : null;
};

export default PropertyImprovementsListContainer;
