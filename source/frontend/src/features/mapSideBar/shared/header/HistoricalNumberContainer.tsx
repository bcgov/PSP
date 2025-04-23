import { useState } from 'react';
import { ColProps } from 'react-bootstrap';

import { useHistoricalNumberRepository } from '@/hooks/repositories/useHistoricalNumberRepository';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { ApiGen_Concepts_HistoricalFileNumber } from '@/models/api/generated/ApiGen_Concepts_HistoricalFileNumber';

import { IHistoricalNumbersViewProps } from './HistoricalNumberSectionView';

export interface IHistoricalNumbersContainerProps {
  propertyIds: number[];
  labelWidth?: ColProps;
  contentWidth?: ColProps;
  View: React.FunctionComponent<IHistoricalNumbersViewProps>;
}
const HistoricalNumbersContainer: React.FC<IHistoricalNumbersContainerProps> = ({
  propertyIds,
  labelWidth,
  contentWidth,
  View,
}) => {
  const [historicalNumbers, setHistoricalNumbers] = useState<
    ApiGen_Concepts_HistoricalFileNumber[]
  >([]);

  const { getPropertyHistoricalNumbers } = useHistoricalNumberRepository();

  const getHistoricalExecute = getPropertyHistoricalNumbers.execute;

  useDeepCompareEffect(() => {
    const tasks: Promise<ApiGen_Concepts_HistoricalFileNumber[]>[] = [];

    for (let i = 0; i < propertyIds.length; i++) {
      tasks.push(getHistoricalExecute(propertyIds[i]));
    }

    Promise.all(tasks).then(results => {
      const flatted = results.flat();
      setHistoricalNumbers(flatted);
    });
  }, [propertyIds, getHistoricalExecute]);

  return (
    <View
      historicalNumbers={historicalNumbers}
      labelWidth={labelWidth}
      contentWidth={contentWidth}
    />
  );
};
export default HistoricalNumbersContainer;
