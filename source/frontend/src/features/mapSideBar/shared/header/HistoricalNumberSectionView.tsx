import { useMemo } from 'react';
import styled from 'styled-components';

import ExpandableTextList from '@/components/common/ExpandableTextList';
import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import { Dictionary } from '@/interfaces/Dictionary';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_HistoricalFileNumber } from '@/models/api/generated/ApiGen_Concepts_HistoricalFileNumber';
import { exists } from '@/utils';

export interface IHistoricalNumbersViewProps {
  valuesOnly: boolean;
  historicalNumbers: ApiGen_Concepts_HistoricalFileNumber[];
}

interface HistoricalGroup {
  historicalType: ApiGen_Base_CodeType<string>;
  historicalValues: Dictionary<ApiGen_Concepts_HistoricalFileNumber>;
}

const HistoricalNumberFieldView: React.FC<IHistoricalNumbersViewProps> = ({
  historicalNumbers,
  valuesOnly,
}) => {
  const uniqueNumbers = useMemo(() => {
    const flatNumberArray: Dictionary<HistoricalGroup> = historicalNumbers
      .filter(exists)
      .reduce((accumulator, h) => {
        const historicalTypeKey = `[${h.historicalFileNumberTypeCode.description}]`;
        if (!(historicalTypeKey in accumulator)) {
          accumulator[historicalTypeKey] = {
            historicalType: h.historicalFileNumberTypeCode,
            historicalValues: {},
          };
        }

        const historicalValueKey = `[${h.historicalFileNumberTypeCode.description}][${h.historicalFileNumber}]`;
        accumulator[historicalTypeKey].historicalValues[historicalValueKey] = h;
        return accumulator;
      }, {} as Dictionary<HistoricalGroup>);

    return Object.values(flatNumberArray).sort(p => p.historicalType.displayOrder);
  }, [historicalNumbers]);

  if (valuesOnly) {
    return (
      <ExpandableTextList<HistoricalGroup>
        items={uniqueNumbers}
        keyFunction={(p, index: number) => `historical-number-${p.historicalType.id}-${index}`}
        renderFunction={p => (
          <>
            <StyledLabel key={`historical-group-${p.historicalType.id}`} className="pr-2">
              {p.historicalType.description}:
            </StyledLabel>

            {Object.values(p.historicalValues).map((historicalValue, index) => {
              return (
                <span key={index}>
                  {historicalValue.historicalFileNumber}
                  {index + 1 < Object.values(p.historicalValues).length && <span>, </span>}
                </span>
              );
            })}
          </>
        )}
        delimiter={'; '}
        maxCollapsedLength={2}
      />
    );
  }

  return (
    <HeaderField label="Historical File #:" labelWidth="3" contentWidth="9">
      <>
        <ExpandableTextList<HistoricalGroup>
          items={uniqueNumbers}
          keyFunction={(p, index: number) => `historical-number-${p.historicalType.id}-${index}`}
          renderFunction={p => (
            <>
              <StyledLabel key={`historical-group-${p.historicalType.id}`} className="pr-2">
                {p.historicalType.description}:
              </StyledLabel>

              {Object.values(p.historicalValues).map((historicalValue, index) => {
                return (
                  <span key={index}>
                    {historicalValue.historicalFileNumber}
                    {index + 1 < Object.values(p.historicalValues).length && <span>, </span>}
                  </span>
                );
              })}
            </>
          )}
          delimiter={'; '}
          maxCollapsedLength={2}
        />
      </>
    </HeaderField>
  );
};

export default HistoricalNumberFieldView;

export const StyledLabel = styled.label`
  font-weight: bold;
`;
