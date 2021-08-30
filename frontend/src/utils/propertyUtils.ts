/**
 * The pidFormatter is used to format the specified PID value
 * @param {string} pid This is the target PID to be formatted
 */
export const pidFormatter = (pid: string) => {
  if (!!pid) {
    let result = pid.padStart(9, '0');
    const regex = /(\d\d\d)[\s-]?(\d\d\d)[\s-]?(\d\d\d)/;
    const format = result.match(regex);
    if (format !== null && format.length === 4) {
      result = `${format[1]}-${format[2]}-${format[3]}`;
    }
    return result;
  }
  return pid;
};
