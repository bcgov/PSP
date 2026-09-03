import classNames from 'classnames';
import { getIn, useFormikContext } from 'formik';
import { ReactNode } from 'react';
import Form from 'react-bootstrap/Form';
import { FormCheckProps } from 'react-bootstrap/FormCheck';
import styled from 'styled-components';

import TooltipIcon from '../TooltipIcon';

export type CheckGroupOption = {
  label: ReactNode;
  value: string;
  disabled?: boolean;
};

type RequiredAttributes = {
  field: string;
  checkValues: CheckGroupOption[];
};

type OptionalAttributes = {
  label?: string;
  isLabelBold?: boolean;
  placeholder?: string;
  className?: string;
  checkGroupClassName?: string;
  required?: boolean;
  disabled?: boolean;
  custom?: boolean;
  innerClassName?: string;
  toolTip?: string;
  toolTipId?: string;
  flexDirection?: 'column' | 'row';
  handleChange?: (values: string[]) => void;

  children?: ReactNode;
};

export type CheckGroupProps = Omit<FormCheckProps, 'onChange'> &
  OptionalAttributes &
  RequiredAttributes;

export const CheckGroup = ({
  field,
  checkValues,
  label,
  isLabelBold,
  placeholder,
  className,
  checkGroupClassName,
  innerClassName,
  required,
  disabled,
  custom,
  toolTip,
  toolTipId,
  flexDirection,
  handleChange,
  children,
  ...rest
}: CheckGroupProps) => {
  const { values, errors, touched, setFieldValue, setFieldTouched } =
    useFormikContext<Record<string, unknown>>();

  const touch = getIn(touched, field);
  const error = getIn(errors, field);

  const fieldValue = getIn(values, field);
  const activeValues: string[] = Array.isArray(fieldValue) ? fieldValue : [];

  const toggleValue = (checkValue: string) => {
    const nextValues = activeValues.includes(checkValue)
      ? activeValues.filter(value => value !== checkValue)
      : [...activeValues, checkValue];

    setFieldValue(field, nextValues);
    setFieldTouched(field, true, false);
    handleChange?.(nextValues);
  };

  return (
    <StyledCheckGroup
      className={classNames(required && 'required', className)}
      $flexDirection={flexDirection ?? 'column'}
    >
      {label && (
        <Form.Label>
          {isLabelBold ? <b>{label}</b> : label}

          {toolTip && toolTipId && <TooltipIcon toolTipId={toolTipId} toolTip={toolTip} />}
        </Form.Label>
      )}

      <div className="check-group">
        {checkValues.map(option => {
          const id = `input-${field}-${option.value}`;
          const checked = activeValues.includes(option.value);

          return (
            <Form.Check
              key={option.value}
              id={id}
              name={field}
              className={classNames(checkGroupClassName, innerClassName)}
              label={option.label}
              required={required}
              disabled={disabled || option.disabled}
              custom={custom}
              isInvalid={Boolean(touch && error)}
              type="checkbox"
              {...rest}
              value={option.value}
              placeholder={placeholder}
              checked={checked}
              onChange={() => toggleValue(option.value)}
              onBlur={() => setFieldTouched(field, true)}
              data-testid={`checkbox-${field}-${option.value}`}
            />
          );
        })}
        {children}
      </div>
    </StyledCheckGroup>
  );
};

export const StyledCheckGroup = styled(Form.Group)`
  &.form-group {
    margin-bottom: 0;
    text-align: left;
  }

  .check-group {
    display: grid;
    grid-template-columns: repeat(2, minmax(0, 1fr));
    gap: 0.4rem 1rem;
  }

  .form-check {
    display: flex;
    padding: 0;
  }

  .form-check-input {
    margin-left: 0;
  }

  .form-check-label {
    margin-left: 2rem;
  }

  .form-label {
    margin-bottom: 0;
  }

  @media (max-width: 576px) {
    .check-group {
      grid-template-columns: 1fr;
    }
  }
`;
