import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import { Api_FinancialCode } from '@/models/api/FinancialCode';

import { columns } from './columns';

export interface IFinancialCodeResultsProps {
  results: Api_FinancialCode[];
  loading?: boolean;
  sort: TableSort<Api_FinancialCode>;
  setSort: (value: TableSort<Api_FinancialCode>) => void;
}

export function FinancialCodeResults(props: IFinancialCodeResultsProps) {
  const { results, setSort, sort, ...rest } = props;

  return (
    <Table<Api_FinancialCode>
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
