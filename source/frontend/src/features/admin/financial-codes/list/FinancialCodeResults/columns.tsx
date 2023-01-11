import { ColumnWithProps, DateCell } from 'components/Table';
import { Claims } from 'constants/claims';
import { FinancialCodeTypes, formatFinancialCodeType } from 'constants/financialCodeTypes';
import { useKeycloakWrapper } from 'hooks/useKeycloakWrapper';
import { Api_FinancialCode } from 'models/api/FinancialCode';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';
import { stringToFragment } from 'utils';

export const columns: ColumnWithProps<Api_FinancialCode>[] = [
  {
    Header: 'Code value',
    accessor: 'code',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<Api_FinancialCode>) => {
      const { hasClaim } = useKeycloakWrapper();
      if (hasClaim(Claims.ADMIN_FINANCIAL_CODES)) {
        return (
          <Link to={`/admin/financial-codes/${props.row.original.id}`}>
            {props.row.original.id}
          </Link>
        );
      }
      return stringToFragment(props.row.original.id);
    },
  },
  {
    Header: 'Code description',
    accessor: 'description',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 20,
    maxWidth: 40,
  },
  {
    Header: 'Code type',
    accessor: 'type',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<Api_FinancialCode>) => {
      return stringToFragment(
        formatFinancialCodeType(props.row.original.type as FinancialCodeTypes),
      );
    },
  },
  {
    Header: 'Effective date',
    accessor: 'effectiveDate',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: DateCell,
  },
  {
    Header: 'Expiry date',
    accessor: 'expiryDate',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: DateCell,
  },
];
