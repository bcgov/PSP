export const formatMoney = (value?: number | '' | null): string => {
  const formatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  });
  return formatter.format(value || 0);
};

export const formatNumber = (
  value: number,
  minFractionDigits = 0,
  maxFractionDigits = 0,
): string => {
  const formatter = new Intl.NumberFormat('en-US', {
    style: 'decimal',
    minimumFractionDigits: minFractionDigits,
    maximumFractionDigits: maxFractionDigits,
    useGrouping: true, // use thousands separators - 1,000,000
  });
  return formatter.format(value);
};
