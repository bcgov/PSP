import { IPropertyEvaluation } from 'interfaces';
import moment, { Moment } from 'moment';
import { generateMultiSortCriteria, resolveSortCriteriaFromUrl } from 'utils';

import {
  getCurrentFiscal,
  getCurrentFiscalYear,
  getMostRecentAppraisal,
  getMostRecentEvaluation,
  isPositiveNumberOrZero,
} from '.';

describe('Is Positive or Zero', () => {
  it('Should return false, undefined', () => {
    expect(isPositiveNumberOrZero(undefined)).toBeFalsy();
  });

  it('Should return false, null', () => {
    expect(isPositiveNumberOrZero(null)).toBeFalsy();
  });

  it('Should return false, empty string', () => {
    expect(isPositiveNumberOrZero('')).toBeFalsy();
  });

  it('Should return false, white space', () => {
    expect(isPositiveNumberOrZero(' ')).toBeFalsy();
  });

  it('Should return false, string', () => {
    expect(isPositiveNumberOrZero('test')).toBeFalsy();
  });

  it('Should return false, negative number string', () => {
    expect(isPositiveNumberOrZero('-1')).toBeFalsy();
  });

  it('Should return false, negative number', () => {
    expect(isPositiveNumberOrZero(-1)).toBeFalsy();
  });

  it('Should return true, positive number string', () => {
    expect(isPositiveNumberOrZero('1')).toBeTruthy();
  });

  it('Should return true, positive number', () => {
    expect(isPositiveNumberOrZero(1)).toBeTruthy();
  });

  it('Should return true, zero string', () => {
    expect(isPositiveNumberOrZero('0')).toBeTruthy();
  });

  it('Should return true, zero', () => {
    expect(isPositiveNumberOrZero(0)).toBeTruthy();
  });

  it('Should resolve sort fields', () => {
    // setup
    const input = ['Name asc', 'Organization desc'];
    const expectedOutput = { name: 'asc', organization: 'desc' };

    // act
    const output = resolveSortCriteriaFromUrl(input);

    // assert
    expect(output).toEqual(expectedOutput);
  });

  it('Should generate sort query', () => {
    // setup
    const input: any = { name: 'asc', organization: 'desc' };
    const expectedOutput = ['Name asc', 'Organization desc'];

    // act
    const output = generateMultiSortCriteria(input);

    // assert
    expect(output).toEqual(expectedOutput);
  });

  const createAppraisal = (date: Moment): IPropertyEvaluation => {
    return {
      key: 3,
      value: 123,
      evaluatedOn: date.format('YYYY-MM-DD'),
    };
  };

  const createFiscal = (year: number): IPropertyEvaluation => {
    return {
      key: 2,
      value: 123,
      evaluatedOn: new Date(`{year}-01-01`),
    };
  };

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
      expect(getCurrentFiscal([], 2)).toBeUndefined();
    });
    it('returns the most recent fiscal', () => {
      const fiscals = [];
      fiscals.push(createFiscal(getCurrentFiscalYear() - 1));
      fiscals.push(createFiscal(getCurrentFiscalYear()));
      fiscals.push(createFiscal(getCurrentFiscalYear() - 2));
      expect(getCurrentFiscal(fiscals, 2)).toBe(fiscals[1]);
    });
  });
  describe('getMostRecentEvaluation', () => {
    it('returns undefined if passed an empty array', () => {
      expect(getMostRecentEvaluation([], 1)).toBeUndefined();
    });
    it('returns the most recent evaluation', () => {
      const evaluations = [];
      evaluations.push(createAppraisal(moment('2018-01-01')));
      evaluations.push(createAppraisal(moment('2021-01-01')));
      evaluations.push(createAppraisal(moment('2020-01-01')));
      expect(getMostRecentEvaluation(evaluations, 3)).toBe(evaluations[1]);
    });
  });
});
