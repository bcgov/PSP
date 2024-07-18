import { FaTrash } from 'react-icons/fa';
import { MdUndo } from 'react-icons/md';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import { LinkButton, StyledRemoveLinkButton } from '@/components/common/buttons';
import EditButton from '@/components/common/EditButton';
import { InlineFlexDiv } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { ColumnWithProps, renderDate, renderMoney } from '@/components/Table';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_Contact } from '@/models/api/generated/ApiGen_Concepts_Contact';
import { ApiGen_Concepts_SecurityDeposit } from '@/models/api/generated/ApiGen_Concepts_SecurityDeposit';
import { formatNames } from '@/utils/personUtils';
import { exists, isValidIsoDateTime } from '@/utils/utils';

export class DepositListEntry {
  public id: number;
  public depositTypeDescription: string;
  public depositDescription: string;
  public amountPaid: number;
  public paidDate: string;
  public contactHolder?: ApiGen_Concepts_Contact;
  public depositReturnCount: number;

  public constructor(baseDeposit: ApiGen_Concepts_SecurityDeposit) {
    this.id = baseDeposit.id || -1;
    if (baseDeposit.depositType?.id === 'OTHER') {
      this.depositTypeDescription = (baseDeposit.otherTypeDescription || '') + ' (Other)';
    } else {
      this.depositTypeDescription = baseDeposit.depositType?.description || '';
    }
    this.depositDescription = baseDeposit.description ?? '';
    this.amountPaid = baseDeposit.amountPaid;
    this.paidDate = isValidIsoDateTime(baseDeposit.depositDateOnly)
      ? baseDeposit.depositDateOnly
      : '';
    this.contactHolder = baseDeposit.contactHolder || undefined;
    this.depositReturnCount = baseDeposit.depositReturns?.length ?? 0;
  }
}

function renderHolder({
  row: { original },
}: CellProps<DepositListEntry, ApiGen_Concepts_Contact | undefined>) {
  if (exists(original.contactHolder)) {
    const holder = original.contactHolder;
    if (exists(holder.person)) {
      return (
        <Link to={`/contact/${holder.id}`}>
          {formatNames([holder.person.firstName, holder.person.middleNames, holder.person.surname])}
        </Link>
      );
    } else if (exists(holder.organization)) {
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
      <InlineFlexDiv>
        {hasClaim(Claims.LEASE_EDIT) && original.depositReturnCount > 0 && (
          <TooltipIcon
            toolTipId={`no-delete-tooltip-${original.id}`}
            toolTip="A deposit with associated return(s) cannot be deleted. To delete this deposit first delete any associated return(s)"
            innerClassName="mt-3"
          />
        )}
        {hasClaim(Claims.LEASE_EDIT) && (
          <EditButton title="edit deposit" onClick={() => onEdit(original.id)} />
        )}
        {hasClaim(Claims.LEASE_ADD) && (
          <LinkButton
            title="return deposit"
            icon={<MdUndo size={20} id={`return-deposit-${index}`} title="return deposit" />}
            onClick={() => onReturn(original.id)}
          />
        )}
        {hasClaim(Claims.LEASE_EDIT) && original.depositReturnCount === 0 && (
          <StyledRemoveLinkButton
            title="delete deposit"
            icon={<FaTrash size={20} id={`delete-deposit-${index}`} title="document deposit" />}
            onClick={() => original?.id && onDelete(original.id)}
          />
        )}
      </InlineFlexDiv>
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
