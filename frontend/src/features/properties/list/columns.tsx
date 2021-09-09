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

// NOTE - There numbers below match the total number of columns ATM (13)
// If additional columns are added or deleted, these numbers need tp be updated...
const howManyColumns = 13;
const totalWidthPercent = 100; // how wide the table should be; e.g. 100%

// Setup a few sample widths: x/2, 1x, 2x (percentage-based)
const unit = Math.floor(totalWidthPercent / howManyColumns);
const spacing = {
  xxsmall: 1,
  xsmall: unit / 4,
  small: unit / 2,
  medium: unit,
  large: unit * 2,
  xlarge: unit * 4,
  xxlarge: unit * 8,
};

type Props = {
  municipalities: ILookupCode[];
};

export const columns = ({ municipalities }: Props): ColumnWithProps<IProperty>[] => [
  {
    Header: 'PID',
    accessor: 'pid',
    width: spacing.medium,
    responsive: true,
    align: 'left',
  },
  {
    Header: 'PIN',
    accessor: 'pin',
    width: spacing.medium,
    responsive: true,
    align: 'left',
  },
  {
    Header: 'Civic Address',
    accessor: p => formatStreetAddress(p.address),
    align: 'left',
    clickable: true,
    responsive: true,
    width: spacing.medium,
    minWidth: 100,
    sortable: true,
  },
  {
    Header: 'Location',
    accessor: p => p.address.municipality,
    align: 'left',
    responsive: true,
    width: spacing.medium,
    minWidth: 80,
    clickable: true,
    sortable: true,
    filterable: true,
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
    responsive: true,
    width: spacing.small,
    minWidth: 120,
    clickable: true,
    sortable: true,
    filterable: true,
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
  // TODO: FIXME: Find out how to get registered owner of properties!
  {
    Header: 'Registered Owner',
    accessor: p => null,
    align: 'left',
    responsive: true,
    width: spacing.medium,
    minWidth: 100,
    sortable: true,
  },
];
