import { AxiosError, AxiosResponse } from 'axios';
import { SelectOption } from 'components/common/form';
import { TableSort } from 'components/Table/TableSort';
import { FormikProps, getIn } from 'formik';
import _, { isEmpty, isNull, isUndefined, keys, lowerFirst, startCase } from 'lodash';
import moment, { Moment } from 'moment-timezone';
import { hideLoading, showLoading } from 'react-redux-loading-bar';
import { ILookupCode } from 'store/slices/lookupCodes';
import { logError, logRequest, logSuccess } from 'store/slices/network/networkSlice';

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

/**
 * Convert the specified 'input' value into a decimal or undefined.
 * @param input The string value to convert to a decimal.
 */
export const decimalOrUndefined = (input: string): number | undefined => {
  return input !== '' && input !== undefined ? parseInt(input, 10) : undefined;
};

/**
 * Convert the specified 'input' value into a float or undefined.
 * @param input The string value to convert to a float.
 */
export const floatOrUndefined = (input: string): number | undefined => {
  return input !== '' && input !== undefined ? parseFloat(input) : undefined;
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
  return value === null || value === undefined || value.trim() === '';
};

/** used for filters that need to display the string value of a parent organization organization */
export const mapLookupCodeWithParentString = (
  code: ILookupCode,
  /** the list of lookup codes to look for parent */
  options: ILookupCode[],
): SelectOption => ({
  label: code.name,
  value: code.id.toString(),
  code: code.code,
  parentId: code.parentId,
  parent: options.find((a: ILookupCode) => a.id.toString() === code.parentId?.toString())?.name,
});

/** used for inputs that need to display the string value of a parent organization organization */
export const mapSelectOptionWithParent = (
  code: SelectOption,
  /** the list of lookup codes to look for parent */
  options: SelectOption[],
): SelectOption => ({
  label: code.label,
  value: code.value.toString(),
  code: code.code,
  parentId: code.parentId,
  parent: options.find((a: SelectOption) => a.value.toString() === code.parentId?.toString())
    ?.label,
});

type FormikMemoProps = {
  formikProps: FormikProps<any>;
  field: string;
  disabled?: boolean;
  options?: SelectOption[];
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
  }: FormikMemoProps,
  {
    formikProps: prevProps,
    field: prevField,
    disabled: prevDisabled,
    options: prevOptions,
  }: FormikMemoProps,
) => {
  return !(
    currField !== prevField ||
    currentDisabled !== prevDisabled ||
    currentOptions !== prevOptions ||
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
  dispatch: Function,
  actionType: string,
  axiosPromise: Promise<AxiosResponse<ResponseType>>,
  skipErrorLogCodes?: number[],
): Promise<ResponseType> => {
  dispatch(logRequest(actionType));
  dispatch(showLoading());
  return axiosPromise
    .then((response: AxiosResponse<ResponseType>) => {
      dispatch(logSuccess({ name: actionType, status: response.status }));
      dispatch(hideLoading());
      return response?.data;
    })
    .catch((axiosError: AxiosError) => {
      if (
        !skipErrorLogCodes ||
        (axiosError?.response?.status && !skipErrorLogCodes.includes(axiosError?.response?.status))
      ) {
        dispatch(
          logError({
            name: actionType,
            status: axiosError?.response?.status,
            error: axiosError ?? {},
          }),
        );
      }
      throw axiosError;
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

  return keys(sort).map(key => `${startCase(key).replaceAll(' ', '')} ${sort[key]}`);
};

/**
 * Convert sort query string to TableSort config
 * ['Name desc'] = {name: 'desc'}
 */
export const resolveSortCriteriaFromUrl = (input: string[]): TableSort<any> | {} => {
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

/**
 * get the fiscal year (ending in) based on the current date.
 */
export const getCurrentFiscalYear = (): number => {
  const now = moment();
  return now.month() >= 4 ? now.add(1, 'years').year() : now.year();
};

export const formatDate = (date?: string | Date | Moment) => {
  return !!date ? moment(date).format('YYYY-MM-DD') : '';
};

export const prettyFormatDate = (date?: string | Date | Moment) => {
  return !!date ? moment(date).format('MMM D, YYYY') : '';
};

export const prettyFormatDateTime = (date?: string | Date | Moment) => {
  return !!date ? moment.utc(date).local().format('MMM D, YYYY hh:mm a') : '';
};

/**
 * Format the passed string date assuming the date was recorded in UTC (as is the case with the pims API server).
 * Returns a date formatted for display in the current time zone of the user.
 * @param date utc date/time string.
 */
export const formatApiDateTime = (date?: string | Date | Moment) => {
  if (typeof date === 'string') return moment.utc(date).local().format('YYYY-MM-DD hh:mm a');
  return !!date ? moment.utc(date).local().format('YYYY-MM-DD hh:mm a') : '';
};

/**
 * Get the current date time in the UTC timezone. This allows the frontend to create timestamps that are compatible with timestamps created by the API.
 */
export const generateUtcNowDateTime = () =>
  moment(new Date()).utc().format('YYYY-MM-DDTHH:mm:ss.SSSSSSS');

/**
 * Returns true only if the passed mouse event occurred within the last 500ms, or the mouse event is null.
 */
export const isMouseEventRecent = (timeStamp?: number) =>
  !!timeStamp && timeStamp >= (document?.timeline?.currentTime ?? 0) - 500;

/**
 * postalCodeFormatter takes the specified postal code and formats it with a space in the middle
 * @param {string} postal The target postal to be formatted
 */
export const postalCodeFormatter = (postal: string) => {
  const regex = /([a-zA-z][0-9][a-zA-z])[\s-]?([0-9][a-zA-z][0-9])/;
  const format = postal.match(regex);
  if (format !== null && format.length === 3) {
    postal = `${format[1]} ${format[2]}`;
  }
  return postal.toUpperCase();
};

/**
 * Using the administrative areas code set, find the matching municipality returned by the parcel layer, if present.
 * @param administrativeAreas the full list from the administrative areas code set.
 * @param layerMunicipality the municipality returned by the layer.
 */
export const getAdminAreaFromLayerData = (
  administrativeAreas: ILookupCode[],
  layerMunicipality: string,
) => {
  let administrativeArea = _.find(administrativeAreas, { name: layerMunicipality });
  if (administrativeArea) {
    return administrativeArea;
  }
  if (!!layerMunicipality?.length) {
    const splitLayerMunicipality = layerMunicipality.split(',');
    if (splitLayerMunicipality.length === 2) {
      const formattedLayerMunicipality = `${splitLayerMunicipality[1].trim()} ${splitLayerMunicipality[0].trim()}`;
      let match = _.find(administrativeAreas, { name: formattedLayerMunicipality });
      if (!match) {
        match = _.find(administrativeAreas, { name: splitLayerMunicipality[0].trim() });
      }
      return match;
    }
  }
};

/**
 * @param pageIndex the current index of the page, 0 based.
 * @param pageSize the size of the current page.
 * @param data all of the data that is being paged.
 * @returns the current page from the passed data array.
 */
export const getPage = (pageIndex: number, pageSize: number, data: any[]) => {
  const pageStart = (pageIndex ?? 0) * pageSize;
  return data.slice(pageStart, pageStart + pageSize);
};

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
