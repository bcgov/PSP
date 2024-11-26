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
  const historicalNumberGroup = useMemo(() => {
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
      moreText="more categories..."
      hideText="hide categories"
      items={Object.values(historicalNumberGroup)}
      keyFunction={p => `historical-number-group-${p?.historicalType?.id}-${p.otherDescription}`}
      renderFunction={group => (
        <GroupWrapper
          key={`historical-group-${group.historicalType.id}-${group.otherDescription ?? ''}`}
          data-testid={`historical-group-${group.historicalType.id}-${
            group.otherDescription ?? ''
          }`}
        >
          <StyledLabel className="pr-2">
            {group.historicalType.id === ApiGen_CodeTypes_HistoricalFileNumberTypes.OTHER
              ? group.otherDescription
              : group.historicalType.description}
            :
          </StyledLabel>
          <ExpandableTextList<ApiGen_Concepts_HistoricalFileNumber>
            items={Object.values(group.historicalValues)}
            keyFunction={p => `historical-number-${p.id}`}
            renderFunction={p => <>{p.historicalFileNumber}</>}
            delimiter={'; '}
            maxCollapsedLength={3}
            className="d-flex flex-wrap"
          />
        </GroupWrapper>
      )}
      maxCollapsedLength={2}
      className="d-flex flex-wrap"
    ></ExpandableTextList>
  );
};

export const StyledLabel = styled.label`
  font-weight: bold;
  margin-bottom: 0;
  min-width: fit-content;
`;

export const GroupWrapper = styled.div`
  display: flex;
  gap: 0.5rem;
  align-items: baseline;
  flex-wrap: wrap;
`;
