import { Api_Project, defaultProject } from 'models/api/Project';

export interface IProjectForm extends ExtendOverride<Api_Project, {}> {}

export const defaultProjectForm: IProjectForm = {
  ...defaultProject,
};
