import './PropertyListView.scss';

import variables from '_variables.module.scss';
import { ReactComponent as BuildingSvg } from 'assets/images/icon-business.svg';
import { ReactComponent as LandSvg } from 'assets/images/icon-lot.svg';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { Table } from 'components/Table';
import { SortDirection, TableSort } from 'components/Table/TableSort';
import * as API from 'constants/API';
import { ENVIRONMENT } from 'constants/environment';
import { EvaluationKeys } from 'constants/evaluationKeys';
import { FiscalKeys } from 'constants/fiscalKeys';
import { PropertyTypes } from 'constants/index';
import { Roles } from 'constants/roles';
import { Form, Formik, FormikProps, getIn, useFormikContext } from 'formik';
import { useApi } from 'hooks/useApi';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { useRouterFilter } from 'hooks/useRouterFilter';
import { IApiProperty } from 'interfaces';
import fill from 'lodash/fill';
import intersection from 'lodash/intersection';
import isEmpty from 'lodash/isEmpty';
import keys from 'lodash/keys';
import noop from 'lodash/noop';
import pick from 'lodash/pick';
import range from 'lodash/range';
import queryString from 'query-string';
import React, { useCallback, useMemo, useRef, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Container from 'react-bootstrap/Container';
import { FaEdit, FaFileExport, FaFolder, FaFolderOpen } from 'react-icons/fa';
import { FaFileAlt, FaFileExcel } from 'react-icons/fa';
import { useDispatch } from 'react-redux';
import { toast } from 'react-toastify';
import styled from 'styled-components';
import {
  decimalOrUndefined,
  getCurrentFiscal,
  getCurrentYearEvaluation,
  mapLookupCode,
  toApiProperty,
} from 'utils';
import download from 'utils/download';
import * as Yup from 'yup';

import { PropertyTypeNames } from '../../../constants/propertyTypeNames';
import { PropertyFilter } from '../filter';
import { IPropertyFilter } from '../filter/IPropertyFilter';
import service from '../service';
import { IProperty, IPropertyQueryParams } from '.';
import { Buildings } from './buildings';
import { buildingColumns as buildingCols, columns as cols } from './columns';

const getPropertyReportUrl = (filter: IPropertyQueryParams) =>
  `${ENVIRONMENT.apiUrl}/reports/properties?${filter ? queryString.stringify(filter) : ''}`;

const getAllFieldsPropertyReportUrl = (filter: IPropertyQueryParams) =>
  `${ENVIRONMENT.apiUrl}/reports/properties/all/fields?${
    filter ? queryString.stringify(filter) : ''
  }`;

const FileIcon = styled(Button)`
  background-color: #fff !important;
  color: ${variables.primaryColor} !important;
  padding: 6px 5px;
`;

const EditIconButton = styled(FileIcon)`
  margin-right: 12px;
`;

const VerticalDivider = styled.div`
  border-left: 6px solid rgba(96, 96, 96, 0.2);
  height: 40px;
  margin-left: 1%;
  margin-right: 1%;
  border-width: 2px;
`;

const initialQuery: IPropertyQueryParams = {
  page: 1,
  quantity: 10,
  organizations: [],
};

const defaultFilterValues: IPropertyFilter = {
  searchBy: 'address',
  pid: '',
  address: '',
  administrativeArea: '',
  name: '',
  organizations: '',
  classificationId: '',
  minLotSize: '',
  maxLotSize: '',
  rentableArea: '',
  propertyType: PropertyTypeNames.Land,
  maxAssessedValue: '',
  maxNetBookValue: '',
  maxMarketValue: '',
};

export const flattenProperty = (apiProperty: IApiProperty): IProperty => {
  const assessedLand = getCurrentYearEvaluation(apiProperty.evaluations, EvaluationKeys.Assessed);
  const assessedBuilding = apiProperty.parcelId
    ? getCurrentYearEvaluation(apiProperty.evaluations, EvaluationKeys.Improvements)
    : getCurrentYearEvaluation(apiProperty.evaluations, EvaluationKeys.Assessed);
  const netBook = getCurrentFiscal(apiProperty.fiscals, FiscalKeys.NetBook);
  const market = getCurrentFiscal(apiProperty.fiscals, FiscalKeys.Market);
  const property: any = {
    id: apiProperty.id,
    parcelId: apiProperty.parcelId ?? apiProperty.id,
    pid: apiProperty.pid ?? '',
    name: apiProperty.name,
    description: apiProperty.description,
    landLegalDescription: apiProperty.landLegalDescription,
    zoning: apiProperty.zoning,
    zoningPotential: apiProperty.zoningPotential,
    isSensitive: apiProperty.isSensitive,
    latitude: apiProperty.latitude,
    longitude: apiProperty.longitude,
    organizationId: apiProperty.organizationId,
    organization: apiProperty.organization ?? '',
    organizationCode: apiProperty.organization ?? '',
    subOrganization: apiProperty.subOrganization,
    classification: apiProperty.classification ?? '',
    classificationId: apiProperty.classificationId,
    addressId: apiProperty.address?.id as number,
    address: `${apiProperty.address?.line1 ?? ''} , ${apiProperty.address?.administrativeArea ??
      ''}`,
    administrativeArea: apiProperty.address?.administrativeArea ?? '',
    province: apiProperty.address?.province ?? '',
    postal: apiProperty.address?.postal ?? '',
    assessedLand: (assessedLand?.value as number) ?? 0,
    assessedLandDate: assessedLand?.date,
    assessedBuilding: (assessedBuilding?.value as number) ?? 0,
    assessedBuildingDate: assessedBuilding?.date,
    assessedFirm: assessedLand?.firm,
    assessedRowVersion: assessedLand?.rowVersion,
    netBook: (netBook?.value as number) ?? 0,
    netBookFiscalYear: netBook?.fiscalYear as number,
    netBookRowVersion: netBook?.rowVersion,
    market: (market?.value as number) ?? 0,
    marketFiscalYear: market?.fiscalYear as number,
    marketRowVersion: market?.rowVersion,
    rowVersion: apiProperty.rowVersion,
    landArea: apiProperty.landArea,
  };
  return property;
};

/**
 * Get the server query
 * @param state - Table state
 */
const getServerQuery = (state: {
  pageIndex: number;
  pageSize: number;
  filter: IPropertyFilter;
  organizationIds: number[];
}) => {
  const {
    pageIndex,
    pageSize,
    filter: {
      pid,
      address,
      administrativeArea,
      classificationId,
      bareLandOnly,
      name,
      organizations,
      minLotSize,
      maxLotSize,
      propertyType,
      maxAssessedValue,
      maxMarketValue,
      maxNetBookValue,
    },
  } = state;

  let parsedOrganizations: number[] = [];
  if (organizations !== null && organizations !== undefined && organizations !== '') {
    parsedOrganizations = Array.isArray(organizations)
      ? (organizations as any)
          .filter((x: any) => !!x)
          .map((a: any) => {
            return parseInt(typeof a === 'string' ? a : a.value, 10);
          })
      : [parseInt(organizations, 10)];
  }

  const query: IPropertyQueryParams = {
    ...initialQuery,
    address,
    pid,
    administrativeArea,
    classificationId: decimalOrUndefined(classificationId),
    organizations: parsedOrganizations,
    minLandArea: decimalOrUndefined(minLotSize),
    maxLandArea: decimalOrUndefined(maxLotSize),
    bareLandOnly: bareLandOnly,
    page: pageIndex + 1,
    quantity: pageSize,
    propertyType: propertyType,
    name: name,
    maxAssessedValue,
    maxMarketValue,
    maxNetBookValue,
  };
  return query;
};

interface IChangedRow {
  rowId: number;
  assessedLand?: boolean;
  assessedBuilding?: boolean;
  netBook?: boolean;
  market?: boolean;
}

/**
 *  Component to track edited rows in the formik table
 * @param param0 {setDirtyRows: event listener for changed rows}
 */
const DirtyRowsTracker: React.FC<{ setDirtyRows: (changes: IChangedRow[]) => void }> = ({
  setDirtyRows,
}) => {
  const { touched, isSubmitting } = useFormikContext();

  React.useEffect(() => {
    if (!!touched && !isEmpty(touched) && !isSubmitting) {
      const changed = Object.keys(getIn(touched, 'properties')).map(key => ({
        rowId: Number(key),
        ...getIn(touched, 'properties')[key],
      }));
      setDirtyRows(changed);
    }
  }, [touched, setDirtyRows, isSubmitting]);

  return null;
};

const PropertyListView: React.FC = () => {
  const lookupCodes = useLookupCodeHelpers();
  const { updateBuilding, updateParcel } = useApi();
  const [editable, setEditable] = useState(false);
  const tableFormRef = useRef<FormikProps<{ properties: IProperty[] }> | undefined>();
  /** maintains state of table's propertyType when user switches via tabs  */
  const [propertyType, setPropertyType] = useState<PropertyTypeNames>(PropertyTypeNames.Land);
  const [dirtyRows, setDirtyRows] = useState<IChangedRow[]>([]);
  const keycloak = useKeycloakWrapper();
  const municipalities = useMemo(
    () => lookupCodes.getByType(API.ADMINISTRATIVE_AREA_CODE_SET_NAME),
    [lookupCodes],
  );
  const organizations = useMemo(() => lookupCodes.getByType(API.ORGANIZATION_CODE_SET_NAME), [
    lookupCodes,
  ]);

  const organizationsList = organizations.filter(a => !a.parentId).map(mapLookupCode);
  const subOrganizations = organizations.filter(a => !!a.parentId).map(mapLookupCode);

  const propertyClassifications = useMemo(() => lookupCodes.getPropertyClassificationOptions(), [
    lookupCodes,
  ]);
  const administrativeAreas = useMemo(
    () => lookupCodes.getByType(API.ADMINISTRATIVE_AREA_CODE_SET_NAME),
    [lookupCodes],
  );

  const organizationIds = useMemo(() => organizations.map(x => x.id), [organizations]);
  const [sorting, setSorting] = useState<TableSort<IProperty>>({ description: 'asc' });

  // We'll start our table without any data
  const [data, setData] = useState<IProperty[] | undefined>();
  // For getting the buildings on parcel folder click
  const [expandData, setExpandData] = useState<any>({});

  // Filtering and pagination state
  const [filter, setFilter] = useState<IPropertyFilter>(defaultFilterValues);
  const isParcel =
    !filter ||
    [PropertyTypeNames.Land.toString(), PropertyTypeNames.Subdivision.toString()].includes(
      filter?.propertyType ?? '',
    );
  const parcelColumns = useMemo(
    () =>
      cols(
        organizationsList,
        subOrganizations,
        municipalities,
        propertyClassifications,
        PropertyTypes.Parcel,
        editable,
      ),
    [organizationsList, subOrganizations, municipalities, propertyClassifications, editable],
  );

  const buildingExpandColumns = useMemo(
    () =>
      cols(
        organizationsList,
        subOrganizations,
        municipalities,
        propertyClassifications,
        PropertyTypes.Building,
        false,
      ),
    [organizationsList, subOrganizations, municipalities, propertyClassifications],
  );

  const buildingColumns = useMemo(
    () =>
      buildingCols(
        organizationsList,
        subOrganizations,
        municipalities,
        propertyClassifications,
        PropertyTypes.Building,
        editable,
      ),
    [organizationsList, subOrganizations, municipalities, propertyClassifications, editable],
  );

  const [pageSize, setPageSize] = useState(10);
  const [pageIndex, setPageIndex] = useState(0);
  const [pageCount, setPageCount] = useState(0);

  const fetchIdRef = useRef(0);

  const parsedFilter = useMemo(() => {
    const data = { ...filter };
    if (data.organizations) {
      data.organizations = Array.isArray(data.organizations)
        ? (data.organizations as any)
            .filter((x: any) => !!x)
            .map((a: any) => {
              return parseInt(typeof a === 'string' ? a : a.value, 10);
            })
        : [parseInt(data.organizations, 10)];
    }

    return data;
  }, [filter]);

  const { updateSearch } = useRouterFilter({
    filter: parsedFilter,
    setFilter: setFilter,
    key: 'propertyFilter',
  });

  // Update internal state whenever the filter bar state changes
  const handleFilterChange = useCallback(
    async (value: IPropertyFilter) => {
      setFilter({ ...value, propertyType: propertyType });
      updateSearch({ ...value, propertyType: propertyType });
      setPageIndex(0); // Go to first page of results when filter changes
    },
    [setFilter, setPageIndex, updateSearch, propertyType],
  );
  // This will get called when the table needs new data
  const handleRequestData = useCallback(
    async ({ pageIndex, pageSize }: { pageIndex: number; pageSize: number }) => {
      setPageSize(pageSize);
      setPageIndex(pageIndex);
    },
    [setPageSize, setPageIndex],
  );

  const fetchData = useCallback(
    async ({
      pageIndex,
      pageSize,
      filter,
      organizationIds,
      sorting,
    }: {
      pageIndex: number;
      pageSize: number;
      filter: IPropertyFilter;
      organizationIds: number[];
      sorting: TableSort<IProperty>;
    }) => {
      // Give this fetch an ID
      const fetchId = ++fetchIdRef.current;

      // TODO: Set the loading state
      // setLoading(true);

      // Only update the data if this is the latest fetch
      if (organizationIds?.length > 0) {
        setData(undefined);
        const query = getServerQuery({ pageIndex, pageSize, filter, organizationIds });
        const data = await service.getPropertyList(query, sorting);
        // The server could send back total page count.
        // For now we'll just calculate it.
        if (fetchId === fetchIdRef.current && data?.items) {
          setData(data.items);
          setPageCount(Math.ceil(data.total / pageSize));
        }

        // setLoading(false);
      }
    },
    [setData, setPageCount],
  );

  // Listen for changes in pagination and use the state to fetch our new data
  useDeepCompareEffect(() => {
    fetchData({ pageIndex, pageSize, filter, organizationIds, sorting });
  }, [fetchData, pageIndex, pageSize, filter, organizationIds, sorting]);

  const dispatch = useDispatch();

  /**
   * @param {'csv' | 'excel'} accept Whether the fetch is for type of CSV or EXCEL
   * @param {boolean} getAllFields Enable this field to generate report with additional fields. For SRES only.
   */
  const fetch = (accept: 'csv' | 'excel', getAllFields?: boolean) => {
    const query = getServerQuery({ pageIndex, pageSize, filter, organizationIds });
    return dispatch(
      download({
        url: getAllFields
          ? getAllFieldsPropertyReportUrl({ ...query, all: true, propertyType: undefined })
          : getPropertyReportUrl({ ...query, all: true, propertyType: undefined }),
        fileName: `pims-inventory.${accept === 'csv' ? 'csv' : 'xlsx'}`,
        actionType: 'properties-report',
        headers: {
          Accept: accept === 'csv' ? 'text/csv' : 'application/vnd.ms-excel',
        },
      }),
    );
  };

  const checkExpanded = (row: IProperty, property: IProperty) => {
    return row.id === property.id;
  };

  const loadBuildings = async (expandedRows: IProperty[]) => {
    if (expandedRows.length > 0) {
      await Promise.all(
        expandedRows.map(async property => {
          if (property.propertyTypeId === PropertyTypes.Parcel) {
            if (expandData[property.id] === undefined) {
              setExpandData({
                ...expandData,
                [property.id]: (await service.loadBuildings(property.id)).items,
              });
            }
          }
        }),
      );
    }
  };

  const changePropertyType = (type: PropertyTypeNames) => {
    setPropertyType(type);
    setPageIndex(0);
    setFilter(state => {
      return {
        ...state,
        propertyType: type,
      };
    });
  };

  const appliedFilter = { ...filter };
  if (appliedFilter.organizations && typeof appliedFilter.organizations === 'string') {
    const organizationSelections = organizations.map(mapLookupCode);
    appliedFilter.organizations = filter.organizations
      .split(',')
      .map(
        value => organizationSelections.find(organization => organization.value === value) || '',
      ) as any;
  }

  const onRowClick = useCallback((row: IProperty) => {
    window.open(
      `/mapview?${queryString.stringify({
        sidebar: true,
        disabled: true,
        loadDraft: false,
        parcelId: [PropertyTypes.Parcel, PropertyTypes.Subdivision].includes(row.propertyTypeId)
          ? row.id
          : undefined,
        buildingId: row.propertyTypeId === PropertyTypes.Building ? row.id : undefined,
      })}`,
      '_blank',
    );
  }, []);

  const submitTableChanges = async (
    values: { properties: IProperty[] },
    actions: FormikProps<{ properties: IProperty[] }>,
  ) => {
    let nextProperties = [...values.properties];
    const editableColumnKeys = ['assessedLand', 'assessedBuilding', 'netBook', 'market'];

    const changedRows = dirtyRows
      .map(change => {
        const data = { ...values.properties![change.rowId] };
        return { data, ...change } as any;
      })
      .filter(c => intersection(keys(c), editableColumnKeys).length > 0);

    let errors: any[] = fill(range(nextProperties.length), undefined);
    let touched: any[] = fill(range(nextProperties.length), undefined);
    if (changedRows.length > 0) {
      const changedRowIds = changedRows.map(x => x.rowId);
      // Manually validate the table form
      const currentErrors = await actions.validateForm();
      const errorRowIds = keys(currentErrors.properties)
        .map(Number)
        .filter(i => !!currentErrors.properties![i]);
      const foundRowErrorsIndexes = intersection(changedRowIds, errorRowIds);
      if (foundRowErrorsIndexes.length > 0) {
        for (const index of foundRowErrorsIndexes) {
          errors[index] = currentErrors.properties![index];
          // Marked the editable cells as touched
          touched[index] = keys(currentErrors.properties![index]).reduce(
            (acc: any, current: string) => {
              return { ...acc, [current]: true };
            },
            {},
          );
        }
      } else {
        for (const change of changedRows) {
          const apiProperty = toApiProperty(change.data as any, true);
          const callApi = apiProperty.parcelId ? updateParcel : updateBuilding;
          try {
            const response: any = await callApi(apiProperty.id, apiProperty);
            nextProperties = nextProperties.map((item, index: number) => {
              if (index === change.rowId) {
                item = {
                  ...item,
                  ...flattenProperty(response),
                } as any;
              }
              return item;
            });

            toast.info(
              `Successfully saved changes for ${apiProperty.name || apiProperty.address?.line1}`,
            );
          } catch (error) {
            const errorMessage = (error as Error).message;

            touched[change.rowId] = pick(change, ['assessedLand', 'netBook', 'market']);
            toast.error(
              `Failed to save changes for ${apiProperty.name ||
                apiProperty.address?.line1}. ${errorMessage}`,
            );
            errors[change.rowId] = {
              assessedLand: change.assessedland && (errorMessage || 'Save request failed.'),
              netBook: change.netBook && (errorMessage || 'Save request failed.'),
              market: change.market && (errorMessage || 'Save request failed.'),
            };
          }
        }
      }

      setDirtyRows([]);
      if (!errors.find(x => !!x)) {
        actions.setTouched({ properties: [] });
        setData(nextProperties);
      } else {
        actions.resetForm({
          values: { properties: nextProperties },
          errors: { properties: errors },
          touched: { properties: touched },
        });
      }
    }
  };
  return (
    <Container fluid className="PropertyListView">
      <Container fluid className="filter-container border-bottom">
        <Container className="px-0">
          <PropertyFilter
            defaultFilter={defaultFilterValues}
            organizationLookupCodes={organizations}
            adminAreaLookupCodes={administrativeAreas}
            onChange={handleFilterChange}
            sort={sorting}
            onSorting={setSorting}
          />
        </Container>
      </Container>
      <div className="ScrollContainer">
        <Container fluid className="TableToolbar">
          <h3>View Inventory</h3>
          <div className="menu">
            <div>
              <TooltipWrapper toolTipId="show-parcels" toolTip="Show Parcels">
                <div
                  className={
                    filter.propertyType === PropertyTypeNames.Land ? 'svg-btn active' : 'svg-btn'
                  }
                  onClick={() => changePropertyType(PropertyTypeNames.Land)}
                >
                  <LandSvg className="svg" />
                  Parcels view
                </div>
              </TooltipWrapper>
            </div>
            <div>
              <TooltipWrapper toolTipId="show-buildings" toolTip="Show Buildings">
                <div
                  className={
                    filter.propertyType === PropertyTypeNames.Building
                      ? 'svg-btn active'
                      : 'svg-btn'
                  }
                  onClick={() => changePropertyType(PropertyTypeNames.Building)}
                >
                  <BuildingSvg className="svg" />
                  Buildings view
                </div>
              </TooltipWrapper>
            </div>
          </div>
          <TooltipWrapper toolTipId="export-to-excel" toolTip="Export to Excel">
            <FileIcon>
              <FaFileExcel data-testid="excel-icon" size={36} onClick={() => fetch('excel')} />
            </FileIcon>
          </TooltipWrapper>
          <TooltipWrapper toolTipId="export-to-excel" toolTip="Export to CSV">
            <FileIcon>
              <FaFileAlt data-testid="csv-icon" size={36} onClick={() => fetch('csv')} />
            </FileIcon>
          </TooltipWrapper>
          {(keycloak.hasRole(Roles.SRES_FINANCIAL_MANAGER) || keycloak.hasRole(Roles.SRES)) && (
            <TooltipWrapper toolTipId="export-to-excel" toolTip="Export all properties and fields">
              <FileIcon>
                <FaFileExport
                  data-testid="file-icon"
                  size={36}
                  onClick={() => fetch('excel', true)}
                />
              </FileIcon>
            </TooltipWrapper>
          )}
          <VerticalDivider />

          {!editable && (
            <TooltipWrapper toolTipId="edit-financial-values" toolTip={'Edit financial values'}>
              <EditIconButton>
                <FaEdit data-testid="edit-icon" size={36} onClick={() => setEditable(!editable)} />
              </EditIconButton>
            </TooltipWrapper>
          )}
          {editable && (
            <>
              <TooltipWrapper toolTipId="cancel-edited-financial-values" toolTip={'Cancel edits'}>
                <Button
                  data-testid="cancel-changes"
                  variant="outline-primary"
                  style={{ marginRight: 10 }}
                  onClick={() => {
                    if (tableFormRef.current?.dirty) {
                      tableFormRef.current.resetForm();
                    }
                    setEditable(false);
                  }}
                >
                  Cancel
                </Button>
              </TooltipWrapper>
              <TooltipWrapper
                toolTipId="save-edited-financial-values"
                toolTip={'Save financial values'}
              >
                <Button
                  data-testid="save-changes"
                  onClick={async () => {
                    if (tableFormRef.current?.dirty && dirtyRows.length > 0) {
                      const values = tableFormRef.current.values;
                      const actions = tableFormRef.current;
                      await submitTableChanges(values, actions);
                    }
                  }}
                >
                  Save edits
                </Button>
              </TooltipWrapper>
            </>
          )}
        </Container>

        <Table<IProperty>
          name="propertiesTable"
          lockPageSize={true}
          columns={isParcel ? parcelColumns : buildingColumns}
          data={data || []}
          loading={data === undefined}
          filterable
          sort={sorting}
          pageIndex={pageIndex}
          onRequestData={handleRequestData}
          onRowClick={onRowClick}
          tableToolbarText={
            filter.propertyType === PropertyTypeNames.Building
              ? undefined
              : '* Assessed value per building'
          }
          pageCount={pageCount}
          onSortChange={(column: string, direction: SortDirection) => {
            if (!!direction) {
              setSorting({ ...sorting, [column]: direction });
            } else {
              const data: any = { ...sorting };
              delete data[column];
              setSorting(data);
            }
          }}
          canRowExpand={(val: any) => val.values.propertyTypeId === PropertyTypes.Parcel}
          detailsPanel={{
            render: val => {
              if (expandData[val.id]) {
                return (
                  <Buildings
                    hideHeaders={true}
                    data={expandData[val.id]}
                    columns={buildingExpandColumns}
                    onRowClick={onRowClick}
                  />
                );
              }
            },
            icons: {
              open: <FaFolderOpen color="black" size={20} />,
              closed: <FaFolder color="black" size={20} />,
            },
            checkExpanded: (row, state) => !!state.find(x => checkExpanded(x, row)),
            onExpand: loadBuildings,
            getRowId: row => row.id,
          }}
          filter={appliedFilter}
          onFilterChange={values => {
            setFilter({ ...filter, ...values });
          }}
          renderBodyComponent={({ body }) => (
            <Formik
              innerRef={tableFormRef as any}
              initialValues={{ properties: data || [] }}
              validationSchema={Yup.object().shape({
                properties: Yup.array().of(
                  Yup.object().shape({
                    assessedLand: Yup.number()
                      .min(0, 'Minimum value is $0')
                      .max(1000000000, 'Maximum value is $1,000,000,000')
                      .required('Required'),
                    market: Yup.number()
                      .min(0, 'Minimum value is $0')
                      .max(1000000000, 'Maximum value is $1,000,000,000')
                      .required('Required'),
                    netBook: Yup.number()
                      .min(0, 'Minimum value is $0')
                      .max(1000000000, 'Maximum value is $1,000,000,000')
                      .required('Required'),
                  }),
                ),
              })}
              onSubmit={noop}
            >
              <Form>
                <DirtyRowsTracker setDirtyRows={setDirtyRows} />
                {body}
              </Form>
            </Formik>
          )}
        />
      </div>
    </Container>
  );
};

export default PropertyListView;
