import { groupBy } from 'lodash';
import { useCallback, useEffect, useState } from 'react';

import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { usePropertyOperationRepository } from '@/hooks/repositories/usePropertyOperationRepository';
import { ApiGen_CodeTypes_PropertyOperationTypes } from '@/models/api/generated/ApiGen_CodeTypes_PropertyOperationTypes';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { exists, unique } from '@/utils';

import { IOperationSectionViewProps } from './OperationSectionView';

export interface IOperationContainerProps {
  propertyId: number | undefined;
  View: React.FunctionComponent<IOperationSectionViewProps>;
}

export interface OperationSet {
  operationDateTime: UtcIsoDateTime;
  sourceProperties: ApiGen_Concepts_Property[];
  destinationProperties: ApiGen_Concepts_Property[];
  operationType: ApiGen_CodeTypes_PropertyOperationTypes;
}

export const OperationContainer: React.FunctionComponent<IOperationContainerProps> = ({
  propertyId,
  View,
}) => {
  const [subdivisionOperations, setSubdivisionOperations] = useState<OperationSet[]>([]);
  const [consolidationOperations, setConsolidationOperations] = useState<OperationSet[]>([]);

  const { getPropertyWrapper } = usePimsPropertyRepository();
  const getPropertyExecute = getPropertyWrapper.execute;

  const { getPropertyOperations } = usePropertyOperationRepository();
  const getPropertyOperationsExecute = getPropertyOperations.execute;

  const fetchOperations = useCallback(
    async (propertyId?: number) => {
      if (propertyId) {
        const results = await getPropertyOperationsExecute(propertyId);

        const newOperations: OperationSet[] = [];
        if (exists(results)) {
          const resultGroup = groupBy(results, x => x.propertyOperationNo);

          for (const key in resultGroup) {
            const result = resultGroup[key];

            const sourceIds = result
              .map(po => po.sourcePropertyId)
              .filter(exists)
              .filter(unique);
            const destinationIds = result
              .map(po => po.destinationPropertyId)
              .filter(exists)
              .filter(unique);

            const sourcePromises: Promise<ApiGen_Concepts_Property | undefined>[] = [];
            const destinationPromises: Promise<ApiGen_Concepts_Property | undefined>[] = [];

            for (let i = 0; i < sourceIds.length; i++) {
              sourcePromises.push(getPropertyExecute(sourceIds[i]));
            }
            for (let i = 0; i < destinationIds.length; i++) {
              destinationPromises.push(getPropertyExecute(destinationIds[i]));
            }

            const retrievedSources = await Promise.all(sourcePromises);
            const retrievedDestinations = await Promise.all(destinationPromises);

            const resultOperationType =
              result[0].propertyOperationTypeCode?.id ??
              ApiGen_CodeTypes_PropertyOperationTypes.SUBDIVIDE;
            const operationType =
              ApiGen_CodeTypes_PropertyOperationTypes[
                resultOperationType as keyof typeof ApiGen_CodeTypes_PropertyOperationTypes
              ];

            const operationSet: OperationSet = {
              operationDateTime: result[0].operationDt ?? '',
              sourceProperties: retrievedSources.filter(exists) ?? [],
              destinationProperties: retrievedDestinations.filter(exists) ?? [],
              operationType,
            };

            newOperations.push(operationSet);
          }
          const subdivisions = newOperations.filter(
            x => x.operationType === ApiGen_CodeTypes_PropertyOperationTypes.SUBDIVIDE,
          );
          const consolidations = newOperations.filter(
            x => x.operationType === ApiGen_CodeTypes_PropertyOperationTypes.CONSOLIDATE,
          );

          setSubdivisionOperations(subdivisions);
          setConsolidationOperations(consolidations);
        }
      }
    },
    [getPropertyExecute, getPropertyOperationsExecute],
  );

  useEffect(() => {
    fetchOperations(propertyId);
  }, [fetchOperations, propertyId]);

  return (
    <View
      subdivisionOperations={subdivisionOperations}
      consolidationOperations={consolidationOperations}
      loading={!propertyId || getPropertyOperations.loading}
    />
  );
};
