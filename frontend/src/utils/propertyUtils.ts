import { IAddress } from 'interfaces';

/**
 * The pidFormatter is used to format the specified PID value
 * @param {string} pid This is the target PID to be formatted
 */
export const pidFormatter = (pid?: string) => {
  if (!!pid) {
    let result = pid.padStart(9, '0');
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
 * Provides a formatted address as a string.
 * @param address Address object from property.
 * @returns Civic address string value.
 */
export const formatAddress = (address?: IAddress) => {
  const values = [
    address?.streetAddress1 ?? '',
    address?.streetAddress2 ?? '',
    address?.streetAddress3 ?? '',
    address?.province ?? '',
  ];
  return values.join(' ') + (address?.postal ? ', ' + (address?.postal ?? '') : '');
};
