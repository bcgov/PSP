import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';

import { ISubFileListViewProps } from './SubFileListView';

export interface ISubFileListContainerProps {
  View: React.FC<ISubFileListViewProps>;
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
}

export const SubFileListContainer: React.FunctionComponent<ISubFileListContainerProps> = ({
  View,
  acquisitionFile,
}) => {
  // TODO: Add an "useEffect" to fetch the list of linked files from the backend API
  // Use this loading flag to render a spinner in the view while loading
  const loading = false;

  const onAddSubFile = (): void => {
    // TODO: Here we will copy some data from main file into sub-file and redirect to "Create Acquisition File" for sub-file
    // This will be done in another pull-request.
    throw new Error('Function not implemented yet.');
  };

  return <View loading={loading} acquisitionFile={acquisitionFile} onAdd={onAddSubFile} />;
};

export default SubFileListContainer;
