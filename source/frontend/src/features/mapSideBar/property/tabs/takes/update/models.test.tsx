import { Api_Take } from '@/models/api/Take';

import { TakeModel } from './models';
import { emptyTake } from './TakesUpdateForm';

describe('take model tests', () => {
  it("converts all false it values to 'false'", () => {
    const apiTake: Api_Take = {
      ...emptyTake,
      isLicenseToConstruct: false,
      isNewRightOfWay: false,
      isLandAct: false,
      isStatutoryRightOfWay: false,
      isSurplus: false,
    };
    const takeModel = new TakeModel(apiTake);
    expect(takeModel.isLicenseToConstruct).toBe('false');
    expect(takeModel.isNewRightOfWay).toBe('false');
    expect(takeModel.isLandAct).toBe('false');
    expect(takeModel.isStatutoryRightOfWay).toBe('false');
    expect(takeModel.isSurplus).toBe('false');
  });

  it("converts all true it values to 'true'", () => {
    const apiTake: Api_Take = {
      ...emptyTake,
      isLicenseToConstruct: true,
      isNewRightOfWay: true,
      isLandAct: true,
      isStatutoryRightOfWay: true,
      isSurplus: true,
    };
    const takeModel = new TakeModel(apiTake);
    expect(takeModel.isLicenseToConstruct).toBe('true');
    expect(takeModel.isNewRightOfWay).toBe('true');
    expect(takeModel.isLandAct).toBe('true');
    expect(takeModel.isStatutoryRightOfWay).toBe('true');
    expect(takeModel.isSurplus).toBe('true');
  });
  it('sets all undefined areas to 0', () => {
    const apiTake: Api_Take = {
      ...emptyTake,
      landActArea: null,
      newRightOfWayArea: null,
      surplusArea: null,
      statutoryRightOfWayArea: null,
      licenseToConstructArea: null,
    };
    const takeModel = new TakeModel(apiTake);
    expect(takeModel.landActArea).toBe(0);
    expect(takeModel.newRightOfWayArea).toBe(0);
    expect(takeModel.surplusArea).toBe(0);
    expect(takeModel.statutoryRightOfWayArea).toBe(0);
    expect(takeModel.licenseToConstructArea).toBe(0);
  });

  it('sets all area units to the unit from the backend', () => {
    const apiTake: Api_Take = {
      ...emptyTake,
      areaUnitTypeCode: 'FEET2',
    };
    const takeModel = new TakeModel(apiTake);
    expect(takeModel.landActAreaUnitTypeCode).toBe('FEET2');
    expect(takeModel.newRightOfWayAreaUnitTypeCode).toBe('FEET2');
    expect(takeModel.surplusAreaUnitTypeCode).toBe('FEET2');
    expect(takeModel.statutoryRightOfWayAreaUnitTypeCode).toBe('FEET2');
    expect(takeModel.licenseToConstructAreaUnitTypeCode).toBe('FEET2');
  });

  describe('translating the model to the api format', () => {
    it('converts all areas to their m2 equivalents', () => {
      const apiTake: Api_Take = {
        ...emptyTake,
        landActArea: 1,
        newRightOfWayArea: 2,
        surplusArea: 3,
        statutoryRightOfWayArea: 4,
        licenseToConstructArea: 5,
      };
      const takeModel = new TakeModel(apiTake);
      takeModel.landActAreaUnitTypeCode = 'M2';
      takeModel.newRightOfWayAreaUnitTypeCode = 'FEET2';
      takeModel.surplusAreaUnitTypeCode = 'HA';
      takeModel.statutoryRightOfWayAreaUnitTypeCode = 'ACRE';
      takeModel.licenseToConstructAreaUnitTypeCode = 'ACRE';

      const actualApiTake = takeModel.toApi();
      expect(actualApiTake.areaUnitTypeCode).toBe('M2');
      expect(actualApiTake.landActArea).toBe(1);
      expect(actualApiTake.newRightOfWayArea).toBe(0.18580608);
      expect(actualApiTake.surplusArea).toBe(30000);
      expect(actualApiTake.statutoryRightOfWayArea).toBe(16187.4256896);
      expect(actualApiTake.licenseToConstructArea).toBe(20234.282112);
    });
  });
});
