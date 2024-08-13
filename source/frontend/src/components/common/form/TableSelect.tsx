import React from 'react';
import { Container, FormControlProps } from 'react-bootstrap';

import { DisplayError } from '@/components/common/form';
import * as Styled from '@/components/common/form/styles';
import { ColumnWithProps, Table } from '@/components/Table';
import { getColumnsWithRemove } from '@/utils/columnUtils';

export interface ISelectedTableHeaderProps {
  selectedCount?: number;
  isPayableLease?: boolean;
}

type RequiredAttributes<T extends object> = {
  /** The field name */
  field: string;
  /** The items that are currently selected in the passed selectComponent */
  selectedItems: T[];
  /** A header for the second table that contains selected items */
  selectedTableHeader: React.FC<React.PropsWithChildren<ISelectedTableHeaderProps>>;
  /** The columns that should be used for the secondary, "saved items" table */
  columns: ColumnWithProps<T>[];
  onRemove: (items: T[]) => void;
  isPayableLease: boolean;
};

export type TableSelectProps<T extends object> = FormControlProps & RequiredAttributes<T>;

/**
 * Formik-connected allowing multiple table rows to be selected/removed
 */
export const TableSelect = <T extends { id?: string | number }>({
  field,
  selectedItems,
  selectedTableHeader: SelectedTableHeader,
  onRemove,
  columns,
  isPayableLease,
}: TableSelectProps<T>) => {
  const columnsWithRemove = getColumnsWithRemove<T>(
    (rows: T[]) => onRemove(rows),
    [...columns],
    'Action',
  );

  return (
    <Container className="p-0 m-0">
      <Styled.SaveTableWrapper>
        <SelectedTableHeader selectedCount={selectedItems.length} isPayableLease={isPayableLease} />
        <Table<T>
          name="selected-items"
          lockPageSize
          hidePagination
          footer
          columns={columnsWithRemove}
          data={selectedItems}
          pageSize={selectedItems.length}
        />
      </Styled.SaveTableWrapper>
      <DisplayError field={field} />
    </Container>
  );
};
