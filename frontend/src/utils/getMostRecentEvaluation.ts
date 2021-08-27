import { IPropertyEvaluation } from 'interfaces';
import _ from 'lodash';

/**
 * Get the most recent evaluation matching the passed evaluation type.
 * @param evaluations a list of evaluations belonging to this project
 * @param key only return evaluations matching this key
 */
export const getMostRecentEvaluation = (
  evaluations: IPropertyEvaluation[],
  key: number,
): IPropertyEvaluation | undefined => {
  const mostRecentEvaluation = _.find(_.orderBy(evaluations ?? [], 'date', 'desc'), { key: key });
  return mostRecentEvaluation;
};
