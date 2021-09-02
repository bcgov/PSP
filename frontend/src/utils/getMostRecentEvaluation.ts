import { IPropertyEvaluation } from 'interfaces';
import _ from 'lodash';
import moment from 'moment';

/**
 * Get the most recent evaluation matching the passed evaluation type.
 * @param evaluations a list of evaluations belonging to this project
 * @param key only return evaluations matching this key
 */
export const getMostRecentEvaluation = (
  evaluations: IPropertyEvaluation[],
  key: number,
): IPropertyEvaluation | undefined => {
  const mostRecentEvaluation = _.find(
    _.orderBy(evaluations ?? [], evaluation => moment(evaluation.evaluatedOn), 'desc'),
    { key: key },
  );
  return mostRecentEvaluation;
};
