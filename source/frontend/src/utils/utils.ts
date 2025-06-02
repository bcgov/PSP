import { AxiosResponse } from 'axios';
import { FormikProps, getIn } from 'formik';
import { isEmpty, isNull, isUndefined, lowerFirst, startCase } from 'lodash';
import { hideLoading, showLoading } from 'react-redux-loading-bar';

import { SelectOption } from '@/components/common/form';
import { TableSort } from '@/components/Table/TableSort';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { logRequest, logSuccess } from '@/store/slices/network/networkSlice';

/**
 * Removes a trailing slash from a string.
 * Useful when creating nested URLs or routes.
 * @param value The input string
 * @returns The string without trailing slash
 */
export function stripTrailingSlash(value: string) {
  return value ? value.replace(/\/$/, '') : value;
}

/**
 * Rounds the supplied number to a certain number of decimal places
 * @param value The number to round
 * @param decimalPlaces The number of decimal places. Defaults to 2
 * @returns The rounded number
 */
export function round(value: number, decimalPlaces = 2): number {
  const factorOfTen = Math.pow(10, decimalPlaces);
  return Math.round((value + Number.EPSILON) * factorOfTen) / factorOfTen;
}

export const enumFromValue = <T extends Record<string, string>>(val: string, _enum: T) => {
  const enumName = (Object.keys(_enum) as Array<keyof T>).find(k => _enum[k] === val);
  if (!enumName) return undefined;
  return _enum[enumName];
};

/**
 * Determine if the specified 'input' value is a positive number of zero.
 * @param input The value to evaluate.
 * @returns True if the value is a positive number or zero, false otherwise.
 */
export const isPositiveNumberOrZero = (input: string | number | undefined | null) => {
  if (isNull(input) || isUndefined(input)) {
    return false;
  }

  if (typeof input === 'string' && isEmpty(input.trim())) {
    return false;
  }

  return !isNaN(Number(input)) && Number(input) > -1;
};

export const isNullOrWhitespace = (value: string | null | undefined): boolean => {
  return !exists(value) || value.trim() === '';
};

type FormikMemoProps = {
  formikProps: FormikProps<any>;
  field: string;
  disabled?: boolean;
  options?: SelectOption[];
  required?: boolean;
} & any;
/**
 * Common use memo function prevents renders unless associated field data has been changed.
 * Essential for performance on large forms. Adapted from:
 * https://jaredpalmer.com/formik/docs/api/fastfield
 * @param param0 params from previous render
 * @param param1 params from current render
 */
export const formikFieldMemo = (
  {
    formikProps: currentProps,
    field: currField,
    disabled: currentDisabled,
    options: currentOptions,
    required: currentRequired,
  }: FormikMemoProps,
  {
    formikProps: prevProps,
    field: prevField,
    disabled: prevDisabled,
    options: prevOptions,
    required: prevRequired,
  }: FormikMemoProps,
) => {
  return !(
    currField !== prevField ||
    currentDisabled !== prevDisabled ||
    currentOptions !== prevOptions ||
    currentRequired !== prevRequired ||
    getIn(currentProps.values, prevField) !== getIn(prevProps.values, prevField) ||
    getIn(currentProps.errors, prevField) !== getIn(prevProps.errors, prevField) ||
    getIn(currentProps.touched, prevField) !== getIn(prevProps.touched, prevField) ||
    Object.keys(currentProps).length !== Object.keys(prevProps).length ||
    currentProps.isSubmitting !== prevProps.isSubmitting
  );
};

/** Provides default redux boilerplate applicable to most axios redux actions
 * @param dispatch Dispatch function
 * @param actionType All dispatched GenericNetworkActions will use this action type.
 * @param axiosPromise The result of an axios.get, .put, ..., call.
 */
export const handleAxiosResponse = <ResponseType>(
  // eslint-disable-next-line @typescript-eslint/ban-types
  dispatch: Function,
  actionType: string,
  axiosPromise: Promise<AxiosResponse<ResponseType>>,
): Promise<AxiosResponse<ResponseType>> => {
  dispatch(logRequest(actionType));
  dispatch(showLoading());
  return axiosPromise
    .then((response: AxiosResponse<ResponseType>) => {
      dispatch(logSuccess({ name: actionType, status: response.status }));
      dispatch(hideLoading());
      return response;
    })
    .finally(() => {
      dispatch(hideLoading());
    });
};

/**
 * convert table sort config to api sort query
 * {name: 'desc} = ['Name desc']
 */
export const generateMultiSortCriteria = (sort: TableSort<any>) => {
  if (!sort) {
    return '';
  }

  return Object.keys(sort).map(key => `${startCase(key).replaceAll(' ', '')} ${sort[key]}`);
};

/**
 * Convert sort query string to TableSort config
 * ['Name desc'] = {name: 'desc'}
 */
export const resolveSortCriteriaFromUrl = (
  input: string[],
): TableSort<any> | Record<string, never> => {
  if (isEmpty(input)) {
    return {};
  }

  return input.reduce((acc: any, sort: string) => {
    const fields = sort.split(' ');
    if (fields.length !== 2) {
      return { ...acc };
    }

    return { ...acc, [lowerFirst(fields[0])]: fields[1] };
  }, {});
};

export function unique<T>(value: T, index: number, array: T[]) {
  return array.indexOf(value) === index;
}

/**
 * Meant to be used as the function passed during a conditional statement to remove null or undefined entries.
 * example. myArray.filter(exists);
 *          if(exists(a?.b?.c)){}
 */
export function exists<T>(value: T | null | undefined): value is T {
  return value === (value ?? !value);
}

/**
 * Returns true id an identifier belongs to an existing entry on the backend
 * @param value the paraneter to be assessed
 * @returns true if valid, false otherwise
 */
export function isValidId(value: number | null | undefined): value is number {
  return exists(value) && !isNaN(value) && value !== 0;
}

export function isValidString(value: string | null | undefined): value is string {
  return exists(value) && value.length > 0;
}

export function isValidIsoDateTime(value: string | null | undefined): value is string {
  return isValidString(value) && value !== EpochIsoDateTime;
}

export function isString(value: unknown): value is string {
  return typeof value === 'string';
}

export function isNumber(value: unknown): value is number {
  return typeof value === 'number';
}

export function truncateName(name: string, length: number): string {
  if (name.length > length) {
    return name.slice(0, length) + '...';
  } else {
    return name;
  }
}

export function firstOrNull<T>(array: Array<T> | null): T | null {
  if (exists(array) && array.length > 0) {
    return array[0];
  }
  return null;
}

/**
 * Add a simple retry wrapper to help avoid chunk errors in deployed pims application, recursively calls promise based on attemptsLeft parameter.
 * @param lazyComponent
 * @param attemptsLeft
 */
export default function componentLoader(lazyComponent: Promise<any>, attemptsLeft: number) {
  return new Promise<any>((resolve, reject) => {
    lazyComponent.then(resolve).catch((error: any) => {
      setTimeout(() => {
        if (attemptsLeft === 0) {
          reject(error);
          return;
        }
        componentLoader(lazyComponent, attemptsLeft - 1).then(resolve, reject);
      }, 500);
    });
  });
}

export function capitalizeFirstLetter(value: string) {
  return value.charAt(0).toUpperCase() + value.slice(1);
}

export function relationshipTypeToPathName(
  relationshipType: ApiGen_CodeTypes_DocumentRelationType,
) {
  switch (relationshipType) {
    case ApiGen_CodeTypes_DocumentRelationType.Templates:
      return 'template';

    case ApiGen_CodeTypes_DocumentRelationType.ResearchFiles:
      return 'research';

    case ApiGen_CodeTypes_DocumentRelationType.AcquisitionFiles:
      return 'acquisition';

    case ApiGen_CodeTypes_DocumentRelationType.Leases:
      return 'lease';

    case ApiGen_CodeTypes_DocumentRelationType.Projects:
      return 'project';

    case ApiGen_CodeTypes_DocumentRelationType.ManagementActivities:
      return 'activities';

    case ApiGen_CodeTypes_DocumentRelationType.ManagementFiles:
      return 'management';

    case ApiGen_CodeTypes_DocumentRelationType.DispositionFiles:
      return 'disposition';

    case ApiGen_CodeTypes_DocumentRelationType.Properties:
      return 'property';
  }
}
