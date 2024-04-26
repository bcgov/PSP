import { FaHandHoldingUsd, FaMoneyCheckAlt, FaPencilRuler, FaPlus } from 'react-icons/fa';
import { useHistory, useRouteMatch } from 'react-router';
import styled from 'styled-components';

import { ToggleSaveInputContainer } from '@/components/common/form/ToggleSaveInput/ToggleSaveInputContainer';
import { ToggleSaveInputView } from '@/components/common/form/ToggleSaveInput/ToggleSaveInputView';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import Claims from '@/constants/claims';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { formatMoney } from '@/utils';

import StatusUpdateSolver from '../../fileDetails/detail/statusUpdateSolver';
import { CompensationResults } from './CompensationResults';

export interface ICompensationListViewProps {
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  compensations: ApiGen_Concepts_CompensationRequisition[];
  onAdd: () => void;
  onDelete: (compensationId: number) => void;
  onUpdateTotalCompensation: (totalAllowableCompensation: number | null) => Promise<number | null>;
}

export const CompensationListView: React.FunctionComponent<ICompensationListViewProps> = ({
  acquisitionFile,
  compensations,
  onAdd,
  onDelete,
  onUpdateTotalCompensation: onUpdateCompensation,
}) => {
  const history = useHistory();
  const match = useRouteMatch();

  const fileCompensationTotal = compensations
    .filter(x => !x.isDraft)
    .reduce((fileTotal: number, current: ApiGen_Concepts_CompensationRequisition) => {
      const compensationTotal =
        current.financials?.reduce(
          (financialTotal: number, financial: ApiGen_Concepts_CompensationFinancial) => {
            return financialTotal + (financial.totalAmount || 0);
          },
          0,
        ) || 0;
      return fileTotal + compensationTotal;
    }, 0);

  const fileDraftCompensationTotal = compensations
    .filter(x => x.isDraft)
    .reduce((fileTotal: number, current: ApiGen_Concepts_CompensationRequisition) => {
      const compensationTotal =
        current.financials?.reduce(
          (financialTotal: number, financial: ApiGen_Concepts_CompensationFinancial) => {
            return financialTotal + (financial.totalAmount || 0);
          },
          0,
        ) || 0;
      return fileTotal + compensationTotal;
    }, 0);

  const totalAllowableCompensation = acquisitionFile?.totalAllowableCompensation || 0;

  const statusSolver = new StatusUpdateSolver(acquisitionFile);

  return (
    <>
      <StyledSection
        header={
          <SectionListHeader
            claims={[Claims.COMPENSATION_REQUISITION_ADD]}
            title="Compensation Requisitions"
            addButtonText="Add a Requisition"
            addButtonIcon={<FaPlus size={'2rem'} />}
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
          statusSolver={statusSolver}
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
