import { IPaginateRequest } from '@/hooks/pims-api/interfaces/IPaginateRequest';

export const PAGINATION_MARGIN_PAGES = 3;
export const PAGINATION_MAX_PAGES = 9;

/**
 * Converts the filter into a paginated filter.
 *
 * @template T
 * @param {number} page
 * @param {number} [quantity]
 * @param {(string | string[])} [sort]
 * @param {T} [filter]
 * @returns {IPaginateRequest<T>}
 */
export const toFilteredApiPaginateParams = <T extends object = {}>(
  page: number,
  quantity?: number,
  sort?: string | string[],
  filter?: T,
): IPaginateRequest<T> => {
  const apiPaginateParams = {
    ...filter,
    page: page + 1,
    quantity: quantity,
    sort: sort,
  };
  return apiPaginateParams as IPaginateRequest<T>;
};
