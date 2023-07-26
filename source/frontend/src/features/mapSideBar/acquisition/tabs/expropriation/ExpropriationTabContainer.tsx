import { useCallback, useContext, useEffect, useState } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { Api_Form8 } from '@/models/api/Form8';

import { IExpropriationTabContainerViewProps } from './ExpropriationTabContainerView';

export interface IExpropriationTabContainerProps {
  acquisitionFile: Api_AcquisitionFile;
  View: React.FunctionComponent<IExpropriationTabContainerViewProps>;
}

export const ExpropriationTabContainer: React.FunctionComponent<
  React.PropsWithChildren<IExpropriationTabContainerProps>
> = ({ View, acquisitionFile }) => {
  const { fileLoading } = useContext(SideBarContext);
  const [form8s, setForm8s] = useState<Api_Form8[]>([]);

  const {
    getAcquisitionFileForm8s: { execute: getAcquisitionFileForm8s, loading: loadingForm8s },
  } = useAcquisitionProvider();

  const fetchForms = useCallback(async () => {
    if (acquisitionFile.id) {
      var retrieved = await getAcquisitionFileForm8s(acquisitionFile.id);
      if (retrieved) {
        setForm8s(retrieved);
      }
    }
  }, [acquisitionFile.id, getAcquisitionFileForm8s, setForm8s]);

  useEffect(() => {
    fetchForms();
  }, [fetchForms]);

  if (!!acquisitionFile && acquisitionFile?.id === undefined && fileLoading === false) {
    throw new Error('Unable to determine id of current file.');
  }

  return !!acquisitionFile?.id ? (
    <View
      loading={fileLoading || loadingForm8s}
      acquisitionFile={acquisitionFile}
      form8s={form8s}
    ></View>
  ) : null;
};

export default ExpropriationTabContainer;
