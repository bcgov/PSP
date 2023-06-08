import OverflowTip from 'components/common/OverflowTip';
import { ColumnWithProps, Table } from 'components/Table';
import { Api_Property } from 'models/api/Property';
import { Api_PropertyFile } from 'models/api/PropertyFile';
import * as React from 'react';
import { CellProps } from 'react-table';
import { getFilePropertyName } from 'utils/mapPropertyUtils';

interface IInterestHolderPropertiesProps {
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

const InterestHolderPropertiesTable: React.FunctionComponent<
  React.PropsWithChildren<IInterestHolderPropertiesProps>
> = ({ fileProperties, selectedFileProperties, setSelectedFileProperties, disabledSelection }) => {
  return (
    <>
      <Table<Api_PropertyFile>
        name="selectableProperties"
        columns={columns}
        manualPagination
        hideToolbar
        showSelectedRowCount
        data={fileProperties}
        setSelectedRows={setSelectedFileProperties}
        selectedRows={selectedFileProperties}
        disableSelection={disabledSelection}
        hideHeaders
      />
    </>
  );
};

export default InterestHolderPropertiesTable;
