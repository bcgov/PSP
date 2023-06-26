import { useContext } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';

import { IExpropriationTabcontainerViewProps } from './ExpropriationTabContainerView';

export interface IExpropriationTabContainer {
  acquisitionFileId: number;
  acquisitionFileTypeCode: string;
  View: React.FunctionComponent<React.PropsWithChildren<IExpropriationTabcontainerViewProps>>;
}

export const ExpropriationTabContainer: React.FunctionComponent<
  React.PropsWithChildren<IExpropriationTabContainer>
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

export default ExpropriationTabContainer;
