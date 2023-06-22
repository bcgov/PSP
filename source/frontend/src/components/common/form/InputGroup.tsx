import classNames from 'classnames';
import React, { CSSProperties } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FormControlProps } from 'react-bootstrap/FormControl';
import BootstrapInputGroup from 'react-bootstrap/InputGroup';
import styled from 'styled-components';

import { Label } from '../Label';
import { Input } from './Input';

type RequiredAttributes = {
  /** The field name */
  field: string;
};

type OptionalAttributes = {
  /** The form component label */
  label?: string;
  /** The underlying HTML element to use when rendering the FormControl */
  as?: React.ElementType;
  /** optional help text to display below the FormControl */
  helpText?: string;
  /** Short hint that describes the expected value of an <input> element */
  placeholder?: string;
  /** Adds a custom class to the input element of the <Input> component */
  className?: string;
  /** Whether the field is required. Shows an asterisk after the label. */
  required?: boolean;
  /** Specifies that the HTML element should be disabled */
  disabled?: boolean;
  /** Use React-Bootstrap's custom form elements to replace the browser defaults */
  custom?: boolean;
  preText?: string;
  prepend?: React.ReactNode;
  postText?: string;
  content?: React.ReactNode;
  innerClassName?: string;
  displayErrorTooltips?: boolean;
  /** style to pass down to the FastInput or Input */
  style?: CSSProperties;
  /** pass options for typeahead component */
  options?: string[];
  /** autocomplete flag to determine whether to use typeahed or not */
  autoComplete?: boolean;
  autoSetting?: string;
};

// only "field" is required for <Input>, the rest are optional
export type InputGroupProps = FormControlProps & OptionalAttributes & RequiredAttributes;

/**
 * Formik-connected <InputGroup> form control - allows for an input with pre or posttext.
 */
export const InputGroup: React.FC<React.PropsWithChildren<InputGroupProps>> = ({
  field,
  label,
  style,
  as: is, // `as` is reserved in typescript
  placeholder,
  disabled,
  required,
  custom,
  preText,
  prepend: PrependComponent,
  postText,
  innerClassName,
  className,
  content,
  options,
  autoComplete,
  displayErrorTooltips,
  ...rest
}) => {
  return (
    <Row
      className={classNames(
        !!required ? 'required' : '',
        className,
        disabled ? 'disabled' : '',
        'flex-nowrap',
        'input-group',
      )}
    >
      {!!label && required && <Label required={required}>{label}</Label>}

      {preText && (
        <BootstrapInputGroup.Prepend>
          <BootstrapInputGroup.Text>{preText}</BootstrapInputGroup.Text>
        </BootstrapInputGroup.Prepend>
      )}
      {PrependComponent && (
        <ColPrepend xs="auto">
          <BootstrapInputGroup.Prepend>{PrependComponent}</BootstrapInputGroup.Prepend>
        </ColPrepend>
      )}
      <ColContent>
        {content ? (
          content
        ) : (
          <Input
            as={is}
            disabled={disabled}
            field={field}
            className={innerClassName}
            style={style}
            placeholder={placeholder}
            displayErrorTooltips={displayErrorTooltips}
            {...rest}
          />
        )}
      </ColContent>
      {postText && (
        <BootstrapInputGroup.Append>
          <BootstrapInputGroup.Text className={disabled ? 'append-disabled' : ''}>
            {postText}
          </BootstrapInputGroup.Text>
        </BootstrapInputGroup.Append>
      )}
    </Row>
  );
};

const ColPrepend = styled(Col)`
  padding-right: 0;
  .form-control {
    border-top-right-radius: 0;
    border-bottom-right-radius: 0;
  }
`;

const ColContent = styled(Col)`
  &:not(:first-child) {
    padding-left: 0;
    .form-control {
      border-top-left-radius: 0;
      border-bottom-left-radius: 0;
    }
  }
`;
