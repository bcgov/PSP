import { CellProps } from 'react-table';
import styled from 'styled-components';

import { LinkButton, RemoveIconButton } from '@/components/common/buttons';
import ViewButton from '@/components/common/buttons/ViewButton';
import { InlineFlexDiv } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { ColumnWithProps } from '@/components/Table';
import Claims from '@/constants/claims';
import Roles from '@/constants/roles';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_CompensationFinancial } from '@/models/api/generated/ApiGen_Concepts_CompensationFinancial';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { formatMoney, prettyFormatDate, stringToFragment } from '@/utils';

import { cannotEditMessage } from '../../acquisition/common/constants';
import { IUpdateCompensationStrategy } from '../models/IUpdateCompensationStrategy';

export function createCompensationTableColumns(
  statusSolver: IUpdateCompensationStrategy,
  onShow: (compensationId: number) => void,
  onDelete: (compensationId: number) => void,
) {
  const columns: ColumnWithProps<ApiGen_Concepts_CompensationRequisition>[] = [
    {
      Header: 'Final Date',
      align: 'left',
      sortable: false,
      minWidth: 30,
      maxWidth: 30,
      Cell: (cellProps: CellProps<ApiGen_Concepts_CompensationRequisition>) => {
        return stringToFragment(prettyFormatDate(cellProps.row.original.finalizedDate));
      },
    },
    {
      Header: 'Requisition #',
      accessor: 'id',
      align: 'left',
      sortable: false,
      minWidth: 35,
      maxWidth: 35,
      Cell: (cellProps: CellProps<ApiGen_Concepts_CompensationRequisition>) => {
        const { hasClaim } = useKeycloakWrapper();
        return hasClaim(Claims.COMPENSATION_REQUISITION_VIEW) ? (
          <LinkButton
            onClick={() => cellProps.row.original.id && onShow(cellProps.row.original.id)}
          >
            {cellProps.row.original.id}
          </LinkButton>
        ) : (
          stringToFragment(cellProps.row.original.id ?? undefined)
        );
      },
    },
    {
      Header: 'Amount',
      align: 'left',
      sortable: false,
      width: 40,
      maxWidth: 40,
      Cell: (cellProps: CellProps<ApiGen_Concepts_CompensationRequisition>) => {
        const totalAmount = cellProps.row.original.financials?.reduce(
          (total: number, method: ApiGen_Concepts_CompensationFinancial) => {
            return total + (method.totalAmount ?? 0);
          },
          0,
        );
        const isFinal = cellProps.row.original.isDraft === false;
        const amount = formatMoney(totalAmount);
        return isFinal ? <b>{amount}</b> : stringToFragment(amount);
      },
    },
    {
      Header: 'Status',
      align: 'left',
      sortable: false,
      minWidth: 20,
      maxWidth: 20,
      Cell: (cellProps: CellProps<ApiGen_Concepts_CompensationRequisition>) => {
        return cellProps.row.original.isDraft ? stringToFragment('Draft') : <b>Final</b>;
      },
    },
    {
      Header: 'Actions',
      align: 'left',
      sortable: false,
      width: 20,
      maxWidth: 20,
      Cell: (cellProps: CellProps<ApiGen_Concepts_CompensationRequisition>) => {
        const { hasClaim, hasRole } = useKeycloakWrapper();
        const canEditDetails = (isDraft: boolean | null) => {
          if (
            statusSolver.canEditOrDeleteCompensation(isDraft, hasRole(Roles.SYSTEM_ADMINISTRATOR))
          ) {
            return true;
          }
          return false;
        };
        return (
          <StyledDiv className="no-gutters">
            {hasClaim(Claims.COMPENSATION_REQUISITION_VIEW) && (
              <ViewButton
                data-testId={`compensation-view-${cellProps.row.id}`}
                title="Compensation view details"
                id={`compensation-view-${cellProps.row.id}`}
                onClick={() => cellProps.row.original.id && onShow(cellProps.row.original.id)}
              />
            )}
            {hasClaim(Claims.COMPENSATION_REQUISITION_DELETE) &&
              canEditDetails(cellProps.row.original.isDraft) && (
                <RemoveIconButton
                  id={`compensation-delete-${cellProps.row.id}`}
                  data-testId={`compensation-delete-${cellProps.row.id}`}
                  onRemove={() => cellProps.row.original.id && onDelete(cellProps.row.original.id)}
                  title="Delete Compensation"
                />
              )}
            {hasClaim(Claims.COMPENSATION_REQUISITION_DELETE) &&
              !canEditDetails(cellProps.row.original.isDraft) && (
                <TooltipIcon
                  toolTipId={`${cellProps.row.original?.id || 0}-summary-cannot-edit-tooltip`}
                  toolTip={cannotEditMessage}
                />
              )}
          </StyledDiv>
        );
      },
    },
  ];

  return columns;
}

const StyledDiv = styled(InlineFlexDiv)`
  justify-content: space-around;
  width: 100%;

  .btn.btn-primary {
    background-color: transparent;
    padding: 0;
    margin-left: 0.5rem;
  }
`;
