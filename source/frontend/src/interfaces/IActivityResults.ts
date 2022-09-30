export interface IActivityFilter {
  activityTypeId?: string;
  status: string;
}

export const defaultActivityFilter: IActivityFilter = {
  activityTypeId: undefined,
  status: '',
};
