import noop from 'lodash/noop';
import { createContext, useState } from 'react';

import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';

export interface IProjectState {
  project?: ApiGen_Concepts_Project;
  setProject: (project: ApiGen_Concepts_Project) => void;
}

export const ProjectStateContext = createContext<IProjectState>({
  project: undefined,
  setProject: noop,
});

export const ProjectContextProvider = (props: {
  children?: any;
  initialProject?: ApiGen_Concepts_Project;
}) => {
  const [project, setProject] = useState<ApiGen_Concepts_Project | undefined>(props.initialProject);

  return (
    <ProjectStateContext.Provider value={{ project, setProject }}>
      {props.children}
    </ProjectStateContext.Provider>
  );
};
