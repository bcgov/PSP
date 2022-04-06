import { InputGroup, Select } from 'components/common/form';
import { useFormikContext } from 'formik';
import React from 'react';

import { IPropertyFilter } from './IPropertyFilter';

interface IPropertyFilterOptions {
  disabled?: boolean;
  options?: { label: string; value: string }[];
  placeholders?: Record<string, string>;
}

/**
 * Provides a dropdown with list of search options for properties.
 */
export const PropertyFilterOptions: React.FC<IPropertyFilterOptions &
  React.HTMLAttributes<HTMLElement>> = ({ disabled, options, placeholders, ...rest }) => {
  const state: {
    options: { label: string; value: string }[];
    placeholders: Record<string, string>;
  } = {
    options: options ?? [
      { label: 'PID/PIN', value: 'pinOrPid' },
      { label: 'Address', value: 'address' },
    ],
    placeholders: placeholders ?? {
      address: 'Enter an address',
      pinOrPid: 'Enter a PID or PIN',
    },
  };

  // access the form context values, no need to pass props
  const {
    values: { searchBy },
    setFieldValue,
  } = useFormikContext<IPropertyFilter>();
  const desc = state.placeholders[searchBy] || '';

  const reset = () => {
    setFieldValue(searchBy ? searchBy : 'pinOrPid', '');
  };

  return (
    <InputGroup
      formikProps={null as any}
      prepend={
        <Select field="searchBy" options={state.options} onChange={reset} disabled={disabled} />
      }
      field={searchBy}
      placeholder={desc}
      disabled={disabled}
      {...rest}
    ></InputGroup>
  );
};
