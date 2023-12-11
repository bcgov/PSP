import cx from 'classnames';
import { getIn, useFormikContext } from 'formik';
import MultiselectBase from 'multiselect-react-dropdown';
import React, { forwardRef } from 'react';
import Form from 'react-bootstrap/Form';
import { FaWindowClose } from 'react-icons/fa';
import styled from 'styled-components';

import TooltipIcon from '../TooltipIcon';
import TooltipWrapper from '../TooltipWrapper';
import { DisplayError } from './DisplayError';

// multiselect-react-dropdown doesn't export this interface so it is inlined here
interface IMultiselectBaseProps<T, U> {
  options: Array<T>;
  disablePreSelectedValues?: boolean;
  selectedValues?: Array<U>;
  isObject?: boolean;
  displayValue?: string;
  showCheckbox?: boolean;
  selectionLimit?: any;
  placeholder?: string;
  groupBy?: string;
  loading?: boolean;
  style?: object;
  emptyRecordMsg?: string;
  onSelect?: (selectedList: any, selectedItem: any) => void;
  onRemove?: (selectedList: any, selectedItem: any) => void;
  onSearch?: (value: string) => void;
  onKeyPressFn?: (event: any, value: string) => void;
  closeIcon?: string;
  singleSelect?: boolean;
  caseSensitiveSearch?: boolean;
  id?: string;
  closeOnSelect?: boolean;
  avoidHighlightFirstOption?: boolean;
  hidePlaceholder?: boolean;
  showArrow?: boolean;
  keepSearchTerm?: boolean;
  customCloseIcon?: React.ReactNode | string;
  customArrow?: any;
  disable?: boolean;
  className?: string;
}

/**
 * Public interface for a Formik connected Multiselect component
 * NOTE: It hides two internal multiselect properties - "id", "selectedValues" as they are set internally here.
 */
export interface IMultiselectProps<T, U>
  extends Omit<IMultiselectBaseProps<T, U>, 'id' | 'selectedValues'> {
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
  /** (Optional) a function that returns a list of options that match the passed selected items */
  selectFunction?: (allOptions: T[], selected: U[]) => T[];
}

export function MultiselectInner<T, U>(
  props: IMultiselectProps<T, U>,
  ref: React.ForwardedRef<MultiselectBase>,
) {
  const {
    field,
    label,
    formGroupClassName,
    required,
    tooltip,
    displayErrorTooltips,
    style,
    onSelect,
    onRemove,
    selectFunction,
    ...rest
  } = props;

  const { values, setFieldValue, setFieldTouched, errors, touched } = useFormikContext();
  const formikValues = getIn(values, field) as Array<U>;
  const selectedValues = selectFunction ? selectFunction(rest.options, formikValues) : undefined;
  const error = getIn(errors, field);
  const touch = getIn(touched, field);
  const errorTooltip = error && touch && displayErrorTooltips ? error : undefined;

  // merge internal styles with the ones passed in props
  const mergedStyle = { ...defaultStyle, ...style };

  // Allow external consumers to handle onSelect, onRemove events via callbacks
  const onChange = (selectedList: T[], func?: Function) => {
    setFieldValue(field, selectedList);
    setFieldTouched(field, true);
    if (typeof func === 'function') {
      func(selectedList);
    }
  };

  return (
    <Form.Group className={cx({ required: required }, formGroupClassName)}>
      {label && <Form.Label htmlFor={`multiselect-${field}_input`}>{label}</Form.Label>}
      {tooltip && <TooltipIcon toolTipId={`${field}-tooltip`} toolTip={tooltip} />}

      <TooltipWrapper toolTipId={`${field}-error-tooltip}`} toolTip={errorTooltip}>
        <MultiselectBase
          {...rest}
          ref={ref as any}
          id={`multiselect-${field}`}
          selectedValues={selectedValues ?? formikValues}
          style={mergedStyle}
          customCloseIcon={<CloseIcon size={16} />}
          onSelect={selectedList => onChange(selectedList, onSelect)}
          onRemove={selectedList => onChange(selectedList, onRemove)}
        />
      </TooltipWrapper>

      {!displayErrorTooltips && <DisplayError field={field} />}
    </Form.Group>
  );
}

/**
 * Formik-connected <Multiselect> form control
 */
export const Multiselect = forwardRef(MultiselectInner) as <T, U>(
  props: IMultiselectProps<T, U> & { ref?: React.ForwardedRef<MultiselectBase> },
) => ReturnType<typeof MultiselectInner>;

const defaultStyle = {
  chips: {
    background: '#F2F2F2',
    borderRadius: '4px',
    color: 'black',
    fontSize: '16px',
    marginRight: '1em',
    whiteSpace: 'normal',
  },
  multiselectContainer: {
    width: 'auto',
    color: 'black',
  },
  searchBox: {
    background: 'white',
    border: '1px solid #606060',
  },
};

const CloseIcon = styled(FaWindowClose)`
  margin-left: 0.5rem;
  cursor: pointer;
  fill: ${props => props.theme.css.textColor};
  &:hover {
    fill: ${props => props.theme.css.dangerColor};
  }
`;
