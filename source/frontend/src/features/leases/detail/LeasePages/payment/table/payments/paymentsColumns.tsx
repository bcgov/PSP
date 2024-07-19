import { FaTrash } from 'react-icons/fa';
import { MdEdit, MdReceipt } from 'react-icons/md';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import NotesModal from '@/components/common/form/NotesModal';
import { InlineFlexDiv } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { ColumnWithProps, renderDate, renderMoney, renderTypeCode } from '@/components/Table';
import { Claims } from '@/constants';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_CodeTypes_LeasePaymentCategoryTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePaymentCategoryTypes';
import { ApiGen_Concepts_Payment } from '@/models/api/generated/ApiGen_Concepts_Payment';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { formatMoney, stringToFragment } from '@/utils';

import { FormLeasePayment } from '../../models';

const actualsActions = (
  onEdit: (values: FormLeasePayment) => void,
  onDelete: (values: FormLeasePayment) => void,
) => {
  return function ({ row: { original, index } }: CellProps<FormLeasePayment, unknown>) {
    const { hasClaim } = useKeycloakWrapper();
    return (
      <StyledIcons>
        {hasClaim(Claims.LEASE_EDIT) && (
          <Button
            title="edit actual"
            icon={<MdEdit size={24} id={`edit-actual-${index}`} title="edit actual" />}
            onClick={() => onEdit(original)}
          ></Button>
        )}
        {hasClaim(Claims.LEASE_EDIT) && (
          <Button
            title="delete actual"
            icon={<FaTrash size={24} id={`delete-actual-${index}`} title="delete actual" />}
            onClick={() => original.id && onDelete(original)}
          ></Button>
        )}
      </StyledIcons>
    );
  };
};

const renderCategory = () =>
  function ({ row: { original } }: CellProps<FormLeasePayment, ApiGen_Base_CodeType<string>>) {
    let categoryName = '';
    switch (original.leasePaymentCategoryTypeCode?.id) {
      case ApiGen_CodeTypes_LeasePaymentCategoryTypes.ADDL.toString():
        categoryName = 'Additional Rent';
        break;
      case ApiGen_CodeTypes_LeasePaymentCategoryTypes.VBL.toString():
        categoryName = 'Variable Rent';
        break;
      case ApiGen_CodeTypes_LeasePaymentCategoryTypes.BASE.toString():
        categoryName = 'Base Rent';
    }
    return <>{categoryName}</>;
  };

export interface IPaymentColumnProps {
  onEdit: (values: FormLeasePayment) => void;
  onSave: (values: FormLeasePayment) => void;
  onDelete: (values: FormLeasePayment) => void;
  isReceivable?: boolean;
  isGstEligible?: boolean;
  payments: FormLeasePayment[];
}

export const getActualsColumns = ({
  onEdit,
  onDelete,
  onSave,
  isReceivable,
  isGstEligible,
}: IPaymentColumnProps): ColumnWithProps<
  FormLeasePayment,
  { properties: ApiGen_Concepts_Payment[] }
>[] => {
  return [
    {
      Header: 'Date',
      align: 'left',
      maxWidth: 70,
      accessor: 'receivedDate',
      Cell: renderDate,
      Footer: () => (
        <span>
          <MdReceipt /> Payment Summary
        </span>
      ),
    },
    {
      Header: 'Rent category',
      accessor: 'leasePaymentCategoryTypeCode',
      align: 'left',
      maxWidth: 60,
      Cell: renderCategory(),
    },
    {
      Header: 'Payment method',
      accessor: 'leasePaymentMethodType',
      align: 'left',
      maxWidth: 60,
      Cell: renderTypeCode,
      Footer: ({ properties }: { properties: ApiGen_Concepts_Payment[] }) => (
        <>({properties?.length}) payments</>
      ),
    },
    {
      Header: () => (
        <>
          {isReceivable ? 'Received payment ($)' : 'Sent payment ($)'}
          <TooltipIcon
            toolTipId="actualReceivedPaymentTooltip"
            toolTip="Actual payment amount, not including GST. This calculation can be overridden by editing the payment row"
          />
        </>
      ),
      accessor: 'amountPreTax',
      align: 'right',
      Cell: renderMoney,
      Footer: ({ properties }: { properties: ApiGen_Concepts_Payment[] }) => (
        <>{formatMoney(properties.reduce((total, current) => total + current.amountPreTax, 0))}</>
      ),
    },
    {
      Header: () => (
        <>
          GST ($)
          <TooltipIcon
            toolTipId="actualGstTooltip"
            toolTip="GST is calculated as (expected amount) x GST rate (5%). This calculation can be overridden by editing the payment row"
          />
        </>
      ),
      align: 'right',
      accessor: 'amountGst',
      maxWidth: 35,
      Cell: ({ value }: CellProps<FormLeasePayment, NumberFieldValue>) => {
        return stringToFragment(isGstEligible ? formatMoney(value) : '-');
      },
      Footer: ({ properties }: { properties: ApiGen_Concepts_Payment[] }) => (
        <>
          {isGstEligible
            ? formatMoney(
                properties.reduce((total, current) => total + (current?.amountGst ?? 0), 0),
              )
            : '-'}
        </>
      ),
    },
    {
      Header: () => (
        <>
          Total ($)
          <TooltipIcon
            toolTipId="receivedTotalTooltip"
            toolTip="Actual payment amount, including GST if applicable"
          />
        </>
      ),
      accessor: 'amountTotal',
      align: 'right',
      Cell: renderMoney,
      Footer: ({ properties }: { properties: ApiGen_Concepts_Payment[] }) => (
        <>
          {formatMoney(
            properties.reduce((total, current) => total + (current?.amountTotal ?? 0), 0),
          )}
        </>
      ),
    },
    {
      Header: () => (
        <>
          Payment status
          <TooltipIcon
            toolTipId="paymentStatusTooltip"
            toolTip="Variance between expected and actual payment"
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
      Cell: ({ row }: CellProps<FormLeasePayment, string | undefined>) => {
        return (
          <NotesModal<FormLeasePayment>
            title="Payment Notes"
            notesLabel="Notes:"
            onSave={(values: FormLeasePayment) => {
              const valuesToSave = row.original;
              valuesToSave.note = values.note;
              onSave(valuesToSave);
            }}
            initialValues={row.original}
          />
        );
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
    color: ${props => props.theme.css.activeActionColor};
  }
  [id^='delete-actual'] {
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
