import classNames from 'classnames';
import { getIn, useFormikContext } from 'formik';
import React, { ReactNode } from 'react';
import Form from 'react-bootstrap/Form';
import { FormCheckProps } from 'react-bootstrap/FormCheck';
import styled from 'styled-components';

import { InlineFlexDiv } from '../styles';
import TooltipIcon from '../TooltipIcon';
import { DisplayError } from './DisplayError';

type RequiredAttributes = {
  field: string;
  radioValues: { radioLabel: ReactNode; radioValue: string }[];
};

type OptionalAttributes = {
  /** The form component label to display before the checkbox */
  label?: string;
  /** The form component label to display after the checkbox */
  postLabel?: string;
  /** The underlying HTML element to use when rendering the FormControl */
  as?: React.ElementType;
  /** Short hint that describes the expected value of an <input> element */
  placeholder?: string;
  /** Adds a custom class to the input element of the <Input> component */
  className?: string;
  /** Whether the field is required. Makes the field border blue. */
  required?: boolean;
  /** Specifies that the HTML element should be disabled */
  disabled?: boolean;
  /** Use React-Bootstrap's custom form elements to replace the browser defaults */
  custom?: boolean;
  /** style to use for the formgroup wrapping the inner element */
  innerClassName?: string;
  /** override the display of the component, default is checkbox. Select radio to display this checkbox as two radio buttons. */
  type?: string;
  /** Optional tool tip message to add to checkbox */
  toolTip?: string;
  /** id for tooltip */
  toolTipId?: string;
};

// only "field" is required for <RadioGroup>, the rest are optional
export type RadioGroupProps = FormCheckProps & OptionalAttributes & RequiredAttributes;

/**
 * Formik-connected <RadioGroup> form control
 */
export const RadioGroup = ({
  field,
  radioValues,
  label,
  postLabel,
  as: is, // `as` is reserved in typescript
  placeholder,
  className,
  innerClassName,
  required,
  disabled,
  type,
  custom,
  toolTip,
  toolTipId,
  ...rest
}: RadioGroupProps) => {
  const { values, errors, touched, handleBlur, handleChange } = useFormikContext();
  const touch = getIn(touched, field);
  const activeValue = getIn(values, field);
  const error = getIn(errors, field);
  return (
    <StyledRadioGroup
      controlId={`input-${field}`}
      className={classNames(!!required ? 'required' : '', className)}
    >
      {!!label && (
        <Form.Label>
          {label}
          {!!toolTip && <TooltipIcon toolTipId={toolTipId!} toolTip={toolTip} />}
        </Form.Label>
      )}
      <div className="radio-group">
        {radioValues.map(({ radioLabel, radioValue }, index) => (
          <InlineFlexDiv key={`field-${radioValue}-${index}`}>
            <Form.Check
              id={`input-${radioValue}`}
              name={field}
              className={innerClassName}
              required={required}
              disabled={disabled}
              custom={custom}
              isInvalid={!!touch && !!error}
              type="radio"
              {...rest}
              value={radioValue}
              placeholder={placeholder}
              checked={activeValue === radioValue}
              onChange={handleChange}
              onBlur={handleBlur}
            />
            <Form.Label className="form-check-label">{radioLabel}</Form.Label>
          </InlineFlexDiv>
        ))}
      </div>
      {!!postLabel && !!required && <Form.Label>{postLabel}</Form.Label>}
      {!!postLabel && !required && <Form.Label className="text-wrap">{postLabel}</Form.Label>}
      <DisplayError field={field} />
    </StyledRadioGroup>
  );
};

export const StyledRadioGroup = styled(Form.Group)`
  &.form-group {
    display: flex;
    margin-bottom: 0;
    .radio-group {
      display: flex;
      flex-direction: column;
      row-gap: 0.4rem;
      .form-check {
        display: flex;
        .form-check-input {
          margin-left: 0;
        }
      }
      .form-check-label {
        margin-left: 1rem;
      }
    }
  }
`;
