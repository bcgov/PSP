import { getIn, useFormikContext } from 'formik';
import React from 'react';

import { InputGroup, Select, SelectOption } from '@/components/common/form';

interface ISelectInputProps<SelectTypes> {
  /** Specifies that the HTML element should be disabled */
  disabled?: boolean;
  /** The option to show on the drop-down by default */
  defaultKey?: keyof SelectTypes;
  /** The field name */
  field: string;
  /** The underlying HTML element to use when rendering the FormControl */
  as?: React.ElementType;
  /** optional help text to display below the FormControl */
  helpText?: string;
  /** The drop-down options to render  */
  selectOptions?: {
    key: keyof SelectTypes;
    label: string;
    placeholder: string;
  }[];
  autoSetting?: string;
  /** Callback to notify when the selected option in the drop-down changes */
  onSelectItemChange?: (e: React.ChangeEvent<HTMLSelectElement>) => void;
}
/**
 * Provides a dropdown with list of search options for a given type.
 */
export function SelectInput<SelectTypes, FormikType>({
  disabled,
  selectOptions,
  defaultKey,
  field,
  as: is, // `as` is reserved in typescript
  onSelectItemChange,
  ...rest
}: ISelectInputProps<SelectTypes> & React.HTMLAttributes<HTMLElement>) {
  // access the form context values, no need to pass props
  const { values, setFieldValue } = useFormikContext<FormikType>();
  const value = getIn(values, field);

  const reset = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setFieldValue(field ? value : defaultKey, '');
    if (onSelectItemChange) {
      onSelectItemChange(e);
    }
  };
  const options = selectOptions?.map(o => ({ label: o.label, value: o.key } as SelectOption));
  const selectedOption = selectOptions?.find(o => o.key === value);

  return (
    <InputGroup
      prepend={
        <Select field={field} options={options ?? []} onChange={reset} disabled={disabled} />
      }
      as={is}
      field={value}
      placeholder={selectedOption?.placeholder}
      disabled={disabled}
      {...rest}
    ></InputGroup>
  );
}
