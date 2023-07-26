import * as React from 'react';
import { FaHandHoldingUsd, FaMoneyCheckAlt, FaPencilRuler } from 'react-icons/fa';
import { useHistory, useRouteMatch } from 'react-router';
import styled from 'styled-components';

import { ToggleSaveInputContainer } from '@/components/common/form/ToggleSaveInput/ToggleSaveInputContainer';
import { ToggleSaveInputView } from '@/components/common/form/ToggleSaveInput/ToggleSaveInputView';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import Claims from '@/constants/claims';
import { Api_CompensationFinancial } from '@/models/api/CompensationFinancial';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { formatMoney } from '@/utils';

import { CompensationResults } from './CompensationResults';

export interface ICompensationListViewProps {
  compensations: Api_CompensationRequisition[];
  onAdd: () => void;
  onDelete: (compensationId: number) => void;
  onUpdateTotalCompensation: (totalAllowableCompensation: number | null) => Promise<number | null>;
  totalAllowableCompensation?: number;
}

export const CompensationListView: React.FunctionComponent<ICompensationListViewProps> = ({
  compensations,
  onAdd,
  onDelete,
  onUpdateTotalCompensation: onUpdateCompensation,
  totalAllowableCompensation,
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

  const fileDraftCompensationTotal = compensations
    .filter(x => x.isDraft)
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
      <StyledSection
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
          label={
            <>
              <FaHandHoldingUsd size={24} className="mr-4" />
              Total allowable compensation
            </>
          }
          tooltip={`This is the maximum allowable for this file. Edit to set or change this value.`}
          labelWidth="8"
          className="summary-row"
          valueClassName="text-right d-flex justify-content-end"
        >
          <ToggleSaveInputContainer
            onSave={async (value: string) => {
              return (await onUpdateCompensation(Number(value)))?.toString() ?? '';
            }}
            initialValue={totalAllowableCompensation?.toString() ?? ''}
            asCurrency
            View={ToggleSaveInputView}
          />
        </SectionField>
        <hr />
        <SectionField
          label={
            <>
              <FaMoneyCheckAlt size={24} className="mr-4" />
              Total payments made on this file
            </>
          }
          tooltip={`This is the total of all requisitions in the "Final" status. Draft entries are not included here.`}
          labelWidth="8"
          className="summary-row no-icon"
          valueClassName="text-right"
        >
          {formatMoney(fileCompensationTotal)}
        </SectionField>
        <hr />
        <SectionField
          label={
            <>
              <FaPencilRuler size={24} className="mr-4" />
              Drafts
            </>
          }
          tooltip={`This is the total of all requisitions in the "Draft" state.`}
          labelWidth="8"
          className="summary-row no-icon"
          valueClassName="text-right"
        >
          {formatMoney(fileDraftCompensationTotal)}
        </SectionField>
      </StyledSection>
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

const StyledSection = styled(Section)`
  & > {
    font: ${props => props.theme.css.textColor};
  }
  .summary-row {
    align-items: center;
    justify-content: space-between;
    min-height: 4.3rem;
    text-align: right;
    &.no-icon {
      margin-right: 2.2rem;
    }
  }
`;

export default CompensationListView;
