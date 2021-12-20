import { getIn, useFormikContext } from 'formik';
import React from 'react';
import {
  AsyncTypeahead as BaseAsyncTypeahead,
  AsyncTypeaheadProps,
  TypeaheadModel,
} from 'react-bootstrap-typeahead';

export interface ITypeaheadFieldProps<T extends TypeaheadModel> extends AsyncTypeaheadProps<T> {
  /** The form field name */
  field: string;
}

// Bypass client-side filtering by returning `true`. Results are already
// filtered by the search endpoint, so no need to do it again.
const filterBy = () => true;

/**
 * Formik-connected <AsyncTypeahead> form control
 */
export function AsyncTypeahead<T extends TypeaheadModel>({
  id,
  multiple,
  field,
  options,
  labelKey,
  minLength = 3,
  onSearch,
  ...rest
}: ITypeaheadFieldProps<T>) {
  // Hook into Formik state
  const { errors, touched, values, setFieldValue } = useFormikContext();
  const value = getIn(values, field);
  const error = getIn(errors, field);
  const touch = getIn(touched, field);

  // Invoked whenever items are added or removed. Receives an array of the selected options.
  const handleChange = (selected: T[]) => {
    if (!!multiple) {
      setFieldValue(field, selected);
    } else {
      const newValue = selected.length > 0 ? selected[0] : '';
      setFieldValue(field, newValue);
    }
  };

  return (
    <BaseAsyncTypeahead
      id={id}
      options={options}
      multiple={multiple}
      filterBy={filterBy}
      labelKey={labelKey}
      minLength={minLength}
      useCache={false}
      selected={[].concat(value)} // always set an array here - works for multiple and single selections
      isInvalid={!!touch && !!error}
      onSearch={onSearch}
      onChange={handleChange}
      {...rest}
    />
  );
}
