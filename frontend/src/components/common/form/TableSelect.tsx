import { Button } from 'components/common/buttons/Button';
import { DisplayError } from 'components/common/form';
import * as Styled from 'components/common/form/styles';
import { ColumnWithProps, Table } from 'components/Table';
import { getIn, useFormikContext } from 'formik';
import _ from 'lodash';
import React, { ReactNode, useMemo } from 'react';
import { Container, FormControlProps } from 'react-bootstrap';
import { getColumnsWithRemove } from 'utils/columnUtils';

export interface ISelectedTableHeaderProps {
  selectedCount?: number;
}

type RequiredAttributes<T extends object> = {
  /** The field name */
  field: string;
  /** The items that are currently selected in the passed selectComponent */
  selectedItems: T[];
  /** A header for the second table that contains selected items */
  selectedTableHeader: React.FC<ISelectedTableHeaderProps>;
  /** The columns that should be used for the secondary, "saved items" table */
  columns: ColumnWithProps<T>[];
  /** child component that handles selection */
  children: ReactNode;
};

type OptionalAttributes = {
  /** The form component label */
  label?: string;
  /** The underlying HTML element to use when rendering the FormControl */
  as?: React.ElementType;
  /** Short hint that describes the expected value of an <input> element */
  placeholder?: string;
  /** Specifies that the HTML element should be disabled */
  disabled?: boolean;
  /** Use React-Bootstrap's custom form elements to replace the browser defaults */
  custom?: boolean;
  /** Optional label to be assigned to the add button */
  addLabel?: string;
};

export type TableSelectProps<T extends object> = FormControlProps &
  OptionalAttributes &
  RequiredAttributes<T>;

/**
 * Formik-connected allowing multiple table rows to be selected/removed
 */
export const TableSelect = <T extends { id?: string | number }>({
  field,
  disabled,
  selectedItems,
  children,
  selectedTableHeader: SelectedTableHeader,
  columns,
  addLabel,
}: TableSelectProps<T>) => {
  const { values, setFieldValue } = useFormikContext<any>();
  const existingItems: T[] = getIn(values, field);
  const columnsWithRemove = useMemo(
    () => getColumnsWithRemove<T>((rows: T[]) => setFieldValue(field, rows), [...columns]),
    [columns, field, setFieldValue],
  );

  return (
    <Container className="col-md-12">
      {!disabled && (
        <div>
          {children}
          <Button
            variant="secondary"
            onClick={() => {
              setFieldValue(
                field,
                _.uniqWith(_.concat(existingItems, selectedItems), (p1, p2) => p1.id === p2.id),
              );
            }}
          >
            {addLabel ?? 'Add Selected'}
          </Button>
        </div>
      )}
      <Styled.SaveTableWrapper>
        <SelectedTableHeader selectedCount={existingItems?.length} />
        <Table<T>
          name="selected-items"
          columns={columnsWithRemove}
          data={existingItems ?? []}
          lockPageSize
          pageSize={-1}
          footer
        />
      </Styled.SaveTableWrapper>
      <DisplayError field={field} />
    </Container>
  );
};
