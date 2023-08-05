import moment, { Moment } from 'moment-timezone';

/**
 * get the fiscal year (ending in) based on the current date.
 */
export const getCurrentFiscalYear = (): number => {
  const now = moment();
  return now.month() >= 4 ? now.add(1, 'years').year() : now.year();
};

export const prettyFormatDate = (date?: string | Date | Moment | null) => {
  return formatApiDateTime(date, 'MMM D, YYYY');
};

export const prettyFormatDateTime = (date?: string | Date | Moment | null) => {
  return formatApiDateTime(date, 'MMM D, YYYY hh:mm a');
};

/**
 * Format the passed string date assuming the date was recorded in UTC (as is the case with the pims API server).
 * Returns a date formatted for display in the current time zone of the user.
 * @param date utc date/time string.
 */
export const formatApiDateTime = (
  date?: string | Date | Moment | null,
  format: string = 'YYYY-MM-DD hh:mm a',
) => {
  if (typeof date === 'string') return moment.utc(date).local().format(format);
  return !!date ? moment.utc(date).local().format(format) : '';
};
