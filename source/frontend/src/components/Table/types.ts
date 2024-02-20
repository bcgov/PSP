import { Cell, Column, ColumnInstance } from 'react-table';

// Mixed bag of optional properties to supply to the ColumnDefinitions below
// NOTE - make sure you all properties below are optional!
interface IExtraColumnProps<T> {
  align?: 'left' | 'right' | 'center';
  clickable?: boolean;
  sortable?: boolean;
  // Whether to use width percentages vs hard-coded widths in pixels. Percentages support responsive design.
  responsive?: boolean;
  expandable?: boolean;
  filterable?: boolean;
  filter?: {
    component?:
      | React.ComponentType<React.PropsWithChildren<unknown>>
      | React.FC<React.PropsWithChildren<any>>;
    props?: { [key: string]: any } | (() => { [key: string]: any });
  };
  Footer?: React.FC<T>;
  className?: string;
}

// Typings for configuration sent to `react-table`
export type ColumnWithProps<D extends object = object, T extends object = object> = Column<D> &
  IExtraColumnProps<T>;

// Typings for object instances - as returned by `react-table`
export type ColumnInstanceWithProps<
  D extends object = object,
  T extends object = object,
> = ColumnInstance<D> & IExtraColumnProps<T>;
export type CellWithProps<D extends object = object> = Cell<D> & {
  column: ColumnInstanceWithProps<D>;
};
