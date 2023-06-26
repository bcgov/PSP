import { useContext } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';

import { IExpropiationTabcontainerViewProps } from './ExpropiationTabContainerView';

export interface IExpropiationTabContainer {
  acquisitionFileId: number;
  acquisitionFileTypeCode: string;
  View: React.FunctionComponent<React.PropsWithChildren<IExpropiationTabcontainerViewProps>>;
}

export const ExpropiationTabContainer: React.FunctionComponent<
  React.PropsWithChildren<IExpropiationTabContainer>
> = ({ View, acquisitionFileTypeCode }) => {
  const { file, fileLoading } = useContext(SideBarContext);
  if (!!file && file?.id === undefined && fileLoading === false) {
    throw new Error('Unable to determine id of current file.');
  }

  return !!file?.id ? (
    <>
      <View loading={fileLoading} acquisitionFileTypeCode={acquisitionFileTypeCode}></View>
    </>
  ) : null;
};

export default ExpropiationTabContainer;
