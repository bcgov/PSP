// Generic Params
export type IPaginateRequest<T> = T & {
  page: number;
  quantity?: number;
  sort?: string | string[];
};
