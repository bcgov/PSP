import './Check.scss';

import classNames from 'classnames';
import { getIn, useFormikContext } from 'formik';
import React, { useEffect } from 'react';
import Form from 'react-bootstrap/Form';
import { FormCheckProps } from 'react-bootstrap/FormCheck';

import TooltipIcon from '../TooltipIcon';
import { DisplayError } from './DisplayError';

type RequiredAttributes = {
  /** The field name */
  field: string;
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
  /** style to use for the formgroup wrapping the inner element */
  className?: string;
  /** Whether the field is required. Makes the field border blue. */
  required?: boolean;
  /** Specifies that the HTML element should be disabled */
  disabled?: boolean;
  /** Use React-Bootstrap's custom form elements to replace the browser defaults */
  custom?: boolean;
  /** Adds a custom class to the input element of the <Input> component */
  innerClassName?: string;
  /** override the display of the component, default is checkbox. Select radio to display this checkbox as two radio buttons. */
  type?: string;
  /** label of the first radio button */
  radioLabelOne?: string;
  /** label of the second radio button */
  radioLabelTwo?: string;
  /** Optional tool tip message to add to checkbox */
  toolTip?: string;
  /** id for tooltip */
  toolTipId?: string;
};

// only "field" is required for <Check>, the rest are optional
export type CheckProps = FormCheckProps & OptionalAttributes & RequiredAttributes;

/**
 * Formik-connected <Check> form control
 */
export const Check: React.FC<React.PropsWithChildren<CheckProps>> = ({
  field,
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
  radioLabelOne,
  radioLabelTwo,
  toolTip,
  toolTipId,
  ...rest
}) => {
  const { values, initialValues, setFieldValue, setFieldTouched, errors, touched, handleBlur } =
    useFormikContext();
  const initialChecked = getIn(initialValues, field);
  const touch = getIn(touched, field);
  const checked = getIn(values, field);
  const error = getIn(errors, field);
  const asElement: any = is || 'input';
  useEffect(() => {
    if ((checked === true || checked === false) && initialChecked !== checked) {
      setFieldTouched(field, true);
    }
  }, [checked, field, handleBlur, setFieldTouched, initialChecked]);
  return (
    <Form.Group
      controlId={`input-${field}`}
      className={classNames(!!required ? 'required' : '', className)}
    >
      <div className="check-field">
        {!!label && (
          <Form.Label>
            {label}
            {!!toolTip && <TooltipIcon toolTipId={toolTipId!} toolTip={toolTip} />}
          </Form.Label>
        )}
        <>
          <Form.Check
            label={radioLabelOne}
            as={asElement}
            name={field}
            className={innerClassName}
            required={required}
            disabled={disabled}
            custom={custom}
            isInvalid={!!touch && !!error}
            type={type}
            {...rest}
            value={(checked === true) as any}
            placeholder={placeholder}
            checked={checked === true}
            onChange={() => {
              if (type !== 'radio') {
                setFieldValue(field, !checked);
              } else {
                setFieldValue(field, true);
              }
            }}
            onBlur={handleBlur}
          />
          {type === 'radio' && (
            <Form.Check
              label={radioLabelTwo}
              as={asElement}
              name={field}
              className={innerClassName}
              required={required}
              disabled={disabled}
              custom={custom}
              isInvalid={!!touch && !!error}
              type={type}
              id={`input-${field}-2`}
              {...rest}
              value={(checked === false) as any}
              placeholder={placeholder}
              checked={checked === false}
              onChange={(e: any) => {
                setFieldValue(field, false);
              }}
              onBlur={handleBlur}
            />
          )}
        </>
        {!!postLabel && !!required && (
          <>
            <Form.Label>{postLabel}</Form.Label>
          </>
        )}
        {!!postLabel && !required && <Form.Label className="text-wrap">{postLabel}</Form.Label>}
      </div>
      <DisplayError field={field} />
    </Form.Group>
  );
};
