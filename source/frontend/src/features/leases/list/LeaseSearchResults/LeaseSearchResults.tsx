import { groupBy } from 'lodash';
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
import { ApiGen_CodeTypes_HistoricalFileNumberTypes } from '@/models/api/generated/ApiGen_CodeTypes_HistoricalFileNumberTypes';
import { ApiGen_Concepts_HistoricalFileNumber } from '@/models/api/generated/ApiGen_Concepts_HistoricalFileNumber';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists, prettyFormatDate } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import LeaseProperties from './LeaseProperties';
import LeaseTenants from './LeaseTenants';

const maxPropertyDisplayCount = 2;

const columns: ColumnWithProps<ApiGen_Concepts_Lease>[] = [
  {
    Header: 'L-File Number',
    accessor: 'lFileNo',
    align: 'right',
    clickable: true,
    sortable: true,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<ApiGen_Concepts_Lease>) => (
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
    width: 20,
    Cell: (props: CellProps<ApiGen_Concepts_Lease>) => {
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
    align: 'left',
    width: 40,
    maxWidth: 100,
    Cell: (props: CellProps<ApiGen_Concepts_Lease>) => {
      return (
        <LeaseTenants
          tenantNames={
            props.row.original.tenants?.map<string>(t =>
              exists(t.person) ? formatApiPersonNames(t.person) : t.organization?.name ?? '',
            ) ?? []
          }
          maxDisplayCount={maxPropertyDisplayCount}
        ></LeaseTenants>
      );
    },
  },
  {
    Header: 'Properties',
    align: 'left',

    Cell: (props: CellProps<ApiGen_Concepts_Lease>) => {
      return (
        <LeaseProperties
          properties={
            props.row.original.fileProperties?.map(lp => lp.property).filter(exists) ?? []
          }
          maxDisplayCount={maxPropertyDisplayCount}
        ></LeaseProperties>
      );
    },
  },
  {
    Header: 'Historical File #',
    align: 'left',
    clickable: false,
    sortable: false,
    width: 10,
    maxWidth: 20,
    Cell: (props: CellProps<ApiGen_Concepts_Lease>) => {
      // File numbers types to display
      const numberTypes: string[] = [
        ApiGen_CodeTypes_HistoricalFileNumberTypes.LISNO.toString(),
        ApiGen_CodeTypes_HistoricalFileNumberTypes.PSNO.toString(),
        ApiGen_CodeTypes_HistoricalFileNumberTypes.OTHER.toString(),
      ];

      // Get unique file numbers from lease properties
      const fileNumbers: ApiGen_Concepts_HistoricalFileNumber[] = [];
      props.row.original.fileProperties?.forEach(fl => {
        fl.property.historicalFileNumbers?.forEach(number => {
          if (numberTypes.includes(number.historicalFileNumberTypeCode.id)) {
            if (
              !fileNumbers.find(
                x =>
                  x.historicalFileNumber === number.historicalFileNumber &&
                  x.historicalFileNumberTypeCode.id === number.historicalFileNumberTypeCode.id,
              )
            ) {
              fileNumbers.push(number);
            }
          }
        });
      });

      const groupByType = groupBy(fileNumbers, x => x.historicalFileNumberTypeCode.id);

      let lisNumbers = '';
      let psNumbers = '';
      let otherNumbers = '';
      if (groupByType[ApiGen_CodeTypes_HistoricalFileNumberTypes.LISNO.toString()]?.length) {
        lisNumbers = groupByType[ApiGen_CodeTypes_HistoricalFileNumberTypes.LISNO.toString()]
          .map(x => x.historicalFileNumber)
          .join(', ');
      }

      if (groupByType[ApiGen_CodeTypes_HistoricalFileNumberTypes.PSNO.toString()]?.length) {
        psNumbers = groupByType[ApiGen_CodeTypes_HistoricalFileNumberTypes.PSNO.toString()]
          .map(x => x.historicalFileNumber)
          .join(', ');
      }

      if (groupByType[ApiGen_CodeTypes_HistoricalFileNumberTypes.OTHER.toString()]?.length) {
        otherNumbers = groupByType[ApiGen_CodeTypes_HistoricalFileNumberTypes.OTHER.toString()]
          .map(x => x.historicalFileNumber)
          .join(', ');
      }

      return (
        <FileNumbersDiv>
          {lisNumbers ? (
            <label>
              <span>LIS: </span>
              {lisNumbers};
            </label>
          ) : null}
          {psNumbers ? (
            <label>
              <span>PS: </span>
              {psNumbers};
            </label>
          ) : null}
          {otherNumbers ? (
            <label>
              <span>OTHER: </span>
              {otherNumbers}.
            </label>
          ) : null}
        </FileNumbersDiv>
      );
    },
  },
  {
    Header: 'Status',
    align: 'left',
    sortable: true,
    width: 20,
    maxWidth: 20,
    accessor: 'fileStatusTypeCode',
    Cell: renderTypeCode,
  },
];

export interface ILeaseSearchResultsProps {
  results: ApiGen_Concepts_Lease[];
  totalItems?: number;
  pageCount?: number;
  pageSize?: number;
  pageIndex?: number;
  sort?: TableSort<ApiGen_Concepts_Lease>;
  setSort: (value: TableSort<ApiGen_Concepts_Lease>) => void;
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
    <Table<ApiGen_Concepts_Lease>
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

const FileNumbersDiv = styled('div')`
  label {
    display: inline-block;

    span {
      font-weight: bold;
    }
  }
`;

const ExpiredIcon = styled('span')`
  color: ${props => props.theme.bcTokens.surfaceColorPrimaryDangerButtonDefault};
`;

const ExpiredOverlay = styled(Tooltip)`
  .tooltip-inner {
    color: ${props => props.theme.bcTokens.surfaceColorPrimaryDangerButtonDefault};
    background-color: ${props => props.theme.css.dangerBackgroundColor};
  }

  .arrow::before {
    color: ${props => props.theme.bcTokens.surfaceColorPrimaryDangerButtonDefault};
    background-color: ${props => props.theme.css.dangerBackgroundColor};
  }
`;
