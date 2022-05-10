import { ColumnWithProps, IIdentifiedObject } from 'components/Table';
import { AreaUnitTypes } from 'constants/index';
import Api_TypeCode from 'models/api/TypeCode';
import React from 'react';
import { CellProps } from 'react-table';
import { convertArea } from 'utils/convertUtils';

export interface TableDataSource extends IIdentifiedObject {
  value: number;
  unit: string;
  unitDisplayValue: string;
}

export function createColumns(
  state: Record<string, number>,
  setState: React.Dispatch<React.SetStateAction<Record<string, number>>>,
  setFocus: React.Dispatch<React.SetStateAction<string>>,
): ColumnWithProps<TableDataSource>[] {
  const columns: ColumnWithProps<TableDataSource>[] = [
    {
      accessor: 'value',
      align: 'right',
      Cell: ({ value, row }: CellProps<TableDataSource>) => {
        console.log('re-render');
        const unit = row.original.unit;
        return (
          <input
            type="number"
            style={{ maxWidth: '12.5rem' }}
            value={state[unit]}
            onChange={e => {
              if (unit) {
                setFocus(unit);
                setState(prevState => ({ ...prevState, [unit]: e.target.valueAsNumber }));
              }
            }}
          />
        );
      },
    },
    { accessor: 'unitDisplayValue', align: 'left' },
  ];
  return columns;
}

export function generateTableData(
  landArea: number,
  areaUnit: Api_TypeCode<string>,
): TableDataSource[] {
  const unitId = areaUnit.id as string;

  if (typeof landArea === 'undefined' || typeof unitId === 'undefined') {
    return [];
  }

  return [
    {
      value: convertArea(landArea, unitId, AreaUnitTypes.SquareMeters),
      unit: AreaUnitTypes.SquareMeters,
      unitDisplayValue: 'sq. metres',
    },
    {
      value: convertArea(landArea, unitId, AreaUnitTypes.SquareFeet),
      unit: AreaUnitTypes.SquareFeet,
      unitDisplayValue: 'sq. feet',
    },
    {
      value: convertArea(landArea, unitId, AreaUnitTypes.Hectares),
      unit: AreaUnitTypes.Hectares,
      unitDisplayValue: 'hectares',
    },
    {
      value: convertArea(landArea, unitId, AreaUnitTypes.Acres),
      unit: AreaUnitTypes.Acres,
      unitDisplayValue: 'acres',
    },
  ];
}

export function roundToTwoDecimals(value: number) {
  return Math.round((value + Number.EPSILON) * 100) / 100;
}
