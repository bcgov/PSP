import React from 'react';
import { FaTrash } from 'react-icons/fa';
import { MdEdit, MdUndo } from 'react-icons/md';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { InlineFlexDiv } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { ColumnWithProps, renderDate, renderMoney } from '@/components/Table';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_Contact } from '@/models/api/Contact';
import { Api_SecurityDeposit } from '@/models/api/SecurityDeposit';
import { formatNames } from '@/utils/personUtils';

export class DepositListEntry {
  public id: number;
  public depositTypeDescription: string;
  public depositDescription: string;
  public amountPaid: number;
  public paidDate: string;
  public contactHolder?: Api_Contact;
  public depositReturnCount: number;

  public constructor(baseDeposit: Api_SecurityDeposit) {
    this.id = baseDeposit.id || -1;
    if (baseDeposit.depositType.id === 'OTHER') {
      this.depositTypeDescription = (baseDeposit.otherTypeDescription || '') + ' (Other)';
    } else {
      this.depositTypeDescription = baseDeposit.depositType.description || '';
    }
    this.depositDescription = baseDeposit.description;
    this.amountPaid = baseDeposit.amountPaid;
    this.paidDate = baseDeposit.depositDate || '';
    this.contactHolder = baseDeposit.contactHolder || undefined;
    this.depositReturnCount = baseDeposit.depositReturns.length;
  }
}

function renderHolder({ row: { original } }: CellProps<DepositListEntry, Api_Contact | undefined>) {
  if (original.contactHolder !== undefined && original.contactHolder !== null) {
    const holder = original.contactHolder;
    if (holder.person !== undefined && holder.person !== null) {
      return (
        <Link to={`/contact/${holder.id}`}>
          {formatNames([holder.person.firstName, holder.person.middleNames, holder.person.surname])}
        </Link>
      );
    } else if (holder.organization !== undefined && holder.organization !== null) {
      return <Link to={`/contact/${holder.id}`}>{holder.organization.name}</Link>;
    }
  }

  return <></>;
}

function depositActions(
  onEdit: (id: number) => void,
  onDelete: (id: number) => void,
  onReturn: (id: number) => void,
) {
  return function ({ row: { original, index } }: CellProps<DepositListEntry, string>) {
    const { hasClaim } = useKeycloakWrapper();
    return (
      <StyledIcons>
        {hasClaim(Claims.LEASE_EDIT) && original.depositReturnCount === 0 && (
          <Button
            title="delete deposit"
            icon={<FaTrash size={24} id={`delete-deposit-${index}`} title="delete deposit" />}
            onClick={() => original.id && onDelete(original.id)}
          ></Button>
        )}
        {hasClaim(Claims.LEASE_EDIT) && original.depositReturnCount > 0 && (
          <TooltipIcon
            toolTipId={`no-delete-tooltip-${original.id}`}
            toolTip="A deposit with associated return(s) cannot be deleted. To delete this deposit first delete any associated return(s)."
          />
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
            icon={<MdUndo size={24} id={`return-deposit-${index}`} title="return deposit" />}
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
      Header: 'Deposit type',
      accessor: 'depositTypeDescription',
      maxWidth: 50,
    },
    {
      Header: 'Description',
      accessor: 'depositDescription',
      minWidth: 150,
    },
    {
      Header: 'Amount paid',
      accessor: 'amountPaid',
      align: 'right',
      maxWidth: 40,
      Cell: renderMoney,
    },
    {
      Header: 'Paid date',
      accessor: 'paidDate',
      align: 'right',
      maxWidth: 50,
      Cell: renderDate,
    },
    {
      Header: 'Deposit holder',
      accessor: 'contactHolder',
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
  align-items: center;
  [id^='edit-deposit'] {
    color: ${props => props.theme.css.slideOutBlue};
  }
  [id^='return-deposit'] {
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
