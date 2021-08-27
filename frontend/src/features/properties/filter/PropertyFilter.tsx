import './PropertyFilter.scss';

import { ParentSelect } from 'components/common/form/ParentSelect';
import ResetButton from 'components/common/form/ResetButton';
import SearchButton from 'components/common/form/SearchButton';
import { TypeaheadField } from 'components/common/form/Typeahead';
import { TableSort } from 'components/Table/TableSort';
import { Claims } from 'constants/claims';
import { usePropertyNames } from 'features/properties/common/slices/usePropertyNames';
import { Formik, getIn } from 'formik';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import useLookupCodes from 'hooks/useLookupCodeHelpers';
import { useMyOrganizations } from 'hooks/useMyOrganizations';
import { useRouterFilter } from 'hooks/useRouterFilter';
import React, { useMemo, useRef, useState } from 'react';
import Col from 'react-bootstrap/Col';
import { AsyncTypeahead } from 'react-bootstrap-typeahead';
import { ILookupCode } from 'store/slices/lookupCodes';
import styled from 'styled-components';
import { mapLookupCode, mapLookupCodeWithParentString } from 'utils';
import { mapSelectOptionWithParent } from 'utils';
import { FilterBarSchema } from 'utils/YupSchema';

import { Form, Select } from '../../../components/common/form';
import { PropertyFilterOptions } from './';
import { IPropertyFilter } from './IPropertyFilter';
import { PropertyFilterOrganizationOptions } from './PropertyFilterOrganizationOptions';

/**
 * PropertyFilter component properties.
 */
export interface IPropertyFilterProps {
  /** The default filter to apply if a different one hasn't been set in the URL or stored in redux. */
  defaultFilter: IPropertyFilter;
  /** An array of organization lookup codes. */
  organizationLookupCodes: ILookupCode[];
  /** An array of administrative area codes. */
  adminAreaLookupCodes: ILookupCode[];
  /** Callback event when the filter is changed during Mount. */
  onChange: (filter: IPropertyFilter) => void;
  /** Comma separated list of column names to sort by. */
  sort?: TableSort<any>;
  /** Event fire when sorting changes. */
  onSorting?: (sort: TableSort<any>) => void;
  /** Show select with my organizations/All Government dropdown */
  showAllOrganizationSelect?: boolean;
  /** Override to trigger filterchanged in the parent */
  setTriggerFilterChanged?: (used: boolean) => void;
}

const OrganizationCol = styled(Col)`
  display: flex;
  .form-control {
    width: 165px;
    height: 35px;
  }
  .form-group {
    .rbt {
      .rbt-menu {
        width: 370px !important;
      }
    }
  }
`;

/**
 * Property filter bar to search for properties.
 * Applied filter will be added to the URL query parameters and stored in a redux store.
 */
export const PropertyFilter: React.FC<IPropertyFilterProps> = ({
  defaultFilter,
  organizationLookupCodes,
  adminAreaLookupCodes,
  onChange,
  sort,
  onSorting,
  showAllOrganizationSelect,
  setTriggerFilterChanged,
}) => {
  const [propertyFilter, setPropertyFilter] = React.useState<IPropertyFilter>(defaultFilter);
  const keycloak = useKeycloakWrapper();
  const lookupCodes = useLookupCodes();
  const [initialLoad, setInitialLoad] = useState(false);
  useRouterFilter({
    filter: propertyFilter,
    setFilter: filter => {
      onChange(filter);
      setPropertyFilter(filter);
    },
    key: 'propertyFilter',
    sort: sort,
    setSorting: onSorting,
  });

  const organizations = (organizationLookupCodes ?? []).map(c =>
    mapLookupCodeWithParentString(c, organizationLookupCodes),
  );
  const classifications = lookupCodes.getPropertyClassificationTypeOptions();
  const adminAreas = (adminAreaLookupCodes ?? []).map(c => mapLookupCode(c));
  const [clear, setClear] = useState<boolean | undefined>(false);
  const [options, setOptions] = useState<string[]>([]);
  const { fetchPropertyNames } = usePropertyNames();

  const initialValues = useMemo(() => {
    const values = { ...defaultFilter, ...propertyFilter };
    if (typeof values.organizations === 'string') {
      const organization = organizations.find(
        x => x.value.toString() === values.organizations?.toString(),
      ) as any;
      if (organization) {
        values.organizations = organization;
      }
    } else {
      const organizations: any[] = values?.organizations || [];
      if (organizations.length > 0) {
        values.organizations =
          organizations[0] || (organizations.length === 2 ? organizations[1] : undefined);
      }
    }
    return values;
  }, [defaultFilter, propertyFilter, organizations]);

  const myOrganizations = useMyOrganizations();

  const changeFilter = (values: IPropertyFilter) => {
    const organizationIds = (values.organizations as any)?.value
      ? (values.organizations as any).value
      : values.organizations;
    setPropertyFilter({ ...values, organizations: organizationIds });
    onChange?.({ ...values, organizations: organizationIds });
  };

  const resetFilter = () => {
    ref.current.clear();
    changeFilter(defaultFilter);
    setClear(true);
  };

  const [findMoreOpen] = useState<boolean>(false);
  const ref = useRef<any>();

  return (
    <Formik<IPropertyFilter>
      initialValues={{ ...initialValues }}
      enableReinitialize
      validationSchema={FilterBarSchema}
      onSubmit={(values, { setSubmitting }) => {
        setSubmitting(true);
        changeFilter(values);
        setSubmitting(false);
      }}
    >
      {({ isSubmitting, setFieldValue, values }) => (
        <Form>
          <Form.Row className="map-filter-bar">
            <OrganizationCol>
              {showAllOrganizationSelect ? (
                <PropertyFilterOrganizationOptions
                  disabled={findMoreOpen}
                  organizations={organizations}
                />
              ) : (
                <ParentSelect
                  field="organizations"
                  options={myOrganizations.map(c => mapSelectOptionWithParent(c, myOrganizations))}
                  filterBy={['code', 'label', 'parent']}
                  placeholder="My Organizations"
                  selectClosest
                  disabled={findMoreOpen}
                />
              )}
            </OrganizationCol>
            <Col className="map-filter-typeahead">
              <AsyncTypeahead
                disabled={findMoreOpen && !keycloak.hasClaim(Claims.ADMIN_PROPERTIES)}
                isLoading={initialLoad}
                id={`name-field`}
                inputProps={{ id: `name-field` }}
                placeholder="Property name"
                onSearch={() => {
                  setInitialLoad(true);
                  fetchPropertyNames(keycloak.organizationId!).then(results => {
                    setOptions(results);
                    setInitialLoad(false);
                  });
                }}
                options={options}
                onChange={(newValues: string[]) => {
                  setFieldValue('name', getIn(newValues[0], 'value') ?? newValues[0]);
                }}
                ref={ref}
                onBlur={(e: any) =>
                  getIn(values, 'name') !== e.target.value && setFieldValue('name', e.target.value)
                }
              />
            </Col>
            <Col className="map-filter-typeahead">
              <TypeaheadField
                name="administrativeArea"
                placeholder="Location"
                selectClosest
                hideValidation={true}
                options={adminAreas.map(x => x.label)}
                onChange={(vals: any) => {
                  setFieldValue('administrativeArea', getIn(vals[0], 'name') ?? vals[0]);
                }}
                clearSelected={clear}
                setClear={setClear}
                disabled={findMoreOpen}
              />
            </Col>
            <Col className="bar-item">
              <PropertyFilterOptions disabled={findMoreOpen} />
            </Col>
            <Col className="bar-item">
              <Select
                field="classificationId"
                placeholder="Classification"
                options={classifications}
                disabled={findMoreOpen}
              />
            </Col>
            <Col className="bar-item flex-grow-0">
              <SearchButton
                disabled={isSubmitting || findMoreOpen}
                onClick={() => setTriggerFilterChanged && setTriggerFilterChanged(true)}
              />
            </Col>
            <Col className="bar-item flex-grow-0">
              <ResetButton disabled={isSubmitting || findMoreOpen} onClick={resetFilter} />
            </Col>
          </Form.Row>
        </Form>
      )}
    </Formik>
  );
};
