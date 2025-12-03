import '../components/GeocoderAutoComplete.scss';

import { useFormikContext } from 'formik';
import { Feature, Geometry } from 'geojson';
import { sortBy } from 'lodash';
import debounce from 'lodash/debounce';
import { useEffect, useRef, useState } from 'react';
import Form from 'react-bootstrap/Form';
import { FormControlProps } from 'react-bootstrap/FormControl';
import ClickAwayListener from 'react-click-away-listener';

import TooltipWrapper from '@/components/common/TooltipWrapper';
import { IGeographicNamesProperties } from '@/hooks/pims-api/interfaces/IGeographicNamesProperties';
import { defaultGeographicNameSearchCriteria } from '@/hooks/pims-api/useApiGeographicNames';
import { useGeographicNamesRepository } from '@/hooks/useGeographicNamesRepository';
import { exists } from '@/utils';

interface IGeographicNameInputProps {
  field: string;
  placeholder?: string;
  disabled?: boolean;
  debounceTimeout?: number;
  onSelectionChanged?: (data: Feature<Geometry, IGeographicNamesProperties>) => void;
}

const MIN_SEARCH_LENGTH = 3;

export const GeographicNameInput: React.FC<React.PropsWithChildren<IGeographicNameInputProps>> = ({
  field,
  placeholder,
  disabled,
  debounceTimeout,
  onSelectionChanged,
  ...rest
}) => {
  const [options, setOptions] = useState<Feature<Geometry, IGeographicNamesProperties>[]>([]);
  const { handleBlur, setFieldValue, values } = useFormikContext<any>();
  const textValue = values[field];
  const { searchName } = useGeographicNamesRepository();
  const debouncedSearch = useRef(
    debounce(
      async (val: string, abort: boolean) => {
        if (!abort) {
          const collection = await searchName.execute({
            ...defaultGeographicNameSearchCriteria,
            name: val,
          });
          const sortedFeatures = sortBy(collection?.features, item => {
            return item.properties.score;
          });
          setOptions(sortedFeatures ?? []);
        }
      },
      debounceTimeout ?? 500,
      { trailing: true },
    ),
  ).current;

  const onTextChanged = async (val?: string) => {
    setFieldValue(field, val);
    if (val && val.length >= MIN_SEARCH_LENGTH) {
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

  const suggestionSelected = (val: Feature<Geometry, IGeographicNamesProperties>) => {
    setOptions([]);
    if (onSelectionChanged) {
      setFieldValue(field, val?.properties?.name ?? '');
      onSelectionChanged(val);
    }
  };

  const renderSuggestions = () => {
    if (options !== undefined && options.length === 0) {
      return null;
    }
    return (
      <ul className="suggestionList">
        {options.map((x: Feature<Geometry, IGeographicNamesProperties>, index: number) => (
          <li
            key={x.id ?? index}
            onClick={e => {
              suggestionSelected(x);
              e.stopPropagation();
            }}
          >
            {[
              x?.properties?.name,
              x?.properties?.featureType,
              x?.properties?.featureCategoryDescription,
            ]
              .filter(exists)
              .join(' - ')}
          </li>
        ))}
      </ul>
    );
  };

  return (
    <div className="GeographicNameInput">
      <ClickAwayListener
        onClickAway={e => {
          if (e.type === 'click') {
            setOptions([]);
          }
        }}
      >
        <Form.Group controlId={`input-${field}`}>
          <TooltipWrapper
            tooltipId={`${field}-input-tooltip`}
            tooltip={
              textValue?.length > 1 && textValue?.length < MIN_SEARCH_LENGTH
                ? 'Type at least 3 characters to see results'
                : 'Type the name of a landmark or geographic feature. Select a result and search to jump to that location'
            }
          >
            <InputControl
              autoComplete="off"
              data-testid="geographic-name-input"
              field={field}
              value={textValue}
              onTextChange={onTextChanged}
              placeholder={placeholder}
              disabled={disabled}
              onBlur={handleBlur}
              {...rest}
            />
          </TooltipWrapper>
          {renderSuggestions()}
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
      {...props}
      autoComplete={props.autoComplete}
      name={props.field}
      value={props.value ?? ''}
      isInvalid={props.isInvalid}
      onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
        onChange(e.target.value);
      }}
      placeholder={props.placeholder}
      disabled={props.disabled}
      required={props.required}
    />
  );
};
