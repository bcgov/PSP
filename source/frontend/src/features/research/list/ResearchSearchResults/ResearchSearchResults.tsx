import { useCallback } from 'react';
import { CellProps } from 'react-table';

import { ExternalLink } from '@/components/common/ExternalLink';
import { ColumnWithProps, renderTypeCode, Table } from '@/components/Table';
import { UtcDateCell } from '@/components/Table/DateCell';
import { TableSort } from '@/components/Table/TableSort';
import { Claims } from '@/constants/claims';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';
import { stringToFragment } from '@/utils';

import { ResearchSearchResultModel } from '../models';

const columns: ColumnWithProps<ResearchSearchResultModel>[] = [
  {
    Header: 'File #',
    accessor: 'rfileNumber',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<ResearchSearchResultModel>) => {
      const { hasClaim } = useKeycloakWrapper();
      if (hasClaim(Claims.RESEARCH_VIEW)) {
        return (
          <ExternalLink to={`/mapview/sidebar/research/${props.row.original.id}`}>
            {props.row.original.rfileNumber}
          </ExternalLink>
        );
      }
      return stringToFragment(props.row.original.rfileNumber);
    },
  },
  {
    Header: 'File name',
    accessor: 'name',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'MOTT region',
    accessor: 'fileProperties',
    align: 'right',
    clickable: true,
    width: 10,
    maxWidth: 20,
    Cell: ({ value }: CellProps<any, ApiGen_Concepts_ResearchFileProperty[] | undefined>) => {
      const regions = [...new Set(value?.map(pr => pr?.property?.region?.description))];
      return stringToFragment(regions.join(', '));
    },
  },
  {
    Header: 'Created by',
    accessor: 'appCreateUserid',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'Created date',
    accessor: 'appCreateTimestamp',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: UtcDateCell,
  },
  {
    Header: 'Last updated by',
    accessor: 'appLastUpdateUserid',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
  },
  {
    Header: 'Last updated date',
    accessor: 'appLastUpdateTimestamp',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: UtcDateCell,
  },
  {
    Header: 'Status',
    accessor: 'researchFileStatusTypeCode',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: renderTypeCode,
  },
];

export interface IResearchSearchResultsProps {
  results: ResearchSearchResultModel[];
  totalItems?: number;
  pageCount?: number;
  pageSize?: number;
  pageIndex?: number;
  sort?: TableSort<ResearchSearchResultModel>;
  setSort: (value: TableSort<ResearchSearchResultModel>) => void;
  setPageSize?: (value: number) => void;
  setPageIndex?: (value: number) => void;
  loading?: boolean;
}

export function ResearchSearchResults(props: IResearchSearchResultsProps) {
  const { results, sort = {}, setSort, setPageSize, setPageIndex, ...rest } = props;

  // This will get called when the table needs new data
  const updateCurrentPage = useCallback(
    ({ pageIndex }: { pageIndex: number }) => setPageIndex && setPageIndex(pageIndex),
    [setPageIndex],
  );

  return (
    <Table<ResearchSearchResultModel>
      name="researchFilesTable"
      columns={columns}
      data={results ?? []}
      externalSort={{ sort: sort, setSort: setSort }}
      onRequestData={updateCurrentPage}
      onPageSizeChange={setPageSize}
      noRowsMessage="No matching Research Files found"
      totalItems={props.totalItems}
      {...rest}
    ></Table>
  );
}
