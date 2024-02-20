import { findIndex } from 'lodash';
import * as React from 'react';
import { useCallback, useEffect, useState } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { FileTypes } from '@/constants/fileTypes';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { exists } from '@/utils';
import { getLatLng } from '@/utils/mapPropertyUtils';

export interface TypedFile extends ApiGen_Concepts_File {
  fileType: FileTypes;
  projectId?: number | null;
  productId?: number | null;
}

export interface ISideBarContext {
  file?: TypedFile;
  setFile: (file?: TypedFile) => void;
  staleFile: boolean;
  setStaleFile: (stale: boolean) => void;
  fileLoading: boolean;
  setFileLoading: (loading: boolean) => void;
  resetFilePropertyLocations: () => void;
  projectLoading: boolean;
  project?: ApiGen_Concepts_Project;
  setProject: (project?: ApiGen_Concepts_Project) => void;
  setProjectLoading: (loading: boolean) => void;
  getFilePropertyIndexById: (filePropertyId: number) => number;
  fullWidth: boolean;
  setFullWidth: (fullWidth: boolean) => void;

  lastUpdatedBy: Api_LastUpdatedBy | null;
  setLastUpdatedBy: (lastUpdatedBy: Api_LastUpdatedBy | null) => void;
  staleLastUpdatedBy: boolean;
  setStaleLastUpdatedBy: (stale: boolean) => void;
}

export const SideBarContext = React.createContext<ISideBarContext>({
  file: undefined,
  setFile: () => {
    throw Error('setFile function not defined');
  },
  fileLoading: false,
  setFileLoading: () => {
    throw Error('setFileLoading function not defined');
  },
  resetFilePropertyLocations: () => {
    throw Error('resetFilePropertyLocations function not defined');
  },
  staleFile: false,
  setStaleFile: () => {
    throw Error('setStaleFile function not defined');
  },
  getFilePropertyIndexById: () => {
    throw Error('setStaleFile function not defined');
  },
  fullWidth: false,
  setFullWidth: () => {
    throw Error('setFullWidth function not defined');
  },
  setProject: () => {
    throw Error('setProject function not defined');
  },
  projectLoading: false,
  setProjectLoading: () => {
    throw Error('setProjectLoading function not defined');
  },
  lastUpdatedBy: null,
  setLastUpdatedBy: () => {
    throw Error('setLastUpdatedBy function not defined');
  },
  staleLastUpdatedBy: false,
  setStaleLastUpdatedBy: () => {
    throw Error('setStaleLastUpdatedBy function not defined');
  },
});

export const SideBarContextProvider = (props: {
  children: React.ReactChild | React.ReactChild[] | React.ReactNode;
  file?: TypedFile;
  project?: ApiGen_Concepts_Project;
  lastUpdatedBy?: Api_LastUpdatedBy;
}) => {
  const [file, setFile] = useState<TypedFile | undefined>(props.file);
  const [project, setProject] = useState<ApiGen_Concepts_Project | undefined>(props.project);
  const [staleFile, setStaleFile] = useState<boolean>(false);
  const [lastUpdatedBy, setLastUpdatedBy] = useState<Api_LastUpdatedBy | null>(
    props.lastUpdatedBy ?? null,
  );
  const [staleLastUpdatedBy, setStaleLastUpdatedBy] = useState<boolean>(false);
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

  const setLastUpdatedByAndStale = useCallback(
    (lastUpdatedBy: Api_LastUpdatedBy | null) => {
      setLastUpdatedBy(lastUpdatedBy);
      setStaleLastUpdatedBy(false);
    },
    [setLastUpdatedBy, setStaleLastUpdatedBy],
  );

  const setProjectInstance = useCallback(
    (project?: ApiGen_Concepts_Project) => {
      setProject(project);
    },
    [setProject],
  );

  const getFilePropertyIndexById = (filePropertyId: number) =>
    findIndex(file?.fileProperties, fp => fp.id === filePropertyId);

  const { setFilePropertyLocations } = useMapStateMachine();

  const fileProperties = file?.fileProperties;

  const resetFilePropertyLocations = useCallback(() => {
    if (exists(fileProperties)) {
      const propertyLocations = fileProperties
        .map(x => getLatLng(x.property?.location))
        .filter(exists);

      setFilePropertyLocations(propertyLocations);
    } else {
      setFilePropertyLocations([]);
    }
  }, [fileProperties, setFilePropertyLocations]);

  useEffect(() => {
    resetFilePropertyLocations();
  }, [resetFilePropertyLocations]);

  useEffect(() => {
    if (staleLastUpdatedBy) {
      setLastUpdatedByAndStale(lastUpdatedBy);
    }
  }, [lastUpdatedBy, setLastUpdatedByAndStale, staleLastUpdatedBy]);

  return (
    <SideBarContext.Provider
      value={{
        setFile: setFileAndStale,
        file: file,
        setFileLoading: setFileLoading,
        fileLoading: fileLoading,
        resetFilePropertyLocations,
        staleFile,
        setStaleFile,
        getFilePropertyIndexById,
        fullWidth,
        setFullWidth,
        projectLoading,
        setProject: setProjectInstance,
        setProjectLoading: setProjectLoading,
        project: project,
        lastUpdatedBy,
        setLastUpdatedBy: setLastUpdatedByAndStale,
        staleLastUpdatedBy,
        setStaleLastUpdatedBy,
      }}
    >
      {props.children}
    </SideBarContext.Provider>
  );
};
