import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import { ApiGen_Concepts_HistoricalFileNumber } from '@/models/api/generated/ApiGen_Concepts_HistoricalFileNumber';

import { HistoricalNumberFieldView } from './HistoricalNumberFieldView';

export interface IHistoricalNumbersViewProps {
  historicalNumbers: ApiGen_Concepts_HistoricalFileNumber[];
  labelWidth?: '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' | '10' | '11' | '12' | 'auto';
  contentWidth?: '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' | '10' | '11' | '12' | 'auto';
}

export const HistoricalNumberSectionView: React.FC<IHistoricalNumbersViewProps> = ({
  historicalNumbers,
  labelWidth,
  contentWidth,
}) => {
  return (
    <HeaderField
      label="Historical file #:"
      labelWidth={labelWidth ?? '3'}
      contentWidth={contentWidth ?? '9'}
    >
      <HistoricalNumberFieldView historicalNumbers={historicalNumbers} />
    </HeaderField>
  );
};
