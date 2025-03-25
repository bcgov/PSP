import classNames from 'classnames';
import { getIn, useFormikContext } from 'formik';
import { Form } from 'react-bootstrap';
import { FormControlProps } from 'react-bootstrap/FormControl';

import TooltipIcon from '../TooltipIcon';
import TooltipWrapper from '../TooltipWrapper';
import { DisplayError } from './DisplayError';

export interface INumberInputProps extends FormControlProps {
  /** The field name */
  field: string;
  /** The form component label */
  label?: string;
  /** Short hint that describes the expected value of an <input> element */
  placeholder?: string;
  /** optional help text to display below the FormControl */
  helpText?: string;
  /** Adds a custom class to the input element of the <Input> component */
  className?: string;
  /** class to apply to the inner input */
  innerClassName?: string;
  /** Whether the field is required. Makes the field border blue. */
  required?: boolean;
  /** optional tooltip text to display after the label */
  tooltip?: string;
  /** Display errors in a tooltip instead of in a div */
  displayErrorTooltips?: boolean;
  /** extra error keys to display */
  errorKeys?: string[];
  /** Specifies that the HTML element should be disabled */
  disabled?: boolean;
  /** Optional event handler for focus events */
  onFocus?: React.FocusEventHandler<any>;
}

export const NumberInput: React.FunctionComponent<INumberInputProps> = ({
  field,
  label,
  placeholder,
  helpText,
  className,
  innerClassName,
  required,
  tooltip,
  displayErrorTooltips,
  errorKeys,
  disabled,
  onChange,
  onFocus,
  ...rest
}) => {
  const { handleBlur, errors, touched, values, setFieldValue } = useFormikContext<any>();
  const error = getIn(errors, field);
  const extraErrors = errorKeys?.map(key => getIn(errors, key)).filter(e => e) ?? [];
  const touch = getIn(touched, field);
  const value = getIn(values, field);
  const errorTooltip =
    (error || extraErrors) && touch && displayErrorTooltips
      ? (error ? [error, ...extraErrors] : extraErrors).join('\n')
      : undefined;

  return (
    <Form.Group
      controlId={`number-input-${field}`}
      className={classNames(required ? 'required' : '', className, 'input')}
    >
      {!!label && (
        <Form.Label>
          {label} {!!tooltip && <TooltipIcon toolTipId={`${field}-tooltip`} toolTip={tooltip} />}
        </Form.Label>
      )}
      {!!tooltip && !label && <TooltipIcon toolTipId={`${field}-tooltip`} toolTip={tooltip} />}

      <TooltipWrapper tooltipId={`${field}-error-tooltip`} tooltip={errorTooltip}>
        <Form.Control
          className={innerClassName}
          name={field}
          disabled={disabled}
          isValid={false}
          isInvalid={!!touch && (!!error || extraErrors.length > 0)}
          {...rest}
          as="input"
          type="number"
          value={value}
          title={value}
          placeholder={placeholder}
          aria-describedby={helpText ? `${field}-help-text` : undefined}
          onFocus={(e: React.FocusEvent<HTMLInputElement>) => {
            if (onFocus) {
              onFocus(e);
            }
          }}
          onBlur={(e: React.FocusEvent<HTMLInputElement>) => {
            handleBlur(e);
          }}
          onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
            const parsed = parseFloat(e.target.value);
            setFieldValue(field, isNaN(parsed) ? undefined : parsed);
            if (onChange) {
              onChange(e);
            }
          }}
        />
      </TooltipWrapper>
      {helpText && (
        <Form.Text id={`${field}-help-text`} muted>
          {helpText}
        </Form.Text>
      )}
      {!displayErrorTooltips && <DisplayError field={field} />}
    </Form.Group>
  );
};
