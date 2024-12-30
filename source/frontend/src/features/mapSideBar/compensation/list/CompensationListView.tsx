import { FaHandHoldingUsd, FaMoneyCheckAlt, FaPencilRuler, FaPlus } from 'react-icons/fa';
import { useHistory, useRouteMatch } from 'react-router';
import styled from 'styled-components';

import { ToggleSaveInputContainer } from '@/components/common/form/ToggleSaveInput/ToggleSaveInputContainer';
import { ToggleSaveInputView } from '@/components/common/form/ToggleSaveInput/ToggleSaveInputView';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import Claims from '@/constants/claims';
import { LeaseStatusUpdateSolver } from '@/features/leases/models/LeaseStatusUpdateSolver';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { exists, formatMoney } from '@/utils';

import AcquisitionFileStatusUpdateSolver from '../../acquisition/tabs/fileDetails/detail/AcquisitionFileStatusUpdateSolver';
import { UpdateCompensationContext } from '../models/UpdateCompensationContext';
import { CompensationResults } from './CompensationResults';

export interface ICompensationListViewProps {
  fileType: ApiGen_CodeTypes_FileTypes;
  file: ApiGen_Concepts_File;
  compensationsResults: ApiGen_Concepts_CompensationRequisition[];
  subFilescompensations: ApiGen_Concepts_CompensationRequisition[] | null;
  isLoading: boolean;
  onAdd: () => void;
  onDelete: (compensationId: number) => void;
  onUpdateTotalCompensation: (totalAllowableCompensation: number | null) => Promise<number | null>;
}

export const CompensationListView: React.FunctionComponent<ICompensationListViewProps> = ({
  fileType,
  file,
  compensationsResults,
  subFilescompensations,
  isLoading,
  onAdd,
  onDelete,
  onUpdateTotalCompensation,
}) => {
  const history = useHistory();
  const match = useRouteMatch();

  const isSubfile =
    fileType === ApiGen_CodeTypes_FileTypes.Acquisition &&
    (file as ApiGen_Concepts_AcquisitionFile).parentAcquisitionFileId !== null;

  const calculateCompensationTotal = (
    compReqs: ApiGen_Concepts_CompensationRequisition[],
    isDraft: boolean,
  ): number => {
    if (!exists(compReqs)) {
      return 0;
    }

    return compReqs
      .filter(x => x.isDraft === isDraft)
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
  };

  const fileCompensationTotal = calculateCompensationTotal(compensationsResults, false);
  const fileDraftCompensationTotal = calculateCompensationTotal(compensationsResults, true);
  const mainAndSubFilesCompensationTotal = subFilescompensations
    ? calculateCompensationTotal(subFilescompensations, false) + fileCompensationTotal
    : null;

  let updateCompensationContext: UpdateCompensationContext | null;
  switch (fileType) {
    case ApiGen_CodeTypes_FileTypes.Acquisition:
      {
        const solver = new AcquisitionFileStatusUpdateSolver(file.fileStatusTypeCode);
        updateCompensationContext = new UpdateCompensationContext(solver);
      }
      break;
    case ApiGen_CodeTypes_FileTypes.Lease:
      {
        const solver = new LeaseStatusUpdateSolver(file.fileStatusTypeCode);
        updateCompensationContext = new UpdateCompensationContext(solver);
      }
      break;
    default:
      updateCompensationContext = null;
      break;
  }

  const getFileCalculatedCompensationTotalLabel = (): string => {
    if (fileType !== ApiGen_CodeTypes_FileTypes.Acquisition) {
      return 'Total payments made on this file';
    }

    if (isSubfile) {
      return 'Total payments made on the sub file';
    }

    return 'Total payments made on the main file';
  };

  return (
    <>
      <LoadingBackdrop show={isLoading} parentScreen={true} />

      <StyledSection
        header={
          <SectionListHeader
            claims={[Claims.COMPENSATION_REQUISITION_ADD]}
            title="Compensation Requisitions"
            addButtonText="Add Requisition"
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
              return (await onUpdateTotalCompensation(Number(value)))?.toString() ?? '';
            }}
            initialValue={file.totalAllowableCompensation?.toString() ?? ''}
            asCurrency
            View={ToggleSaveInputView}
          />
        </SectionField>
        <hr />

        <FlexDiv>
          <FaMoneyCheckAlt size={24} className="mr-4 mt-2" />
          <SectionField
            label={<>{getFileCalculatedCompensationTotalLabel()}</>}
            tooltip={`This is the total of all requisitions in the "Final" status. Draft entries are not included here.`}
            labelWidth="8"
            className="summary-row no-icon"
            valueClassName="text-right"
            valueTestId="payment-total-main-file"
          >
            {formatMoney(fileCompensationTotal)}
          </SectionField>
        </FlexDiv>

        {fileType === ApiGen_CodeTypes_FileTypes.Acquisition && !isSubfile && (
          <FlexDiv>
            <FaMoneyCheckAlt size={24} className="mr-4 mt-1" />
            <SectionField
              label="Total payments made on main file and all sub-files"
              tooltip={`This is the total of all requisitions in the "Final" status. Draft entries are not included here.`}
              labelWidth="8"
              className="summary-row no-icon"
              valueClassName="text-right"
              valueTestId="payment-total-subfiles"
            >
              {formatMoney(mainAndSubFilesCompensationTotal)}
            </SectionField>
          </FlexDiv>
        )}
        <hr />

        <FlexDiv>
          <FaPencilRuler size={24} className="mr-4 mt-2" />
          <SectionField
            label="Drafts"
            tooltip={`This is the total of all requisitions in the "Draft" state.`}
            labelWidth="9"
            className="summary-row no-icon"
            valueClassName="text-right"
            valueTestId="payment-total-drafts"
          >
            {formatMoney(fileDraftCompensationTotal)}
          </SectionField>
        </FlexDiv>
      </StyledSection>

      <Section header="Requisitions in this File (H120)" isCollapsable initiallyExpanded>
        <CompensationResults
          isLoading={isLoading}
          results={compensationsResults}
          statusSolver={updateCompensationContext}
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
    font: ${props => props.theme.bcTokens.typographyColorSecondary};
  }
  .summary-row {
    flex-grow: 1;
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

const FlexDiv = styled.div`
  display: flex;
  flex-direction: row;
  flex-grow: 1;
  align-items: flex-start;
`;
