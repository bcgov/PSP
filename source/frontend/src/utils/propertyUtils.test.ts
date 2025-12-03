import { describe, it, expect } from 'vitest';
import {
  pidFormatter,
  pidParser,
  pinParser,
  formatApiAddress,
  formatSplitAddress,
  formatBcaAddress,
  formatApiPropertyManagementLease,
  isStrataCommonProperty,
} from './propertyUtils';
import { getMockApiAddress } from '@/mocks/address.mock';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { getMockApiPropertyManagement } from '@/mocks/propertyManagement.mock';
import { Feature, Geometry } from 'geojson';

describe('propertyUtils', () => {
  describe('pidFormatter', () => {
    it('should format a PID with dashes', () => {
      expect(pidFormatter('123456789')).toBe('123-456-789');
    });

    it('should pad and format a PID with less than 9 digits', () => {
      expect(pidFormatter('123')).toBe('000-000-123');
    });

    it('should return an empty string for invalid PID', () => {
      expect(pidFormatter(undefined)).toBe('');
    });
  });

  describe('pidParser', () => {
    it('should parse a formatted PID into a number', () => {
      expect(pidParser('123-456-789')).toBe(123456789);
    });

    it('should parse a numeric PID directly', () => {
      expect(pidParser(123456789)).toBe(123456789);
    });

    it('should return undefined for invalid PID', () => {
      expect(pidParser(null)).toBeUndefined();
    });
  });

  describe('pinParser', () => {
    it('should parse a numeric PIN directly', () => {
      expect(pinParser(123456)).toBe(123456);
    });

    it('should parse a string PIN into a number', () => {
      expect(pinParser('123456')).toBe(123456);
    });

    it('should return undefined for invalid PIN', () => {
      expect(pinParser(null)).toBeUndefined();
    });
  });

  describe('formatApiAddress', () => {
    it('should format an address object into a string', () => {
      const address = getMockApiAddress();
      expect(formatApiAddress(address)).toBe('1234 mock Street N/A Victoria BC, V1V1V1');
    });

    it('should handle null or undefined address', () => {
      expect(formatApiAddress(null)).toBe('');
    });
  });

  describe('formatSplitAddress', () => {
    it('should format address components into a string', () => {
      expect(formatSplitAddress('123 Main St', '', '', 'Vancouver', 'BC', 'V6A1A1')).toBe(
        '123 Main St Vancouver BC, V6A1A1',
      );
    });

    it('should handle missing components gracefully', () => {
      expect(formatSplitAddress('', '', '', '', '', '')).toBe('');
    });
  });

  describe('formatBcaAddress', () => {
    it('should format a BC assessment address into a string', () => {
      const address = {
        UNIT_NUMBER: '4',
        STREET_NUMBER: '123',
        STREET_DIRECTION_PREFIX: '',
        STREET_NAME: 'Main',
        STREET_TYPE: 'St',
        STREET_DIRECTION_SUFFIX: '',
      };
      expect(formatBcaAddress(address)).toBe('4 123 Main St');
    });

    it('should handle undefined address', () => {
      expect(formatBcaAddress(undefined)).toBe('');
    });
  });

  describe('formatApiPropertyManagementLease', () => {
    it('should return "Yes" for active lease with expiry date', () => {
      const property = {
        ...getMockApiPropertyManagement(),
        hasActiveLease: true,
        activeLeaseHasExpiryDate: true,
      };
      expect(formatApiPropertyManagementLease(property)).toBe('Yes');
    });

    it('should return "Yes (No Expiry Date)" for active lease without expiry date', () => {
      const property = {
        ...getMockApiPropertyManagement(),
        hasActiveLease: true,
        activeLeaseHasExpiryDate: false,
      };
      expect(formatApiPropertyManagementLease(property)).toBe('Yes (No Expiry Date)');
    });

    it('should return "No" for no active lease', () => {
      const property = {
        ...getMockApiPropertyManagement(),
        hasActiveLease: false,
        activeLeaseHasExpiryDate: false,
      };
      expect(formatApiPropertyManagementLease(property)).toBe('No');
    });
  });

  describe('isStrataCommonProperty', () => {
    it('should return true for a strata lot feature', () => {
      const feature = {
        properties: {
          PLAN_NUMBER: 'EPS1234',
          PID: null,
          PIN: null,
          OWNER_TYPE: 'Unclassified',
        },
      } as unknown as Feature<Geometry, PMBC_FullyAttributed_Feature_Properties>;
      expect(isStrataCommonProperty(feature)).toBe(true);
    });

    it('should return false if the prefix indicates a non-strata lot', () => {
      const feature = {
        properties: {
          PLAN_NUMBER: 'EPP1234',
          PID: null,
          PIN: null,
          OWNER_TYPE: 'Unclassified',
        },
      } as unknown as Feature<Geometry, PMBC_FullyAttributed_Feature_Properties>;
      expect(isStrataCommonProperty(feature)).toBe(false);
    });

    it('should return false if the there is a pid/pin', () => {
      const feature = {
        properties: {
          PLAN_NUMBER: 'EPS1234',
          PID: 124515,
          PIN: 1,
          OWNER_TYPE: 'Unclassified',
        },
      } as unknown as Feature<Geometry, PMBC_FullyAttributed_Feature_Properties>;
      expect(isStrataCommonProperty(feature)).toBe(false);
    });

    it('should return false if the ownership type is incorrect', () => {
      const feature = {
        properties: {
          PLAN_NUMBER: 'EPS1234',
          PID: 124515,
          PIN: 1,
          OWNER_TYPE: 'Mixed',
        },
      } as unknown as Feature<Geometry, PMBC_FullyAttributed_Feature_Properties>;
      expect(isStrataCommonProperty(feature)).toBe(false);
    });

    it('should return false for undefined feature', () => {
      expect(isStrataCommonProperty(undefined)).toBe(false);
    });
  });
});
