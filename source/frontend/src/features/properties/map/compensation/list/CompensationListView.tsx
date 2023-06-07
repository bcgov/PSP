import { SectionListHeader } from 'components/common/SectionListHeader';
import Claims from 'constants/claims';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Api_CompensationFinancial } from 'models/api/CompensationFinancial';
import { Api_CompensationRequisition } from 'models/api/CompensationRequisition';
import * as React from 'react';
import { useHistory, useRouteMatch } from 'react-router';
import { formatMoney } from 'utils';

import { CompensationResults } from './CompensationResults';

export interface ICompensationListViewProps {
  compensations: Api_CompensationRequisition[];
  onAdd: () => void;
  onDelete: (compensationId: number) => void;
}

export const CompensationListView: React.FunctionComponent<ICompensationListViewProps> = ({
  compensations,
  onAdd,
  onDelete,
}) => {
  const history = useHistory();
  const match = useRouteMatch();

  const fileCompensationTotal = compensations
    .filter(x => !x.isDraft)
    .reduce((fileTotal: number, current: Api_CompensationRequisition) => {
      const compensationTotal =
        current.financials?.reduce(
          (financialTotal: number, financial: Api_CompensationFinancial) => {
            return financialTotal + (financial.totalAmount || 0);
          },
          0,
        ) || 0;
      return fileTotal + compensationTotal;
    }, 0);

  return (
    <>
      <Section
        header={
          <SectionListHeader
            claims={[Claims.COMPENSATION_REQUISITION_ADD]}
            title="Add Compensation"
            addButtonText="Add a Requisition"
            addButtonIcon={'add'}
            onAdd={onAdd}
          />
        }
      >
        <SectionField
          label="Total payment amount for this file"
          tooltip={`This is the total of all requisitions in the "Final" status.\nDraft entries are not included here.`}
          labelWidth="9"
        >
          {formatMoney(fileCompensationTotal)}
        </SectionField>
      </Section>
      <Section header="Requisitions in this file (H120)" isCollapsable initiallyExpanded>
        <CompensationResults
          results={compensations}
          onShow={(compensationId: number) => {
            history.push(`${match.url}/compensation-requisition/${compensationId}`);
          }}
          onDelete={onDelete}
        />
      </Section>
    </>
  );
};

export default CompensationListView;
