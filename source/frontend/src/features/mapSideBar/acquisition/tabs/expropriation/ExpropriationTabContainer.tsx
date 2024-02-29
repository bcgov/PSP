import { useCallback, useContext, useEffect, useState } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useForm8Repository } from '@/hooks/repositories/useForm8Repository';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_ExpropriationPayment } from '@/models/api/generated/ApiGen_Concepts_ExpropriationPayment';
import { isValidId } from '@/utils/utils';

import { IExpropriationTabContainerViewProps } from './ExpropriationTabContainerView';

export interface IExpropriationTabContainerProps {
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  View: React.FunctionComponent<IExpropriationTabContainerViewProps>;
}

export const ExpropriationTabContainer: React.FunctionComponent<
  React.PropsWithChildren<IExpropriationTabContainerProps>
> = ({ View, acquisitionFile }) => {
  const { fileLoading, setStaleLastUpdatedBy } = useContext(SideBarContext);
  const [form8s, setForm8s] = useState<ApiGen_Concepts_ExpropriationPayment[]>([]);

  const {
    getAcquisitionFileForm8s: { execute: getAcquisitionFileForm8s, loading: loadingForm8s },
  } = useAcquisitionProvider();

  const fetchForms = useCallback(async () => {
    if (acquisitionFile.id) {
      const retrieved = await getAcquisitionFileForm8s(acquisitionFile.id);
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
    const updatedForms =
      acquisitionFile?.id !== undefined &&
      acquisitionFile.id >= 0 &&
      (await getAcquisitionFileForm8s(acquisitionFile.id));
    if (updatedForms) {
      setForm8s(updatedForms);
    }
  };

  if (!isValidId(acquisitionFile?.id) && fileLoading === false) {
    throw new Error('Unable to determine id of current file.');
  }

  return isValidId(acquisitionFile?.id) ? (
    <View
      loading={fileLoading || loadingForm8s || deletingForm8}
      acquisitionFile={acquisitionFile}
      form8s={form8s}
      onForm8Deleted={handleForm8Deleted}
    ></View>
  ) : null;
};

export default ExpropriationTabContainer;
