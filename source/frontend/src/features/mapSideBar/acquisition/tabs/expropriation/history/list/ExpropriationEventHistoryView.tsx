import React from 'react';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { ApiGen_Concepts_ExpropriationEvent } from '@/models/api/generated/ApiGen_Concepts_ExpropriationEvent';

export interface IExpropriationEventHistoryViewProps {
  isLoading?: boolean;
  expropriationEvents: ApiGen_Concepts_ExpropriationEvent[];
}

export const ExpropriationEventHistoryView: React.FunctionComponent<
  IExpropriationEventHistoryViewProps
> = ({ isLoading, expropriationEvents }) => {
  return (
    <Section header="Expropriation Date History" isCollapsable initiallyExpanded={false}>
      <LoadingBackdrop show={isLoading} />
      {/* TODO: table goes here */}
    </Section>
  );
};

export default ExpropriationEventHistoryView;
