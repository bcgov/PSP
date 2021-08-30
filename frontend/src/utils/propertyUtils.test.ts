import { IPropertyEvaluation } from 'interfaces';
import moment, { Moment } from 'moment';
import { getCurrentFiscalYear } from 'utils';

import { getCurrentFiscal, getMostRecentAppraisal, getMostRecentEvaluation } from '.';

const createEvaluation = (date: Moment): IPropertyEvaluation => {
  return {
    id: 1,
    propertyId: 1,
    key: 3,
    value: 123,
    evaluatedOn: date.format('YYYY-MM-DD'),
  };
};

const createFiscal = (date: Moment): IPropertyEvaluation => {
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
      fiscals.push(createFiscal(moment(`${getCurrentFiscalYear() - 1}-01-01`)));
      fiscals.push(createFiscal(moment(`${getCurrentFiscalYear()}-01-01`)));
      fiscals.push(createFiscal(moment(`${getCurrentFiscalYear() - 2}-01-01`)));
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
      expect(getMostRecentEvaluation(evaluations, 3)).toBe(evaluations[1]);
    });
  });
});
