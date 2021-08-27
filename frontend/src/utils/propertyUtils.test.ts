import { PropertyClassificationTypes } from 'constants/propertyClassificationTypes';
import { PropertyTypes } from 'constants/propertyTypes';
import { IPropertyEvaluation } from 'interfaces';
import moment, { Moment } from 'moment';
import { getCurrentFiscalYear } from 'utils';

import { getCurrentFiscal, getMostRecentAppraisal, getMostRecentEvaluation } from '.';

const createEvaluation = (date: Moment): IPropertyEvaluation => {
  return {
    id: 1,
    propertyId: 1,
    key: 1,
    value: 123,
    evaluatedOn: date.format('YYYY-MM-DD'),
  };
};

describe('property util function tests', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });
  describe('getMostRecentAppraisal', () => {
    it('returns undefined if passed an empty list of evaluations', () => {
      expect(getMostRecentAppraisal([])).toBeUndefined();
    });
    it('returns undefined if there are no appraisals within a year of the current date', () => {
      const evaluation = createEvaluation(moment().add(2, 'years'));
      expect(getMostRecentAppraisal([evaluation])).toBeUndefined();
    });
    it('returns the most recent appraisal if there is an appraisal within a year of the current date', () => {
      const evaluation = createEvaluation(moment().add(300, 'days'));
      expect(getMostRecentAppraisal([evaluation])).toBe(evaluation);
    });
    it('returns the most recent appraisal if there are multiple appraisals within a year of the current date', () => {
      const appraisals = [];
      appraisals.push(createEvaluation(moment().subtract(1, 'days')));
      appraisals.push(createEvaluation(moment().subtract(2, 'days')));
      expect(getMostRecentAppraisal(appraisals)).toBe(appraisals[0]);
    });
    it('returns the most recent appraisal if there are multiple appraisals within a year of the disposal date', () => {
      const appraisals = [];
      const disposalDate = moment('2018-01-01');
      appraisals.push(createEvaluation(disposalDate.subtract(1, 'days')));
      appraisals.push(createEvaluation(disposalDate.subtract(2, 'days')));
      appraisals.push(createEvaluation(moment().subtract(1, 'days')));
      expect(getMostRecentAppraisal(appraisals, '2018-01-01')).toBe(appraisals[0]);
    });
    it('returns undefined if there are no appraisals within a year of the disposal date', () => {
      const disposalDate = moment('2018-01-01');
      const appraisal = createEvaluation(disposalDate.add(366, 'days'));
      expect(getMostRecentAppraisal([appraisal], '2018-01-01')).toBeUndefined();
    });
  });
  describe('getCurrentFiscal', () => {
    it('returns undefined if passed an empty array', () => {
      expect(getCurrentFiscal([], 1)).toBeUndefined();
    });
    it('returns the most recent fiscal', () => {
      const fiscals = [];
      fiscals.push(createEvaluation(moment(`${getCurrentFiscalYear() - 1}-01-01`)));
      fiscals.push(createEvaluation(moment(`${getCurrentFiscalYear()}-01-01`)));
      fiscals.push(createEvaluation(moment(`${getCurrentFiscalYear() - 2}-01-01`)));
      expect(getCurrentFiscal(fiscals, 1)).toBe(fiscals[1]);
    });
  });
  describe('getMostRecentEvaluation', () => {
    it('returns undefined if passed an empty array', () => {
      expect(getMostRecentEvaluation([], 1)).toBeUndefined();
    });
    it('returns the most recent evaluation', () => {
      const evaluations = [];
      evaluations.push(createEvaluation(moment('2018-01-01')));
      evaluations.push(createEvaluation(moment('2021-01-01')));
      evaluations.push(createEvaluation(moment('2020-01-01')));
      expect(getMostRecentEvaluation(evaluations, 1)).toBe(evaluations[1]);
    });
  });
});

export const mockApiProjectParcel = {
  id: 1007,
  projectId: '1007',
  propertyType: 'Land',
  parcelId: 87,
  propertyTypeId: PropertyTypes.Land,
  parcel: {
    pid: '000-359-173',
    landArea: 34.74,
    landLegalDescription:
      'Lot 1, Plan 35726, Section 1, Nanaimo District, Except Plan VIP 66138 and Except Plan VIP 66141',
    zoning: '',
    zoningPotential: '',
    evaluations: [
      {
        parcelId: 87,
        date: '2018-01-01T00:00:00',
        key: 'Assessed',
        value: 10048000,
        firm: '',
        createdOn: '0001-01-01T00:00:00',
        updatedOn: '2020-07-27T19:20:44.5984243',
        rowVersion: 'AAAAAAAAWyU=',
      },
    ],
    fiscals: [
      {
        parcelId: 87,
        fiscalYear: getCurrentFiscalYear() - 1,
        key: 'NetBook',
        value: 0,
        createdOn: '2020-07-19T03:52:56.3079867',
        rowVersion: 'AAAAAAAAEF4=',
      },
      {
        parcelId: 87,
        fiscalYear: getCurrentFiscalYear(),
        key: 'NetBook',
        value: 1,
        createdOn: '0001-01-01T00:00:00',
        updatedOn: '2020-07-27T19:20:44.5984245',
        rowVersion: 'AAAAAAAAWyY=',
      },
      {
        parcelId: 87,
        fiscalYear: getCurrentFiscalYear(),
        key: 'Market',
        value: 1,
        createdOn: '0001-01-01T00:00:00',
        updatedOn: '2020-07-27T19:20:44.5984247',
        rowVersion: 'AAAAAAAAWyc=',
      },
    ],
    buildings: [],
    id: 87,
    projectNumber: 'SPP-10006',
    description: 'Nanaimo Main Campus except Wakesiah Ave \u0026 Res.',
    classificationId: PropertyClassificationTypes.Disposed,
    classification: 'Disposed',
    organizationId: 0,
    address: {
      id: 374,
      line1: '900 Fifth Street',
      administrativeArea: 'Nanaimo',
      provinceId: 'BC',
      province: 'British Columbia',
      postal: 'V9R 5S5',
      createdOn: '2020-07-19T03:52:56.3079873',
      rowVersion: 'AAAAAAAAEFs=',
    },
    latitude: 49.1556433,
    longitude: -123.9681175,
    isSensitive: false,
    isVisibleToOtherOrganizations: false,
    createdOn: '2020-07-19T03:52:56.3079875',
    updatedOn: '2020-07-27T19:17:36.3603827',
    rowVersion: 'AAAAAAAAWuU=',
  },
  createdOn: '2020-07-27T19:15:57.8361774',
  updatedOn: '2020-07-27T19:17:36.3603819',
  rowVersion: 'AAAAAAAAWuY=',
};

export const mockApiProjectBuilding = {
  id: 1007,
  projectId: '1007',
  propertyType: 'Building',
  propertyTypeId: PropertyTypes.Building,
  buildingId: 87,
  building: {
    pid: '000-359-173',
    landArea: 34.74,
    landLegalDescription:
      'Lot 1, Plan 35726, Section 1, Nanaimo District, Except Plan VIP 66138 and Except Plan VIP 66141',
    zoning: '',
    zoningPotential: '',
    evaluations: [
      {
        parcelId: 87,
        date: '2018-01-01T00:00:00',
        key: 'Assessed',
        value: 10048000,
        firm: '',
        createdOn: '0001-01-01T00:00:00',
        updatedOn: '2020-07-27T19:20:44.5984243',
        rowVersion: 'AAAAAAAAWyU=',
      },
    ],
    fiscals: [
      {
        parcelId: 87,
        fiscalYear: getCurrentFiscalYear() - 1,
        key: 'NetBook',
        value: 0,
        createdOn: '2020-07-19T03:52:56.3079867',
        rowVersion: 'AAAAAAAAEF4=',
      },
      {
        parcelId: 87,
        fiscalYear: getCurrentFiscalYear(),
        key: 'NetBook',
        value: 1,
        createdOn: '0001-01-01T00:00:00',
        updatedOn: '2020-07-27T19:20:44.5984245',
        rowVersion: 'AAAAAAAAWyY=',
      },
      {
        parcelId: 87,
        fiscalYear: getCurrentFiscalYear(),
        key: 'Market',
        value: 1,
        createdOn: '0001-01-01T00:00:00',
        updatedOn: '2020-07-27T19:20:44.5984247',
        rowVersion: 'AAAAAAAAWyc=',
      },
    ],
    buildings: [],
    id: 87,
    projectNumber: 'SPP-10006',
    description: 'Nanaimo Main Campus except Wakesiah Ave \u0026 Res.',
    classificationId: PropertyClassificationTypes.Disposed,
    classification: 'Disposed',
    organizationId: 0,
    address: {
      id: 374,
      line1: '900 Fifth Street',
      administrativeArea: 'Nanaimo',
      provinceId: 'BC',
      province: 'British Columbia',
      postal: 'V9R 5S5',
      createdOn: '2020-07-19T03:52:56.3079873',
      rowVersion: 'AAAAAAAAEFs=',
    },
    latitude: 49.1556433,
    longitude: -123.9681175,
    isSensitive: false,
    isVisibleToOtherOrganizations: false,
    createdOn: '2020-07-19T03:52:56.3079875',
    updatedOn: '2020-07-27T19:17:36.3603827',
    rowVersion: 'AAAAAAAAWuU=',
  },
  createdOn: '2020-07-27T19:15:57.8361774',
  updatedOn: '2020-07-27T19:17:36.3603819',
  rowVersion: 'AAAAAAAAWuY=',
};
