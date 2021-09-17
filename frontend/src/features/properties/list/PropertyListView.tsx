import './PropertyListView.scss';

import { SearchToggleOption } from 'components/common/form';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { Table } from 'components/Table';
import { SortDirection, TableSort } from 'components/Table/TableSort';
import * as API from 'constants/API';
import { PropertyTypes, Roles } from 'constants/index';
import { Form, Formik, FormikProps } from 'formik';
import { useApiProperties } from 'hooks/pims-api';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { useRouterFilter } from 'hooks/useRouterFilter';
import { IProperty } from 'interfaces';
import isEmpty from 'lodash/isEmpty';
import noop from 'lodash/noop';
import queryString from 'query-string';
import React, { useCallback, useMemo, useRef, useState } from 'react';
import Container from 'react-bootstrap/Container';
import { FaFileAlt, FaFileExcel, FaFileExport } from 'react-icons/fa';
import { useDispatch } from 'react-redux';
import { generateMultiSortCriteria } from 'utils';
import { toFilteredApiPaginateParams } from 'utils/CommonFunctions';
import download from 'utils/download';

import { PropertyFilter } from '../filter';
import { IPropertyFilter } from '../filter/IPropertyFilter';
import { columns as columnDefinitions } from './columns';
import * as Styled from './PropertyListView.styled';
import { defaultFilterValues, getAllFieldsPropertyReportUrl, getPropertyReportUrl } from './utils';

const PropertyListView: React.FC = () => {
  const lookupCodes = useLookupCodeHelpers();
  const tableFormRef = useRef<FormikProps<{ properties: IProperty[] }> | undefined>();
  const keycloak = useKeycloakWrapper();

  const municipalities = useMemo(
    () => lookupCodes.getByType(API.ADMINISTRATIVE_AREA_CODE_SET_NAME),
    [lookupCodes],
  );

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

  const dispatch = useDispatch();

  /**
   * @param {'csv' | 'excel'} accept Whether the fetch is for type of CSV or EXCEL
   * @param {boolean} getAllFields Enable this field to generate report with additional fields. For SRES only.
   */
  const fetch = (accept: 'csv' | 'excel', getAllFields?: boolean) => {
    // Call API with appropriate search parameters
    const query = toFilteredApiPaginateParams<IPropertyFilter>(
      pageIndex,
      pageSize,
      sort && !isEmpty(sort) ? generateMultiSortCriteria(sort) : undefined,
      filter,
    );
    return dispatch(
      download({
        url: getAllFields ? getAllFieldsPropertyReportUrl(query) : getPropertyReportUrl(query),
        fileName: `pims-inventory.${accept === 'csv' ? 'csv' : 'xlsx'}`,
        actionType: 'properties-report',
        headers: {
          Accept: accept === 'csv' ? 'text/csv' : 'application/vnd.ms-excel',
        },
      }),
    );
  };

  const appliedFilter = { ...filter };

  const onRowClick = useCallback((row: IProperty) => {
    window.open(
      `/mapview?${queryString.stringify({
        sidebar: true,
        disabled: true,
        loadDraft: false,
        parcelId: [PropertyTypes.Land, PropertyTypes.Subdivision].includes(row.propertyTypeId)
          ? row.id
          : undefined,
        buildingId: row.propertyTypeId === PropertyTypes.Building ? row.id : undefined,
      })}`,
      '_blank',
    );
  }, []);

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
          {(keycloak.hasRole(Roles.SRES_FINANCIAL_MANAGER) || keycloak.hasRole(Roles.SRES)) && (
            <TooltipWrapper toolTipId="export-to-excel" toolTip="Export all properties and fields">
              <Styled.FileIcon>
                <FaFileExport
                  data-testid="file-icon"
                  size={36}
                  onClick={() => fetch('excel', true)}
                />
              </Styled.FileIcon>
            </TooltipWrapper>
          )}
        </Container>

        <Table<IProperty>
          name="propertiesTable"
          columns={columns}
          data={data || []}
          loading={data === undefined}
          sort={sort}
          pageIndex={pageIndex}
          onRequestData={onRequestData}
          onRowClick={onRowClick}
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
