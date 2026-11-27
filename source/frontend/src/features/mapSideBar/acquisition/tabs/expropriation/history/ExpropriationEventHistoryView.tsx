import React from 'react';
import { FaPlus } from 'react-icons/fa';

import { Section } from '@/components/common/Section/Section';
import { SimpleSectionHeader } from '@/components/common/SimpleSectionHeader';
import { StyledSectionAddButton } from '@/components/common/styles';
import { TableSort } from '@/components/Table/TableSort';

import { ExpropriationEventResults } from './list/ExpropriationEventResults';
import { ExpropriationEventRow } from './models';

export interface IExpropriationEventHistoryViewProps {
  isLoading?: boolean;
  eventRows: ExpropriationEventRow[];
  sort: TableSort<ExpropriationEventRow>;
  canEdit?: boolean;
  setSort: (value: TableSort<ExpropriationEventRow>) => void;
  onAdd: () => void;
  onUpdate: (expropriationEventId: number) => void;
  onDelete: (expropriationEventId: number) => void;
}

export const ExpropriationEventHistoryView: React.FunctionComponent<
  IExpropriationEventHistoryViewProps
> = ({ isLoading, eventRows, sort, setSort, onAdd, onUpdate, onDelete, canEdit }) => {
  return (
    <Section
      header={
        <SimpleSectionHeader title="Expropriation Date History">
          {canEdit && (
            <StyledSectionAddButton data-testid="add-expropriation-event" onClick={onAdd}>
              <FaPlus size="2rem" />
              &nbsp;{'Add'}
            </StyledSectionAddButton>
          )}
        </SimpleSectionHeader>
      }
      isCollapsable
      initiallyExpanded={false}
    >
      <ExpropriationEventResults
        loading={isLoading}
        results={eventRows}
        onUpdate={onUpdate}
        onDelete={onDelete}
        sort={sort}
        setSort={setSort}
        canEdit={canEdit}
      />
    </Section>
  );
};

export default ExpropriationEventHistoryView;
