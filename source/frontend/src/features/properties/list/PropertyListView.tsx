import './PropertyListView.scss';

import { Form, Formik, FormikProps } from 'formik';
import isEmpty from 'lodash/isEmpty';
import noop from 'lodash/noop';
import Multiselect from 'multiselect-react-dropdown';
import React, { useCallback, useMemo, useRef, useState } from 'react';
import { Row } from 'react-bootstrap';
import { Col } from 'react-bootstrap';
import Container from 'react-bootstrap/Container';
import { FaFileAlt, FaFileExcel, FaTimes } from 'react-icons/fa';
import styled from 'styled-components';

import { StyledIconButton } from '@/components/common/buttons';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { Table } from '@/components/Table';
import { TableSort } from '@/components/Table/TableSort';
import * as API from '@/constants/API';
import { MultiSelectOption } from '@/features/acquisition/list/interfaces';
import { useApiProperties } from '@/hooks/pims-api/useApiProperties';
import { useProperties } from '@/hooks/repositories/useProperties';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { ApiGen_Concepts_PropertyView } from '@/models/api/generated/ApiGen_Concepts_PropertyView';
import { generateMultiSortCriteria } from '@/utils';
import { toFilteredApiPaginateParams } from '@/utils/CommonFunctions';

import { PropertyFilter } from '../filter';
import { defaultPropertyFilter, IPropertyFilter } from '../filter/IPropertyFilter';
import { SearchToggleOption } from '../filter/PropertySearchToggle';
import { columns as columnDefinitions } from './columns';

export const ownershipFilterOptions: MultiSelectOption[] = [
  { id: 'isCoreInventory', text: 'Core Inventory' },
  { id: 'isPropertyOfInterest', text: 'Property Of Interest' },
  { id: 'isOtherInterest', text: 'Other Interest' },
  { id: 'isDisposed', text: 'Disposed' },
  { id: 'isRetired', text: 'Retired' },
];

const PropertyListView: React.FC<React.PropsWithChildren<unknown>> = () => {
  const { getByType } = useLookupCodeHelpers();
  const tableFormRef = useRef<
    FormikProps<{ properties: ApiGen_Concepts_PropertyView[] }> | undefined
  >();

  const municipalities = useMemo(() => getByType(API.ADMINISTRATIVE_AREA_TYPES), [getByType]);

  const columns = useMemo(() => columnDefinitions({ municipalities }), [municipalities]);

  // We'll start our table without any data
  const [data, setData] = useState<ApiGen_Concepts_PropertyView[] | undefined>();

  // Filtering and pagination state
  const [filter, setFilter] = useState<IPropertyFilter>(defaultPropertyFilter);

  const [pageSize, setPageSize] = useState(10);
  const [pageIndex, setPageIndex] = useState(0);
  const [pageCount, setPageCount] = useState(0);
  const [totalItems, setTotalItems] = useState(0);
  const [sort, setSort] = useState<TableSort<ApiGen_Concepts_PropertyView>>({});

  const fetchIdRef = useRef(0);

  // Update internal state whenever the filter bar state changes
  const handleFilterChange = useCallback(
    (value: IPropertyFilter) => {
      setFilter({ ...value });
      setPageIndex(0); // Go to first page of results when filter changes
    },
    [setFilter, setPageIndex],
  );
  // This will get called when the table needs new data
  const onRequestData = useCallback(
    ({ pageIndex }: { pageIndex: number }) => {
      setPageIndex(pageIndex);
    },
    [setPageIndex],
  );

  const { getPropertiesViewPagedApi } = useApiProperties();

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
      sort: TableSort<ApiGen_Concepts_PropertyView>;
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
      const { data } = await getPropertiesViewPagedApi(queryParams);

      setTotalItems(data.total);

      // The server could send back total page count.
      // For now we'll just calculate it.
      if (fetchId === fetchIdRef.current && data?.items) {
        setData(data.items);
        setPageCount(Math.ceil(data.total / pageSize));
      }
    },
    [setData, setPageCount, getPropertiesViewPagedApi],
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

  const multiselectOwnershipRef = React.createRef<Multiselect>();

  const onSelectedOwnershipChange = (selectedList: MultiSelectOption[]) => {
    setPageIndex(0);
    const selectedIds = selectedList.map(o => o.id);
    setFilter({ ...filter, ownership: selectedIds.join(',') });
  };

  return (
    <Container fluid className="PropertyListView">
      <Container fluid className="filter-container border-bottom">
        <StyledFilterContainer fluid className="px-0">
          <PropertyFilter
            useGeocoder={false}
            defaultFilter={defaultPropertyFilter}
            onChange={handleFilterChange}
            sort={sort}
            onSorting={setSort}
            toggle={SearchToggleOption.List}
          />
        </StyledFilterContainer>
      </Container>
      <div className="mt-5 mx-5">
        <StyledPageHeader>Search Results</StyledPageHeader>
        <Row>
          <Col xs="10">
            <StyledFilterBox className="p-3">
              <Row>
                <Col xl="1">
                  <strong>View by:</strong>
                </Col>
                <Col xs="auto">
                  <Multiselect
                    id="properties-selector"
                    ref={multiselectOwnershipRef}
                    options={ownershipFilterOptions}
                    selectedValues={filter.ownership
                      .split(',')
                      .map<MultiSelectOption | undefined>(o =>
                        ownershipFilterOptions.find(op => op.id === o),
                      )
                      .filter((x): x is MultiSelectOption => x !== undefined)}
                    onSelect={onSelectedOwnershipChange}
                    onRemove={onSelectedOwnershipChange}
                    displayValue="text"
                    placeholder="Select ownership status"
                    customCloseIcon={<FaTimes size="18px" className="ml-3" />}
                    hidePlaceholder={true}
                    style={defaultStyle}
                  />
                </Col>
              </Row>
            </StyledFilterBox>
          </Col>
          <Col>
            <Container fluid className="TableToolbar px-0">
              <div className="menu"></div>
              <TooltipWrapper tooltipId="export-to-excel" tooltip="Export to Excel">
                <StyledIconButton onClick={() => fetch('excel')}>
                  <FaFileExcel data-testid="excel-icon" size={36} />
                </StyledIconButton>
              </TooltipWrapper>
              <TooltipWrapper tooltipId="export-to-excel" tooltip="Export to CSV">
                <StyledIconButton onClick={() => fetch('csv')}>
                  <FaFileAlt data-testid="csv-icon" size={36} />
                </StyledIconButton>
              </TooltipWrapper>
            </Container>
          </Col>
        </Row>
      </div>
      <div className="ScrollContainer">
        <Table<ApiGen_Concepts_PropertyView>
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

export const StyledPageHeader = styled.h3`
  text-align: left;
`;

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
export const StyledFilterBox = styled.div`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
`;

const defaultStyle = {
  chips: {
    background: '#F2F2F2',
    borderRadius: '4px',
    color: 'black',
    fontSize: '16px',
    marginRight: '1em',
    whiteSpace: 'normal',
  },
  multiselectContainer: {
    width: 'auto',
    color: 'black',
  },
  searchBox: {
    background: 'white',
    border: '1px solid #606060',
  },
};
