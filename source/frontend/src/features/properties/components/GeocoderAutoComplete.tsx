import './GeocoderAutoComplete.scss';

import classNames from 'classnames';
import { useFormikContext } from 'formik';
import debounce from 'lodash/debounce';
import * as React from 'react';
import Form from 'react-bootstrap/Form';
import { FormControlProps } from 'react-bootstrap/FormControl';
import ClickAwayListener from 'react-click-away-listener';

import { DisplayError } from '@/components/common/form';
import TooltipIcon from '@/components/common/TooltipIcon';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { IGeocoderResponse } from '@/hooks/pims-api/interfaces/IGeocoder';
import { useGeocoderRepository } from '@/hooks/useGeocoderRepository';

interface IGeocoderAutoCompleteProps {
  field: string;
  placeholder?: string;
  disabled?: boolean;
  autoSetting?: string;
  required?: boolean;
  debounceTimeout?: number;
  value?: string;
  onSelectionChanged?: (data: IGeocoderResponse) => void;
  error?: any;
  touch?: any;
  onTextChange?: (value?: string) => void;
  tooltip?: string;
  displayErrorTooltips?: boolean;
  /** class to apply to entire form group */
  outerClassName?: string;
}

export const GeocoderAutoComplete: React.FC<
  React.PropsWithChildren<IGeocoderAutoCompleteProps>
> = ({
  field,
  placeholder,
  disabled,
  autoSetting,
  required,
  onSelectionChanged,
  debounceTimeout,
  value,
  touch,
  error,
  onTextChange,
  tooltip,
  displayErrorTooltips,
  outerClassName,
  ...rest
}) => {
  const [options, setOptions] = React.useState<IGeocoderResponse[]>([]);
  const { handleBlur } = useFormikContext<any>();
  const errorTooltip = error && touch && displayErrorTooltips ? error : undefined;
  const { searchAddress } = useGeocoderRepository();
  const [textValue, setTextValue] = React.useState<string | undefined>(value);
  const debouncedSearch = React.useRef(
    debounce(
      async (val: string, abort: boolean) => {
        if (!abort) {
          const addresses = (await searchAddress(val)) || [];
          setOptions(addresses);
        }
      },
      debounceTimeout || 500,
      { trailing: true },
    ),
  ).current;

  const onTextChanged = async (val?: string) => {
    onTextChange && onTextChange(val);
    setTextValue(val);
    if (val && val.length >= 5 && val !== value) {
      debouncedSearch(val, false);
    } else {
      setOptions([]);
    }
  };
  React.useEffect(() => {
    return () => {
      debouncedSearch('', true);
    };
  }, [debouncedSearch]);

  const suggestionSelected = (val: IGeocoderResponse) => {
    setOptions([]);
    if (onSelectionChanged) {
      val.fullAddress = (val.fullAddress || '').split(',')[0];
      setTextValue(val.fullAddress);
      onSelectionChanged(val);
    }
  };

  const renderSuggestions = () => {
    if (options !== undefined && options.length === 0) {
      return null;
    }
    if (options !== undefined) {
      return (
        <div className="suggestionList">
          {options.map((x: IGeocoderResponse, index: number) => (
            <option key={index} onClick={() => suggestionSelected(x)}>
              {x.fullAddress}
            </option>
          ))}
        </div>
      );
    }
    return null;
  };

  return (
    <div className="GeocoderAutoComplete">
      <ClickAwayListener onClickAway={() => setOptions([])}>
        <Form.Group
          controlId={`input-${field}`}
          className={classNames(required ? 'required' : '', outerClassName)}
        >
          <TooltipWrapper tooltipId={`${field}-error-tooltip}`} tooltip={errorTooltip}>
            <InputControl
              data-testid="geocoder-input"
              autoComplete={autoSetting}
              field={field}
              value={textValue}
              isInvalid={!!touch && !!error}
              onTextChange={onTextChanged}
              placeholder={placeholder}
              disabled={disabled}
              required={required}
              onBlur={handleBlur}
              {...rest}
            />
          </TooltipWrapper>
          {!!tooltip && <TooltipIcon toolTipId={`${field}-tooltip`} toolTip={tooltip} />}
          {renderSuggestions()}
          {!errorTooltip && <DisplayError field={field} />}
        </Form.Group>
      </ClickAwayListener>
    </div>
  );
};

interface IDebounceInputProps extends FormControlProps {
  field: string;
  autoComplete?: string;
  required?: boolean;
  placeholder?: string;
  isInvalid?: boolean;
  onTextChange: (value?: string) => void;
  onBlur: {
    (e: React.FocusEvent<any>): void;
    <T = any>(fieldOrEvent: T): T extends string ? (e: any) => void : void;
  };
}

const InputControl: React.FC<React.PropsWithChildren<IDebounceInputProps>> = ({
  onTextChange,
  ...props
}) => {
  const onChange = (value: string) => {
    onTextChange(value);
  };

  return (
    <Form.Control
      autoComplete={props.autoComplete}
      name={props.field}
      value={props.value}
      isInvalid={props.isInvalid}
      onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
        onChange(e.target.value);
      }}
      placeholder={props.placeholder}
      disabled={props.disabled}
      required={props.required}
      {...props}
    />
  );
};
