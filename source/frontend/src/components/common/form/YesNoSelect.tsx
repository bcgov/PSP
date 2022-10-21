import cx from 'classnames';
import { getIn, useFormikContext } from 'formik';
import React from 'react';
import Form from 'react-bootstrap/Form';
import { nullableBooleanToString, stringToNullableBoolean } from 'utils/formUtils';

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
  formGroupClassName?: string;
  /** (Optional) Change event handler */
  onChange?: React.FormEventHandler;
}

export const YesNoSelect: React.FC<IYesNoSelectProps> = props => {
  const { field, label, formGroupClassName, required, tooltip, displayErrorTooltips, onChange } =
    props;

  const { values, errors, touched, handleBlur, setFieldValue } = useFormikContext();
  const value = getIn(values, field) as boolean | null;
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
    <Form.Group
      controlId={`input-${field}`}
      className={cx({ required: required }, formGroupClassName)}
    >
      {label && <Form.Label>{label}</Form.Label>}
      {tooltip && <TooltipIcon toolTipId={`${field}-tooltip`} toolTip={tooltip} />}

      <TooltipWrapper toolTipId={`${field}-error-tooltip}`} toolTip={errorTooltip}>
        <Form.Control
          as="select"
          name={field}
          value={nullableBooleanToString(value)}
          onBlur={handleBlur}
          onChange={handleChange}
        >
          <option value="null">Unknown</option>
          <option value="true">Yes</option>
          <option value="false">No</option>
        </Form.Control>
      </TooltipWrapper>

      {!displayErrorTooltips && <DisplayError field={field} />}
    </Form.Group>
  );
};
