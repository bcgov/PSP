import moment, { Moment } from 'moment-timezone';

import { exists, isValidIsoDateTime } from './utils';

/**
 * Gets the fiscal year (ending in) based on the current date.
 */
export const getCurrentFiscalYear = (): number => {
  const now = moment();
  return now.month() >= 4 ? now.add(1, 'years').year() : now.year();
};

/**
 * Formats the passed string date (in local timezone) with custom date format: 'MMM D, YYYY'
 * @param date date/time string in LOCAL timezone (not in UTC).
 * @returns A string representing the input date in the supplied date/time format
 */
export const prettyFormatDate = (date?: string | Date | Moment | null) => {
  if (typeof date === 'string') {
    return isValidIsoDateTime(date) ? moment(date).format('MMM D, YYYY') : '';
  }
  return exists(date) ? moment(date).format('MMM D, YYYY') : '';
};

/**
 * Formats the passed string UTC date with custom date format: 'MMM D, YYYY'
 * @param date UTC date/time string.
 * @returns A string representing the input date in the supplied date/time format
 */
export const prettyFormatUTCDate = (date?: string | Date | Moment | null) => {
  return formatUTCDateTime(date, 'MMM D, YYYY');
};

/**
 * Formats the passed string date with custom date format: 'MMM D, YYYY hh:mm a'
 * @param date UTC date/time string.
 * @returns A string representing the input date in the supplied date/time format
 */
export const prettyFormatDateTime = (date?: string | Date | Moment | null) => {
  return formatUTCDateTime(date, 'MMM D, YYYY hh:mm a');
};

/**
 * Formats the passed string date assuming the date was recorded in UTC (as is the case with the pims API server).
 * Returns a date formatted for display in the current time zone of the user.
 * @param date UTC date/time string.
 * @param format (Optional) Date time string format to use. Default value: 'YYYY-MM-DD hh:mm a'
 * @returns A string representing the input date in the supplied date/time format
 */
export const formatUTCDateTime = (
  date?: string | Date | Moment | null,
  format = 'YYYY-MM-DD hh:mm a',
) => {
  if (typeof date === 'string' && isValidIsoDateTime(date)) {
    return moment.utc(date).local().format(format);
  }
  return exists(date) ? moment.utc(date).local().format(format) : '';
};
