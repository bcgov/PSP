import { FastCurrencyInput, Input, Select, SelectOption } from 'components/common/form';
import { TypeaheadField } from 'components/common/form/Typeahead';
import { ColumnWithProps } from 'components/Table';
import { AsterixMoneyCell, EditableMoneyCell, MoneyCell } from 'components/Table/MoneyCell';
import { PropertyTypeCell } from 'components/Table/PropertyTypeCell';
import { PropertyTypes } from 'constants/index';
import { isEqual } from 'lodash';
import React from 'react';
import { CellProps } from 'react-table';
import { ILookupCode } from 'store/slices/lookupCodes';
import styled from 'styled-components';
import { formatNumber, mapLookupCode } from 'utils';

import { IProperty } from '.';

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
  propertyType: number,
  editable?: boolean,
): ColumnWithProps<IProperty>[] => [
  {
    Header: 'Organization',
    accessor: 'organizationCode', // accessor is the "key" in the data
    align: 'left',
    responsive: true,
    width: spacing.xsmall,
    minWidth: 80, // px
    clickable: true,
    sortable: true,
    filterable: true,
    filter: {
      component: TypeaheadField,
      props: {
        className: 'organization-search',
        name: 'organizations[0]',
        options: organizationOptions.map(a => ({ ...a, parentId: a.value })),
        inputSize: 'large',
        placeholder: 'Filter by organization',
        filterBy: ['code'],
        hideParent: true,
        clearButton: true,
        getOptionByValue: (value: number | string) => {
          return organizationOptions.filter(a => Number(a.value) === Number(value));
        },
      },
    },
  },
  {
    Header: 'Sub Organization',
    accessor: 'subOrganization',
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
        name: 'organizations[1]',
        placeholder: 'Filter by sub organization',
        className: 'organization-search',
        options: subOrganizations,
        clearButton: true,
        labelKey: (option: SelectOption) => {
          return `${option.label}`;
        },
        getOptionByValue: (value: number | string) => {
          return subOrganizations.filter(a => Number(a.value) === Number(value));
        },
      },
    },
  },
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
    accessor: 'administrativeArea',
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
        name: 'administrativeArea',
        placeholder: 'Filter by location',
        className: 'location-search',
        options: municipalities.map(mapLookupCode).map(x => x.label),
        clearButton: true,
        hideValidation: true,
      },
    },
  },
  {
    Header: 'Assessed Land',
    accessor: 'assessedLand',
    Cell: !editable
      ? MoneyCell
      : (props: any) => <EditableMoneyCell {...props} suppressValidation />,
    align: 'right',
    responsive: true,
    width: spacing.small,
    minWidth: 100,
    clickable: !editable,
    sortable: true,
    filterable: true,
    filter: {
      component: FastCurrencyInput,
      props: {
        injectFormik: true,
        field: 'maxAssessedValue',
        name: 'maxAssessedValue',
        placeholder: 'Max Assessed value',
        tooltip: 'Filter by max assessed value',
        className: 'filter-input-control',
      },
    },
  },
  {
    Header: 'Assessed Building(s)',
    accessor: 'assessedBuilding',
    Cell: !editable
      ? propertyType === PropertyTypes.Building
        ? AsterixMoneyCell
        : MoneyCell
      : (props: any) => <EditableMoneyCell {...props} suppressValidation />,
    align: 'right',
    responsive: true,
    width: spacing.small,
    minWidth: 100,
    clickable: !editable,
    sortable: true,
    filterable: true,
    filter: {
      component: FastCurrencyInput,
      props: {
        injectFormik: true,
        field: 'maxAssessedValue',
        name: 'maxAssessedValue',
        placeholder: 'Max Assessed value',
        tooltip: 'Filter by max assessed value',
        className: 'filter-input-control',
      },
    },
  },
  {
    Header: 'Net Book Value',
    accessor: 'netBook',
    Cell: !editable
      ? MoneyCell
      : (props: any) => <EditableMoneyCell {...props} suppressValidation />,
    align: 'right',
    responsive: true,
    width: spacing.small,
    minWidth: 100,
    clickable: !editable,
    sortable: true,
    filterable: true,
    filter: {
      component: FastCurrencyInput,
      props: {
        injectFormik: true,
        field: 'maxNetBookValue',
        name: 'maxNetBookValue',
        placeholder: 'Max Net Book Value',
        tooltip: 'Filter by max net book value',
        className: 'filter-input-control',
      },
    },
  },
  {
    Header: 'Market Value',
    accessor: 'market',
    Cell: !editable
      ? MoneyCell
      : (props: any) => <EditableMoneyCell {...props} suppressValidation />,
    align: 'right',
    responsive: true,
    width: spacing.small,
    minWidth: 100,
    clickable: !editable,
    sortable: true,
    filterable: true,
    filter: {
      component: FastCurrencyInput,
      props: {
        injectFormik: true,
        field: 'maxMarketValue',
        name: 'maxMarketValue',
        placeholder: 'Max Market Value',
        tooltip: 'Filter by max market value',
        className: 'filter-input-control',
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

export const buildingColumns = (
  organizationOptions: SelectOption[],
  subOrganizations: SelectOption[],
  municipalities: ILookupCode[],
  propertyClassifications: SelectOption[],
  propertyType: number,
  editable?: boolean,
): ColumnWithProps<IProperty>[] => [
  {
    Header: 'Organization',
    accessor: 'organizationCode', // accessor is the "key" in the data
    align: 'left',
    responsive: true,
    width: spacing.xsmall,
    minWidth: 80, // px
    clickable: true,
    sortable: true,
    filterable: true,
    filter: {
      component: TypeaheadField,
      props: {
        className: 'organization-search',
        name: 'organizations[0]',
        options: organizationOptions.map(a => ({ ...a, parentId: a.value })),
        inputSize: 'large',
        placeholder: 'Filter by organization',
        filterBy: ['code'],
        hideParent: true,
        clearButton: true,
      },
    },
  },
  {
    Header: 'Sub Organization',
    accessor: 'subOrganization',
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
        name: 'organizations[1]',
        placeholder: 'Filter by sub organization',
        className: 'organization-search',
        options: subOrganizations,
        labelKey: (option: SelectOption) => {
          return `${option.label}`;
        },
      },
    },
  },
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
      component: Select,
      props: {
        field: 'classificationId',
        name: 'classificationId',
        placeholder: 'Filter by class',
        className: 'location-search',
        options: propertyClassifications,
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
    accessor: 'administrativeArea',
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
        name: 'administrativeArea',
        placeholder: 'Filter by location',
        className: 'location-search',
        options: municipalities.map(mapLookupCode).map(x => x.label),
        clearButton: true,
        hideValidation: true,
      },
    },
  },
  {
    Header: 'Assessed Building(s)',
    accessor: 'assessedBuilding',
    Cell: !editable
      ? MoneyCell
      : (props: any) => <EditableMoneyCell {...props} suppressValidation />,
    align: 'right',
    responsive: true,
    width: spacing.small,
    minWidth: 100,
    clickable: !editable,
    sortable: true,
    filterable: true,
    filter: {
      component: FastCurrencyInput,
      props: {
        injectFormik: true,
        field: 'maxAssessedValue',
        name: 'maxAssessedValue',
        placeholder: 'Max Assessed value',
        tooltip: 'Filter by max assessed value',
        className: 'filter-input-control',
      },
    },
  },
  {
    Header: 'Net Book Value',
    accessor: 'netBook',
    Cell: !editable
      ? MoneyCell
      : (props: any) => <EditableMoneyCell {...props} suppressValidation />,
    align: 'right',
    responsive: true,
    width: spacing.small,
    minWidth: 100,
    clickable: !editable,
    sortable: true,
    filterable: true,
    filter: {
      component: FastCurrencyInput,
      props: {
        injectFormik: true,
        field: 'maxNetBookValue',
        name: 'maxNetBookValue',
        placeholder: 'Max Net Book Value',
        tooltip: 'Filter by max net book value',
        className: 'filter-input-control',
      },
    },
  },
  {
    Header: 'Market Value',
    accessor: 'market',
    Cell: !editable
      ? MoneyCell
      : (props: any) => <EditableMoneyCell {...props} suppressValidation />,
    align: 'right',
    responsive: true,
    width: spacing.small,
    minWidth: 100,
    clickable: !editable,
    sortable: true,
    filterable: true,
    filter: {
      component: FastCurrencyInput,
      props: {
        injectFormik: true,
        field: 'maxMarketValue',
        name: 'maxMarketValue',
        placeholder: 'Max Market Value',
        tooltip: 'Filter by max market value',
        className: 'filter-input-control',
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
