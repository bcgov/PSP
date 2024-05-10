import { useMemo } from 'react';

import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import { ApiGen_Concepts_PropertyFileNumber } from '@/models/api/generated/ApiGen_Concepts_PropertyFileNumber';
import { exists } from '@/utils';

export interface IHistoricalNumbersViewProps {
  historicalNumbers: ApiGen_Concepts_PropertyFileNumber[];
}

const HistoricalNumberFieldView: React.FC<IHistoricalNumbersViewProps> = ({
  historicalNumbers,
}) => {
  const uniqueNumbers = useMemo(() => {
    const flatNumberArray = historicalNumbers
      .filter(exists)
      .sort(h => h.fileNumberTypeCode.displayOrder)
      .map(h => `${h.fileNumber}[${h.fileNumberTypeCode.description}]`);

    const uniqueNumberSet = new Set(flatNumberArray);
    return Array.from(uniqueNumberSet);
  }, [historicalNumbers]);

  return (
    <HeaderField label="Historical File #:" labelWidth="3" contentWidth="9">
      {uniqueNumbers.join(', ')}
    </HeaderField>
  );
};

export default HistoricalNumberFieldView;
