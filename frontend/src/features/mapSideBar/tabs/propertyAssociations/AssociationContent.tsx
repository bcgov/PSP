import { ColumnWithProps, renderDate, Table } from 'components/Table';
import { Api_PropertyAssociation } from 'models/api/Property';
import * as React from 'react';
import { Link } from 'react-router-dom';
import { CellProps } from 'react-table';

interface IPimsInfo {
  id: string;
  linkUrl: string;
  fileIdentifier: string;
  fileName: string;
  createdBy: string;
  createdDate: string;
  status: string;
}

export interface IAssociationContentProps {
  aquisitionFiles?: Api_PropertyAssociation[];
  linkUrlMask: string;
}

const AssociationContent: React.FunctionComponent<IAssociationContentProps> = props => {
  const noDataMessage = 'There are no aquisition files availiable';
  if (props.aquisitionFiles === undefined) {
    return <>{noDataMessage}</>;
  }
  const tableData = props.aquisitionFiles.map<IPimsInfo>(x => {
    return {
      id: x.id?.toString() || '',
      linkUrl: props.linkUrlMask.replace('|id|', x.id?.toString() || ''),
      fileIdentifier: x.fileNumber || '',
      fileName: x.fileName || '',
      createdBy: x.createdBy || '',
      createdDate: x.createdDateTime || '',
      status: x.status || '',
    };
  });
  return (
    <Table<IPimsInfo>
      name="acquisitionFiles"
      columns={aquisitionColumns}
      data={tableData ?? []}
      manualSortBy={true}
      noRowsMessage={noDataMessage}
      hideToolbar
    ></Table>
  );
};

const aquisitionColumns: ColumnWithProps<IPimsInfo>[] = [
  {
    Header: 'File #',
    accessor: 'fileIdentifier',
    align: 'left',
    Cell: (props: CellProps<IPimsInfo>) => {
      return <Link to={props.row.original.linkUrl}>{props.row.original.fileIdentifier}</Link>;
    },
  },
  {
    Header: 'File name',
    accessor: 'fileName',
    align: 'left',
    minWidth: 200,
  },
  {
    Header: 'Created by',
    accessor: 'createdBy',
    align: 'left',
  },
  {
    Header: 'Created date',
    accessor: 'createdDate',
    align: 'left',
    Cell: renderDate,
  },
  {
    Header: 'Status',
    accessor: 'status',
    align: 'left',
    width: 100,
  },
];

export default AssociationContent;
