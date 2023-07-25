import { SelectOption } from '@/components/common/form';

export const generateFiscalYearOptions = (minYear: number, maxYear: number) => {
  const options: SelectOption[] = [];
  for (let year: number = minYear; year < maxYear; year++) {
    const yearString = year.toString();
    const yearEndString = (year + 1).toString();
    options.push({
      label: `${yearString}-${yearEndString.substring(
        yearEndString.length - 2,
        yearEndString.length,
      )}`,
      value: year,
    });
  }
  return options;
};
