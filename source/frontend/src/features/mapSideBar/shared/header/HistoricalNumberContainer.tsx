import { useEffect, useState } from 'react';

import { useHistoricalNumberRepository } from '@/hooks/repositories/useHistoricalNumberRepository';
import { ApiGen_Concepts_HistoricalNumber } from '@/models/api/generated/ApiGen_Concepts_HistoricalNumber';

import { IHistoricalNumbersViewProps } from './HistoricalNumberSectionView';

export interface IHistoricalNumbersContainerProps {
  propertyIds: number[];
  View: React.FunctionComponent<IHistoricalNumbersViewProps>;
}
const HistoricalNumbersContainer: React.FC<IHistoricalNumbersContainerProps> = ({
  propertyIds,
  View,
}) => {
  const [historicalNumbers, setHistoricalNumbers] = useState<ApiGen_Concepts_HistoricalNumber[]>(
    [],
  );

  const { getPropertyHistoricalNumbers } = useHistoricalNumberRepository();

  const getHistoricalExecute = getPropertyHistoricalNumbers.execute;

  useEffect(() => {
    const tasks: Promise<ApiGen_Concepts_HistoricalNumber[]>[] = [];

    for (let i = 0; i < propertyIds.length; i++) {
      tasks.push(getHistoricalExecute(propertyIds[i]));
    }

    Promise.all(tasks).then(results => {
      const flatted = results.flat();
      setHistoricalNumbers(flatted);
    });
  }, [propertyIds, getHistoricalExecute]);

  return <View historicalNumbers={historicalNumbers} />;
};
export default HistoricalNumbersContainer;
