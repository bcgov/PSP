import clsx from 'classnames';
import { getIn, useFormikContext } from 'formik';
import React from 'react';

import { FastInput } from '.';
import { Input, InputProps } from './Input';

// only "field" is required for <Input>, the rest are optional
export type TextProps = InputProps & {
  /** use FastInput instead of Input */
  fast?: boolean;
  rows?: number;
  cols?: number;
};

/**
 * Formik-connected <Input> form control
 */
export const TextArea: React.FC<TextProps> = ({
  field,
  label,
  placeholder,
  className,
  required,
  disabled,
  custom,
  fast,
  innerClassName,
  ...rest
}) => {
  const formikProps = useFormikContext();
  const { values, handleChange, errors, touched } = formikProps;
  const error = getIn(errors, field);
  const touch = getIn(touched, field);

  return fast ? (
    <FastInput
      formikProps={formikProps}
      label={label}
      as="textarea"
      field={field}
      className={clsx(className, 'textarea')}
      required={required}
      disabled={disabled}
      custom={custom}
      isInvalid={!!touch && !!error}
      {...rest}
      value={getIn(values, field)}
      placeholder={placeholder}
      onChange={handleChange}
    />
  ) : (
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
      value={getIn(values, field)}
      placeholder={placeholder}
      onChange={handleChange}
    />
  );
};
