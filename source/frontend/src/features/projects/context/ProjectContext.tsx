import { noop } from 'lodash';
import { Api_Project } from 'models/api/Project';
import * as React from 'react';

export interface IProjectState {
  project?: Api_Project;
  setProject: (project: Api_Project) => void;
}

export const ProjectStateContext = React.createContext<IProjectState>({
  project: undefined,
  setProject: noop,
});

export const ProjectContextProvider = (props: { children?: any; initialProject?: Api_Project }) => {
  const [project, setProject] = React.useState<Api_Project | undefined>(props.initialProject);

  return (
    <ProjectStateContext.Provider value={{ project, setProject }}>
      {props.children}
    </ProjectStateContext.Provider>
  );
};
