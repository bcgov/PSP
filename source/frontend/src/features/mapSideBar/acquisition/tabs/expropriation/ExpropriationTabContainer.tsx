import { useContext } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';

import { IExpropriationTabContainerViewProps } from './ExpropriationTabContainerView';

export interface IExpropriationTabContainer {
  acquisitionFile: Api_AcquisitionFile;
  View: React.FunctionComponent<IExpropriationTabContainerViewProps>;
}

export const ExpropriationTabContainer: React.FunctionComponent<IExpropriationTabContainer> = ({
  View,
  acquisitionFile,
}) => {
  const { fileLoading } = useContext(SideBarContext);
  if (!!acquisitionFile && acquisitionFile?.id === undefined && fileLoading === false) {
    throw new Error('Unable to determine id of current file.');
  }

  return !!acquisitionFile?.id ? (
    <View loading={fileLoading} acquisitionFile={acquisitionFile}></View>
  ) : null;
};

export default ExpropriationTabContainer;
