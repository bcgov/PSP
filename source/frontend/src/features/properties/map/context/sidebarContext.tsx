import { FileTypes } from 'constants/fileTypes';
import { findIndex } from 'lodash';
import { Api_File } from 'models/api/File';
import { Api_Project } from 'models/api/Project';
import * as React from 'react';
import { useCallback, useState } from 'react';

export interface TypedFile extends Api_File {
  fileType: FileTypes;
}

export interface ISideBarContext {
  file?: TypedFile;
  setFile: (file?: TypedFile) => void;
  staleFile: boolean;
  setStaleFile: (stale: boolean) => void;
  fileLoading: boolean;
  setFileLoading: (loading: boolean) => void;
  projectLoading: boolean;
  project?: Api_Project;
  setProject: (project?: Api_Project) => void;
  setProjectLoading: (loading: boolean) => void;
  getFilePropertyIndexById: (filePropertyId: number) => number;
  fullWidth: boolean;
  setFullWidth: (fullWidth: boolean) => void;
}

export const SideBarContext = React.createContext<ISideBarContext>({
  file: undefined,
  setFile: (file?: TypedFile) => {
    throw Error('setFile function not defined');
  },
  fileLoading: false,
  setFileLoading: (loading: boolean) => {
    throw Error('setFileLoading function not defined');
  },
  staleFile: false,
  setStaleFile: (stale: boolean) => {
    throw Error('setStaleFile function not defined');
  },
  getFilePropertyIndexById: (filePropertyId: number) => {
    throw Error('setStaleFile function not defined');
  },
  fullWidth: false,
  setFullWidth: (fullWidth: boolean) => {
    throw Error('setFullWidth function not defined');
  },
  setProject: (project?: Api_Project) => {
    throw Error('setProject function not defined');
  },
  projectLoading: false,
  setProjectLoading: (loading: boolean) => {
    throw Error('setProjectLoading function not defined');
  },
});

export const SideBarContextProvider = (props: {
  children: React.ReactChild | React.ReactChild[] | React.ReactNode;
  file?: TypedFile;
  project?: Api_Project;
}) => {
  const [file, setFile] = useState<TypedFile | undefined>(props.file);
  const [project, setProject] = useState<Api_Project | undefined>(props.project);
  const [staleFile, setStaleFile] = useState<boolean>(false);
  const [fileLoading, setFileLoading] = useState<boolean>(false);
  const [projectLoading, setProjectLoading] = useState<boolean>(false);
  const [fullWidth, setFullWidth] = useState<boolean>(false);

  const setFileAndStale = useCallback(
    (file?: TypedFile) => {
      setFile(file);
      setStaleFile(false);
    },
    [setFile, setStaleFile],
  );

  const setProjectInstance = useCallback(
    (project?: Api_Project) => {
      setProject(project);
    },
    [setProject],
  );

  const getFilePropertyIndexById = (filePropertyId: number) =>
    findIndex(file?.fileProperties, fp => fp.id === filePropertyId);

  return (
    <SideBarContext.Provider
      value={{
        setFile: setFileAndStale,
        file: file,
        setFileLoading: setFileLoading,
        fileLoading: fileLoading,
        staleFile,
        setStaleFile,
        getFilePropertyIndexById,
        fullWidth,
        setFullWidth,
        projectLoading,
        setProject: setProjectInstance,
        setProjectLoading: setProjectLoading,
        project: project,
      }}
    >
      {props.children}
    </SideBarContext.Provider>
  );
};
