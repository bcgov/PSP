import * as React from 'react';
import { CellProps } from 'react-table';

import OverflowTip from '@/components/common/OverflowTip';
import { ColumnWithProps, Table } from '@/components/Table';
import { Api_Property } from '@/models/api/Property';
import { Api_PropertyFile } from '@/models/api/PropertyFile';
import { getFilePropertyName } from '@/utils/mapPropertyUtils';

interface IFilePropertiesTableProps {
  fileProperties: Api_PropertyFile[];
  setSelectedFileProperties: (properties: Api_PropertyFile[]) => void;
  selectedFileProperties: Api_PropertyFile[];
  disabledSelection: boolean;
}

const columns: ColumnWithProps<Api_PropertyFile>[] = [
  {
    Header: 'Identifier',
    accessor: 'property',
    align: 'left',
    minWidth: 60,
    maxWidth: 60,
    Cell: (cellProps: CellProps<Api_PropertyFile, Api_Property | undefined>) => {
      const propertyName = getFilePropertyName(cellProps.row.original);
      return <OverflowTip fullText={`${propertyName.label}: ${propertyName.value}`} />;
    },
  },
];

const FilePropertiesTable: React.FunctionComponent<IFilePropertiesTableProps> = ({
  fileProperties,
  selectedFileProperties,
  setSelectedFileProperties,
  disabledSelection,
}) => {
  return (
    <>
      <Table<Api_PropertyFile>
        name="selectableFileProperties"
        hideHeaders
        hideToolbar
        manualPagination
        showSelectedRowCount
        columns={columns}
        data={fileProperties}
        setSelectedRows={setSelectedFileProperties}
        selectedRows={selectedFileProperties}
        disableSelection={disabledSelection}
      />
    </>
  );
};

export default FilePropertiesTable;
