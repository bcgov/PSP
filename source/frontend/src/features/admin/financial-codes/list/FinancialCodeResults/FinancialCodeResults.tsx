import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';

import { columns } from './columns';

export interface IFinancialCodeResultsProps {
  results: ApiGen_Concepts_FinancialCode[];
  loading?: boolean;
  sort: TableSort<ApiGen_Concepts_FinancialCode>;
  setSort: (value: TableSort<ApiGen_Concepts_FinancialCode>) => void;
}

export function FinancialCodeResults(props: IFinancialCodeResultsProps) {
  const { results, setSort, sort, ...rest } = props;

  return (
    <Table<ApiGen_Concepts_FinancialCode>
      name="FinancialCodeTable"
      manualSortBy={false}
      lockPageSize={false}
      manualPagination={false}
      totalItems={results.length}
      columns={columns}
      externalSort={{ sort, setSort }}
      data={results ?? []}
      noRowsMessage="No matching results can be found. Try widening your search criteria."
      {...rest}
    ></Table>
  );
}
