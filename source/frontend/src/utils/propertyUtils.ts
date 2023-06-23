import { IAddress } from '@/interfaces';
import { Api_Address } from '@/models/api/Address';
import { IBcAssessmentSummary } from '@/models/layers/bcAssesment';

/**
 * The pidFormatter is used to format the specified PID value
 * @param {string} pid This is the target PID to be formatted
 */
export const pidFormatter = (pid?: string) => {
  if (!!pid) {
    let result = pid.toString().padStart(9, '0');
    const regex = /(\d\d\d)[\s-]?(\d\d\d)[\s-]?(\d\d\d)/;
    const format = result.match(regex);
    if (format !== null && format.length === 4) {
      result = `${format[1]}-${format[2]}-${format[3]}`;
    }
    return result;
  }
  return '';
};

/**
 * The pidParser is used to return a numeric pid value from a formatted pid.
 * @param {string} pid This is the target PID to be parsed
 */
export const pidParser = (pid?: string | number | null): number | undefined => {
  if (typeof pid === 'number') {
    return pid;
  }
  if (pid !== undefined && pid !== null) {
    const regex = /(\d\d\d)[\s-]?(\d\d\d)[\s-]?(\d\d\d)/;
    const format = pid.match(regex);
    if (format !== null && format.length === 4) {
      return parseInt(`${format[1]}${format[2]}${format[3]}`);
    } else {
      return parseInt(pid);
    }
  }
  return undefined;
};

/**
 * Provides a formatted address as a string.
 * @param address Address object from property.
 * @returns Civic address string value.
 */
export const formatApiAddress = (address?: Api_Address) => {
  const values = [
    address?.streetAddress1 ?? '',
    address?.streetAddress2 ?? '',
    address?.streetAddress3 ?? '',
    address?.municipality ?? '',
    address?.province?.code ?? '',
  ];
  return (
    values.filter(text => text !== '').join(' ') +
    (address?.postal ? ', ' + (address?.postal ?? '') : '')
  );
};

/**
 * Provides a formatted street address as a string.
 * Combines streetAddress1, streetAddress2 and streetAddress3 into a single string.
 *
 * @param address Address object from property.
 * @returns Civic address string value.
 */
export const formatStreetAddress = (address?: IAddress) => {
  const values = [
    address?.streetAddress1 ?? '',
    address?.streetAddress2 ?? '',
    address?.streetAddress3 ?? '',
  ];
  return values.filter(text => text !== '').join(' ');
};

/**
 * Provides a formatted street address as a string.
 * Combines data from a BC assessment address into a formatted address string.
 *
 * @param address Address object from bc assessment.
 * @returns Civic address string value.
 */
export const formatBcaAddress = (address?: IBcAssessmentSummary['ADDRESSES'][0]) =>
  [
    address?.UNIT_NUMBER,
    address?.STREET_NUMBER,
    address?.STREET_DIRECTION_PREFIX,
    address?.STREET_NAME,
    address?.STREET_TYPE,
    address?.STREET_DIRECTION_SUFFIX,
  ]
    .filter(a => !!a)
    .join(' ');
