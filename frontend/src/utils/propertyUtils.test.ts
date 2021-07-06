import { EvaluationKeys } from 'constants/evaluationKeys';
import { FiscalKeys } from 'constants/fiscalKeys';
import { PropertyTypes } from 'constants/propertyTypes';
import { IEvaluation, IFiscal } from 'interfaces';
import moment, { Moment } from 'moment';
import { getCurrentFiscalYear } from 'utils';

import { toFlatProperty } from './propertyUtils';
import { getCurrentFiscal, getMostRecentAppraisal, getMostRecentEvaluation } from './utils';

const createAppraisal = (date: Moment): IEvaluation => {
  return {
    key: EvaluationKeys.Appraised,
    value: 123,
    date: date.format('YYYY-MM-DD'),
  };
};

const createFiscal = (year: number): IFiscal => {
  return {
    key: FiscalKeys.Market,
    value: 123,
    fiscalYear: year,
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
      const appraisal = createAppraisal(moment().add(2, 'years'));
      expect(getMostRecentAppraisal([appraisal])).toBeUndefined();
    });
    it('returns the most recent appraisal if there is an appraisal within a year of the current date', () => {
      const appraisal = createAppraisal(moment().add(300, 'days'));
      expect(getMostRecentAppraisal([appraisal])).toBe(appraisal);
    });
    it('returns the most recent appraisal if there are multiple appraisals within a year of the current date', () => {
      const appraisals = [];
      appraisals.push(createAppraisal(moment().subtract(1, 'days')));
      appraisals.push(createAppraisal(moment().subtract(2, 'days')));
      expect(getMostRecentAppraisal(appraisals)).toBe(appraisals[0]);
    });
    it('returns the most recent appraisal if there are multiple appraisals within a year of the disposal date', () => {
      const appraisals = [];
      const disposalDate = moment('2018-01-01');
      appraisals.push(createAppraisal(disposalDate.subtract(1, 'days')));
      appraisals.push(createAppraisal(disposalDate.subtract(2, 'days')));
      appraisals.push(createAppraisal(moment().subtract(1, 'days')));
      expect(getMostRecentAppraisal(appraisals, '2018-01-01')).toBe(appraisals[0]);
    });
    it('returns undefined if there are no appraisals within a year of the disposal date', () => {
      const disposalDate = moment('2018-01-01');
      const appraisal = createAppraisal(disposalDate.add(366, 'days'));
      expect(getMostRecentAppraisal([appraisal], '2018-01-01')).toBeUndefined();
    });
  });
  describe('getCurrentFiscal', () => {
    it('returns undefined if passed an empty array', () => {
      expect(getCurrentFiscal([], FiscalKeys.Market)).toBeUndefined();
    });
    it('returns the most recent fiscal', () => {
      const fiscals = [];
      fiscals.push(createFiscal(getCurrentFiscalYear() - 1));
      fiscals.push(createFiscal(getCurrentFiscalYear()));
      fiscals.push(createFiscal(getCurrentFiscalYear() - 2));
      expect(getCurrentFiscal(fiscals, FiscalKeys.Market)).toBe(fiscals[1]);
    });
  });
  describe('getMostRecentEvaluation', () => {
    it('returns undefined if passed an empty array', () => {
      expect(getMostRecentEvaluation([], EvaluationKeys.Assessed)).toBeUndefined();
    });
    it('returns the most recent evaluation', () => {
      const evaluations = [];
      evaluations.push(createAppraisal(moment('2018-01-01')));
      evaluations.push(createAppraisal(moment('2021-01-01')));
      evaluations.push(createAppraisal(moment('2020-01-01')));
      expect(getMostRecentEvaluation(evaluations, EvaluationKeys.Appraised)).toBe(evaluations[1]);
    });
  });
  describe('toFlatProperty', () => {
    describe('evaluation population', () => {
      it('does not populate parcel evaluation fields if no evaluations present', () => {
        const apiProperty = { ...mockApiProjectParcel, evaluations: [] };
        const property = toFlatProperty(apiProperty as any);
        expect(property.assessedLand).toBe('');
        expect(property.assessedBuilding).toBe('');
      });
      it('does not populate building evaluation fields if no evaluations present', () => {
        const apiProperty = { ...mockApiProjectBuilding, evaluations: [] };
        const property = toFlatProperty(apiProperty as any);
        expect(property.assessedLand).toBe('');
        expect(property.assessedBuilding).toBe('');
      });
      it('populates parcel assessed fields properly', () => {
        const apiProperty = {
          ...mockApiProjectParcel,
          evaluations: [
            { key: EvaluationKeys.Assessed, value: 200, date: new Date() },
            { key: EvaluationKeys.Improvements, value: 300, date: new Date() },
          ] as IEvaluation[],
        };
        const property = toFlatProperty(apiProperty as any);
        expect(property.assessedLand).toBe(200);
        expect(property.assessedBuilding).toBe(300);
      });
      it('populates building assessed fields properly', () => {
        const apiProperty = {
          ...mockApiProjectBuilding,
          evaluations: [
            { key: EvaluationKeys.Assessed, value: 200, date: moment() },
            { key: EvaluationKeys.Improvements, value: 300 },
          ] as IEvaluation[],
        };
        const property = toFlatProperty(apiProperty as any);
        expect(property.assessedLand).toBe('');
        expect(property.assessedBuilding).toBe(200);
      });
      it('does not populate parcel assessed fields if all evaluations are too old', () => {
        const apiProperty = {
          ...mockApiProjectParcel,
          evaluations: [
            {
              key: EvaluationKeys.Assessed,
              value: 200,
              date: moment()
                .add(-1, 'years')
                .toDate(),
            },
            {
              key: EvaluationKeys.Improvements,
              value: 300,
              date: moment()
                .add(-1, 'years')
                .toDate(),
            },
          ] as IEvaluation[],
        };
        const property = toFlatProperty(apiProperty as any);
        expect(property.assessedLand).toBe('');
        expect(property.assessedBuilding).toBe('');
      });
      it('does not populate building assessed fields if all evaluations are too old', () => {
        const apiProperty = {
          ...mockApiProjectBuilding,
          evaluations: [
            {
              key: EvaluationKeys.Assessed,
              value: 200,
              date: moment()
                .add(-1, 'years')
                .toDate(),
            },
            {
              key: EvaluationKeys.Improvements,
              value: 300,
              date: moment()
                .add(-1, 'years')
                .toDate(),
            },
          ] as IEvaluation[],
        };
        const property = toFlatProperty(apiProperty as any);
        expect(property.assessedLand).toBe('');
        expect(property.assessedBuilding).toBe('');
      });
    });
    describe('fiscal population', () => {
      it('does not populate parcel fiscal fields if no fiscals present', () => {
        const apiProperty = { ...mockApiProjectParcel, fiscals: [], evaluations: [] };
        const property = toFlatProperty(apiProperty as any);
        expect(property.market).toBe('');
        expect(property.netBook).toBe('');
      });
      it('does not populate building fiscal fields if no fiscals present', () => {
        const apiProperty = { ...mockApiProjectBuilding, fiscals: [], evaluations: [] };
        const property = toFlatProperty(apiProperty as any);
        expect(property.market).toBe('');
        expect(property.netBook).toBe('');
      });
      it('populates parcel fiscal fields properly', () => {
        const apiProperty = {
          ...mockApiProjectParcel,
          evaluations: [],
          fiscals: [
            { key: FiscalKeys.Market, value: 200, fiscalYear: getCurrentFiscalYear() },
            { key: FiscalKeys.NetBook, value: 300, fiscalYear: getCurrentFiscalYear() },
          ] as IFiscal[],
        };
        const property = toFlatProperty(apiProperty as any);
        expect(property.market).toBe(200);
        expect(property.netBook).toBe(300);
      });
      it('populates building fiscal fields properly', () => {
        const apiProperty = {
          ...mockApiProjectBuilding,
          evaluations: [],
          fiscals: [
            { key: FiscalKeys.Market, value: 200, fiscalYear: getCurrentFiscalYear() },
            { key: FiscalKeys.NetBook, value: 300, fiscalYear: getCurrentFiscalYear() },
          ] as IFiscal[],
        };
        const property = toFlatProperty(apiProperty as any);
        expect(property.market).toBe(200);
        expect(property.netBook).toBe(300);
      });
      it('does not populate parcel fiscal fields if all fiscals are too old', () => {
        const apiProperty = {
          ...mockApiProjectParcel,
          evaluations: [],
          fiscals: [
            {
              key: FiscalKeys.Market,
              value: 200,
              fiscalYear: moment()
                .add(-1, 'years')
                .year(),
            },
            {
              key: FiscalKeys.NetBook,
              value: 300,
              fiscalYear: moment()
                .add(-1, 'years')
                .year(),
            },
          ] as IFiscal[],
        };
        const property = toFlatProperty(apiProperty as any);
        expect(property.market).toBe('');
        expect(property.netBook).toBe('');
      });
      it('does not populate building fiscal fields if all fiscals are too old', () => {
        const apiProperty = {
          ...mockApiProjectBuilding,
          evaluations: [],
          fiscals: [
            {
              key: FiscalKeys.Market,
              value: 200,
              fiscalYear: moment()
                .add(-1, 'years')
                .year(),
            },
            {
              key: FiscalKeys.NetBook,
              value: 300,
              fiscalYear: moment()
                .add(-1, 'years')
                .year(),
            },
          ] as IFiscal[],
        };
        const property = toFlatProperty(apiProperty as any);
        expect(property.market).toBe('');
        expect(property.netBook).toBe('');
      });
    });
  });
});

export const mockApiProjectParcel = {
  id: 1007,
  projectId: '1007',
  propertyType: 'Land',
  parcelId: 87,
  propertyTypeId: PropertyTypes.PARCEL,
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
    classificationId: 4,
    classification: 'Disposed',
    agencyId: 0,
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
    isVisibleToOtherAgencies: false,
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
  propertyTypeId: PropertyTypes.BUILDING,
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
    classificationId: 4,
    classification: 'Disposed',
    agencyId: 0,
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
    isVisibleToOtherAgencies: false,
    createdOn: '2020-07-19T03:52:56.3079875',
    updatedOn: '2020-07-27T19:17:36.3603827',
    rowVersion: 'AAAAAAAAWuU=',
  },
  createdOn: '2020-07-27T19:15:57.8361774',
  updatedOn: '2020-07-27T19:17:36.3603819',
  rowVersion: 'AAAAAAAAWuY=',
};
