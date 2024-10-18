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
  } = useAcquisitionProvider();

  const fetchSubFilesData = useCallback(async () => {
    const response = await fetchSubFiles(acquisitionFile.id);
    if (exists(response)) {
      setAcquisitionSubFiles(response);
    }
  }, [acquisitionFile.id, fetchSubFiles]);

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
      fetchSubFilesData();
    }
  }, [acquisitionSubFiles, fetchSubFilesData]);

  return (
    <View
      loading={loading || loadingSubFiles}
      acquisitionFile={acquisitionFile}
      subFiles={acquisitionSubFiles}
      onAdd={onAddSubFile}
    />
  );
};

export default SubFileListContainer;
