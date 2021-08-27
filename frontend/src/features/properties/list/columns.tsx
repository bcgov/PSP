import { Input, SelectOption } from 'components/common/form';
import { TypeaheadField } from 'components/common/form/Typeahead';
import { ColumnWithProps } from 'components/Table';
import { PropertyTypeCell } from 'components/Table/PropertyTypeCell';
import { PropertyTypes } from 'constants/index';
import { IProperty } from 'interfaces';
import { isEqual } from 'lodash';
import React from 'react';
import { CellProps } from 'react-table';
import { ILookupCode } from 'store/slices/lookupCodes';
import styled from 'styled-components';
import { formatNumber, mapLookupCode } from 'utils';

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

export const columns = (
  organizationOptions: SelectOption[],
  subOrganizations: SelectOption[],
  municipalities: ILookupCode[],
  propertyClassifications: SelectOption[],
  propertyType: PropertyTypes,
  editable?: boolean,
): ColumnWithProps<IProperty>[] => [
  {
    Header: 'Property Name',
    accessor: 'name',
    align: 'left',
    clickable: true,
    responsive: true,
    width: spacing.medium,
    minWidth: 140,
    sortable: true,
  },
  {
    Header: 'Classification',
    accessor: 'classification',
    align: 'left',
    responsive: false,
    width: spacing.small,
    minWidth: 90,
    clickable: true,
    sortable: true,
    filterable: true,
    filter: {
      component: TypeaheadField,
      props: {
        field: 'classificationId',
        name: 'classificationId',
        placeholder: 'Filter by class',
        className: 'location-search',
        options: propertyClassifications,
        labelKey: (option: SelectOption) => {
          return `${option.label}`;
        },
        clearButton: true,
        getOptionByValue: (value: number | string) => {
          return propertyClassifications.filter(a => isEqual(a.value, value));
        },
      },
    },
  },
  {
    Header: 'Type',
    accessor: 'propertyTypeId',
    Cell: PropertyTypeCell,
    clickable: true,
    responsive: true,
    width: spacing.xsmall,
    minWidth: 60,
  },
  {
    Header: 'PID',
    accessor: 'pid',
    width: spacing.medium,
    responsive: true,
    align: 'left',
  },
  {
    Header: 'Street Address',
    accessor: 'address',
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
];
