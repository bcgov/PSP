import { Table } from 'components/Table';
import { Api_Compensation } from 'models/api/Compensation';

import { createCompensationTableColumns } from './columns';

export interface ICompensationResultProps {
  results: Api_Compensation[];
  onShow: (compensationId: number) => void;
  onDelete: (compensationId: number) => void;
}

export function CompensationResults(props: ICompensationResultProps) {
  const { results, ...rest } = props;

  const columns = createCompensationTableColumns(props.onShow, props.onDelete);

  return (
    <Table<Api_Compensation>
      name="AcqusitionCompensationTable"
      manualSortBy={false}
      lockPageSize={true}
      manualPagination={false}
      totalItems={results.length}
      columns={columns}
      data={results ?? []}
      noRowsMessage="No matching Compensation Requisition(s) found"
      {...rest}
    ></Table>
  );
}
