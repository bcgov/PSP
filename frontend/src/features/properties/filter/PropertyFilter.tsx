import './PropertyFilter.scss';

import {
  Form,
  ResetButton,
  SearchButton,
  SearchToggle,
  SearchToggleOption,
} from 'components/common/form';
import { TableSort } from 'components/Table/TableSort';
import { Formik } from 'formik';
import { useRouterFilter } from 'hooks/useRouterFilter';
import React, { useMemo, useState } from 'react';
import Col from 'react-bootstrap/Col';
import { FilterBarSchema } from 'utils/YupSchema';

import { PropertyFilterOptions } from './';
import { IPropertyFilter } from './IPropertyFilter';

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
  setTriggerFilterChanged?: (used: boolean) => void;
  /** Which toggle view is currently active */
  toggle?: SearchToggleOption;
}

/**
 * Property filter bar to search for properties.
 * Applied filter will be added to the URL query parameters and stored in a redux store.
 */
export const PropertyFilter: React.FC<IPropertyFilterProps> = ({
  defaultFilter,
  onChange,
  sort,
  onSorting,
  setTriggerFilterChanged,
  toggle = SearchToggleOption.Map,
}) => {
  const [propertyFilter, setPropertyFilter] = useState<IPropertyFilter>(defaultFilter);
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

  const initialValues = useMemo(() => {
    const values = { ...defaultFilter, ...propertyFilter };
    return values;
  }, [defaultFilter, propertyFilter]);

  const changeFilter = (values: IPropertyFilter) => {
    setPropertyFilter({ ...values });
    onChange?.({ ...values });
  };

  const resetFilter = () => {
    changeFilter(defaultFilter);
  };

  const handlePageToggle = () => {};

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
            <Col xs="5">
              <PropertyFilterOptions />
            </Col>
            <Col xs="auto">
              <SearchButton
                disabled={isSubmitting}
                onClick={() => setTriggerFilterChanged && setTriggerFilterChanged(true)}
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
              <SearchToggle onClick={handlePageToggle} toolId={'toggle'} toggle={toggle} />
            </Col>
          </Form.Row>
        </Form>
      )}
    </Formik>
  );
};
