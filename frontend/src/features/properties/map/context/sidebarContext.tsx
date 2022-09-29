import { FileTypes } from 'constants/fileTypes';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_File } from 'models/api/File';
import { Api_PropertyFile } from 'models/api/PropertyFile';
import { Api_ResearchFile } from 'models/api/ResearchFile';
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
  getFileProperties: () => Api_PropertyFile[];
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
  getFileProperties: () => {
    throw Error('getFileProperties function not defined');
  },
});

export const SideBarContextProvider = (props: {
  children: React.ReactChild | React.ReactChild[] | React.ReactNode;
  file?: TypedFile;
}) => {
  const [file, setFile] = useState<TypedFile | undefined>(props.file);
  const [staleFile, setStaleFile] = useState<boolean>(false);
  const [fileLoading, setFileLoading] = useState<boolean>(false);

  const setFileAndStale = useCallback(
    (file?: TypedFile) => {
      setFile(file);
      setStaleFile(false);
    },
    [setFile, setStaleFile],
  );

  const getFileProperties = () => {
    switch (file?.fileType) {
      case FileTypes.Acquisition:
        return (file as Api_AcquisitionFile).acquisitionProperties ?? [];
      case FileTypes.Research:
        return (file as Api_ResearchFile).researchProperties ?? [];
      default:
        throw Error('invalid file type');
    }
  };

  return (
    <SideBarContext.Provider
      value={{
        setFile: setFileAndStale,
        file: file,
        setFileLoading: setFileLoading,
        fileLoading: fileLoading,
        getFileProperties,
        staleFile,
        setStaleFile,
      }}
    >
      {props.children}
    </SideBarContext.Provider>
  );
};
