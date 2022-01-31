import { Button } from 'components/common/form';
import { NotesModal } from 'components/common/form/NotesModal';
import { InlineFlexDiv } from 'components/common/styles';
import TooltipIcon from 'components/common/TooltipIcon';
import { ColumnWithProps, renderDate, renderMoney, renderTypeCode } from 'components/Table';
import { Claims } from 'constants/claims';
import { useKeycloakWrapper } from 'hooks/useKeycloakWrapper';
import { ILeasePayment } from 'interfaces';
import { FaTrash } from 'react-icons/fa';
import { MdEdit, MdReceipt } from 'react-icons/md';
import { CellProps } from 'react-table';
import styled from 'styled-components';
import { formatMoney } from 'utils/numberFormatUtils';

const actualsActions = (
  onEdit: (values: ILeasePayment) => void,
  onDelete: (values: ILeasePayment) => void,
) => {
  return function({ row: { original, index } }: CellProps<ILeasePayment, string>) {
    const { hasClaim } = useKeycloakWrapper();
    return hasClaim(Claims.LEASE_EDIT) ? (
      <StyledIcons>
        <Button
          title="edit actual"
          icon={<MdEdit size={24} id={`edit-actual-${index}`} title="edit actual" />}
          onClick={() => onEdit(original)}
        ></Button>
        <Button
          title="delete actual"
          icon={<FaTrash size={24} id={`delete-actual-${index}`} title="delete actual" />}
          onClick={() => original.id && onDelete(original)}
        ></Button>
      </StyledIcons>
    ) : null;
  };
};

export interface IPaymentColumnProps {
  onEdit: (values: ILeasePayment) => void;
  onDelete: (values: ILeasePayment) => void;
  isReceivable: boolean;
}

export const getActualsColumns = ({
  onEdit,
  onDelete,
  isReceivable,
}: IPaymentColumnProps): ColumnWithProps<ILeasePayment>[] => {
  return [
    {
      Header: isReceivable ? 'Received date' : 'Sent date',
      align: 'left',
      maxWidth: 70,
      accessor: 'receivedDate',
      Cell: renderDate,
      Footer: ({ properties }: { properties: ILeasePayment[] }) => (
        <span>
          <MdReceipt /> Payment Summary
        </span>
      ),
    },
    {
      Header: 'Payment method',
      accessor: 'leasePaymentMethodType',
      align: 'left',
      maxWidth: 60,
      Cell: renderTypeCode,
      Footer: ({ properties }: { properties: ILeasePayment[] }) => (
        <>({properties?.length}) payments</>
      ),
    },
    {
      Header: () => (
        <>
          {isReceivable ? 'Received payment ($)' : 'Sent payment ($)'}
          <TooltipIcon
            toolTipId="actualReceivedPaymentTooltip"
            toolTip="Actual payment amount, not including GST. This calculation can be overridden by editing the payment row."
          />
        </>
      ),
      accessor: 'amountPreTax',
      align: 'right',
      Cell: renderMoney,
      Footer: ({ properties }: { properties: ILeasePayment[] }) =>
        formatMoney(properties.reduce((total, current) => total + current.amountPreTax, 0)),
    },
    {
      Header: () => (
        <>
          GST ($)
          <TooltipIcon
            toolTipId="actualGstTooltip"
            toolTip="GST is calculated as (expected amount) x GST rate (5%). This calculation can be overridden by editing the payment row."
          />
        </>
      ),
      align: 'right',
      accessor: 'amountGst',
      maxWidth: 35,
      Cell: renderMoney,
      Footer: ({ properties }: { properties: ILeasePayment[] }) =>
        formatMoney(properties.reduce((total, current) => total + (current?.amountGst ?? 0), 0)),
    },
    {
      Header: () => (
        <>
          {isReceivable ? 'Received total ($)' : 'Sent total ($)'}
          <TooltipIcon
            toolTipId="receivedTotalTooltip"
            toolTip="Actual payment amount, including GST if applicable."
          />
        </>
      ),
      accessor: 'amountTotal',
      align: 'right',
      Cell: renderMoney,
      Footer: ({ properties }: { properties: ILeasePayment[] }) =>
        formatMoney(properties.reduce((total, current) => total + (current?.amountTotal ?? 0), 0)),
    },
    {
      Header: () => (
        <>
          Payment status
          <TooltipIcon
            toolTipId="paymentStatusTooltip"
            toolTip="Variance between expected and actual payment."
          />
        </>
      ),
      accessor: 'leasePaymentStatusTypeCode',
      align: 'right',
      maxWidth: 60,
      Cell: renderTypeCode,
    },
    {
      Header: 'Notes',
      maxWidth: 40,
      accessor: 'note',
      align: 'center',
      Cell: ({ value }: CellProps<ILeasePayment, boolean>) => {
        return <NotesModal title="Payment Notes" notesLabel="Notes:" />;
      },
    },
    {
      Header: 'Actions',
      align: 'right',
      maxWidth: 30,
      Cell: actualsActions(onEdit, onDelete),
    },
  ];
};

const StyledIcons = styled(InlineFlexDiv)`
  [id^='edit-actual'] {
    color: ${props => props.theme.css.slideOutBlue};
  }
  [id^='delete-actual'] {
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
