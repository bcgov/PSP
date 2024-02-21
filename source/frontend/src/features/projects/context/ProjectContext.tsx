import { noop } from 'lodash';
import * as React from 'react';

import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';

export interface IProjectState {
  project?: ApiGen_Concepts_Project;
  setProject: (project: ApiGen_Concepts_Project) => void;
}

export const ProjectStateContext = React.createContext<IProjectState>({
  project: undefined,
  setProject: noop,
});

export const ProjectContextProvider = (props: {
  children?: any;
  initialProject?: ApiGen_Concepts_Project;
}) => {
  const [project, setProject] = React.useState<ApiGen_Concepts_Project | undefined>(
    props.initialProject,
  );

  return (
    <ProjectStateContext.Provider value={{ project, setProject }}>
      {props.children}
    </ProjectStateContext.Provider>
  );
};
