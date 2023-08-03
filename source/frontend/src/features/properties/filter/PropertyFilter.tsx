import { Formik } from 'formik';
import React, { useMemo, useState } from 'react';
import Col from 'react-bootstrap/Col';
import { useHistory } from 'react-router';
import styled from 'styled-components';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { Form, Input, Select } from '@/components/common/form';
import { TableSort } from '@/components/Table/TableSort';
import { useRouterFilter } from '@/hooks/useRouterFilter';
import { FilterBarSchema } from '@/utils/YupSchema';

import { GeocoderAutoComplete } from '../components/GeocoderAutoComplete';
import { defaultPropertyFilter, IPropertyFilter } from './IPropertyFilter';
import PropertySearchToggle, { SearchToggleOption } from './PropertySearchToggle';

/**
 * PropertyFilter component properties.
 */
export interface IPropertyFilterProps {
  /** The default filter to apply if a different one hasn't been set in the URL or stored in redux. */
  defaultFilter: IPropertyFilter;
  /** Callback event when the filter is changed during Mount. */
  onChange: (filter: IPropertyFilter) => void;
  /** Comma separated list of column names to sort by. */
  sort?: TableSort<any>;
  /** Event fire when sorting changes. */
  onSorting?: (sort: TableSort<any>) => void;
  /** Override to trigger filterchanged in the parent */
  searchButtonClicked?: () => void;
  /** Which toggle view is currently active */
  toggle?: SearchToggleOption;
  /** Which toggle view is currently active */
  useGeocoder?: boolean;
}

/**
 * Property filter bar to search for properties.
 * Applied filter will be added to the URL query parameters and stored in a redux store.
 */
export const PropertyFilter: React.FC<React.PropsWithChildren<IPropertyFilterProps>> = ({
  defaultFilter,
  onChange,
  sort,
  onSorting,
  searchButtonClicked,
  toggle = SearchToggleOption.Map,
  useGeocoder,
}) => {
  const [propertyFilter, setPropertyFilter] = useState<IPropertyFilter>(defaultFilter);

  useRouterFilter<IPropertyFilter>({
    filter: propertyFilter,
    setFilter: filter => {
      onChange(filter);
      setPropertyFilter(filter);
    },
    key: 'propertyFilter',
    sort: sort,
    setSorting: onSorting,
    exactPath: '/mapview',
  });

  const history = useHistory();
  const initialValues = useMemo(() => {
    const values = { ...defaultFilter, ...propertyFilter };
    return values;
  }, [defaultFilter, propertyFilter]);

  const changeFilter = (values: IPropertyFilter) => {
    setPropertyFilter({ ...values });
    onChange?.({ ...values });
  };

  const resetFilter = () => {
    changeFilter(defaultPropertyFilter);
  };

  const handlePageToggle = (option: SearchToggleOption) => {
    if (option === SearchToggleOption.Map) {
      history.push('/mapview');
    } else if (option === SearchToggleOption.List) {
      history.push('/properties/list');
    }
  };

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
      {({ isSubmitting, setFieldValue, values, resetForm }) => (
        <Form>
          <Form.Row className="map-filter-bar pb-4">
            <Col xs="auto">
              <span>Search:</span>
            </Col>
            <NoRightPaddingColumn xs="1" md="1" lg="1" xl="1">
              <StyledSelect
                field="searchBy"
                options={[
                  { label: 'PID/PIN', value: 'pinOrPid' },
                  { label: 'Address', value: 'address' },
                  { label: 'Plan #', value: 'planNumber' },
                ]}
                className="idir-input-group"
                onChange={() => {
                  setFieldValue('pinOrPid', '');
                  setFieldValue('latitude', null);
                  setFieldValue('longitude', null);
                }}
              />
            </NoRightPaddingColumn>
            <StyledCol xs="3" md="2" lg="4" xl="3">
              {values.searchBy === 'pinOrPid' && (
                <Input field="pinOrPid" placeholder="Enter a PID or PIN"></Input>
              )}
              {values.searchBy === 'address' && useGeocoder && (
                <GeocoderAutoComplete
                  data-testid="geocoder-mapview"
                  field="address"
                  placeholder="Enter an address"
                  onSelectionChanged={val => {
                    setFieldValue('latitude', val.latitude);
                    setFieldValue('longitude', val.longitude);
                  }}
                  value={values.address}
                  autoSetting="off"
                ></GeocoderAutoComplete>
              )}
              {values.searchBy === 'address' && !useGeocoder && (
                <Input field="address" placeholder="Enter address"></Input>
              )}
              {values.searchBy === 'planNumber' && (
                <Input field="planNumber" placeholder="Enter a plan number"></Input>
              )}
            </StyledCol>
            <Col xs="auto">
              <SearchButton
                disabled={isSubmitting}
                onClick={() => searchButtonClicked && searchButtonClicked()}
              />
            </Col>
            <Col xs="auto">
              <ResetButton
                disabled={isSubmitting}
                onClick={() => {
                  resetForm();
                  resetFilter();
                }}
              />
            </Col>
            <Col xs="auto" className="bar-item">
              <PropertySearchToggle
                onPageToggle={option => {
                  handlePageToggle(option);
                }}
                toolId={'toggle'}
                toggle={toggle}
              />
            </Col>
          </Form.Row>
        </Form>
      )}
    </Formik>
  );
};
const StyledSelect = styled(Select)`
  padding-right: 0 !important;
  .form-control {
    border-top-right-radius: 0;
    border-bottom-right-radius: 0;
  }
`;
const NoRightPaddingColumn = styled(Col)`
  padding-right: 0 !important;
  border-right: 0 !important;
`;

const StyledCol = styled(Col)`
  padding-left: 0 !important;
  .form-control {
    border-top-left-radius: 0;
    border-bottom-left-radius: 0;
  }
`;
