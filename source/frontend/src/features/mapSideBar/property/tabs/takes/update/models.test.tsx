import { ApiGen_Concepts_Take } from '@/models/api/generated/ApiGen_Concepts_Take';
import { toTypeCode } from '@/utils/formUtils';

import { TakeModel } from './models';
import { emptyTake } from './TakesUpdateForm';

describe('take model tests', () => {
  it("converts all false it values to 'false'", () => {
    const apiTake: ApiGen_Concepts_Take = {
      ...emptyTake,
      isAcquiredForInventory: false,
      isNewLicenseToConstruct: false,
      isNewHighwayDedication: false,
      isNewLandAct: false,
      isNewInterestInSrw: false,
      isThereSurplus: false,
      isLeasePayable: false,
    };
    const takeModel = new TakeModel(apiTake);
    expect(takeModel.isAcquiredForInventory).toBe('false');
    expect(takeModel.isNewLicenseToConstruct).toBe('false');
    expect(takeModel.isNewHighwayDedication).toBe('false');
    expect(takeModel.isNewLandAct).toBe('false');
    expect(takeModel.isNewInterestInSrw).toBe('false');
    expect(takeModel.isThereSurplus).toBe('false');
    expect(takeModel.isLeasePayable).toBe('false');
  });

  it("converts all true it values to 'true'", () => {
    const apiTake: ApiGen_Concepts_Take = {
      ...emptyTake,
      isAcquiredForInventory: true,
      isNewLicenseToConstruct: true,
      isNewHighwayDedication: true,
      isNewLandAct: true,
      isNewInterestInSrw: true,
      isThereSurplus: true,
      isLeasePayable: true,
    };
    const takeModel = new TakeModel(apiTake);
    expect(takeModel.isAcquiredForInventory).toBe('true');
    expect(takeModel.isNewLicenseToConstruct).toBe('true');
    expect(takeModel.isNewHighwayDedication).toBe('true');
    expect(takeModel.isNewLandAct).toBe('true');
    expect(takeModel.isNewInterestInSrw).toBe('true');
    expect(takeModel.isThereSurplus).toBe('true');
    expect(takeModel.isLeasePayable).toBe('true');
  });
  it('sets all undefined areas to 0', () => {
    const apiTake: ApiGen_Concepts_Take = {
      ...emptyTake,
      landActArea: null,
      newHighwayDedicationArea: null,
      surplusArea: null,
      statutoryRightOfWayArea: null,
      licenseToConstructArea: null,
      leasePayableArea: null,
    };
    const takeModel = new TakeModel(apiTake);
    expect(takeModel.landActArea).toBe(0);
    expect(takeModel.newHighwayDedicationArea).toBe(0);
    expect(takeModel.surplusArea).toBe(0);
    expect(takeModel.statutoryRightOfWayArea).toBe(0);
    expect(takeModel.licenseToConstructArea).toBe(0);
    expect(takeModel.leasePayableArea).toBe(0);
  });

  it('sets all area units to the unit from the backend', () => {
    const apiTake: ApiGen_Concepts_Take = {
      ...emptyTake,
      areaUnitTypeCode: toTypeCode('FEET2'),
    };
    const takeModel = new TakeModel(apiTake);
    expect(takeModel.landActAreaUnitTypeCode).toBe('FEET2');
    expect(takeModel.newHighwayDedicationAreaUnitTypeCode).toBe('FEET2');
    expect(takeModel.surplusAreaUnitTypeCode).toBe('FEET2');
    expect(takeModel.statutoryRightOfWayAreaUnitTypeCode).toBe('FEET2');
    expect(takeModel.licenseToConstructAreaUnitTypeCode).toBe('FEET2');
    expect(takeModel.leasePayableAreaUnitTypeCode).toBe('FEET2');
  });

  describe('translating the model to the api format', () => {
    it('converts all areas to their m2 equivalents', () => {
      const apiTake: ApiGen_Concepts_Take = {
        ...emptyTake,
        landActArea: 1,
        newHighwayDedicationArea: 2,
        surplusArea: 3,
        statutoryRightOfWayArea: 4,
        licenseToConstructArea: 5,
      };
      const takeModel = new TakeModel(apiTake);
      takeModel.landActAreaUnitTypeCode = 'M2';
      takeModel.newHighwayDedicationAreaUnitTypeCode = 'FEET2';
      takeModel.surplusAreaUnitTypeCode = 'HA';
      takeModel.statutoryRightOfWayAreaUnitTypeCode = 'ACRE';
      takeModel.licenseToConstructAreaUnitTypeCode = 'ACRE';
      takeModel.leasePayableAreaUnitTypeCode = 'ACRE';

      const actualApiTake = takeModel.toApi();
      expect(actualApiTake.areaUnitTypeCode?.id).toBe('M2');
      expect(actualApiTake.landActArea).toBe(1);
      expect(actualApiTake.newHighwayDedicationArea).toBe(0.18580608);
      expect(actualApiTake.surplusArea).toBe(30000);
      expect(actualApiTake.statutoryRightOfWayArea).toBe(16187.4256896);
      expect(actualApiTake.licenseToConstructArea).toBe(20234.282112);
      expect(actualApiTake.licenseToConstructArea).toBe(20234.282112);
    });
  });
});
