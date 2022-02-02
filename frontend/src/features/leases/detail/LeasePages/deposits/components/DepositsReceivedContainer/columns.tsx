import { Button } from 'components/common/form';
import { InlineFlexDiv } from 'components/common/styles';
import { ColumnWithProps, renderDate, renderMoney } from 'components/Table';
import Claims from 'constants/claims';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { ILeaseSecurityDeposit, IOrganization, IPerson } from 'interfaces';
import { FaTrash } from 'react-icons/fa';
import { MdEdit, MdUndo } from 'react-icons/md';
import { CellProps } from 'react-table';
import styled from 'styled-components';

export class DepositListEntry {
  public id: number;
  public depositTypeDescription: string;
  public depositDescription: string;
  public amountPaid: number;
  public paidDate: string;
  public personDepositHolder?: IPerson;
  public organizationDepositHolder?: IOrganization;
  public depositReturnCount: number;

  public constructor(baseDeposit: ILeaseSecurityDeposit, depositReturnCount: number) {
    this.id = baseDeposit.id || -1;
    if (baseDeposit.depositType.id === 'OTHER') {
      this.depositTypeDescription = (baseDeposit.otherTypeDescription || '') + ' (Other)';
    } else {
      this.depositTypeDescription = baseDeposit.depositType.description || '';
    }
    this.depositDescription = baseDeposit.description;
    this.amountPaid = baseDeposit.amountPaid;
    this.paidDate = baseDeposit.depositDate || '';
    this.personDepositHolder = baseDeposit.personDepositHolder;
    this.organizationDepositHolder = baseDeposit.organizationDepositHolder;
    this.depositReturnCount = depositReturnCount;
  }
}

function renderHolder({ row: { original } }: CellProps<DepositListEntry, string>) {
  return original.personDepositHolder?.fullName || '';
}

function depositActions(
  onEdit: (id: number) => void,
  onDelete: (id: number) => void,
  onReturn: (id: number) => void,
) {
  return function({ row: { original, index } }: CellProps<DepositListEntry, string>) {
    const { hasClaim } = useKeycloakWrapper();
    return (
      <StyledIcons>
        {hasClaim(Claims.LEASE_DELETE) && original.depositReturnCount === 0 && (
          <Button
            title="delete deposit"
            icon={<FaTrash size={24} id={`delete-deposit-${index}`} title="delete deposit" />}
            onClick={() => original.id && onDelete(original.id)}
          ></Button>
        )}
        {hasClaim(Claims.LEASE_EDIT) && (
          <Button
            title="edit deposit"
            icon={<MdEdit size={24} id={`edit-deposit-${index}`} title="edit deposit" />}
            onClick={() => onEdit(original.id)}
          ></Button>
        )}
        {hasClaim(Claims.LEASE_ADD) && (
          <Button
            title="return deposit"
            icon={<MdUndo size={24} id={`edit-deposit-${index}`} title="edit deposit" />}
            onClick={() => onReturn(original.id)}
          ></Button>
        )}
      </StyledIcons>
    );
  };
}

export interface IPaymentColumnProps {
  onEdit: (id: number) => void;
  onDelete: (id: number) => void;
  onReturn: (id: number) => void;
}

export const getColumns = ({
  onEdit,
  onDelete,
  onReturn,
}: IPaymentColumnProps): ColumnWithProps<DepositListEntry>[] => {
  return [
    {
      Header: 'Deposit Type',
      accessor: 'depositTypeDescription',
      maxWidth: 50,
    },
    {
      Header: 'Description',
      accessor: 'depositDescription',
      minWidth: 150,
    },
    {
      Header: 'Amount Paid',
      accessor: 'amountPaid',
      align: 'right',
      maxWidth: 40,
      Cell: renderMoney,
    },
    {
      Header: 'Paid Date',
      accessor: 'paidDate',
      align: 'right',
      maxWidth: 50,
      Cell: renderDate,
    },
    {
      Header: 'Deposit holder',
      accessor: 'personDepositHolder',
      maxWidth: 60,
      Cell: renderHolder,
    },
    {
      Header: 'Actions',
      align: 'right',
      maxWidth: 30,
      Cell: depositActions(onEdit, onDelete, onReturn),
    },
  ];
};

const StyledIcons = styled(InlineFlexDiv)`
  [id^='edit-deposit'] {
    color: ${props => props.theme.css.slideOutBlue};
  }
  [id^='delete-deposit'] {
    color: ${props => props.theme.css.discardedColor};
    :hover {
      color: ${({ theme }) => theme.css.dangerColor};
    }
  }
  .btn.btn-primary {
    background-color: transparent;
    padding: 0;
  }
`;
