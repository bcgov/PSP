export type SortDirection = 'desc' | 'asc' | undefined;

export type TableSort<T extends object = object> = {
  // key (properties from the data object), value = sort direction
  [key in keyof T]?: SortDirection;
};
