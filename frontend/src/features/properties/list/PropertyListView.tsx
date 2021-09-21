import './PropertyListView.scss';

import { SearchToggleOption } from 'components/common/form';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { Table } from 'components/Table';
import { SortDirection, TableSort } from 'components/Table/TableSort';
import * as API from 'constants/API';
import { Form, Formik, FormikProps } from 'formik';
import { useApiProperties } from 'hooks/pims-api';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { useRouterFilter } from 'hooks/useRouterFilter';
import { IProperty } from 'interfaces';
import isEmpty from 'lodash/isEmpty';
import noop from 'lodash/noop';
import React, { useCallback, useMemo, useRef, useState } from 'react';
import Container from 'react-bootstrap/Container';
import { FaFileAlt, FaFileExcel } from 'react-icons/fa';
import { useProperties } from 'store/slices/properties';
import { generateMultiSortCriteria } from 'utils';
import { toFilteredApiPaginateParams } from 'utils/CommonFunctions';

import { PropertyFilter } from '../filter';
import { IPropertyFilter } from '../filter/IPropertyFilter';
import { columns as columnDefinitions } from './columns';
import * as Styled from './PropertyListView.styled';

const defaultFilterValues: IPropertyFilter = {
  searchBy: 'pid',
  pid: '',
  pin: '',
  address: '',
  location: '',
};

const PropertyListView: React.FC = () => {
  const { getByType } = useLookupCodeHelpers();
  const tableFormRef = useRef<FormikProps<{ properties: IProperty[] }> | undefined>();

  const municipalities = useMemo(() => getByType(API.ADMINISTRATIVE_AREA_CODE_SET_NAME), [
    getByType,
  ]);

  const columns = useMemo(() => columnDefinitions({ municipalities }), [municipalities]);

  // We'll start our table without any data
  const [data, setData] = useState<IProperty[] | undefined>();

  // Filtering and pagination state
  const [filter, setFilter] = useState<IPropertyFilter>(defaultFilterValues);

  const [pageSize, setPageSize] = useState(10);
  const [pageIndex, setPageIndex] = useState(0);
  const [pageCount, setPageCount] = useState(0);
  const [sort, setSort] = useState<TableSort<IProperty>>({});

  const fetchIdRef = useRef(0);

  const parsedFilter = useMemo(() => {
    const data = { ...filter };
    return data;
  }, [filter]);

  const { updateSearch } = useRouterFilter({
    filter: parsedFilter,
    setFilter: setFilter,
    key: 'propertyFilter',
  });

  // Update internal state whenever the filter bar state changes
  const handleFilterChange = useCallback(
    (value: IPropertyFilter) => {
      setFilter({ ...value });
      updateSearch({ ...value });
      setPageIndex(0); // Go to first page of results when filter changes
    },
    [setFilter, setPageIndex, updateSearch],
  );
  // This will get called when the table needs new data
  const onRequestData = useCallback(
    ({ pageIndex }: { pageIndex: number }) => {
      setPageIndex(pageIndex);
    },
    [setPageIndex],
  );

  const { getPropertiesPaged } = useApiProperties();

  const fetchData = useCallback(
    async ({
      pageIndex,
      pageSize,
      filter,
      sort,
    }: {
      pageIndex: number;
      pageSize: number;
      filter: IPropertyFilter;
      sort: TableSort<IProperty>;
    }) => {
      // Give this fetch an ID
      const fetchId = ++fetchIdRef.current;
      setData(undefined);
      // Call API with appropriate search parameters
      const queryParams = toFilteredApiPaginateParams<IPropertyFilter>(
        pageIndex,
        pageSize,
        sort && !isEmpty(sort) ? generateMultiSortCriteria(sort) : undefined,
        filter,
      );
      const { data } = await getPropertiesPaged(queryParams);

      // The server could send back total page count.
      // For now we'll just calculate it.
      if (fetchId === fetchIdRef.current && data?.items) {
        setData(data.items);
        setPageCount(Math.ceil(data.total / pageSize));
      }
    },
    [setData, setPageCount, getPropertiesPaged],
  );

  // Listen for changes in pagination and use the state to fetch our new data
  useDeepCompareEffect(() => {
    fetchData({ pageIndex, pageSize, filter, sort });
  }, [fetchData, pageIndex, pageSize, filter, sort]);

  const { exportProperties } = useProperties();

  /**
   * @param {'csv' | 'excel'} accept Whether the fetch is for type of CSV or EXCEL
   * @param {boolean} getAllFields Enable this field to generate report with additional fields. For SRES only.
   */
  const fetch = (accept: 'csv' | 'excel') => {
    // Call API with appropriate search parameters
    const query = toFilteredApiPaginateParams<IPropertyFilter>(
      pageIndex,
      pageSize,
      sort && !isEmpty(sort) ? generateMultiSortCriteria(sort) : undefined,
      filter,
    );

    exportProperties(query, accept);
  };

  const appliedFilter = { ...filter };

  return (
    <Container fluid className="PropertyListView">
      <Container fluid className="filter-container border-bottom">
        <Container fluid className="px-0 map-filter-container">
          <PropertyFilter
            defaultFilter={defaultFilterValues}
            onChange={handleFilterChange}
            sort={sort}
            onSorting={setSort}
            toggle={SearchToggleOption.List}
          />
        </Container>
      </Container>
      <div className="ScrollContainer">
        <Container fluid className="TableToolbar">
          <h3>Property Information</h3>
          <div className="menu"></div>
          <TooltipWrapper toolTipId="export-to-excel" toolTip="Export to Excel">
            <Styled.FileIcon>
              <FaFileExcel data-testid="excel-icon" size={36} onClick={() => fetch('excel')} />
            </Styled.FileIcon>
          </TooltipWrapper>
          <TooltipWrapper toolTipId="export-to-excel" toolTip="Export to CSV">
            <Styled.FileIcon>
              <FaFileAlt data-testid="csv-icon" size={36} onClick={() => fetch('csv')} />
            </Styled.FileIcon>
          </TooltipWrapper>
        </Container>

        <Table<IProperty>
          name="propertiesTable"
          columns={columns}
          data={data || []}
          loading={data === undefined}
          sort={sort}
          pageIndex={pageIndex}
          onRequestData={onRequestData}
          pageCount={pageCount}
          onSortChange={(column: string, direction: SortDirection) => {
            if (!!direction) {
              setSort({ ...sort, [column]: direction });
            } else {
              const data: any = { ...sort };
              delete data[column];
              setSort(data);
            }
          }}
          filter={appliedFilter}
          onFilterChange={values => {
            setFilter({ ...filter, ...values });
          }}
          onPageSizeChange={newSize => setPageSize(newSize)}
          renderBodyComponent={({ body }) => (
            <Formik
              innerRef={tableFormRef as any}
              initialValues={{ properties: data || [] }}
              onSubmit={noop}
            >
              <Form>{body}</Form>
            </Formik>
          )}
        />
      </div>
    </Container>
  );
};

export default PropertyListView;
