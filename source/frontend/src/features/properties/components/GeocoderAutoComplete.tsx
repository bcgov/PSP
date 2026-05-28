import './GeocoderAutoComplete.scss';

import classNames from 'classnames';
import { useFormikContext } from 'formik';
import debounce from 'lodash/debounce';
import React, { useEffect, useRef, useState } from 'react';
import Form from 'react-bootstrap/Form';
import { FormControlProps } from 'react-bootstrap/FormControl';
import Overlay from 'react-bootstrap/Overlay';
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

interface ISuggestionListProps {
  options: IGeocoderResponse[];
  onSelect: (value: IGeocoderResponse) => void;
}

const SuggestionList: React.FC<React.PropsWithChildren<ISuggestionListProps>> = ({
  options,
  onSelect,
}) => {
  return (
    <ul className="suggestionList">
      {options.map((option, index: number) => (
        <li
          key={index}
          onClick={e => {
            onSelect(option);
            e.stopPropagation();
          }}
        >
          {option.fullAddress}
        </li>
      ))}
    </ul>
  );
};

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
  const [options, setOptions] = useState<IGeocoderResponse[]>([]);
  const { handleBlur } = useFormikContext<any>();
  const errorTooltip = error && touch && displayErrorTooltips ? error : undefined;
  const { searchAddress } = useGeocoderRepository();
  const [textValue, setTextValue] = useState<string | undefined>(value);
  const containerRef = useRef<HTMLDivElement | null>(null);
  const inputRef = useRef<HTMLInputElement | null>(null);
  const debouncedSearch = useRef(
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

  useEffect(() => {
    return () => {
      debouncedSearch('', true);
    };
  }, [debouncedSearch]);

  const suggestionSelected = (val: IGeocoderResponse) => {
    setOptions([]);
    if (onSelectionChanged) {
      val.fullAddress = val.fullAddress || '';
      setTextValue(val.fullAddress);
      onSelectionChanged(val);
    }
  };

  const renderSuggestions = () => {
    if (options.length === 0) {
      return null;
    }

    return (
      <Overlay
        target={inputRef.current}
        show
        placement="bottom-start"
        popperConfig={{
          modifiers: [
            {
              name: 'offset',
              options: {
                offset: [0, 0],
              },
            },
          ],
        }}
      >
        {({
          placement,
          show: _show,
          arrowProps: _arrowProps,
          popper: _popper,
          ...overlayProps
        }) => (
          <div {...overlayProps} data-placement={placement} className="suggestionOverlay">
            <SuggestionList options={options} onSelect={suggestionSelected} />
          </div>
        )}
      </Overlay>
    );
  };

  return (
    <div className="GeocoderAutoComplete" ref={containerRef}>
      <ClickAwayListener
        onClickAway={e => {
          if (e.type === 'click') {
            setOptions([]);
          }
        }}
      >
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
              ref={inputRef}
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

const InputControl = React.forwardRef<HTMLInputElement, IDebounceInputProps>(
  ({ onTextChange, ...props }, ref) => {
    const onChange = (value: string) => {
      onTextChange(value);
    };

    return (
      <Form.Control
        ref={ref}
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
  },
);

InputControl.displayName = 'InputControl';
