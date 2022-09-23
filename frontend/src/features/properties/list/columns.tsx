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
  padding-right: 0.5rem;
`;

const NumberCell = ({ cell: { value } }: CellProps<IProperty, number>) => formatNumber(value);

type Props = {
  municipalities: ILookupCode[];
};

export const columns = ({ municipalities }: Props): ColumnWithProps<IProperty>[] => [
  {
    Header: 'PID',
    accessor: 'pid',
    align: 'right',
    width: 40,
  },
  {
    Header: 'PIN',
    accessor: 'pin',
    align: 'right',
    width: 40,
  },
  {
    Header: 'Civic Address',
    accessor: p => formatStreetAddress(p.address),
    align: 'left',
    minWidth: 100,
    width: 150,
  },
  {
    Header: 'Location',
    accessor: p => p.address?.municipality,
    align: 'left',
    width: 50,
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
    width: 20,
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
