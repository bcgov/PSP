import { Api_Project } from 'models/api/Project';

export interface IProjectForm extends ExtendOverride<Api_Project, {}> {}

export const defaultProjectForm: IProjectForm = {
  id: undefined,
};
