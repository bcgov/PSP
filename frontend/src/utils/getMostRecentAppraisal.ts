import { IPropertyEvaluation } from 'interfaces';
import _ from 'lodash';
import moment, { Moment } from 'moment';
import { getMostRecentEvaluation } from 'utils';

/**
 * Get the most recent appraisal, restricted to within one year of either the current year or the year the project was disposed on.
 * @param evaluations all evaluations for the property.
 * @param disposedOn the date the project was disposed on, may be undefined.
 */
export const getMostRecentAppraisal = (
  evaluations: IPropertyEvaluation[],
  disposedOn?: Date | string | Moment,
): IPropertyEvaluation | undefined => {
  let targetDate = moment();
  if (disposedOn) {
    targetDate = moment(disposedOn, 'YYYY-MM-DD');
  }
  const evaluationsForYear = _.filter(evaluations ?? [], evaluation => {
    return (
      moment
        .duration(moment(evaluation.evaluatedOn, 'YYYY-MM-DD').diff(targetDate))
        .abs()
        .asYears() < 1
    );
  });
  return getMostRecentEvaluation(evaluationsForYear, 3);
};
