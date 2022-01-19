import { Button } from 'components/common/form';
import { InlineFlexDiv } from 'components/common/styles';
import TooltipIcon from 'components/common/TooltipIcon';
import {
  ColumnWithProps,
  renderBooleanAsYesNo,
  renderMoney,
  renderTypeCode,
} from 'components/Table';
import { ILeasePayment } from 'interfaces';
import { IFormLeaseTerm, ILeaseTerm } from 'interfaces/ILeaseTerm';
import moment from 'moment';
import { FaTrash } from 'react-icons/fa';
import { MdEdit } from 'react-icons/md';
import { CellProps } from 'react-table';
import { ISystemConstant } from 'store/slices/systemConstants';
import styled from 'styled-components';
import { formatMoney, prettyFormatDate } from 'utils';

function initialOrRenewalTerm({ row: { original, index } }: CellProps<ILeaseTerm, string>) {
  return index === 0 ? 'Initial term' : `Renewal ${index}`;
}

function startAndEndDate({ row: { original, index } }: CellProps<ILeaseTerm, string>) {
  return `${prettyFormatDate(original.startDate)} - ${prettyFormatDate(original.expiryDate)}`;
}

const renderGst = (gstDecimal?: number) =>
  function({ row: { original, index } }: CellProps<ILeaseTerm, string>) {
    gstDecimal = original.isGstEligible ? gstDecimal : undefined;
    return gstDecimal !== undefined && original.paymentAmount !== undefined
      ? formatMoney(calculateExpectedGst(original.paymentAmount, gstDecimal))
      : '-';
  };

const renderExpectedTotal = (gstDecimal?: number) =>
  function({ row: { original, index } }: CellProps<ILeaseTerm, string>) {
    gstDecimal = original.isGstEligible ? gstDecimal : 0;
    return gstDecimal !== undefined && original.paymentAmount !== undefined
      ? formatMoney(calculateExpectedTotal(original.paymentAmount, gstDecimal))
      : '-';
  };

function renderActualTotal({ row: { original, index } }: CellProps<ILeaseTerm, string>) {
  const total = formatMoney(
    (original.payments ?? []).reduce((sum: number, p: ILeasePayment) => (sum += p.amountTotal), 0),
  );
  return original.isTermExercised ? total : '-';
}

const renderExpectedTerm = (gstDecimal?: number) =>
  function({ row: { original, index } }: CellProps<ILeaseTerm, string>) {
    gstDecimal = original.isGstEligible ? gstDecimal : undefined;
    if (!original.startDate || !original.expiryDate || original.paymentAmount === undefined) {
      return '-';
    }
    const expectedTerm = calculateExpectedTermAmount(
      original.leasePmtFreqTypeCode?.description ?? '',
      original.startDate,
      original.expiryDate,
      original.paymentAmount,
      gstDecimal ?? 0,
    );
    return expectedTerm !== undefined ? formatMoney(expectedTerm) : '-';
  };

function calculateExpectedGst(amount: number, gst: number) {
  return amount * (gst / 100);
}

function calculateExpectedTotal(amount: number, gst: number) {
  return amount + calculateExpectedGst(amount, gst);
}

function getNumberOfIntervals(frequency: string, startDate: string, endDate: string) {
  const duration = moment.duration(moment(endDate).diff(moment(startDate)));
  switch (frequency.toUpperCase()) {
    case 'ANNUAL':
      return Math.ceil(duration.asYears());
    case 'SEMIANN':
      return Math.ceil(duration.asMonths() / 6);
    case 'QUARTER':
      return Math.ceil(duration.asMonths() / 3);
    case 'BIMONTH':
      return Math.ceil(duration.asMonths() / 2);
    case 'MONTHLY':
      return Math.ceil(duration.asMonths());
    case 'BIWEEK':
      return Math.ceil(duration.asWeeks() / 2);
    case 'WEEKLY':
      return Math.ceil(duration.asWeeks());
    case 'DAILY':
      return duration.asDays();
    case 'PREPAID':
    case 'NOMINAL':
      return 1;
    default:
      return undefined;
  }
}

function calculateExpectedTermAmount(
  frequency: string,
  startDate: string,
  endDate: string,
  expectedAmount: number,
  gstDecimal: number,
) {
  const numberOfIntervals = getNumberOfIntervals(frequency, startDate, endDate);
  const expectedPayment = calculateExpectedTotal(expectedAmount, gstDecimal);
  return !!numberOfIntervals ? numberOfIntervals * expectedPayment : undefined;
}

const termActions = (
  onEdit: (values: IFormLeaseTerm) => void,
  onDelete: (values: IFormLeaseTerm) => void,
) => {
  return function({ row: { original, index } }: CellProps<IFormLeaseTerm, string>) {
    return (
      <StyledIcons>
        <Button
          icon={<MdEdit size={24} id={`edit-term-${index}`} title="edit term" />}
          onClick={() => onEdit(original)}
        ></Button>
        <Button
          icon={<FaTrash size={24} id={`delete-term-${index}`} title="delete term" />}
          onClick={() => original.id && onDelete(original)}
        ></Button>
      </StyledIcons>
    );
  };
};

export interface IPaymentColumnProps {
  onEdit: (values: IFormLeaseTerm) => void;
  onDelete: (values: IFormLeaseTerm) => void;
  gstConstant?: ISystemConstant;
}

export const getColumns = ({
  onEdit,
  onDelete,
  gstConstant,
}: IPaymentColumnProps): ColumnWithProps<IFormLeaseTerm>[] => {
  const gstDecimal = gstConstant !== undefined ? parseFloat(gstConstant.value) : undefined;
  return [
    {
      Header: '',
      accessor: 'id',
      maxWidth: 30,
      Cell: initialOrRenewalTerm,
    },
    {
      Header: 'Start date - End date',
      minWidth: 80,
      Cell: startAndEndDate,
    },
    {
      Header: 'Payment frequency',
      accessor: 'leasePmtFreqTypeCode',
      align: 'left',
      maxWidth: 40,
      Cell: renderTypeCode,
    },
    {
      Header: 'Payment due',
      accessor: 'paymentDueDate',
      align: 'right',
      maxWidth: 50,
    },
    {
      Header: () => (
        <>
          Expected payment ($)
          <TooltipIcon
            toolTipId="expectedPaymentTooltip"
            toolTip="This is the amount agreed to be paid per interval, ie: the amount of a monthly payment if the lease is paid monthly."
          />
        </>
      ),
      align: 'right',
      accessor: 'paymentAmount',
      maxWidth: 65,
      Cell: renderMoney,
    },
    {
      Header: 'GST?',
      align: 'right',
      accessor: 'isGstEligible',
      maxWidth: 25,
      Cell: renderBooleanAsYesNo,
    },
    {
      Header: () => (
        <>
          GST ($)
          <TooltipIcon
            toolTipId="gstAmountTooltip"
            toolTip="GST is calculated as (expected amount) x GST rate (5%). This calculation can be overridden by editing the payment row."
          />
        </>
      ),
      id: 'gstAmount',
      align: 'right',
      maxWidth: 29,
      Cell: renderGst(gstDecimal),
    },
    {
      Header: () => (
        <>
          Expected total ($)
          <TooltipIcon
            toolTipId="expectedTotalTooltip"
            toolTip="This is the expected payment amount plus GST if applicable."
          />
        </>
      ),
      id: 'expectedTotal',
      align: 'right',
      maxWidth: 55,
      Cell: renderExpectedTotal(gstDecimal),
    },
    {
      Header: () => (
        <>
          Expected term ($)
          <TooltipIcon
            toolTipId="expectedTermTooltip"
            toolTip="This is the full payment amount expected in the duration of the term."
          />
        </>
      ),
      id: 'expectedTerm',
      align: 'right',
      maxWidth: 55,
      Cell: renderExpectedTerm(gstDecimal),
    },
    {
      Header: () => (
        <>
          Actual total ($)
          <TooltipIcon toolTipId="actualTotalTooltip" toolTip="Amount paid this term." />
        </>
      ),
      id: 'actualTotal',
      align: 'right',
      maxWidth: 55,
      Cell: renderActualTotal,
    },
    {
      Header: 'Exercised?',
      align: 'right',
      accessor: 'isTermExercised',
      maxWidth: 40,
      Cell: renderBooleanAsYesNo,
    },
    {
      Header: 'Actions',
      align: 'right',
      maxWidth: 30,
      Cell: termActions(onEdit, onDelete),
    },
  ];
};

const StyledIcons = styled(InlineFlexDiv)`
  [id^='edit-term'] {
    color: ${props => props.theme.css.slideOutBlue};
  }
  [id^='delete-term'] {
    color: ${props => props.theme.css.discardedColor};
  }
  .btn {
    background-color: transparent;
    padding: 0;
  }
`;
