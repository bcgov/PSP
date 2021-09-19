import { Input } from 'components/common/form';
import { TypeaheadField } from 'components/common/form/Typeahead';
import { ColumnWithProps } from 'components/Table';
import { IProperty } from 'interfaces';
import { CellProps } from 'react-table';
import { ILookupCode } from 'store/slices/lookupCodes';
import styled from 'styled-components';
import { formatNumber, formatStreetAddress, mapLookupCode } from 'utils';

export const ColumnDiv = styled.div`
  display: flex;
  flex-flow: column;
  padding-right: 5px;
`;

const NumberCell = ({ cell: { value } }: CellProps<IProperty, number>) => formatNumber(value);

type Props = {
  municipalities: ILookupCode[];
};

export const columns = ({ municipalities }: Props): ColumnWithProps<IProperty>[] => [
  {
    Header: 'PID',
    accessor: 'pid',
    align: 'left',
    sortable: false,
  },
  {
    Header: 'PIN',
    accessor: 'pin',
    align: 'left',
    sortable: false,
  },
  {
    Header: 'Civic Address',
    accessor: p => formatStreetAddress(p.address),
    align: 'left',
    clickable: true,
    minWidth: 100,
    sortable: false,
  },
  {
    Header: 'Location',
    accessor: p => p.address.municipality,
    align: 'left',
    minWidth: 80,
    clickable: true,
    sortable: false,
    filter: {
      component: TypeaheadField,
      props: {
        name: 'municipality',
        placeholder: 'Filter by location',
        className: 'location-search',
        options: municipalities.map(mapLookupCode).map(x => x.label),
        clearButton: true,
        hideValidation: true,
      },
    },
  },
  {
    Header: 'Lot Size (in\u00A0ha)',
    accessor: 'landArea',
    Cell: NumberCell,
    align: 'right',
    minWidth: 120,
    clickable: true,
    sortable: false,
    filter: {
      component: Input,
      props: {
        field: 'maxLotSize',
        name: 'maxLotSize',
        placeholder: 'Filter by Lot Size',
        className: 'filter-input-control',
        type: 'number',
      },
    },
  },
];
