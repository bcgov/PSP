import { Col } from 'react-bootstrap';
import { FaEye, FaTrash } from 'react-icons/fa';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { LinkButton, StyledRemoveIconButton } from '@/components/common/buttons';
import { Button } from '@/components/common/buttons/Button';
import { InlineFlexDiv } from '@/components/common/styles';
import { ColumnWithProps } from '@/components/Table';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_CompensationFinancial } from '@/models/api/CompensationFinancial';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { formatMoney, prettyFormatDate, stringToFragment } from '@/utils';

export function createCompensationTableColumns(
  onShow: (compensationId: number) => void,
  onDelete: (compensationId: number) => void,
) {
  const columns: ColumnWithProps<Api_CompensationRequisition>[] = [
    {
      Header: 'Final Date',
      align: 'left',
      sortable: false,
      minWidth: 40,
      maxWidth: 40,
      Cell: (cellProps: CellProps<Api_CompensationRequisition>) => {
        return stringToFragment(prettyFormatDate(cellProps.row.original.finalizedDate));
      },
    },
    {
      Header: 'Requisition #',
      accessor: 'id',
      align: 'left',
      sortable: false,
      minWidth: 40,
      maxWidth: 40,
      Cell: (cellProps: CellProps<Api_CompensationRequisition>) => {
        const { hasClaim } = useKeycloakWrapper();
        return hasClaim(Claims.COMPENSATION_REQUISITION_VIEW) ? (
          <LinkButton
            onClick={() => cellProps.row.original.id && onShow(cellProps.row.original.id)}
          >
            {cellProps.row.original.id}
          </LinkButton>
        ) : (
          stringToFragment(cellProps.row.original.id!)
        );
      },
    },
    {
      Header: 'Amount',
      align: 'left',
      sortable: false,
      width: 30,
      maxWidth: 30,
      Cell: (cellProps: CellProps<Api_CompensationRequisition>) => {
        const totalAmount = cellProps.row.original.financials?.reduce(
          (total: number, method: Api_CompensationFinancial) => {
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
      Cell: (cellProps: CellProps<Api_CompensationRequisition>) => {
        return cellProps.row.original.isDraft ? stringToFragment('Draft') : <b>Final</b>;
      },
    },
    {
      Header: 'Actions',
      align: 'left',
      sortable: false,
      width: 20,
      maxWidth: 20,
      Cell: (cellProps: CellProps<Api_CompensationRequisition>) => {
        const { hasClaim } = useKeycloakWrapper();
        return (
          <StyledDiv className="no-gutters">
            {hasClaim(Claims.COMPENSATION_REQUISITION_VIEW) && (
              <Col>
                <Button
                  icon={
                    <FaEye
                      size={24}
                      id={`compensation-view-${cellProps.row.id}`}
                      data-testid={`compensation-view-${cellProps.row.id}`}
                      title="Compensation view details"
                    />
                  }
                  onClick={() => cellProps.row.original.id && onShow(cellProps.row.original.id)}
                ></Button>
              </Col>
            )}
            {hasClaim(Claims.COMPENSATION_REQUISITION_DELETE) &&
            cellProps.row.original.isDraft !== false ? (
              <StyledRemoveIconButton
                id={`compensation-delete-${cellProps.row.id}`}
                data-testid={`compensation-delete-${cellProps.row.id}`}
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

  [id^='compensation-view'] {
    color: ${props => props.theme.css.slideOutBlue};
  }
  [id^='compensation-delete'] {
    color: ${props => props.theme.css.discardedColor};
    :hover {
      color: ${({ theme }) => theme.css.dangerColor};
    }
  }

  .btn.btn-primary {
    background-color: transparent;
    padding: 0;
    margin-left: 0.5rem;
  }
`;
