import cx from 'classnames';
import { getIn, useFormikContext } from 'formik';
import head from 'lodash/head';
import React, { useCallback } from 'react';
import Form from 'react-bootstrap/Form';
import {
  AsyncTypeahead as BaseAsyncTypeahead,
  AsyncTypeaheadProps,
  TypeaheadManagerChildProps,
  TypeaheadModel,
} from 'react-bootstrap-typeahead';
import { FaSearch } from 'react-icons/fa';
import styled from 'styled-components';

import TooltipIcon from '../TooltipIcon';
import TooltipWrapper from '../TooltipWrapper';
import { DisplayError } from './DisplayError';

export interface ITypeaheadFieldProps<T extends TypeaheadModel> extends AsyncTypeaheadProps<T> {
  /* The form field name */
  field: string;

  /* The form component label */
  label?: string;

  /* Class names to apply to the outer form group wrapper */
  className?: string;

  /* Custom class names to apply to the input element of the <AsyncTypeahead> component */
  innerClassName?: string;

  /* Whether or not the field is required. */
  required?: boolean;

  /* Whether or not multiple selections are allowed. */
  multiple?: boolean;

  /* Optional tooltip text to display after the label */
  tooltip?: string;

  /* Whether or not to display errors in a tooltip instead of in a div */
  displayErrorTooltips?: boolean;

  /* Whether or not a request is currently pending. Necessary for the component to know when new results are available. */
  isLoading: boolean;

  /* Callback to perform when the search is executed */
  onSearch: (query: string) => void;

  /* Invoked whenever items are added or removed. Receives an array of the selected options. */
  onChange?: (selected: T[]) => void;

  /* Full set of options, including any pre-selected options. Must either be an array of objects (recommended) or strings. */
  options: T[];

  /* The selected option(s) displayed in the input. Use this prop if you want to control the component via its parent. */
  selected?: T[];
}

// Bypass client-side filtering by returning `true`. Results are already
// filtered by the search endpoint, so no need to do it again.
const filterBy = () => true;

/**
 * Formik-connected <AsyncTypeahead> form control
 */
function AsyncTypeaheadInner<T extends TypeaheadModel>(
  {
    id,
    multiple,
    field,
    options,
    labelKey,
    minLength = 3,
    isLoading = false,
    label,
    required,
    tooltip,
    displayErrorTooltips,
    className,
    onSearch,
    onChange,
    ...rest
  }: ITypeaheadFieldProps<T>,
  ref: React.ForwardedRef<BaseAsyncTypeahead<T>>,
) {
  // Hook into Formik state
  const { errors, touched, values, setFieldValue } = useFormikContext();
  const value = getIn(values, field);
  const error = getIn(errors, field);
  const touch = getIn(touched, field);

  const errorTooltip = error && touch && displayErrorTooltips ? error : undefined;

  // Invoked whenever items are added or removed. Receives an array of the selected options.
  const handleChange = useCallback(
    (selected: T[]) => {
      if (!multiple) {
        setFieldValue(field, head(selected) ?? '');
      } else {
        setFieldValue(field, selected);
      }

      if (onChange !== undefined) {
        onChange(selected);
      }
    },
    [field, multiple, setFieldValue, onChange],
  );

  return (
    <StyledFormGroup
      className={cx({ required: required }, className)}
      data-testid={`typeahead-${field}`}
    >
      {label && <Form.Label htmlFor={`typeahead-${field}`}>{label}</Form.Label>}
      {tooltip && <TooltipIcon toolTipId={`${field}-tooltip`} toolTip={tooltip} />}
      <TooltipWrapper toolTipId={`${field}-error-tooltip}`} toolTip={errorTooltip}>
        <BaseAsyncTypeahead<T>
          ref={ref} // forward the ref to the inner typeahead control to be able to call its methods; e.g. typeahead.clear(), .blur() etc
          id={`typeahead-${field}`}
          inputProps={{
            name: `typeahead-${field}`,
            id: `typeahead-${field}`,
          }}
          clearButton
          options={options}
          multiple={multiple}
          filterBy={filterBy}
          labelKey={labelKey}
          minLength={minLength}
          isLoading={isLoading}
          useCache={false}
          selected={value ? [].concat(value) : []} // always set an array here - works for multiple and single selections
          isInvalid={touch && error}
          onSearch={onSearch}
          onChange={handleChange}
          {...rest}
        >
          {/* hide the search icon when user is interacting with typeahead control */}
          {({ selected, isMenuShown }: TypeaheadManagerChildProps) =>
            isLoading || isMenuShown || selected.length ? null : (
              <div className="rbt-aux">
                <FaSearch size="2.5rem" color="#bcbec5" />
              </div>
            )
          }
        </BaseAsyncTypeahead>
      </TooltipWrapper>
      {!displayErrorTooltips && <DisplayError field={field} />}
    </StyledFormGroup>
  );
}

const StyledFormGroup = styled(Form.Group)`
  // This is the close button of the type-ahead
  button.close {
    font-size: 2.4rem;
  }
`;

// React.forwardRef allows to pass a "ref" to the wrapped AsyncTypeahead to invoke methods on the inner typeahead - e.g. ref.clear(), ref.blur() etc
// See https://www.carlrippon.com/react-forwardref-typescript/

// @ts-ignore
export const AsyncTypeahead = React.forwardRef(AsyncTypeaheadInner) as <T extends TypeaheadModel>(
  props: ITypeaheadFieldProps<T> & { ref?: React.ForwardedRef<BaseAsyncTypeahead<T>> },
) => ReturnType<typeof AsyncTypeaheadInner>;
