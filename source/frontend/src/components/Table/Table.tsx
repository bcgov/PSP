import './Table.scss';

import cx from 'classnames';
import { Form, Formik, FormikProps } from 'formik';
import keys from 'lodash/keys';
import map from 'lodash/map';
import React, {
  PropsWithChildren,
  ReactElement,
  ReactNode,
  useEffect,
  useMemo,
  useRef,
} from 'react';
import { Col as ColBootstrap, Row as RowBootstrap } from 'react-bootstrap';
import Collapse from 'react-bootstrap/Collapse';
import Spinner from 'react-bootstrap/Spinner';
import { FaAngleDown, FaAngleRight, FaUndo } from 'react-icons/fa';
import {
  Cell,
  HeaderGroup,
  Row,
  TableOptions,
  useFlexLayout,
  usePagination,
  useRowSelect,
  useSortBy,
  useTable,
} from 'react-table';

import { Button } from '@/components/common/buttons/Button';
import { SelectedText } from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { handleSortChange } from '@/hooks/useSearch';
import useDeepCompareCallback from '@/hooks/util/useDeepCompareCallback';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import useDeepCompareMemo from '@/hooks/util/useDeepCompareMemo';

import { TablePagination } from '.';
import ColumnSort from './ColumnSort';
import { DEFAULT_PAGE_SELECTOR_OPTIONS, DEFAULT_PAGE_SIZE } from './constants';
import { TablePageSizeSelector } from './PageSizeSelector';
import { SortDirection, TableSort } from './TableSort';
import { CellWithProps, ColumnInstanceWithProps } from './types';

// these provide a way to inject custom CSS into table headers and cells
const headerPropsGetter = <T extends object>(
  props: any,
  { column }: { column: ColumnInstanceWithProps<T> },
) => {
  return getStyles(props, true, column);
};

const noHeadersGetter = <T extends object>(
  props: any,
  { column }: { column: ColumnInstanceWithProps<T> },
) => {
  return getStyles(props, true, column, true);
};

const cellPropsGetter = <T extends object>(props: any, { cell }: { cell: Cell<T> }) => {
  return getStyles(props, false, cell.column);
};

const getJustify = (align?: string) => {
  switch (align) {
    case 'center':
      return 'center';
    case 'right':
      return 'right';
    default:
      return 'left';
  }
};

const getStyles = <T extends object>(
  props: any,
  isHeader: boolean,
  column: ColumnInstanceWithProps<T>,
  hideHeaders?: boolean,
) => {
  // override column width when percentage value is provided - react-table deals with pixel values
  const colSize = column?.responsive
    ? {
        width: `${column?.width}%`,
      }
    : {};
  // return CSS styles: `props` are react-table defaults, the rest are our overrides...
  return [
    props,
    {
      style: {
        justifyContent: getJustify(column?.align),
        textAlign: column?.align ?? 'left',
        flexWrap: 'wrap',
        alignItems: 'center',
        display: isHeader && hideHeaders ? 'none' : 'flex',
        ...colSize,
      },
    },
    colSize,
  ];
};

interface DetailsOptions<T extends object> {
  render: (data: T) => ReactNode;
  icons?: { open: ReactNode; closed: ReactNode };
  onExpand?: (data: T[]) => void;
  checkExpanded: (row: T, state: T[]) => boolean;
  getRowId: (row: T) => any;
}

interface ExternalSort<T extends object> {
  sort: TableSort<T>;
  setSort: (sort: TableSort<T>) => void;
}

export interface TableProps<T extends object = object, TFilter extends object = object>
  extends TableOptions<T> {
  name: string;
  showSelectedRowCount?: boolean;
  hideHeaders?: boolean;
  onRequestData?: (props: { pageIndex: number; pageSize: number }) => void;
  loading?: boolean;
  totalItems?: number;
  pageCount?: number;
  pageSize?: number;
  pageSizeOptions?: number[];
  pageIndex?: number;
  onRowClick?: (data: T) => void;
  clickableTooltip?: string;
  onPageSizeChange?: (size: number) => void;
  externalSort?: ExternalSort<T>;
  noRowsMessage?: string;
  selectedRows?: T[];
  setSelectedRows?: (rows: T[]) => void;
  lockPageSize?: boolean;
  detailsPanel?: DetailsOptions<T>;
  footer?: boolean;
  hidePagination?: boolean;
  hideToolbar?: boolean;
  tableToolbarText?: string;
  manualPagination?: boolean;
  isSingleSelect?: boolean;
  disableSelection?: boolean;
  // Limit where you would like an expansion button to appear based off this props criteria
  canRowExpand?: (val: any) => boolean;
  className?: string;
  filterable?: boolean;
  filter?: TFilter;
  onFilterChange?: (values: any) => void;
  /** have page selection menu drop-up to avoid container growing in some scenarios */
  pageSizeMenuDropUp?: boolean;
  /**
   * An optional render callback to wrap the table body
   */
  renderBodyComponent?: ({
    body,
  }: {
    /**
     * table body
     */
    body: React.ReactNode;
  }) => React.ReactNode;
}

const IndeterminateCheckbox = React.forwardRef(
  (
    {
      indeterminate,
      setSelected,
      selectedRef,
      allDataRef,
      checked,
      row,
      isSingleSelect,
      disableSelection,
      ...rest
    }: any,
    ref,
  ) => {
    const defaultRef = React.useRef();
    const resolvedRef: any = ref || defaultRef;
    const isHeaderCheck = !!allDataRef?.current;
    if (isHeaderCheck) {
      rest.title = 'Click to deselect all.';
    }

    React.useEffect(() => {
      if (resolvedRef.current) {
        resolvedRef.current.indeterminate = indeterminate;
      }
    }, [resolvedRef, indeterminate]);

    React.useEffect(() => {
      if (resolvedRef.current) {
        resolvedRef.current.checked = checked;
      }
    }, [resolvedRef, checked]);

    const onChainedChange = (e: any) => {
      rest.onChange && !allDataRef && rest.onChange(e);
      const currentSelected = selectedRef?.current ? [...(selectedRef?.current ?? [])] : [];
      if (isHeaderCheck) {
        if (e.target.checked && !indeterminate) {
          currentSelected.push(...(allDataRef?.current ?? []));
          setSelected(allDataRef?.current ?? []);
        } else {
          setSelected([]);
        }
      } else {
        setSelected([row.original]);
      }
    };

    rest.checked = isHeaderCheck ? !indeterminate && checked : checked;
    return (
      <>
        <input
          disabled={disableSelection}
          type={isSingleSelect === true ? 'radio' : 'checkbox'}
          name={isSingleSelect === true ? 'table-radio' : ''}
          ref={resolvedRef}
          {...rest}
          onChange={onChainedChange}
          data-testid={`selectrow-${row?.original?.id ?? 'parent'}`}
        />
      </>
    );
  },
);

export interface IIdentifiedObject {
  id?: number | string | null;
}

const validateProps = <T extends IIdentifiedObject, TFilter extends object = object>(
  props: React.PropsWithChildren<TableProps<T, TFilter>>,
) => {
  if (props.hideToolbar === true && props.manualPagination === false) {
    throw Error('When hiding pagination toolbar manual pagination must be true.');
  }
  if (props.manualPagination === false && (props.onRequestData !== undefined || props.pageIndex)) {
    throw Error(
      'When manual pagination is set to false do not pass in onRequestData or pageIndex.',
    );
  }
  if ((props.setSelectedRows || props.isSingleSelect) && !props.showSelectedRowCount) {
    throw Error('Show selected row count required for interactive table select logic');
  }
};

/**
 * A table component. Supports sorting, filtering and paging.
 * Uses `react-table` to handle table logic.
 */
export const Table = <T extends IIdentifiedObject, TFilter extends object = object>(
  props: PropsWithChildren<TableProps<T, TFilter>>,
): ReactElement => {
  const filterFormRef = useRef<FormikProps<any>>();

  const [expandedRows, setExpandedRows] = React.useState<T[]>([]);
  const [internalPageSize, setInternalPageSize] = React.useState<number>(
    props.pageSize ?? DEFAULT_PAGE_SIZE,
  );
  const defaultColumn = React.useMemo(
    () => ({
      // When using the useFlexLayout:
      minWidth: 30, // minWidth is only used as a limit for resizing
      width: 100, // width is used for both the flex-basis and flex-grow
    }),
    [],
  );
  validateProps(props);

  const {
    clickableTooltip,
    columns,
    data,
    onRequestData,
    totalItems: externalTotalItems,
    pageCount: externalPageCount,
    selectedRows: externalSelectedRows,
    setSelectedRows: setExternalSelectedRows,
    footer,
    pageSize: pageSizeProp,
    pageIndex: pageIndexProp,
    manualPagination,
    filterable,
    renderBodyComponent,
    externalSort,
    isSingleSelect,
    disableSelection,
  } = props;
  const manualSortBy = !!externalSort || props.manualSortBy;
  const totalItems = externalTotalItems ?? data?.length;
  const pageCount =
    externalPageCount ?? (internalPageSize > 0 ? Math.ceil(totalItems / internalPageSize) : 0);
  const selectedRowsRef = React.useRef<T[]>(externalSelectedRows ?? []); // used as a global var for providing up to date list of selected rows to code within the table (that is arrow function scoped).

  // manually update the contents of the global ref of selected rows if the list of external selected rows changes.
  useEffect(() => {
    selectedRowsRef.current = externalSelectedRows ?? [];
  }, [selectedRowsRef, externalSelectedRows]);

  const dataRef = React.useRef<T[]>(data ?? []);
  React.useEffect(() => {
    dataRef.current = data ?? [];
  }, [data]);

  React.useEffect(() => {
    if (filterFormRef.current) {
      filterFormRef.current.setValues(props.filter);
    }
  }, [filterFormRef, props.filter]);

  const sortBy = useMemo(() => {
    return externalSort?.sort
      ? keys(externalSort.sort).map(key => ({
          id: key,
          desc: (externalSort.sort as any)[key] === 'desc',
        }))
      : [];
  }, [externalSort?.sort]);

  // Use the useTable hook to create your table configuration
  const instance = useTable<T>(
    {
      columns,
      data,
      defaultColumn,
      initialState: pageSizeProp
        ? {
            sortBy,
            pageIndex: pageIndexProp ?? 0,
            pageSize: pageSizeProp ?? internalPageSize,
          }
        : { sortBy, pageIndex: pageIndexProp ?? 0 },
      manualPagination: (props.hideToolbar || manualPagination) ?? true, // Tell the usePagination hook
      // that we'll handle our own data fetching.
      // This means we'll also have to provide our own
      // pageCount. In the majority of cases we want to use manualPagination.
      manualSortBy: manualSortBy,
      pageCount,
      autoResetSelectedRows: false,
    },
    useFlexLayout,
    useSortBy,
    usePagination,
    useRowSelect,
    hooks => {
      hooks.visibleColumns.push(columns => {
        return props.showSelectedRowCount
          ? [
              {
                id: 'selection',
                // Make this column a groupByBoundary. This ensures that groupBy columns
                // are placed after it
                groupByBoundary: true,
                // The header can use the table's getToggleAllRowsSelectedProps method
                // to render a checkbox
                Header: ({ getToggleAllRowsSelectedProps }) =>
                  isSingleSelect !== true ? (
                    <div>
                      <IndeterminateCheckbox
                        {...getToggleAllRowsSelectedProps()}
                        isSingleSelect={isSingleSelect}
                        setSelected={setExternalSelectedRows}
                        selectedRef={selectedRowsRef}
                        allDataRef={dataRef}
                        disableSelection={disableSelection}
                      />
                    </div>
                  ) : null,
                // The cell can use the individual row's getToggleRowSelectedProps method
                // to the render a checkbox
                Cell: ({ row }: { row: any }) => (
                  <div>
                    <IndeterminateCheckbox
                      {...row.getToggleRowSelectedProps()}
                      row={row}
                      disableSelection={disableSelection}
                      setSelected={(values: T[]) => {
                        if (isSingleSelect === true) {
                          setExternalSelectedRows && setExternalSelectedRows([...values]);
                        } else {
                          const allPreviouslySelected = selectedRowsRef?.current ?? [];
                          const previouslySelected = allPreviouslySelected.find(
                            row => values.length === 1 && row.id === values[0].id,
                          );

                          if (previouslySelected) {
                            setExternalSelectedRows &&
                              setExternalSelectedRows(
                                allPreviouslySelected.filter(row => row !== previouslySelected),
                              );
                          } else {
                            setExternalSelectedRows &&
                              setExternalSelectedRows([...allPreviouslySelected, ...values]);
                          }
                        }
                      }}
                      selectedRef={selectedRowsRef}
                      isSingleSelect={isSingleSelect}
                    />
                  </div>
                ),
                maxWidth: 10,
                minWidth: 10,
              },
              ...columns,
            ]
          : columns;
      });
    },
  );

  // Use the state and functions returned from useTable to build your UI
  const {
    getTableProps,
    getTableBodyProps,
    toggleSortBy,
    headerGroups,
    footerGroups,
    prepareRow,
    page, // Instead of using 'rows', we'll use page,
    // which has only the rows for the active page

    // Get state from react-table
    state: { pageIndex, pageSize },
    toggleRowSelected,
    selectedFlatRows,
  } = instance;

  // Listen for changes in pagination and use the state to fetch our new data
  useEffect(() => {
    pageIndexProp !== undefined && pageIndexProp >= 0 && instance.gotoPage(pageIndexProp);
  }, [pageIndexProp, instance]);

  useEffect(() => {
    pageSizeProp && instance.setPageSize(pageSizeProp);
  }, [pageSizeProp, instance]);

  useEffect(() => {
    onRequestData?.({ pageIndex: pageIndex, pageSize });
  }, [onRequestData, pageIndex, pageSize]);

  useDeepCompareEffect(() => {
    page.forEach(r => {
      toggleRowSelected(r.id, !!externalSelectedRows?.find(s => s.id === r.original.id));
    });
  }, [page, externalSelectedRows]);

  const getNextSortDirection = (column: ColumnInstanceWithProps<T>): SortDirection => {
    if (!(props.externalSort?.sort as any)[column.id]) return 'asc';

    if ((props.externalSort?.sort as any)[column.id] === 'desc') {
      return undefined;
    }

    return 'desc';
  };

  const renderHeaderCell = (column: ColumnInstanceWithProps<T>) => {
    return (
      <div className="sortable-column">
        {column.render('Header')}
        <ColumnSort<T>
          onSort={() => {
            const next = getNextSortDirection(column);
            !!externalSort &&
              handleSortChange(column.id, next, externalSort.sort, externalSort.setSort);
            if (next) {
              toggleSortBy(column.id, next === 'desc', true);
            } else {
              column.clearSortBy();
            }
          }}
          column={column}
          sort={externalSort?.sort}
        />
      </div>
    );
  };

  const renderTableHeader = (headerGroup: HeaderGroup<T>, actions: any) => {
    return (
      <>
        {props.canRowExpand ? <div className="th" style={{ width: '4.7rem' }}></div> : null}
        {filterable && (
          <div className={'th reset-filter svg-btn'}>
            <TooltipWrapper tooltipId="properties-list-filter-reset-tooltip" tooltip="Reset Filter">
              <Button
                onClick={() => {
                  const nextState: any = { ...props.filter };
                  const fields = keys(props.filter || {});
                  for (const key of fields) {
                    if (Array.isArray(nextState[key])) {
                      nextState[key] = [];
                    } else {
                      nextState[key] = '';
                    }
                  }

                  actions.resetForm(nextState);
                  if (props.onFilterChange) {
                    props.onFilterChange(nextState);
                  }
                }}
                variant="secondary"
                style={{ width: 20, height: 20 }}
                icon={<FaUndo size={10} />}
              />
            </TooltipWrapper>
          </div>
        )}
        {headerGroup.headers.map((columnProps: ColumnInstanceWithProps<T>) => (
          <div
            {...(props.hideHeaders
              ? columnProps.getHeaderProps(noHeadersGetter)
              : columnProps.getHeaderProps(headerPropsGetter))}
            className={cx(
              'th',
              columnProps.isSorted ? (columnProps.isSortedDesc ? 'sort-desc' : 'sort-asc') : '',
            )}
            key={`th-header-${columnProps.id}`}
          >
            {renderHeaderCell(columnProps)}
          </div>
        ))}
      </>
    );
  };

  const onPageSizeChange = (size: number) => {
    props.onPageSizeChange && props.onPageSizeChange(size);
    if (!instance.manualPagination) {
      setInternalPageSize(size);
      instance.setPageSize(size);
    }
  };

  const renderLoading = () => {
    return (
      <div className="table-loading">
        <Spinner animation="border" role="status" title="table-loading"></Spinner>
      </div>
    );
  };

  const renderExpandRowStateButton = useDeepCompareCallback(
    (
      open?: boolean,
      className?: string,
      onClick?: (e: React.MouseEvent<HTMLDivElement, MouseEvent>) => void,
    ) => {
      const detailsClosedIcon =
        props.detailsPanel && props.detailsPanel.icons?.closed ? (
          props.detailsPanel.icons?.closed
        ) : (
          <FaAngleRight />
        );
      const detailsOpenedIcon =
        props.detailsPanel && props.detailsPanel.icons?.open ? (
          props.detailsPanel.icons?.open
        ) : (
          <FaAngleDown />
        );
      return (
        props.detailsPanel && (
          <TooltipWrapper
            tooltipId="expand-all-rows"
            tooltip={open ? 'Collapse Row' : 'Expand Row'}
          >
            <div className={className + ' svg-btn'} onClick={onClick}>
              {open ? detailsOpenedIcon : detailsClosedIcon}
            </div>
          </TooltipWrapper>
        )
      );
    },
    [props.detailsPanel],
  );

  const renderFooter = () => {
    if (!footer || !page?.length) {
      return null;
    }
    if (props.loading) {
      return renderLoading();
    }
    return (
      <div className="tfoot tfoot-light">
        {footerGroups.map(footerGroup => (
          <div
            {...footerGroup.getHeaderGroupProps()}
            className="tr"
            key={`tr-footer-${footerGroup.id}`}
          >
            {footerGroup.headers.map(
              (column: ColumnInstanceWithProps<T> & { Footer?: React.FC<{ properties: T[] }> }) => (
                <div
                  {...column.getHeaderProps(headerPropsGetter)}
                  className="th"
                  key={`th-footer-${footerGroup.id}`}
                >
                  {column.Footer ? <column.Footer properties={map(page, 'original')} /> : null}
                </div>
              ),
            )}
          </div>
        ))}
      </div>
    );
  };

  const renderBody = useDeepCompareMemo(() => {
    if (props.loading) {
      return renderLoading();
    }

    if (props.data.length === 0) {
      return <div className="no-rows-message">{props.noRowsMessage || 'No rows to display'}</div>;
    }

    const handleExpandClick = (e: React.MouseEvent<HTMLDivElement, MouseEvent>, rowData: T) => {
      e.preventDefault();
      let expanded;
      if (
        props.detailsPanel !== undefined &&
        props.detailsPanel.checkExpanded(rowData, expandedRows)
      ) {
        expanded = expandedRows.filter(
          x => props.detailsPanel?.getRowId(x) !== props.detailsPanel?.getRowId(rowData),
        );
      } else {
        expanded = [...expandedRows, rowData];
      }
      setExpandedRows(expanded);
      if (props.detailsPanel && props.detailsPanel.onExpand && expanded.length > 0) {
        props.detailsPanel.onExpand(expanded);
      }
    };

    const renderRow = (row: Row<T>, index: number) => {
      return (
        <div key={index} className="tr-wrapper">
          <div {...row.getRowProps()} className={cx('tr', row.isSelected ? 'selected' : '')}>
            {/* If canRowExpand prop is passed only allow expansions on those rows */}
            {props.canRowExpand &&
              props.canRowExpand(row) &&
              renderExpandRowStateButton(
                props.detailsPanel && props.detailsPanel.checkExpanded(row.original, expandedRows),
                'td expander',
                e => handleExpandClick(e, row.original),
              )}
            {props.canRowExpand && !props.canRowExpand(row) ? (
              <div className="td">
                <div style={{ width: '2.0rem' }}>&nbsp;</div>
              </div>
            ) : null}
            {filterable ? (
              <div className="td">
                <div style={{ width: '3.0rem' }}>&nbsp;</div>
              </div>
            ) : null}
            {/* Expansion button shown on every row by default */}
            {!props.canRowExpand &&
              renderExpandRowStateButton(
                props.detailsPanel && props.detailsPanel.checkExpanded(row.original, expandedRows),
                'td expander',
                e => handleExpandClick(e, row.original),
              )}
            {row.cells.map((cell: CellWithProps<T>) => {
              return (
                <div
                  {...cell.getCellProps(cellPropsGetter)}
                  title={cell.column.clickable && clickableTooltip ? clickableTooltip : ''}
                  className={cx(
                    'td',
                    cell.column.clickable ? 'clickable' : '',
                    cell.column.className,
                  )}
                  onClick={() =>
                    props.onRowClick && cell.column.clickable && props.onRowClick(row.original)
                  }
                  key={`td-cell-${cell.column.id}`}
                >
                  {cell.render('Cell')}
                </div>
              );
            })}
          </div>
          {props.detailsPanel && (
            <Collapse in={props.detailsPanel.checkExpanded(row.original, expandedRows)}>
              <div style={{ padding: 10 }}>{props.detailsPanel.render(row.original)}</div>
            </Collapse>
          )}
        </div>
      );
    };

    const body = (
      <div {...getTableBodyProps()} className="tbody">
        {page.map((row: Row<T>, index: number) => {
          // This line is necessary to prepare the rows and get the row props from `react-table` dynamically
          prepareRow(row);
          // Each row can be rendered directly as a string using `react-table` render method
          return renderRow(row, index);
        })}
      </div>
    );

    return renderBodyComponent ? renderBodyComponent({ body }) : body;
  }, [
    props.loading,
    props.detailsPanel,
    props.data,
    props.canRowExpand,
    props.onRowClick,
    props.noRowsMessage,
    props.columns,
    renderExpandRowStateButton,
    cellPropsGetter,
    expandedRows,
    clickableTooltip,
    externalSelectedRows,
    selectedFlatRows,
    page,
    pageIndex,
  ]);

  let canShowTotals = false;
  let initialCount = -1;
  let finalCount = -1;
  if (totalItems !== undefined && pageSize !== undefined && pageIndex !== undefined) {
    canShowTotals = true;
    initialCount = pageSize * pageIndex + 1;
    finalCount = Math.min(pageSize * (pageIndex + 1), totalItems);
  }

  // Render the UI for your table
  return (
    <>
      <div
        {...getTableProps({ style: { minWidth: undefined } })}
        className={cx('table', props.className ?? '')}
        data-testid={`${props.name}`}
      >
        <div className="thead thead-light">
          {headerGroups.map(headerGroup => (
            <div
              {...headerGroup.getHeaderGroupProps()}
              className="tr"
              key={`tr-head-${headerGroup.id}`}
            >
              {filterable ? (
                <Formik
                  initialValues={props.filter || {}}
                  onSubmit={values => {
                    if (props.onFilterChange) {
                      props.onFilterChange(values);
                    }
                  }}
                  innerRef={filterFormRef as any}
                >
                  {actions => (
                    <Form style={{ display: 'flex', width: '100%' }}>
                      {renderTableHeader(headerGroup, actions)}
                    </Form>
                  )}
                </Formik>
              ) : (
                renderTableHeader(headerGroup, null)
              )}
            </div>
          ))}
        </div>
        {renderBody}
        {renderFooter()}
      </div>

      {!props.hideToolbar && (
        <RowBootstrap>
          <ColBootstrap xs="auto" className="align-self-center">
            {canShowTotals && props.data.length > 0 && (
              <span>{`${initialCount} - ${finalCount} of ${totalItems}`}</span>
            )}
          </ColBootstrap>
          <ColBootstrap xs="auto" className="ml-auto align-self-center">
            {!!props.showSelectedRowCount && (
              <SelectedText>{props.selectedRows?.length ?? '0'} selected</SelectedText>
            )}
            {props.tableToolbarText && <span>{props.tableToolbarText}</span>}
          </ColBootstrap>

          {!props.lockPageSize && props.data.length > 0 && (
            <ColBootstrap xs="auto" className="align-self-center">
              <TablePageSizeSelector
                options={props.pageSizeOptions || DEFAULT_PAGE_SELECTOR_OPTIONS}
                value={props.pageSize || DEFAULT_PAGE_SIZE}
                onChange={onPageSizeChange}
                alignTop={
                  props.pageSizeMenuDropUp ? props.pageSizeMenuDropUp : props.data.length >= 20
                }
              />
            </ColBootstrap>
          )}
          <ColBootstrap xs="auto" className="align-self-center">
            {!props.hidePagination && <TablePagination<T> instance={instance} />}
          </ColBootstrap>
        </RowBootstrap>
      )}
    </>
  );
};

export default Table;
