import { ApiGen_CodeTypes_AreaUnitTypes } from '@/models/api/generated/ApiGen_CodeTypes_AreaUnitTypes';
import { getAreaUnit } from './convertUtils';

describe('Convert utils', () => {
  describe('getAreaUnit', () => {
    it('should return "m2" when null', () => {
      expect(getAreaUnit(null)).toBe('m2');
    });

    it('should return "m2" when undefined', () => {
      expect(getAreaUnit(null)).toBe('m2');
    });

    it('should return "ac" when area type "ACRE"', () => {
      expect(getAreaUnit(ApiGen_CodeTypes_AreaUnitTypes.ACRE)).toBe('ac');
    });

    it('should return "ha" when area type "HA"', () => {
      expect(getAreaUnit(ApiGen_CodeTypes_AreaUnitTypes.HA)).toBe('ha');
    });

    it('should return "sq ft" when area type "FEET2"', () => {
      expect(getAreaUnit(ApiGen_CodeTypes_AreaUnitTypes.FEET2)).toBe('sq ft');
    });

    it('should return "m2" when area type "M2"', () => {
      expect(getAreaUnit(ApiGen_CodeTypes_AreaUnitTypes.M2)).toBe('m2');
    });

    it('should return "m2" when area type NOT IN ENUM', () => {
      expect(getAreaUnit('XX')).toBe('m2');
    });
  });
});
