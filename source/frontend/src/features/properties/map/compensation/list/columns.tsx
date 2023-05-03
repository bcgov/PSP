import { LinkButton, StyledRemoveIconButton } from 'components/common/buttons';
import { InlineFlexDiv } from 'components/common/styles';
import { ColumnWithProps } from 'components/Table';
import Claims from 'constants/claims';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_Compensation, Api_CompensationFinancial } from 'models/api/Compensation';
import { FaTrash } from 'react-icons/fa';
import { CellProps } from 'react-table';
import styled from 'styled-components';
import { formatMoney, prettyFormatDate, stringToFragment } from 'utils';

export function createCompensationTableColumns(
  onShow: (compensationId: number) => void,
  onDelete: (compensationId: number) => void,
) {
  const columns: ColumnWithProps<Api_Compensation>[] = [
    {
      Header: 'Date',
      align: 'left',
      sortable: false,
      minWidth: 40,
      maxWidth: 40,
      Cell: (cellProps: CellProps<Api_Compensation>) => {
        return stringToFragment(prettyFormatDate(cellProps.row.original.agreementDateTime));
      },
    },
    {
      Header: 'Requisition #',
      accessor: 'id',
      align: 'left',
      sortable: false,
      minWidth: 40,
      maxWidth: 40,
      Cell: (cellProps: CellProps<Api_Compensation>) => {
        const { hasClaim } = useKeycloakWrapper();
        return hasClaim(Claims.COMPENSATION_REQUISITION_VIEW) ? (
          <LinkButton
            onClick={() => cellProps.row.original.id && onShow(cellProps.row.original.id)}
          >
            {cellProps.row.original.id}
          </LinkButton>
        ) : (
          stringToFragment(cellProps.row.original.id)
        );
      },
    },
    {
      Header: 'Amount',
      align: 'left',
      sortable: false,
      width: 30,
      maxWidth: 30,
      Cell: (cellProps: CellProps<Api_Compensation>) => {
        const totalAmount = cellProps.row.original.financials.reduce(
          (total: number, method: Api_CompensationFinancial) => {
            return total + (method.totalAmount ?? 0);
          },
          0,
        );
        return stringToFragment(formatMoney(totalAmount));
      },
    },
    {
      Header: 'Status',
      align: 'left',
      sortable: false,
      minWidth: 20,
      maxWidth: 20,
      Cell: (cellProps: CellProps<Api_Compensation>) => {
        return stringToFragment(cellProps.row.original.isDraft ? 'Draft' : 'Final');
      },
    },
    {
      Header: 'Actions',
      align: 'left',
      sortable: false,
      width: 20,
      maxWidth: 20,
      Cell: (cellProps: CellProps<Api_Compensation>) => {
        const { hasClaim } = useKeycloakWrapper();
        return (
          <StyledDiv>
            {hasClaim(Claims.COMPENSATION_REQUISITION_DELETE) &&
            cellProps.row.original.isDraft !== false ? (
              <StyledRemoveIconButton
                onClick={() => cellProps.row.original.id && onDelete(cellProps.row.original.id)}
                title="Delete Compensation"
              >
                <FaTrash size="2rem" />
              </StyledRemoveIconButton>
            ) : null}
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
`;
