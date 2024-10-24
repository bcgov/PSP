import { useCallback, useEffect, useState } from 'react';

import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { exists } from '@/utils';

import { ISubFileListViewProps } from './SubFileListView';

export interface ISubFileListContainerProps {
  View: React.FC<ISubFileListViewProps>;
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
}

export const SubFileListContainer: React.FunctionComponent<ISubFileListContainerProps> = ({
  View,
  acquisitionFile,
}) => {
  const [acquisitionSubFiles, setAcquisitionSubFiles] = useState<
    ApiGen_Concepts_AcquisitionFile[] | null
  >(null);

  const {
    getAcquisitionSubFiles: { execute: fetchSubFiles, loading: loadingSubFiles },
    getAcquisitionFile: {
      execute: getParentAcquisitionFile,
      loading: loadingParentAcquisitionFile,
    },
  } = useAcquisitionProvider();

  const fetchSubFilesTableData = useCallback(async () => {
    if (acquisitionFile.parentAcquisitionFileId === null) {
      const response = await fetchSubFiles(acquisitionFile.id);
      if (exists(response)) {
        const subFilesList = [acquisitionFile, ...response];
        setAcquisitionSubFiles(subFilesList);
      }
    } else {
      const parentAcquisitionPromise = getParentAcquisitionFile(
        acquisitionFile.parentAcquisitionFileId,
      );
      const subFilesPromise = fetchSubFiles(acquisitionFile.parentAcquisitionFileId);

      const [parentFileResponse, subFilesResponse] = await Promise.all([
        parentAcquisitionPromise,
        subFilesPromise,
      ]);

      if (exists(parentFileResponse) && exists(subFilesResponse)) {
        const subFilesList = [parentFileResponse, ...subFilesResponse];
        setAcquisitionSubFiles(subFilesList);
      }
    }
  }, [acquisitionFile, fetchSubFiles, getParentAcquisitionFile]);

  // TODO: Add an "useEffect" to fetch the list of linked files from the backend API
  // Use this loading flag to render a spinner in the view while loading
  const loading = false;

  const onAddSubFile = (): void => {
    // TODO: Here we will copy some data from main file into sub-file and redirect to "Create Acquisition File" for sub-file
    // This will be done in another pull-request.
    throw new Error('Function not implemented yet.');
  };

  useEffect(() => {
    if (acquisitionSubFiles === null) {
      fetchSubFilesTableData();
    }
  }, [acquisitionSubFiles, fetchSubFilesTableData]);

  return (
    <View
      loading={loading || loadingParentAcquisitionFile || loadingSubFiles}
      acquisitionFile={acquisitionFile}
      subFiles={acquisitionSubFiles}
      onAdd={onAddSubFile}
    />
  );
};

export default SubFileListContainer;
