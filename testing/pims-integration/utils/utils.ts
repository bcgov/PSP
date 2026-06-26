
    export const normalize = (value: string | null): string =>{
    return value == null ? '' : String(value).trim();
  }

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
  }

  export const formatApiBoolean = (value: boolean | null | undefined): string => {
    if (value == null) {
      return '';
    }

    return value ? 'Yes' : 'No';
  }
