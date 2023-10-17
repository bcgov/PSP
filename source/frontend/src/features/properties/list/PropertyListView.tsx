import './PropertyListView.scss';

import { Form, Formik, FormikProps } from 'formik';
import isEmpty from 'lodash/isEmpty';
import noop from 'lodash/noop';
import React, { useCallback, useMemo, useRef, useState } from 'react';
import Container from 'react-bootstrap/Container';
import { FaFileAlt, FaFileExcel } from 'react-icons/fa';
import styled from 'styled-components';

import { StyledIconButton } from '@/components/common/buttons';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import * as API from '@/constants/API';
import { useApiProperties } from '@/hooks/pims-api/useApiProperties';
import { useProperties } from '@/hooks/repositories/useProperties';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useRouterFilter } from '@/hooks/useRouterFilter';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { IProperty } from '@/interfaces';
import { generateMultiSortCriteria } from '@/utils';
import { toFilteredApiPaginateParams } from '@/utils/CommonFunctions';

import { PropertyFilter } from '../filter';
import { IPropertyFilter } from '../filter/IPropertyFilter';
import { SearchToggleOption } from '../filter/PropertySearchToggle';
import { columns as columnDefinitions } from './columns';

const defaultFilterValues: IPropertyFilter = {
  searchBy: 'pinOrPid',
  pinOrPid: '',
  address: '',
  page: '1',
  quantity: '10',
  latitude: undefined,
  longitude: undefined,
};

const PropertyListView: React.FC<React.PropsWithChildren<unknown>> = () => {
  const { getByType } = useLookupCodeHelpers();
  const tableFormRef = useRef<FormikProps<{ properties: IProperty[] }> | undefined>();

  const municipalities = useMemo(() => getByType(API.ADMINISTRATIVE_AREA_TYPES), [getByType]);

  const columns = useMemo(() => columnDefinitions({ municipalities }), [municipalities]);

  // We'll start our table without any data
  const [data, setData] = useState<IProperty[] | undefined>();

  // Filtering and pagination state
  const [filter, setFilter] = useState<IPropertyFilter>(defaultFilterValues);

  const [pageSize, setPageSize] = useState(10);
  const [pageIndex, setPageIndex] = useState(0);
  const [pageCount, setPageCount] = useState(0);
  const [totalItems, setTotalItems] = useState(0);
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

  const { getPropertiesPagedApi } = useApiProperties();

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
      const { data } = await getPropertiesPagedApi(queryParams);

      setTotalItems(data.total);

      // The server could send back total page count.
      // For now we'll just calculate it.
      if (fetchId === fetchIdRef.current && data?.items) {
        setData(data.items);
        setPageCount(Math.ceil(data.total / pageSize));
      }
    },
    [setData, setPageCount, getPropertiesPagedApi],
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
        <StyledFilterContainer fluid className="px-0">
          <PropertyFilter
            useGeocoder={false}
            defaultFilter={defaultFilterValues}
            onChange={handleFilterChange}
            sort={sort}
            onSorting={setSort}
            toggle={SearchToggleOption.List}
          />
        </StyledFilterContainer>
      </Container>
      <div className="ScrollContainer">
        <Container fluid className="TableToolbar px-0">
          <h3>Property Information</h3>
          <div className="menu"></div>
          <TooltipWrapper toolTipId="export-to-excel" toolTip="Export to Excel">
            <StyledIconButton onClick={() => fetch('excel')}>
              <FaFileExcel data-testid="excel-icon" size={36} />
            </StyledIconButton>
          </TooltipWrapper>
          <TooltipWrapper toolTipId="export-to-excel" toolTip="Export to CSV">
            <StyledIconButton onClick={() => fetch('csv')}>
              <FaFileAlt data-testid="csv-icon" size={36} />
            </StyledIconButton>
          </TooltipWrapper>
        </Container>

        <Table<IProperty>
          name="propertiesTable"
          columns={columns}
          data={data || []}
          loading={data === undefined}
          externalSort={{ sort: sort, setSort: setSort }}
          totalItems={totalItems}
          pageIndex={pageIndex}
          pageSize={pageSize}
          onRequestData={onRequestData}
          pageCount={pageCount}
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

const StyledFilterContainer = styled(Container)`
  transition: margin 1s;

  grid-area: filter;
  background-color: $filter-background-color;
  box-shadow: 0px 4px 5px rgba(0, 0, 0, 0.2);
  z-index: 500;
  .map-filter-bar {
    align-items: center;
    justify-content: center;
    padding: 0.5rem 0;
    .vl {
      border-left: 6px solid rgba(96, 96, 96, 0.2);
      height: 4rem;
      margin-left: 1%;
      margin-right: 1%;
      border-width: 0.2rem;
    }
    .btn-primary {
      color: white;
      font-weight: bold;
      height: 3.5rem;
      width: 3.5rem;
      min-height: unset;
      padding: 0;
    }
    .form-control {
      font-size: 1.4rem;
    }
  }
  .form-group {
    margin-bottom: 0;
  }
`;
