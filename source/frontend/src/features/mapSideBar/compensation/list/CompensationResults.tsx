import { matchPath, useLocation } from 'react-router-dom';

import { Table } from '@/components/Table';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';

import { IUpdateCompensationStrategy } from '../models/IUpdateCompensationStrategy';
import { createCompensationTableColumns } from './columns';

export interface ICompensationResultProps {
  isLoading: boolean;
  results: ApiGen_Concepts_CompensationRequisition[];
  statusSolver: IUpdateCompensationStrategy;
  onShow: (compensationId: number) => void;
  onDelete: (compensationId: number) => void;
}

export function CompensationResults(props: ICompensationResultProps) {
  const location = useLocation();
  const { results, isLoading, ...rest } = props;
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
      totalItems={results?.length}
      columns={columns}
      data={results ?? []}
      isRowActive={isActiveRow}
      loading={isLoading}
      noRowsMessage="No matching Compensation Requisition(s) found"
      {...rest}
    ></Table>
  );
}
