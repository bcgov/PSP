import { ColProps } from 'react-bootstrap';

import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import { ApiGen_Concepts_HistoricalFileNumber } from '@/models/api/generated/ApiGen_Concepts_HistoricalFileNumber';

import { HistoricalNumberFieldView } from './HistoricalNumberFieldView';

export interface IHistoricalNumbersViewProps {
  historicalNumbers: ApiGen_Concepts_HistoricalFileNumber[];
  labelWidth?: ColProps;
  contentWidth?: ColProps;
}

export const HistoricalNumberSectionView: React.FC<IHistoricalNumbersViewProps> = ({
  historicalNumbers,
  labelWidth,
  contentWidth,
}) => {
  return (
    <HeaderField
      label="Historical file #:"
      labelWidth={{ ...(labelWidth ?? { xs: 3 }) }}
      contentWidth={{ ...(contentWidth ?? { xs: 9 }) }}
    >
      <HistoricalNumberFieldView historicalNumbers={historicalNumbers} />
    </HeaderField>
  );
};
