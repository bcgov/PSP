import { useCallback, useContext, useEffect, useState } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useForm8Repository } from '@/hooks/repositories/useForm8Repository';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { Api_ExpropriationPayment } from '@/models/api/ExpropriationPayment';

import { IExpropriationTabContainerViewProps } from './ExpropriationTabContainerView';

export interface IExpropriationTabContainerProps {
  acquisitionFile: Api_AcquisitionFile;
  View: React.FunctionComponent<IExpropriationTabContainerViewProps>;
}

export const ExpropriationTabContainer: React.FunctionComponent<
  React.PropsWithChildren<IExpropriationTabContainerProps>
> = ({ View, acquisitionFile }) => {
  const { fileLoading, setStaleLastUpdatedBy } = useContext(SideBarContext);
  const [form8s, setForm8s] = useState<Api_ExpropriationPayment[]>([]);

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

  const {
    deleteForm8: { execute: deleteForm8, loading: deletingForm8 },
  } = useForm8Repository();

  useEffect(() => {
    fetchForms();
  }, [fetchForms]);

  const handleForm8Deleted = async (form8Id: number) => {
    await deleteForm8(form8Id);
    setStaleLastUpdatedBy(true);
    var updatedForms = await getAcquisitionFileForm8s(acquisitionFile.id!);
    if (updatedForms) {
      setForm8s(updatedForms);
    }
  };

  if (!!acquisitionFile && acquisitionFile?.id === undefined && fileLoading === false) {
    throw new Error('Unable to determine id of current file.');
  }

  return !!acquisitionFile?.id ? (
    <View
      loading={fileLoading || loadingForm8s || deletingForm8}
      acquisitionFile={acquisitionFile}
      form8s={form8s}
      onForm8Deleted={handleForm8Deleted}
    ></View>
  ) : null;
};

export default ExpropriationTabContainer;
