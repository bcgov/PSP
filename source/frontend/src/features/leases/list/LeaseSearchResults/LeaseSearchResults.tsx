import moment from 'moment';
import { useCallback } from 'react';
import { Tooltip } from 'react-bootstrap';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import TooltipIcon from '@/components/common/TooltipIcon';
import { ColumnWithProps, renderTypeCode, Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import { ILeaseSearchResult } from '@/interfaces';
import { prettyFormatDate } from '@/utils';

import LeaseProperties from './LeaseProperties';
import LeaseTenants from './LeaseTenants';

const maxPropertyDisplayCount = 2;

const columns: ColumnWithProps<ILeaseSearchResult>[] = [
  {
    Header: 'L-File Number',
    accessor: 'lFileNo',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<ILeaseSearchResult>) => (
      <Link to={`/mapview/sidebar/lease/${props.row.original.id}`}>
        {props.row.original.lFileNo}
      </Link>
    ),
  },
  {
    Header: 'Expiry Date',
    accessor: 'expiryDate',
    align: 'left',
    sortable: true,
    width: 40,
    Cell: (props: CellProps<ILeaseSearchResult>) => {
      const expiryDate = props.row.original.expiryDate;
      const isExpired = moment().isAfter(moment(expiryDate, 'YYYY-MM-DD'), 'day');

      const icon = (
        <ExpiredIcon className="mx-2">
          <AiOutlineExclamationCircle size={16} />
        </ExpiredIcon>
      );
      const overlay = (
        <ExpiredOverlay>
          <strong>EXPIRED</strong>
        </ExpiredOverlay>
      );
      return (
        <>
          <span>{prettyFormatDate(expiryDate)}</span>
          {isExpired && (
            <TooltipIcon
              toolTipId="lease-row-tooltip-expired"
              placement="right"
              customToolTipIcon={icon}
              customOverlay={overlay}
            />
          )}
        </>
      );
    },
  },
  {
    Header: 'Program Name',
    accessor: 'programName',
    align: 'left',
    sortable: true,
    width: 40,
    maxWidth: 80,
  },
  {
    Header: 'Tenant Names',
    accessor: 'tenantNames',
    align: 'left',
    width: 40,
    maxWidth: 100,
    Cell: (props: CellProps<ILeaseSearchResult>) => {
      return (
        <LeaseTenants
          tenantNames={props.row.original.tenantNames}
          maxDisplayCount={maxPropertyDisplayCount}
        ></LeaseTenants>
      );
    },
  },
  {
    Header: 'Properties',
    accessor: 'properties',
    align: 'left',

    Cell: (props: CellProps<ILeaseSearchResult>) => {
      return (
        <LeaseProperties
          properties={props.row.original.properties}
          maxDisplayCount={maxPropertyDisplayCount}
        ></LeaseProperties>
      );
    },
  },
  {
    Header: 'Status',
    accessor: 'statusType',
    align: 'left',
    sortable: true,
    width: 20,
    maxWidth: 20,
    Cell: renderTypeCode,
  },
];

export interface ILeaseSearchResultsProps {
  results: ILeaseSearchResult[];
  totalItems?: number;
  pageCount?: number;
  pageSize?: number;
  pageIndex?: number;
  sort?: TableSort<ILeaseSearchResult>;
  setSort: (value: TableSort<ILeaseSearchResult>) => void;
  setPageSize?: (value: number) => void;
  setPageIndex?: (value: number) => void;
  loading?: boolean;
}

export function LeaseSearchResults(props: ILeaseSearchResultsProps) {
  const { results, sort = {}, setSort, setPageSize, setPageIndex, ...rest } = props;

  // This will get called when the table needs new data
  const updateCurrentPage = useCallback(
    ({ pageIndex }: { pageIndex: number }) => setPageIndex && setPageIndex(pageIndex),
    [setPageIndex],
  );

  return (
    <Table<ILeaseSearchResult>
      name="leasesTable"
      columns={columns}
      data={results ?? []}
      externalSort={{ sort: sort, setSort: setSort }}
      onRequestData={updateCurrentPage}
      onPageSizeChange={setPageSize}
      noRowsMessage="Lease / License details do not exist in PIMS inventory"
      totalItems={props.totalItems}
      {...rest}
    ></Table>
  );
}

const ExpiredIcon = styled('span')`
  color: ${props => props.theme.css.dangerColor};
`;

const ExpiredOverlay = styled(Tooltip)`
  .tooltip-inner {
    color: ${props => props.theme.css.dangerColor};
    background-color: ${props => props.theme.css.dangerBackgroundColor};
  }

  .arrow::before {
    color: ${props => props.theme.css.dangerColor};
    background-color: ${props => props.theme.css.dangerBackgroundColor};
  }
`;
