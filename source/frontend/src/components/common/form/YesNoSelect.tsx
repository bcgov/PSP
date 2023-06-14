import cx from 'classnames';
import { getIn, useFormikContext } from 'formik';
import React from 'react';
import Form from 'react-bootstrap/Form';

import { nullableBooleanToString, stringToNullableBoolean } from '@/utils/formUtils';

import TooltipIcon from '../TooltipIcon';
import TooltipWrapper from '../TooltipWrapper';
import { DisplayError } from './DisplayError';

export interface IYesNoSelectProps {
  /** The field name */
  field: string;
  /** (Optional) The label used above the input element */
  label?: string;
  /* (Optional) Whether or not the field is required */
  required?: boolean;
  /* (Optional) Tooltip text to display after the label */
  tooltip?: string;
  /* (Optional) Whether or not to display errors in a tooltip instead of below the control */
  displayErrorTooltips?: boolean;
  /** (Optional) Class name of the outer form group wrapper */
  className?: string;
  /** (Optional) Change event handler */
  onChange?: React.FormEventHandler;
  /** (Optional) Whether this field is disabled */
  disabled?: boolean;
  /** (Optional) Whether the Unknown will show */
  notNullable?: boolean;
}

export const YesNoSelect: React.FC<React.PropsWithChildren<IYesNoSelectProps>> = props => {
  const { field, label, className, required, tooltip, displayErrorTooltips, onChange, disabled } =
    props;

  const { values, errors, touched, handleBlur, setFieldValue } = useFormikContext();
  const value = getIn(values, field) as boolean | null;

  if (props.notNullable === true && value === null) {
    setFieldValue(field, false);
  }

  const error = getIn(errors, field);
  const touch = getIn(touched, field);
  const errorTooltip = error && touch && displayErrorTooltips ? error : undefined;

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setFieldValue(field, stringToNullableBoolean(e.target.value));
    if (typeof onChange === 'function') {
      onChange(e);
    }
  };

  return (
    <Form.Group controlId={`input-${field}`} className={cx({ required: required }, className)}>
      {label && <Form.Label>{label}</Form.Label>}
      {tooltip && <TooltipIcon toolTipId={`${field}-tooltip`} toolTip={tooltip} />}

      <TooltipWrapper toolTipId={`${field}-error-tooltip}`} toolTip={errorTooltip}>
        <Form.Control
          as="select"
          name={field}
          value={nullableBooleanToString(value)}
          onBlur={handleBlur}
          onChange={handleChange}
          disabled={disabled}
        >
          {props.notNullable !== true ? <option value="null">Unknown</option> : <></>}
          <option value="true">Yes</option>
          <option value="false">No</option>
        </Form.Control>
      </TooltipWrapper>

      {!displayErrorTooltips && <DisplayError field={field} />}
    </Form.Group>
  );
};
