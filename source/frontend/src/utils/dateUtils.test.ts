import {
  formatUTCDateTime,
  getCurrentFiscalYear,
  prettyFormatDateTime,
  prettyFormatUTCDate,
} from './';

describe('Date utils', () => {
  describe('getCurrentFiscalYear', () => {
    beforeAll(() => {
      vi.useFakeTimers();
    });
    afterAll(() => {
      vi.useRealTimers();
    });

    it('should return fiscal year 2023 for December 2022', () => {
      vi.setSystemTime(new Date('04 Dec 2022 10:00:00 GMT').getTime());
      expect(getCurrentFiscalYear()).toBe(2023);
    });
    it('should return fiscal year 2023 for March 2023', () => {
      vi.setSystemTime(new Date('15 Mar 2023 10:00:00 GMT').getTime());
      expect(getCurrentFiscalYear()).toBe(2023);
    });
  });

  describe('prettyFormatUTCDate', () => {
    it('should return empty string if no input date is supplied', () => {
      expect(prettyFormatUTCDate(null)).toBe('');
    });
    it('should support Date object as parameter', () => {
      expect(prettyFormatUTCDate(new Date('2023-07-31T10:00:00-07:00'))).toBe('Jul 31, 2023');
    });
    it('should format API Date assuming the date was recorded in UTC', () => {
      expect(prettyFormatUTCDate('2023-07-31T17:00:00')).toBe('Jul 31, 2023'); // 5pm UTC = 10am PST
      expect(prettyFormatUTCDate('2023-08-01T02:00:00')).toBe('Jul 31, 2023'); // 2am (next day) UTC = 7pm PST
    });
  });

  describe('prettyFormatDateTime', () => {
    it('should return empty string if no input date is supplied', () => {
      expect(prettyFormatDateTime(null)).toBe('');
    });
    it('should support Date object as parameter', () => {
      expect(prettyFormatDateTime(new Date('2023-07-31T10:00:00-07:00'))).toBe(
        'Jul 31, 2023 10:00 am',
      );
    });
    it('should format API Date assuming the date was recorded in UTC', () => {
      expect(prettyFormatDateTime('2023-07-31T17:00:00')).toBe('Jul 31, 2023 10:00 am'); // 5pm UTC = 10am PST
      expect(prettyFormatDateTime('2023-08-01T02:00:00')).toBe('Jul 31, 2023 07:00 pm'); // 2am (next day) UTC = 7pm PST
    });
  });

  describe('formatUTCDateTime', () => {
    it('should format API Date with default date format', () => {
      expect(formatUTCDateTime('2023-07-31T10:00:00-07:00')).toBe('2023-07-31 10:00 am');
    });
    it('should format API Date with custom format passed in parameters', () => {
      expect(formatUTCDateTime('2023-07-31T10:00:00-07:00', 'MMM D, YYYY')).toBe('Jul 31, 2023');
    });
    it('should support Date object as parameter', () => {
      expect(formatUTCDateTime(new Date('2023-07-31T10:00:00-07:00'))).toBe('2023-07-31 10:00 am');
    });
    it('should return empty string if no input date is supplied', () => {
      expect(formatUTCDateTime(null)).toBe('');
    });
    it('should format API Date assuming the date was recorded in UTC', () => {
      expect(formatUTCDateTime('2023-07-31T17:00:00', 'YYYY-MM-DD')).toBe('2023-07-31'); // 5pm UTC = 10am PST
      expect(formatUTCDateTime('2023-08-01T02:00:00', 'YYYY-MM-DD')).toBe('2023-07-31'); // 2am (next day) UTC = 7pm PST
    });
  });
});
