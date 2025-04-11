import React, { useMemo, useState } from 'react';
import { FaPlus } from 'react-icons/fa';

import { Section } from '@/components/common/Section/Section';
import { SimpleSectionHeader } from '@/components/common/SimpleSectionHeader';
import { StyledSectionAddButton } from '@/components/common/styles';
import { TableSort } from '@/components/Table/TableSort';
import { ApiGen_Concepts_ExpropriationEvent } from '@/models/api/generated/ApiGen_Concepts_ExpropriationEvent';

import { ExpropriationEventResults } from './list/ExpropriationEventResults';
import { ExpropriationEventRow } from './models';

export interface IExpropriationEventHistoryViewProps {
  isLoading?: boolean;
  expropriationEvents: ApiGen_Concepts_ExpropriationEvent[];
  onAdd: () => void;
  onUpdate: (expropriationEventId: number) => void;
  onDelete: (expropriationEventId: number) => void;
}

export const ExpropriationEventHistoryView: React.FunctionComponent<
  IExpropriationEventHistoryViewProps
> = ({ isLoading, expropriationEvents, onAdd, onUpdate, onDelete }) => {
  const [sort, setSort] = useState<TableSort<ExpropriationEventRow>>({});

  const sortedResults = useMemo(
    () => expropriationEvents.map(x => ExpropriationEventRow.fromApi(x)),
    [expropriationEvents],
  );

  return (
    <Section
      header={
        <SimpleSectionHeader title="Expropriation Date History">
          <StyledSectionAddButton onClick={onAdd}>
            <FaPlus size="2rem" />
            &nbsp;{'Add'}
          </StyledSectionAddButton>
        </SimpleSectionHeader>
      }
      isCollapsable
      initiallyExpanded={false}
    >
      <ExpropriationEventResults
        loading={isLoading}
        results={sortedResults}
        onUpdate={onUpdate}
        onDelete={onDelete}
        sort={sort}
        setSort={setSort}
      />
    </Section>
  );
};

export default ExpropriationEventHistoryView;
