import React from 'react';
import { FaTrash } from 'react-icons/fa';
import { MdEdit } from 'react-icons/md';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { InlineFlexDiv } from '@/components/common/styles';
import { ColumnWithProps, renderDate, renderMoney } from '@/components/Table';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_Contact } from '@/models/api/Contact';
import { Api_SecurityDeposit, Api_SecurityDepositReturn } from '@/models/api/SecurityDeposit';
import { formatNames } from '@/utils/personUtils';

export class ReturnListEntry {
  public id: number;
  public depositTypeDescription: string;
  public terminationDate: string;
  public depositAmount: number;
  public claimsAgainst: number;
  public returnAmount: number;
  public interestPaid: number;
  public returnDate: string;
  public contactHolder?: Api_Contact;

  public constructor(baseDeposit: Api_SecurityDepositReturn, parentDeposit: Api_SecurityDeposit) {
    this.id = baseDeposit.id || -1;
    if (parentDeposit.depositType.id === 'OTHER') {
      this.depositTypeDescription = (parentDeposit.otherTypeDescription || '') + ' (Other)';
    } else {
      this.depositTypeDescription = parentDeposit.depositType.description || '';
    }

    this.terminationDate = baseDeposit.terminationDate || '';
    this.depositAmount = parentDeposit.amountPaid;
    this.claimsAgainst = baseDeposit.claimsAgainst || 0;
    this.returnAmount = baseDeposit.returnAmount || 0;
    this.interestPaid = baseDeposit.interestPaid || 0;
    this.returnDate = baseDeposit.returnDate || '';
    this.contactHolder = baseDeposit.contactHolder || undefined;
  }
}

function renderHolder({ row: { original } }: CellProps<ReturnListEntry, Api_Contact | undefined>) {
  if (!!original.contactHolder) {
    const holder = original.contactHolder;
    if (!!holder.person) {
      return (
        <Link to={`/contact/${holder.id}`}>
          {formatNames([holder.person.firstName, holder.person.middleNames, holder.person.surname])}
        </Link>
      );
    } else if (!!holder.organization) {
      return <Link to={`/contact/${holder.id}`}>{holder.organization.name}</Link>;
    }
  }

  return <></>;
}

function depositActions(onEdit: (id: number) => void, onDelete: (id: number) => void) {
  return function ({ row: { original, index } }: CellProps<ReturnListEntry, string>) {
    const { hasClaim } = useKeycloakWrapper();
    return (
      <StyledIcons>
        {hasClaim(Claims.LEASE_EDIT) && (
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
      Header: 'Deposit type',
      accessor: 'depositTypeDescription',
      maxWidth: 50,
    },
    {
      Header: 'Termination or Surrender date',
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
      Header: 'Claims against deposit',
      accessor: 'claimsAgainst',
      align: 'right',
      maxWidth: 50,
      Cell: renderMoney,
    },
    {
      Header: 'Returned amount (without interest)',
      accessor: 'returnAmount',
      align: 'right',
      maxWidth: 50,
      Cell: renderMoney,
    },
    {
      Header: 'Interest paid',
      accessor: 'interestPaid',
      align: 'right',
      maxWidth: 50,
      Cell: renderMoney,
    },
    {
      Header: 'Return date',
      accessor: 'returnDate',
      align: 'right',
      maxWidth: 50,
      Cell: renderDate,
    },

    {
      Header: 'Payee name',
      accessor: 'contactHolder',
      maxWidth: 70,
      Cell: renderHolder,
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
