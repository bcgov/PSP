import moment from 'moment';
import { FaFileContract, FaTrash } from 'react-icons/fa';
import { MdEdit } from 'react-icons/md';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { InlineFlexDiv } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import {
  ColumnWithProps,
  renderBooleanAsYesNo,
  renderMoney,
  renderTypeCode,
} from '@/components/Table';
import { Claims } from '@/constants';
import { LeaseTermStatusTypes } from '@/constants/leaseStatusTypes';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ISystemConstant } from '@/store/slices/systemConstants';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { prettyFormatDate } from '@/utils';
import { stringToFragment } from '@/utils/columnUtils';
import { formatMoney } from '@/utils/numberFormatUtils';

import { FormLeaseTerm } from '../../models';

function initialOrRenewalTerm({ row: { index } }: CellProps<FormLeaseTerm, unknown>) {
  return stringToFragment(index === 0 ? 'Initial term' : `Renewal ${index}`);
}

function startAndEndDate({ row: { original } }: CellProps<FormLeaseTerm, string>) {
  return stringToFragment(
    `${prettyFormatDate(original.startDate)} - ${prettyFormatDate(original.expiryDate)}`,
  );
}

const renderExpectedTotal = () =>
  function ({ row: { original } }: CellProps<FormLeaseTerm, string>) {
    return stringToFragment(
      original.paymentAmount !== undefined
        ? formatMoney((original.paymentAmount as number) + ((original?.gstAmount as number) ?? 0))
        : '-',
    );
  };

function renderGstAmount({ row: { original } }: CellProps<FormLeaseTerm, NumberFieldValue>) {
  return stringToFragment(
    original.isGstEligible === true ? formatMoney(original?.gstAmount ?? 0) : '-',
  );
}

function renderActualTotal({ row: { original } }: CellProps<FormLeaseTerm, string>) {
  const total = formatMoney(
    (original.payments ?? []).reduce((sum: number, p) => (sum += p.amountTotal as number), 0),
  );
  return stringToFragment(original.isTermExercised ? total : '-');
}

const renderExpectedTerm = () =>
  function ({ row: { original } }: CellProps<FormLeaseTerm, string>) {
    if (!original.startDate || !original.expiryDate || original.paymentAmount === undefined) {
      return stringToFragment('-');
    }
    const expectedTerm = calculateExpectedTermAmount(
      original.leasePmtFreqTypeCode?.id ?? '',
      original.startDate,
      original.expiryDate,
      original.paymentAmount as number,
      (original.gstAmount as number) ?? 0,
    );
    return stringToFragment(expectedTerm !== undefined ? formatMoney(expectedTerm) : '-');
  };

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
  gstAmount: number,
) {
  const numberOfIntervals = getNumberOfIntervals(frequency, startDate, endDate);
  const expectedPayment = expectedAmount + gstAmount;
  return numberOfIntervals ? numberOfIntervals * expectedPayment : undefined;
}

const termActions = (
  onEdit: (values: FormLeaseTerm) => void,
  onDelete: (values: FormLeaseTerm) => void,
  onGenerate: () => void,
  leaseTypeCode?: string,
) => {
  return function ({ row: { original, index } }: CellProps<FormLeaseTerm, string>) {
    const { hasClaim } = useKeycloakWrapper();
    return (
      <StyledIcons>
        {hasClaim(Claims.LEASE_VIEW) &&
          index === 0 &&
          !!leaseTypeCode &&
          ['LIOCCACCS', 'LIOCCTTLD', 'LIOCCUSE', 'LIOCCUTIL'].includes(leaseTypeCode) && (
            <Button
              title="Generate H1005(a)"
              icon={<GenerateIcon size={24} id={`generate-h1005-a`} title="Generate H1005(a)" />}
              onClick={() => onGenerate()}
            ></Button>
          )}
        {hasClaim(Claims.LEASE_EDIT) && (
          <Button
            title="edit term"
            icon={<MdEdit size={24} id={`edit-term-${index}`} title="edit term" />}
            onClick={() => onEdit(original)}
          ></Button>
        )}
        {hasClaim(Claims.LEASE_EDIT) &&
          original.payments.length <= 0 &&
          original.statusTypeCode?.id !== LeaseTermStatusTypes.EXERCISED && (
            <Button
              title="delete term"
              icon={<FaTrash size={24} id={`delete-term-${index}`} title="delete term" />}
              onClick={() => original.id && onDelete(original)}
            ></Button>
          )}
        {hasClaim(Claims.LEASE_EDIT) &&
          (original.payments.length > 0 ||
            original.statusTypeCode?.id === LeaseTermStatusTypes.EXERCISED) && (
            <TooltipIcon
              toolTipId={`no-delete-tooltip-term-${original.id}`}
              toolTip="An exercised term cannot be deleted. To delete this term ensure that there are no payments recorded for it, and the term has not been exercised."
            />
          )}
      </StyledIcons>
    );
  };
};

export interface IPaymentColumnProps {
  onEdit: (values: FormLeaseTerm) => void;
  onDelete: (values: FormLeaseTerm) => void;
  onGenerate: () => void;
  leaseTypeCode?: string;
  gstConstant?: ISystemConstant;
}

export const getLeaseTermColumns = ({
  onEdit,
  onDelete,
  onGenerate,
  leaseTypeCode,
}: IPaymentColumnProps): ColumnWithProps<FormLeaseTerm>[] => {
  return [
    {
      Header: '',
      id: 'initialOrRenewal',
      align: 'left',
      maxWidth: 50,
      Cell: initialOrRenewalTerm,
    },
    {
      Header: 'Start date - End date',
      align: 'center',
      minWidth: 60,
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
      accessor: 'paymentDueDateStr',
      align: 'left',
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
      maxWidth: 70,
      Cell: renderMoney,
    },
    {
      Header: 'GST?',
      align: 'left',
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
            toolTip="GST is calculated as (expected amount) x GST rate (5%)."
          />
        </>
      ),
      accessor: 'gstAmount',
      align: 'right',
      maxWidth: 35,
      Cell: renderGstAmount,
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
      maxWidth: 60,
      Cell: renderExpectedTotal(),
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
      maxWidth: 60,
      Cell: renderExpectedTerm(),
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
      align: 'left',
      accessor: 'isTermExercised',
      maxWidth: 40,
      Cell: renderBooleanAsYesNo,
    },
    {
      Header: 'Actions',
      align: 'right',
      maxWidth: 30,
      Cell: termActions(onEdit, onDelete, onGenerate, leaseTypeCode),
    },
  ];
};

const StyledIcons = styled(InlineFlexDiv)`
  align-items: center;
  [id^='edit-term'] {
    color: ${props => props.theme.css.slideOutBlue};
  }
  [id^='delete-term'] {
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

const GenerateIcon = styled(FaFileContract)`
  color: ${props => props.theme.css.slideOutBlue};
`;
