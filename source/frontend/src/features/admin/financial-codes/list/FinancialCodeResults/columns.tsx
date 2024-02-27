import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

import { ColumnWithProps, DateCell } from '@/components/Table';
import { Roles } from '@/constants/roles';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';
import { stringToFragment } from '@/utils';

import { formatFinancialCodeType } from '../../financialCodeUtils';

export const columns: ColumnWithProps<ApiGen_Concepts_FinancialCode>[] = [
  {
    Header: 'Code value',
    accessor: 'code',
    align: 'left',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<ApiGen_Concepts_FinancialCode>) => {
      const { hasRole } = useKeycloakWrapper();
      const financialCode = props.row.original;
      if (hasRole(Roles.SYSTEM_ADMINISTRATOR)) {
        return (
          <Link to={`/admin/financial-code/${financialCode.type}/${financialCode.id}`}>
            {financialCode.code}
          </Link>
        );
      }
      return stringToFragment(financialCode.code);
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
    Cell: (props: CellProps<ApiGen_Concepts_FinancialCode>) => {
      return stringToFragment(
        formatFinancialCodeType(props.row.original.type as ApiGen_Concepts_FinancialCodeTypes),
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
