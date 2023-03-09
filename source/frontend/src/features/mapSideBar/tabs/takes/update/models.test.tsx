import { Api_Take } from 'models/api/Take';

import { TakeModel } from './models';
import { emptyTake } from './TakesUpdateForm';

describe('take model tests', () => {
  it("converts all false it values to 'false'", () => {
    const apiTake: Api_Take = {
      ...emptyTake,
      isLicenseToConstruct: false,
      isNewRightOfWay: false,
      isSection16: false,
      isStatutoryRightOfWay: false,
      isSurplusSeverance: false,
    };
    const takeModel = new TakeModel(apiTake);
    expect(takeModel.isLicenseToConstruct).toBe('false');
    expect(takeModel.isNewRightOfWay).toBe('false');
    expect(takeModel.isSection16).toBe('false');
    expect(takeModel.isStatutoryRightOfWay).toBe('false');
    expect(takeModel.isSurplusSeverance).toBe('false');
  });

  it("converts all true it values to 'true'", () => {
    const apiTake: Api_Take = {
      ...emptyTake,
      isLicenseToConstruct: true,
      isNewRightOfWay: true,
      isSection16: true,
      isStatutoryRightOfWay: true,
      isSurplusSeverance: true,
    };
    const takeModel = new TakeModel(apiTake);
    expect(takeModel.isLicenseToConstruct).toBe('true');
    expect(takeModel.isNewRightOfWay).toBe('true');
    expect(takeModel.isSection16).toBe('true');
    expect(takeModel.isStatutoryRightOfWay).toBe('true');
    expect(takeModel.isSurplusSeverance).toBe('true');
  });
  it('sets all undefined areas to 0', () => {
    const apiTake: Api_Take = {
      ...emptyTake,
      section16Area: null,
      newRightOfWayArea: null,
      surplusSeveranceArea: null,
      statutoryRightOfWayArea: null,
      licenseToConstructArea: null,
    };
    const takeModel = new TakeModel(apiTake);
    expect(takeModel.section16Area).toBe(0);
    expect(takeModel.newRightOfWayArea).toBe(0);
    expect(takeModel.surplusSeveranceArea).toBe(0);
    expect(takeModel.statutoryRightOfWayArea).toBe(0);
    expect(takeModel.licenseToConstructArea).toBe(0);
  });

  it('sets all area units to the unit from the backend', () => {
    const apiTake: Api_Take = {
      ...emptyTake,
      areaUnitTypeCode: 'FEET2',
    };
    const takeModel = new TakeModel(apiTake);
    expect(takeModel.section16AreaUnitTypeCode).toBe('FEET2');
    expect(takeModel.newRightOfWayAreaUnitTypeCode).toBe('FEET2');
    expect(takeModel.surplusSeveranceAreaUnitTypeCode).toBe('FEET2');
    expect(takeModel.statutoryRightOfWayAreaUnitTypeCode).toBe('FEET2');
    expect(takeModel.licenseToConstructAreaUnitTypeCode).toBe('FEET2');
  });

  describe('translating the model to the api format', () => {
    it('converts all areas to their m2 equivalents', () => {
      const apiTake: Api_Take = {
        ...emptyTake,
        section16Area: 1,
        newRightOfWayArea: 2,
        surplusSeveranceArea: 3,
        statutoryRightOfWayArea: 4,
        licenseToConstructArea: 5,
      };
      const takeModel = new TakeModel(apiTake);
      takeModel.section16AreaUnitTypeCode = 'M2';
      takeModel.newRightOfWayAreaUnitTypeCode = 'FEET2';
      takeModel.surplusSeveranceAreaUnitTypeCode = 'HA';
      takeModel.statutoryRightOfWayAreaUnitTypeCode = 'ACRE';
      takeModel.licenseToConstructAreaUnitTypeCode = 'ACRE';

      const actualApiTake = takeModel.toApi();
      expect(actualApiTake.areaUnitTypeCode).toBe('M2');
      expect(actualApiTake.section16Area).toBe(1);
      expect(actualApiTake.newRightOfWayArea).toBe(0.18580608);
      expect(actualApiTake.surplusSeveranceArea).toBe(30000);
      expect(actualApiTake.statutoryRightOfWayArea).toBe(16187.4256896);
      expect(actualApiTake.licenseToConstructArea).toBe(20234.282112);
    });
  });
});
