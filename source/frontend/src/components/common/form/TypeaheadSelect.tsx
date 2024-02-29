import cx from 'classnames';
import { getIn, useFormikContext } from 'formik';
import React from 'react';
import Form from 'react-bootstrap/Form';
import { Typeahead } from 'react-bootstrap-typeahead';
import styled from 'styled-components';

import TooltipIcon from '../TooltipIcon';
import TooltipWrapper from '../TooltipWrapper';
import { DisplayError } from './DisplayError';
import { SelectOption } from './Select';

export interface ITypeaheadSelectProps {
  /* The form field name */
  field: string;
  /* The form component label */
  label?: string;
  /* Whether or not the field is required. */
  required?: boolean;
  /** Specifies that the HTML element should be disabled */
  disabled?: boolean;
  /* Optional tooltip text to display after the label */
  tooltip?: string;
  /* Whether or not to display errors in a tooltip instead of in a div */
  displayErrorTooltips?: boolean;
  /* Class names to apply to the outer form group wrapper */
  className?: string;
  /* Short hint that describes the expected value of an <input> element */
  placeholder?: string;
  /* Minimum user input before displaying results. */
  minLength?: number;
  /* Array in the shape of [ { value: string | number, label: string } ] */
  options: SelectOption[];
  /* Invoked when a user changes the selected option */
  onChange?: (selected: SelectOption | undefined) => void;
  /* Invoked when the typeahead loses focus */
  onBlur?: (e: Event) => void;
}

/**
 * Formik-connected <Select> form control with type-ahead capabilities.
 */
export const TypeaheadSelect = React.forwardRef<Typeahead<SelectOption>, ITypeaheadSelectProps>(
  (props, ref) => {
    const {
      field,
      label,
      required,
      disabled,
      tooltip,
      displayErrorTooltips,
      className,
      placeholder,
      minLength = 0,
      options,
      onChange,
      onBlur,
    } = props;
    const { errors, touched, values, setFieldValue, setFieldTouched } = useFormikContext();
    const value = getIn(values, field);
    const error = getIn(errors, field);
    const touch = getIn(touched, field);
    const errorTooltip = error && touch && displayErrorTooltips ? error : undefined;

    const onSelectChange = React.useCallback(
      (selectedArray: SelectOption[]) => {
        const selected = selectedArray.length > 0 ? selectedArray[0] : undefined;
        setFieldValue(field, selected);
        if (typeof onChange === 'function') {
          onChange(selected);
        }
      },
      [field, setFieldValue, onChange],
    );

    const onSelectBlur = React.useCallback(
      (e: Event) => {
        setFieldTouched(field, true);
        if (typeof onBlur === 'function') {
          onBlur(e);
        }
      },
      [field, onBlur, setFieldTouched],
    );

    return (
      <StyledFormGroup
        className={cx({ required: required }, className)}
        data-testid={`typeahead-select-${field}`}
      >
        {label && <Form.Label htmlFor={`typeahead-select-${field}`}>{label}</Form.Label>}
        {tooltip && <TooltipIcon toolTipId={`${field}-tooltip`} toolTip={tooltip} />}
        <TooltipWrapper tooltipId={`${field}-error-tooltip}`} tooltip={errorTooltip}>
          <Typeahead
            ref={ref}
            id={`typeahead-select-${field}`}
            inputProps={{
              name: `typeahead-select-${field}`,
              id: `typeahead-select-${field}`,
            }}
            disabled={disabled}
            flip
            clearButton
            highlightOnlyResult
            multiple={false}
            isInvalid={touch && error}
            placeholder={placeholder ?? 'Type to search...'}
            options={options}
            labelKey="label"
            selected={value ? [].concat(value) : []}
            minLength={minLength}
            onChange={onSelectChange}
            onBlur={onSelectBlur}
          ></Typeahead>
        </TooltipWrapper>
        {!displayErrorTooltips && <DisplayError field={field} />}
      </StyledFormGroup>
    );
  },
);

const StyledFormGroup = styled(Form.Group)`
  // This is the close button of the type-ahead
  button.close {
    font-size: 2.4rem;
  }
`;
