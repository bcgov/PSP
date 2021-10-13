import { ILease } from 'interfaces';

/**
 * return all of the person tenant names and organization tenant names of this lease
 * @param lease
 */
export const getAllNames = (lease?: ILease) => {
  const allNames = (lease?.persons?.map(p => p.fullName) ?? []).concat(
    lease?.organizations?.map(p => p.name) ?? [],
  );
  return allNames.join(', ');
};
