import moment from 'moment';
import { FaTrash } from 'react-icons/fa';
import { IoMdRefreshCircle } from 'react-icons/io';
import { MdEdit } from 'react-icons/md';
import { TbArrowWaveRightUp } from 'react-icons/tb';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import { InlineFlexDiv } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import {
  ColumnWithProps,
  renderBooleanAsYesNo,
  renderMoney,
  renderTypeCode,
} from '@/components/Table';
import { Claims } from '@/constants';
import { LeasePeriodStatusTypes } from '@/constants/leaseStatusTypes';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ISystemConstant } from '@/store/slices/systemConstants';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { prettyFormatDate } from '@/utils';
import { stringToFragment } from '@/utils/columnUtils';
import { formatMoney } from '@/utils/numberFormatUtils';

import { FormLeasePeriod, LeasePeriodByCategoryProjection } from '../../models';

export interface IPeriodColumnProps {
  onEdit: (values: FormLeasePeriod) => void;
  onDelete: (values: FormLeasePeriod) => void;
  leaseTypeCode?: string;
  gstConstant?: ISystemConstant;
}

export const getLeasePeriodColumns = ({
  onEdit,
  onDelete,
}: IPeriodColumnProps): ColumnWithProps<FormLeasePeriod>[] => {
  return [
    {
      Header: '',
      id: 'initialOrRenewal',
      align: 'left',
      maxWidth: 50,
      Cell: getPeriodName,
    },
    {
      Header: 'Start date - end date',
      align: 'center',
      minWidth: 55,
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
      maxWidth: 60,
    },
    {
      Header: () => (
        <>
          Expected payment ($)
          <TooltipIcon
            toolTipId="expectedPaymentTooltip"
            toolTip="This is the amount agreed to be paid per interval, ie: the amount of a monthly payment if the lease is paid monthly"
          />
        </>
      ),
      align: 'right',
      accessor: 'paymentAmount',
      maxWidth: 50,
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
            toolTip="GST is calculated as (expected amount) x GST rate (5%)"
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
            toolTip="This is the expected payment amount plus GST if applicable"
          />
        </>
      ),
      id: 'expectedTotal',
      align: 'right',
      maxWidth: 45,
      Cell: renderExpectedTotal(),
    },
    {
      Header: () => (
        <>
          Expected period ($)
          <TooltipIcon
            toolTipId="expectedPeriodTooltip"
            toolTip="This is the full payment amount expected in the duration of the period"
          />
        </>
      ),
      id: 'expectedPeriod',
      align: 'right',
      maxWidth: 50,
      Cell: renderExpectedPeriod(),
    },
    {
      Header: () => (
        <>
          Actual total ($)
          <TooltipIcon toolTipId="actualTotalTooltip" toolTip="Amount paid this period" />
        </>
      ),
      id: 'actualTotal',
      align: 'right',
      maxWidth: 40,
      Cell: renderActualTotal,
    },
    {
      Header: () => (
        <>
          Exercised?
          <TooltipIcon toolTipId="exercisedTooltip" toolTip="Exercise period to add payments" />
        </>
      ),
      align: 'left',
      accessor: 'isTermExercised',
      maxWidth: 40,
      Cell: renderBooleanAsYesNo,
    },
    {
      Header: 'Actions',
      align: 'right',
      maxWidth: 30,
      Cell: paymentActions(onEdit, onDelete),
    },
  ];
};

const paymentActions = (
  onEdit: (values: FormLeasePeriod) => void,
  onDelete: (values: FormLeasePeriod) => void,
) => {
  return function ({ row: { original, index } }: CellProps<FormLeasePeriod, string>) {
    const { hasClaim } = useKeycloakWrapper();
    return (
      <StyledIcons>
        {hasClaim(Claims.LEASE_EDIT) && (
          <Button
            title="edit period"
            icon={<MdEdit size={24} id={`edit-period-${index}`} title="edit period" />}
            onClick={() => onEdit(original)}
          ></Button>
        )}
        {hasClaim(Claims.LEASE_EDIT) &&
          original.payments.length <= 0 &&
          original.statusTypeCode?.id !== LeasePeriodStatusTypes.EXERCISED && (
            <Button
              title="delete period"
              icon={<FaTrash size={24} id={`delete-period-${index}`} title="delete period" />}
              onClick={() => original.id && onDelete(original)}
            ></Button>
          )}
        {hasClaim(Claims.LEASE_EDIT) &&
          (original.payments.length > 0 ||
            original.statusTypeCode?.id === LeasePeriodStatusTypes.EXERCISED) && (
            <TooltipIcon
              toolTipId={`no-delete-tooltip-period-${original.id}`}
              toolTip="An exercised period cannot be deleted. To delete this period ensure that there are no payments recorded for it, and the period has not been exercised"
            />
          )}
      </StyledIcons>
    );
  };
};

function getPeriodName({ row: { index, original } }: CellProps<FormLeasePeriod, unknown>) {
  return (
    <InlineFlexDiv className="align-items-center">
      <b>{stringToFragment(`Period ${index + 1}`)}</b>&nbsp;
      {original?.isVariable === 'true' && (
        <TooltipWrapper tooltipId="variable-period-tooltip" tooltip="Variable Payments">
          <StyledRefreshIcon size={16} title="Variable Payments" />
        </TooltipWrapper>
      )}
    </InlineFlexDiv>
  );
}

function startAndEndDate({ row: { original } }: CellProps<FormLeasePeriod, string>) {
  return original.isFlexible === 'true' ? (
    <>
      {prettyFormatDate(original.startDate)} - {prettyFormatDate(original.expiryDate)}
      <StyledBreak />
      <i>(anticipated)</i>
      <TooltipWrapper tooltipId="flexible-period-icon" tooltip="Flexible Period">
        <StyledFlexibleIcon size={24} title="Flexible Period" />
      </TooltipWrapper>
    </>
  ) : (
    stringToFragment(
      `${prettyFormatDate(original.startDate)} - ${prettyFormatDate(original.expiryDate)}`,
    )
  );
}

const renderExpectedPeriod = () =>
  function ({ row: { original } }: CellProps<FormLeasePeriod, string>) {
    if (!original.startDate || !original.expiryDate || original.paymentAmount === undefined) {
      return stringToFragment('$0.00');
    }
    const expectedPeriod = calculateExpectedPeriodAmount(
      original.leasePmtFreqTypeCode?.id ?? '',
      original.startDate,
      original.expiryDate,
      original.paymentAmount as number,
      (original.gstAmount as number) ?? 0,
    );
    return stringToFragment(expectedPeriod !== undefined ? formatMoney(expectedPeriod) : '$0.00');
  };

function renderActualTotal({ row: { original } }: CellProps<FormLeasePeriod, string>) {
  const total = formatMoney(
    (original.payments ?? []).reduce((sum: number, p) => (sum += p.amountTotal as number), 0),
  );
  return stringToFragment(original.isTermExercised ? total : '');
}

function renderGstAmount({ row: { original } }: CellProps<FormLeasePeriod, NumberFieldValue>) {
  return stringToFragment(
    original.isGstEligible === true ? formatMoney(original?.gstAmount ?? 0) : '',
  );
}

function calculateExpectedPeriodAmount(
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

export const renderExpectedTotal = () =>
  function ({
    row: { original },
  }: CellProps<FormLeasePeriod, string> | CellProps<LeasePeriodByCategoryProjection, string>) {
    return stringToFragment(
      original.paymentAmount !== undefined
        ? formatMoney((original.paymentAmount as number) + ((original?.gstAmount as number) ?? 0))
        : '-',
    );
  };

const StyledIcons = styled(InlineFlexDiv)`
  align-items: center;
  [id^='edit-period'] {
    color: ${props => props.theme.css.activeActionColor};
  }
  [id^='delete-period'] {
    color: ${props => props.theme.css.activeActionColor};
    :hover {
      color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryDangerButtonDefault};
    }
  }
  .btn.btn-primary {
    background-color: transparent;
    padding: 0;
  }
`;

const StyledFlexibleIcon = styled(TbArrowWaveRightUp)`
  color: ${props => props.theme.css.completedColor};
  margin-left: 0.5rem;
`;

const StyledRefreshIcon = styled(IoMdRefreshCircle)`
  color: ${props => props.theme.css.variableColor};
`;

const StyledBreak = styled.div`
  height: 0;
  flex-basis: 100%;
`;
