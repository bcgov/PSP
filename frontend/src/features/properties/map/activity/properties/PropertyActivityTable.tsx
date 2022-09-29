import { ColumnWithProps, Table } from 'components/Table';
import { Section } from 'features/mapSideBar/tabs/Section';
import { getFilePropertyName } from 'features/properties/selector/utils';
import { Api_Property } from 'models/api/Property';
import { Api_PropertyFile } from 'models/api/PropertyFile';
import * as React from 'react';
import { CellProps } from 'react-table';

interface IPropertyActivityFormProps {
  fileProperties: Api_PropertyFile[];
  setSelectedFileProperties: (properties: Api_PropertyFile[]) => void;
  selectedFileProperties: Api_PropertyFile[];
}

const columns: ColumnWithProps<Api_PropertyFile>[] = [
  {
    Header: '#',
    accessor: 'id',
    align: 'left',
    minWidth: 7,
    maxWidth: 7,
    Cell: (cellProps: CellProps<Api_PropertyFile>) => {
      return cellProps.row.index + 1;
    },
  },
  {
    Header: 'Property name',
    accessor: 'propertyName',
    align: 'left',
    minWidth: 40,
  },
  {
    Header: 'Identifier',
    accessor: 'property',
    align: 'left',
    minWidth: 60,
    maxWidth: 60,
    Cell: (cellProps: CellProps<Api_PropertyFile, Api_Property>) => {
      const propertyName = getFilePropertyName(cellProps.row.original, true);
      return `${propertyName.label}: ${propertyName.value}`;
    },
  },
];

const PropertyActivityTable: React.FunctionComponent<IPropertyActivityFormProps> = ({
  fileProperties,
  selectedFileProperties,
  setSelectedFileProperties,
}) => {
  return (
    <>
      <p>Select the properties from the parent file that this activity relates to:</p>
      <Section
        isCollapsable={false}
        header={`Properties (${selectedFileProperties?.length})`}
        className="m-0 p-0"
      >
        <Table
          name="selectableProperties"
          columns={columns}
          manualPagination
          hideToolbar
          showSelectedRowCount
          data={fileProperties}
          setSelectedRows={setSelectedFileProperties}
          selectedRows={selectedFileProperties}
        />
      </Section>
    </>
  );
};

export default PropertyActivityTable;
