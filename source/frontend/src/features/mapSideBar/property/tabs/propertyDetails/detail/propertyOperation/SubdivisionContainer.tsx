import { Dictionary, groupBy } from 'lodash';
import { useCallback, useEffect, useState } from 'react';

import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { usePropertyOperationRepository } from '@/hooks/repositories/usePropertyOperationRepository';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { exists, unique } from '@/utils';

import { SubdivisionView } from './SubdivisionView';

interface ISubdivisionContainer {
  propertyId: number;
}

interface OperationSet {
  operationDateTime: UtcIsoDateTime;
  sourceProperties: ApiGen_Concepts_Property[];
  destinationProperties: ApiGen_Concepts_Property[];
}

export const SubdivisionContainer: React.FunctionComponent<ISubdivisionContainer> = ({
  propertyId,
}) => {
  const View = SubdivisionView;

  const [operationDictionary, setOperationDictionary] = useState<Dictionary<OperationSet>>({});

  const { getPropertyWrapper } = usePimsPropertyRepository();
  const getPropertyExecute = getPropertyWrapper.execute;

  const { getPropertyOperations } = usePropertyOperationRepository();
  const getPropertyOperationsExecute = getPropertyOperations.execute;

  const fetchOperations = useCallback(
    async (propertyId: number) => {
      const results = await getPropertyOperationsExecute(propertyId);

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

          const operationSet: OperationSet = {
            operationDateTime: result[0].operationDt ?? '',
            sourceProperties: retrievedSources.filter(exists) ?? [],
            destinationProperties: retrievedDestinations.filter(exists) ?? [],
          };

          operationDictionary[key] = operationSet;

          setOperationDictionary(operationDictionary);
        }
      }
    },
    [getPropertyExecute, getPropertyOperationsExecute, operationDictionary],
  );

  useEffect(() => {
    fetchOperations(propertyId);
  }, [fetchOperations, propertyId]);

  if (!exists(operationDictionary)) {
    return <></>;
  }

  return (
    <>
      {Object.values(operationDictionary).map((operationSet, index) => (
        <View
          key={index}
          operationTimeStamp={operationSet.operationDateTime ?? ''}
          sourceProperties={operationSet.sourceProperties}
          destinationProperties={operationSet.destinationProperties}
        />
      ))}
    </>
  );
};
