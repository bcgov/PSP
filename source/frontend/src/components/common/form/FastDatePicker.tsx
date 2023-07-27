import classNames from 'classnames';
import { ErrorMessage, FormikProps, getIn } from 'formik';
import moment from 'moment';
import * as Popper from 'popper.js';
import { FunctionComponent, memo, useEffect } from 'react';
import { Form, FormControlProps } from 'react-bootstrap';
import DatePicker, { ReactDatePickerProps } from 'react-datepicker';
import styled from 'styled-components';

import { formikFieldMemo } from '@/utils';

type RequiredAttributes = {
  /** The field name */
  field: string;
  /** formik state used for context and memo calculations */
  formikProps: FormikProps<any>;
};

type OptionalAttributes = {
  /** Specifies that the HTML element should be disabled */
  disabled?: boolean;
  /** Class name of the input wrapper */
  className?: string;
  /** Class name of the date picker */
  innerClassName?: string;
  /** The minimum data allowable to be chosen in the datepicker */
  minDate?: Date;
  /** form label */
  label?: string;
  /** Whether the field is required. Makes the field border blue. */
  required?: boolean;
  /** optional popper modifiers to pass to the datepicker */
  popperModifiers?: Popper.Modifiers | undefined;
};

export type FastDatePickerProps = FormControlProps &
  RequiredAttributes &
  OptionalAttributes &
  Partial<ReactDatePickerProps>;

/**
 * Formik connected react-datepicker. Uses memo and cleanup inspired by
 * https://jaredpalmer.com/formik/docs/api/fastfield
 */
const FormikDatePicker: FunctionComponent<React.PropsWithChildren<FastDatePickerProps>> = ({
  field,
  disabled,
  className,
  innerClassName,
  minDate,
  label,
  required,
  popperModifiers,
  formikProps: {
    values,
    initialValues,
    errors,
    touched,
    setFieldValue,
    registerField,
    unregisterField,
    setFieldTouched,
    handleChange,
  },
  ...rest
}) => {
  const error = getIn(errors, field);
  const touch = getIn(touched, field);
  let value = getIn(values, field);
  if (value === '') {
    value = null;
  }
  if (typeof value === 'string') {
    value = moment(value, 'YYYY-MM-DD').toDate();
  }
  useEffect(() => {
    registerField(field, {});
    return () => {
      unregisterField(field);
    };
  }, [field, registerField, unregisterField]);
  const isInvalid = error && touch ? 'is-invalid ' : '';
  const isValid = !error && touch && value && !disabled ? 'is-valid ' : '';
  return (
    <StyledGroup
      className={classNames(!!required ? 'required' : '', className)}
      controlId={`datepicker-${field}`}
    >
      {!!label && <Form.Label>{label}</Form.Label>}

      <StyledDatePicker
        id={`datepicker-${field}`}
        showYearDropdown
        scrollableYearDropdown
        yearDropdownItemNumber={10}
        autoComplete="off"
        name={field}
        required={required}
        placeholderText="MTH DD, YYYY"
        className={classNames('form-control', 'date-picker', isInvalid, isValid, innerClassName)}
        dateFormat="MMM dd, yyyy"
        selected={(value && new Date(value)) || null}
        disabled={disabled}
        minDate={minDate ? moment(minDate, 'YYYY-MM-DD').toDate() : undefined}
        {...rest}
        popperModifiers={popperModifiers}
        onBlur={() => {
          setFieldTouched(field);
        }}
        onChange={(val: any) => {
          setFieldValue(field, val ? moment.utc(val).format('YYYY-MM-DD') : '');
        }}
        wrapperClassName="d-block"
        showIcon
      />
      <ErrorMessage component="div" className="invalid-feedback" name={field}></ErrorMessage>
    </StyledGroup>
  );
};

const StyledGroup = styled(Form.Group)`
  .react-datepicker__calendar-icon {
    width: 3rem;
    height: 3rem;
    margin-top: 0.5rem;
    right: 0;
    fill: ${props => props.theme.css.linkColor};
    pointer-events: none;
  }
  .react-datepicker__view-calendar-icon input {
    padding: 0.6rem 1rem 0.5rem 0.6rem;
  }
  .react-datepicker-wrapper {
    max-width: 16rem;
  }
`;

const StyledDatePicker = styled(DatePicker)`
  && {
    &.form-control.is-valid {
      background-image: none;
    }
    &.form-control.is-invalid {
      border-color: #d8292f !important;
    }
  }
`;

export const FastDatePicker = memo(FormikDatePicker, formikFieldMemo);
