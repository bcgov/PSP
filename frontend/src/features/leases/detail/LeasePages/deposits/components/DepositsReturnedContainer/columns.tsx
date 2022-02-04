import { Button } from 'components/common/form';
import { InlineFlexDiv } from 'components/common/styles';
import { ColumnWithProps, renderDate, renderMoney } from 'components/Table';
import Claims from 'constants/claims';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import {
  ILeaseSecurityDeposit,
  ILeaseSecurityDepositReturn,
  IOrganization,
  IPerson,
} from 'interfaces';
import { FaTrash } from 'react-icons/fa';
import { MdEdit } from 'react-icons/md';
import { CellProps } from 'react-table';
import styled from 'styled-components';

export class ReturnListEntry {
  public id: number;
  public depositTypeDescription: string;
  public terminationDate: string;
  public depositAmount: number;
  public claimsAgainst: number;
  public returnAmount: number;
  public returnDate: string;
  public personDepositHolder?: IPerson;
  public organizationDepositHolder?: IOrganization;

  public constructor(
    baseDeposit: ILeaseSecurityDepositReturn,
    parentDeposit: ILeaseSecurityDeposit,
  ) {
    this.id = baseDeposit.id || -1;
    if (baseDeposit.depositType.id === 'OTHER') {
      this.depositTypeDescription = (parentDeposit.otherTypeDescription || '') + ' (Other)';
    } else {
      this.depositTypeDescription = baseDeposit.depositType.description || '';
    }

    this.terminationDate = baseDeposit.terminationDate || '';
    this.depositAmount = parentDeposit.amountPaid;
    this.claimsAgainst = baseDeposit.claimsAgainst || 0;
    this.returnAmount = baseDeposit.returnAmount;
    this.returnDate = baseDeposit.returnDate || '';
    this.personDepositHolder = baseDeposit.personDepositReturnHolder;
    this.organizationDepositHolder = baseDeposit.organizationDepositReturnHolder;
  }
}

function renderPerson({ row: { original } }: CellProps<ReturnListEntry, string>) {
  return original.personDepositHolder?.fullName || '';
}

function depositActions(onEdit: (id: number) => void, onDelete: (id: number) => void) {
  return function({ row: { original, index } }: CellProps<ReturnListEntry, string>) {
    const { hasClaim } = useKeycloakWrapper();
    return (
      <StyledIcons>
        {hasClaim(Claims.LEASE_DELETE) && (
          <Button
            title="delete deposit return"
            icon={
              <FaTrash
                size={24}
                id={`delete-depositreturn-${index}`}
                title="delete depositreturn"
              />
            }
            onClick={() => original.id && onDelete(original.id)}
          ></Button>
        )}
        {hasClaim(Claims.LEASE_EDIT) && (
          <Button
            title="edit deposit return"
            icon={
              <MdEdit size={24} id={`edit-depositreturn-${index}`} title="edit depositreturn" />
            }
            onClick={() => onEdit(original.id)}
          ></Button>
        )}
      </StyledIcons>
    );
  };
}

export interface IPaymentColumnProps {
  onEdit: (id: number) => void;
  onDelete: (id: number) => void;
}

export const getColumns = ({
  onEdit,
  onDelete,
}: IPaymentColumnProps): ColumnWithProps<ReturnListEntry>[] => {
  return [
    {
      Header: 'Deposit Type',
      accessor: 'depositTypeDescription',
      maxWidth: 50,
    },
    {
      Header: 'Termination or Surrender Date',
      accessor: 'terminationDate',
      align: 'right',
      maxWidth: 50,
      Cell: renderDate,
    },
    {
      Header: 'Deposit amount',
      accessor: 'depositAmount',
      align: 'right',
      maxWidth: 40,
      Cell: renderMoney,
    },
    {
      Header: 'Claims against Deposit',
      accessor: 'claimsAgainst',
      align: 'right',
      maxWidth: 50,
      Cell: renderMoney,
    },
    {
      Header: 'Returned Amount',
      accessor: 'returnAmount',
      align: 'right',
      maxWidth: 50,
      Cell: renderMoney,
    },
    {
      Header: 'Return Date',
      accessor: 'returnDate',
      align: 'right',
      maxWidth: 50,
      Cell: renderDate,
    },

    {
      Header: 'Payee Name',
      accessor: 'personDepositHolder',
      maxWidth: 70,
      Cell: renderPerson,
    },
    {
      Header: 'Actions',
      align: 'right',
      maxWidth: 30,
      Cell: depositActions(onEdit, onDelete),
    },
  ];
};

const StyledIcons = styled(InlineFlexDiv)`
  [id^='edit-depositreturn'] {
    color: ${props => props.theme.css.slideOutBlue};
  }
  [id^='delete-depositreturn'] {
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
