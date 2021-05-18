import { toFlatProject } from '../projectConverter';
import { createSlice, createAction, PayloadAction } from '@reduxjs/toolkit';
import { IProject, IApiProject } from '..';

export interface IProjectWrapper {
  project?: IProject;
}

export const saveProject = createAction<IProject>('saveProject');
export const clearProject = createAction('clearProject');

/**
 * Slice to handle storage of project worflow information for all project disposal steps.
 */
const projectSlice = createSlice({
  name: 'project',
  initialState: {} as IProjectWrapper,
  reducers: {},
  extraReducers: (builder: any) => {
    // note that redux-toolkit uses immer to prevent state from being mutated.
    builder.addCase(saveProject, (state: IProjectWrapper, action: PayloadAction<IApiProject>) => {
      const project = action.payload as IApiProject;
      return { project: toFlatProject(project) };
    });
    builder.addCase(clearProject, () => {
      return {};
    });
  },
});

export default projectSlice;
