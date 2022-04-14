import { InputGroup, Select, SelectOption } from 'components/common/form';
import { getIn, useFormikContext } from 'formik';
import React from 'react';

interface ISelectInputProps<SelectTypes> {
  disabled?: boolean;
  defaultKey?: keyof SelectTypes;
  field: string;
  selectOptions?: {
    key: keyof SelectTypes;
    label: string;
    placeholder: string;
  }[];
}
/**
 * Provides a dropdown with list of search options for a given type.
 */
export function SelectInput<SelectTypes, FormikType>({
  disabled,
  selectOptions,
  defaultKey,
  field,
  ...rest
}: ISelectInputProps<SelectTypes> & React.HTMLAttributes<HTMLElement>) {
  // access the form context values, no need to pass props
  const { values, setFieldValue } = useFormikContext<FormikType>();
  const value = getIn(values, field);

  const reset = () => {
    setFieldValue(field ? value : defaultKey, '');
  };
  const options = selectOptions?.map(o => ({ label: o.label, value: o.key } as SelectOption));
  const selectedOption = selectOptions?.find(o => o.key === value);

  return (
    <InputGroup
      prepend={
        <Select field={field} options={options ?? []} onChange={reset} disabled={disabled} />
      }
      field={value}
      placeholder={selectedOption?.placeholder}
      disabled={disabled}
      {...rest}
    ></InputGroup>
  );
}
