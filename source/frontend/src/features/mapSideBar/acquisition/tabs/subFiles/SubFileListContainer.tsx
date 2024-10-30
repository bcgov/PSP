import { useCallback, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

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
  const history = useHistory();

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

  // Redirect to "Create Acquisition File" route for sub-file
  const onAddSubFile = (): void => {
    const params = new URLSearchParams();
    params.set('parentId', acquisitionFile.id.toString());
    history.replace({
      pathname: `/mapview/sidebar/acquisition/new`,
      search: params.toString(),
    });
  };

  useEffect(() => {
    if (acquisitionSubFiles === null) {
      fetchSubFilesTableData();
    }
  }, [acquisitionSubFiles, fetchSubFilesTableData]);

  return (
    <View
      loading={loadingParentAcquisitionFile || loadingSubFiles}
      acquisitionFile={acquisitionFile}
      subFiles={acquisitionSubFiles}
      onAdd={onAddSubFile}
    />
  );
};

export default SubFileListContainer;
