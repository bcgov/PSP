import { useCallback, useEffect, useState } from 'react';
import { useHistory, useLocation } from 'react-router-dom';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useForm8Repository } from '@/hooks/repositories/useForm8Repository';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { Api_ExpropriationPayment } from '@/models/api/ExpropriationPayment';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';

import { Form8FormModel } from '../models/Form8FormModel';
import { IForm8FormProps } from '../UpdateForm8Form';

export interface IUpdateForm8ContainerProps {
  form8Id: number;
  View: React.FC<IForm8FormProps>;
}

export const UpdateForm8Container: React.FunctionComponent<
  React.PropsWithChildren<IUpdateForm8ContainerProps>
> = ({ form8Id, View }) => {
  const history = useHistory();
  const location = useLocation();

  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimalPercentage = gstConstant !== undefined ? parseFloat(gstConstant.value) / 100 : 0;

  const [payeeOptions, setPayeeOptions] = useState<PayeeOption[]>([]);
  const [initialValues, setInitialValues] = useState<Form8FormModel | null>(null);

  const {
    getForm8: { execute: getForm8, loading },
    updateForm8: { execute: updateForm8, loading: updatingForm8 },
  } = useForm8Repository();
  const {
    getAcquisitionOwners: { execute: retrieveAcquisitionOwners, loading: loadingAcquisitionOwners },
  } = useAcquisitionProvider();
  const {
    getAcquisitionInterestHolders: {
      execute: fetchInterestHolders,
      loading: loadingInterestHolders,
    },
  } = useInterestHolderRepository();

  const aquisitionPath = location.pathname.split(`/${FileTabType.EXPROPRIATION}/${form8Id}`)[0];
  const backUrl = `${aquisitionPath}/${FileTabType.EXPROPRIATION}`;

  const loadForm8Details = useCallback(async () => {
    const form8Api = await getForm8(form8Id);
    if (form8Api) {
      const form8Model = Form8FormModel.fromApi(form8Api);
      setInitialValues(form8Model);

      const acquisitionOwnersCall = retrieveAcquisitionOwners(form8Api.acquisitionFileId);
      const interestHoldersCall = fetchInterestHolders(form8Api.acquisitionFileId);

      await Promise.all([acquisitionOwnersCall, interestHoldersCall]).then(
        ([acquisitionOwners, interestHolders]) => {
          const options = [];

          if (acquisitionOwners !== undefined) {
            const ownersOptions: PayeeOption[] = acquisitionOwners.map(x =>
              PayeeOption.createOwner(x),
            );
            options.push(...ownersOptions);
          }

          if (interestHolders !== undefined) {
            const interestHolderOptions: PayeeOption[] = interestHolders.map(x =>
              PayeeOption.createInterestHolder(x),
            );
            options.push(...interestHolderOptions);
          }

          setPayeeOptions(options);
        },
      );
    }
  }, [fetchInterestHolders, form8Id, getForm8, retrieveAcquisitionOwners]);

  const handleSave = async (form8: Api_ExpropriationPayment) => {
    return updateForm8(form8);
  };

  const onUpdateSuccess = async () => {
    history.push(backUrl);
  };

  useEffect(() => {
    loadForm8Details();
  }, [loadForm8Details]);

  if (loading || updatingForm8 || loadingAcquisitionOwners || loadingInterestHolders) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <View
      initialValues={initialValues}
      payeeOptions={payeeOptions}
      gstConstant={gstDecimalPercentage}
      onCancel={() => history.push(backUrl)}
      onSave={handleSave}
      onSuccess={onUpdateSuccess}
    ></View>
  );
};

export default UpdateForm8Container;
