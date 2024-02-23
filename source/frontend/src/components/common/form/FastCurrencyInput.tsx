import './FastCurrencyInput.scss';

import classNames from 'classnames';
import { ErrorMessage, FormikProps, getIn } from 'formik';
import React, { memo, useEffect } from 'react';
import { ColProps } from 'react-bootstrap/Col';
import Form from 'react-bootstrap/Form';
import NumberFormat from 'react-number-format';

import { formikFieldMemo, isPositiveNumberOrZero } from '@/utils';

import TooltipIcon from '../TooltipIcon';

export const defaultMaskOptions = {
  prefix: '$',
  suffix: '',
  includeThousandsSeparator: true,
  thousandsSeparatorSymbol: ',',
  allowDecimal: true,
  decimalSymbol: '.',
  decimalLimit: 2, // how many digits allowed after the decimal
  integerLimit: undefined, // limit length of integer numbers
  allowNegative: false,
  allowLeadingZeroes: false,
};

type RequiredAttributes = {
  /** The field name */
  field: string;
  /** formik state used for context and memo calculations */
  formikProps: FormikProps<any>;
};

type OptionalAttributes = {
  /** optional field label */
  label?: string;
  /** Specifies that the HTML element should be disabled */
  disabled?: boolean;
  /** The value to display, overrides formik values */
  value?: number | '';
  /** Adds a custom class to the input element of the wrapper component */
  className?: string;
  /** Class name of the input */
  innerClassName?: string;
  /** formik state used for context and memo calculations */
  formikProps: FormikProps<any>;
  /** Whether the field is required. Makes the field border blue. */
  required?: boolean;
  /** Whether negative numbers are allowed. */
  allowNegative?: boolean;
  /** Suppress validation on submit */
  suppressValidation?: boolean;

  onBlurChange?: React.FormEventHandler;

  onChange?: React.FormEventHandler;
};

export type CurrencyInputProps = RequiredAttributes &
  OptionalAttributes & { placeholder?: string; tooltip?: string } & ColProps;

/**
 * Formik-wrapped currency input providing a numeric mask over number fields
 * without changing the formik prop value to string.
 */
const CurrencyInput = ({
  field,
  className,
  innerClassName,
  value,
  disabled,
  placeholder,
  tooltip,
  required,
  suppressValidation,
  label,
  formikProps: {
    handleBlur,
    values,
    setFieldValue,
    errors,
    touched,
    registerField,
    unregisterField,
    isSubmitting,
  },
  onBlurChange,
  onChange,
  ...rest
}: CurrencyInputProps) => {
  value = value ? value : getIn(values, field);
  const error = getIn(errors, field);
  let touch = getIn(touched, field);
  if (isSubmitting && suppressValidation) {
    touch = false;
  }

  if (!rest.allowNegative && !isPositiveNumberOrZero(value)) {
    value = '';
  }

  const onHandleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const regex = rest.allowNegative ? /[^0-9.-]/g : /[^0-9.]/g;
    const cleanValue = e.target.value.replace(regex, '');
    setFieldValue(field, cleanValue ? parseFloat(cleanValue) : '');
    if (typeof onChange === 'function') {
      onChange(e);
    }
  };

  useEffect(() => {
    registerField(field, {});
    return () => {
      unregisterField(field);
    };
  }, [field, registerField, unregisterField]);

  const isInvalid = error && touch ? 'is-invalid ' : '';
  const isValid = !error && touch && value && !disabled ? 'is-valid ' : '';

  return (
    <Form.Group
      controlId={`input-${field}`}
      className={classNames(required ? 'required' : '', className)}
    >
      {!!label && (
        <Form.Label>
          {label} {!!tooltip && <TooltipIcon toolTipId={`${field}-tooltip`} toolTip={tooltip} />}
        </Form.Label>
      )}
      <div className="input-tooltip-wrapper">
        <NumberFormat
          id={`input-${field}`}
          value={value}
          name={field}
          onChange={onHandleChange}
          onBlur={(e: any) => {
            handleBlur(e);
            if (typeof onBlurChange === 'function') {
              onBlurChange(e);
            }
          }}
          className={classNames('form-control input-number', innerClassName, isInvalid, isValid)}
          disabled={disabled}
          required={required}
          fixedDecimalScale={true}
          decimalScale={2}
          prefix={'$'}
          thousandSeparator
          placeholder={placeholder || ''}
          allowNegative={rest.allowNegative}
        />
        {!label && !!tooltip && <TooltipIcon toolTipId="currency" toolTip={tooltip} />}

        <ErrorMessage component="div" className="invalid-feedback" name={field}></ErrorMessage>
      </div>
    </Form.Group>
  );
};

/**
 * FastCurrencyInput component provides a Formik input that only accepts financial values.
 */
export const FastCurrencyInput = memo(
  CurrencyInput,
  (prevProps: any, currentProps: any) =>
    prevProps.value === currentProps.value && formikFieldMemo(prevProps, currentProps),
);
