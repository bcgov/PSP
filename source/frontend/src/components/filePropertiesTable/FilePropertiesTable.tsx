import * as React from 'react';
import { CellProps } from 'react-table';

import OverflowTip from '@/components/common/OverflowTip';
import { ColumnWithProps, Table } from '@/components/Table';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { getFilePropertyName } from '@/utils/mapPropertyUtils';

interface IFilePropertiesTableProps {
  fileProperties: ApiGen_Concepts_FileProperty[];
  setSelectedFileProperties: (properties: ApiGen_Concepts_FileProperty[]) => void;
  selectedFileProperties: ApiGen_Concepts_FileProperty[];
  disabledSelection: boolean;
}

const columns: ColumnWithProps<ApiGen_Concepts_FileProperty>[] = [
  {
    Header: 'Identifier',
    accessor: 'property',
    align: 'left',
    minWidth: 60,
    maxWidth: 60,
    Cell: (cellProps: CellProps<ApiGen_Concepts_FileProperty, ApiGen_Concepts_Property | null>) => {
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
      <Table<ApiGen_Concepts_FileProperty>
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
