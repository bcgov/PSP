import clsx from 'classnames';
import { getIn, useFormikContext } from 'formik';
import { isArray } from 'lodash';
import React from 'react';

import { Input, InputProps } from './Input';

// only "field" is required for <Input>, the rest are optional
export type TextAreaProps = InputProps & {
  /** use FastInput instead of Input */
  fast?: boolean;
  rows?: number;
  cols?: number;
  mapFunction?: (value: any) => any;
};

/**
 * Formik-connected <Input> form control
 */
export const TextArea: React.FC<React.PropsWithChildren<TextAreaProps>> = ({
  field,
  label,
  placeholder,
  className,
  required,
  disabled,
  custom,
  innerClassName,
  mapFunction,
  ...rest
}) => {
  const formikProps = useFormikContext();
  const { values, handleChange, errors, touched } = formikProps;
  let value = getIn(values, field);
  if (isArray(value) && mapFunction !== undefined) {
    value = value.map(mapFunction);
  }
  const error = getIn(errors, field);
  const touch = getIn(touched, field);

  return (
    <Input
      label={label}
      innerClassName={innerClassName}
      as="textarea"
      field={field}
      className={clsx(className, 'textarea')}
      required={required}
      disabled={disabled}
      custom={custom}
      isInvalid={!!touch && !!error}
      {...rest}
      value={value}
      placeholder={placeholder}
      onChange={handleChange}
    />
  );
};
