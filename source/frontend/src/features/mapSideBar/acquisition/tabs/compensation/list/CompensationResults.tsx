import { Table } from '@/components/Table';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';

import StatusUpdateSolver from '../../fileDetails/detail/statusUpdateSolver';
import { createCompensationTableColumns } from './columns';

export interface ICompensationResultProps {
  results: Api_CompensationRequisition[];
  statusSolver: StatusUpdateSolver;
  onShow: (compensationId: number) => void;
  onDelete: (compensationId: number) => void;
}

export function CompensationResults(props: ICompensationResultProps) {
  const { results, ...rest } = props;

  const columns = createCompensationTableColumns(props.statusSolver, props.onShow, props.onDelete);

  return (
    <Table<Api_CompensationRequisition>
      name="AcquisitionCompensationTable"
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
