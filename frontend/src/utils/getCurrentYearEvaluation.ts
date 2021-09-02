import { IPropertyEvaluation } from 'interfaces';
import moment from 'moment';
import { getMostRecentEvaluation } from 'utils';

const currentYear = moment().year();

/**
 * Get the most recent evaluation matching the current year and passed evaluation type.
 * @param evaluations a list of evaluations belonging to this project
 * @param key only return evaluations matching this key
 */
export const getCurrentYearEvaluation = (
  evaluations: IPropertyEvaluation[],
  key: number,
): IPropertyEvaluation | undefined => {
  const currentYearEvaluations = (evaluations ?? []).filter(
    (evaluation: IPropertyEvaluation) =>
      moment(evaluation.evaluatedOn, 'YYYY-MM-DD').year() === currentYear,
  );
  return getMostRecentEvaluation(currentYearEvaluations, key);
};
