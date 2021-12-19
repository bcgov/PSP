import { getIn, useFormikContext } from 'formik';
import React from 'react';
import {
  AsyncTypeahead as BaseTypeahead,
  AsyncTypeaheadProps,
  TypeaheadModel,
} from 'react-bootstrap-typeahead';

export interface ITypeaheadFieldProps<T extends TypeaheadModel> extends AsyncTypeaheadProps<T> {}

// Bypass client-side filtering by returning `true`. Results are already
// filtered by the search endpoint, so no need to do it again.
const filterBy = () => true;

export function AsyncTypeahead<T extends TypeaheadModel>({
  id,
  options,
  labelKey,
  minLength = 3,
  onSearch,
  ...rest
}: ITypeaheadFieldProps<T>) {
  return (
    <BaseTypeahead
      id={id}
      options={options}
      filterBy={filterBy}
      labelKey={labelKey}
      minLength={minLength}
      onSearch={onSearch}
      {...rest}
    />
  );
}
