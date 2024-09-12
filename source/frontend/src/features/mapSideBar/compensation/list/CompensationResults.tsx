import { matchPath, useLocation } from 'react-router-dom';

import { Table } from '@/components/Table';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';

import { UpdateCompensationContext } from '../models/UpdateCompensationContext';
import { createCompensationTableColumns } from './columns';

export interface ICompensationResultProps {
  results: ApiGen_Concepts_CompensationRequisition[];
  statusSolver: UpdateCompensationContext;
  onShow: (compensationId: number) => void;
  onDelete: (compensationId: number) => void;
}

export function CompensationResults(props: ICompensationResultProps) {
  const location = useLocation();
  const { results, ...rest } = props;
  const columns = createCompensationTableColumns(props.statusSolver, props.onShow, props.onDelete);

  const isActiveRow = (entityId: number): boolean => {
    const matched = matchPath(location.pathname, {
      path: [
        `/mapview/sidebar/acquisition/*/compensation-requisition/${entityId}`,
        `/mapview/sidebar/lease/*/compensation-requisition/${entityId}`,
      ],
      exact: true,
      strict: true,
    });

    return matched?.isExact ?? false;
  };

  return (
    <Table<ApiGen_Concepts_CompensationRequisition>
      name="AcquisitionCompensationTable"
      manualSortBy={false}
      lockPageSize={true}
      manualPagination={false}
      totalItems={results.length}
      columns={columns}
      data={results ?? []}
      isRowActive={isActiveRow}
      noRowsMessage="No matching Compensation Requisition(s) found"
      {...rest}
    ></Table>
  );
}
