import { useMemo } from 'react';
import styled from 'styled-components';

import ExpandableTextList from '@/components/common/ExpandableTextList';
import { Dictionary } from '@/interfaces/Dictionary';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_CodeTypes_HistoricalFileNumberTypes } from '@/models/api/generated/ApiGen_CodeTypes_HistoricalFileNumberTypes';
import { ApiGen_Concepts_HistoricalFileNumber } from '@/models/api/generated/ApiGen_Concepts_HistoricalFileNumber';
import { exists } from '@/utils';

export interface IHistoricalNumbersViewProps {
  historicalNumbers: ApiGen_Concepts_HistoricalFileNumber[];
}

interface HistoricalGroup {
  historicalType: ApiGen_Base_CodeType<string>;
  historicalValues: Dictionary<ApiGen_Concepts_HistoricalFileNumber>;
  otherDescription: string;
  propertyKey: string;
}

export const HistoricalNumberFieldView: React.FC<IHistoricalNumbersViewProps> = ({
  historicalNumbers,
}) => {
  const uniqueNumbers = useMemo(() => {
    const flatNumberDictionary: Dictionary<HistoricalGroup> = historicalNumbers
      .filter(exists)
      .reduce((accumulator, h) => {
        const historicalTypeKey = `[${h.historicalFileNumberTypeCode.id}-${h.otherHistFileNumberTypeCode}]`;
        if (!(historicalTypeKey in accumulator)) {
          accumulator[historicalTypeKey] = {
            historicalType: h.historicalFileNumberTypeCode,
            historicalValues: {},
            otherDescription: h.otherHistFileNumberTypeCode,
            key: '',
          };
        }

        const historicalValueKey = `[${h.historicalFileNumber}][${h.historicalFileNumberTypeCode.id}][${h.otherHistFileNumberTypeCode}]`;
        if (!(historicalValueKey in accumulator[historicalTypeKey])) {
          accumulator[historicalTypeKey].historicalValues[historicalValueKey] = h;
          accumulator[historicalTypeKey].key += h.propertyId;
        }
        return accumulator;
      }, {});

    const flatNumberArray = Object.values(flatNumberDictionary);

    return flatNumberArray.sort(
      (a, b) => a.historicalType.displayOrder - b.historicalType.displayOrder,
    );
  }, [historicalNumbers]);

  return (
    <ExpandableTextList<HistoricalGroup>
      items={uniqueNumbers}
      keyFunction={(p, index: number) =>
        `historical-group-${p.historicalType.id}-${p.propertyKey}-${index}`
      }
      renderFunction={p => (
        <>
          <StyledLabel key={`historical-group-${p.historicalType.id}`} className="pr-2">
            {p.historicalType.id === ApiGen_CodeTypes_HistoricalFileNumberTypes.OTHER
              ? p.otherDescription
              : p.historicalType.description}
            :
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
};

export const StyledLabel = styled.label`
  font-weight: bold;
`;
