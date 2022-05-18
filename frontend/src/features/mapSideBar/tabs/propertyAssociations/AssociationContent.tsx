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
  associationName: string;
  aquisitionFiles?: Api_PropertyAssociation[];
  linkUrlMask: string;
}

const AssociationContent: React.FunctionComponent<IAssociationContentProps> = props => {
  const noDataMessage = `There are no ${props.associationName} files availiable`;
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
    width: 50,
  },
  {
    Header: 'File name',
    accessor: 'fileName',
    align: 'left',
  },
  {
    Header: 'Created by',
    accessor: 'createdBy',
    align: 'left',
    width: 50,
  },
  {
    Header: 'Created date',
    accessor: 'createdDate',
    align: 'left',
    Cell: renderDate,
    width: 80,
  },
  {
    Header: 'Status',
    accessor: 'status',
    align: 'left',
    width: 60,
  },
];

export default AssociationContent;
