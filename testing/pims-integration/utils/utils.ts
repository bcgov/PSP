export const generateFileName = (fileType: string): string => {
  const now = new Date();

  const timestamp =
    now.getFullYear().toString() +
    String(now.getMonth() + 1).padStart(2, '0') +
    String(now.getDate()).padStart(2, '0') +
    String(now.getHours()).padStart(2, '0') +
    String(now.getMinutes()).padStart(2, '0') +
    String(now.getSeconds()).padStart(2, '0');

  return `${fileType}-${timestamp}`;
};
export const normalize = (value: string | null): string => {
  return value == null ? '' : String(value).trim();
};

export const formatApiDate = (apiDate: string | null | undefined): string => {
  if (!apiDate) {
    return '';
  }

  const date = new Date(apiDate);

  return date.toLocaleDateString('en-CA', {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
    timeZone: 'UTC', // avoids timezone surprises
  });
};

export const formatApiBoolean = (value: boolean | null | undefined): string => {
  if (value == null) {
    return '';
  }

  return value ? 'Yes' : 'No';
};
