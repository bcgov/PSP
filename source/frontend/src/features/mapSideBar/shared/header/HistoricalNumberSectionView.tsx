import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import { ApiGen_Concepts_HistoricalFileNumber } from '@/models/api/generated/ApiGen_Concepts_HistoricalFileNumber';

import { HistoricalNumberFieldView } from './HistoricalNumberFieldView';

export interface IHistoricalNumbersViewProps {
  historicalNumbers: ApiGen_Concepts_HistoricalFileNumber[];
}

export const HistoricalNumberSectionView: React.FC<IHistoricalNumbersViewProps> = ({
  historicalNumbers,
}) => {
  return (
    <HeaderField label="Historical file #:" labelWidth="3" contentWidth="9">
      <HistoricalNumberFieldView historicalNumbers={historicalNumbers} />
    </HeaderField>
  );
};
