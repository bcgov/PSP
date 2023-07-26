import { useCallback, useEffect, useState } from 'react';
import { useHistory, useRouteMatch } from 'react-router-dom';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOption';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useForm8Repository } from '@/hooks/repositories/useForm8Repository';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { Api_Form8 } from '@/models/api/Form8';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { stripTrailingSlash } from '@/utils';

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
  const { path } = useRouteMatch();
  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const gstDecimalPercentage = gstConstant !== undefined ? parseFloat(gstConstant.value) / 100 : 0;

  const [payeeOptions, setPayeeOptions] = useState<PayeeOption[]>([]);
  const [initialValues, setInitialValues] = useState<Form8FormModel | null>(null);

  const {
    getForm8: { execute: getForm8, error, loading },
    updateForm8: { execute: updateForm8, response: updatedForm8, loading: updatingForm8 },
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

  const loadForm8Details = useCallback(async () => {
    const form8Api = await getForm8(form8Id);
    if (form8Api) {
      const form8Model = Form8FormModel.fromApi(form8Api);
      setInitialValues(form8Model);

      const acquisitionOwnersCall = retrieveAcquisitionOwners(form8Api.acquisitionFileId);
      const interestHoldersCall = fetchInterestHolders(form8Api.acquisitionFileId);

      await Promise.all([acquisitionOwnersCall, interestHoldersCall]).then(
        ([acquisitionOwners, interestHolders]) => {
          const options = payeeOptions;

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
  }, [fetchInterestHolders, form8Id, getForm8, payeeOptions, retrieveAcquisitionOwners]);

  const handleSave = async (form8: Api_Form8) => {
    return updateForm8(form8);
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
      onCancel={() => history.push(`${stripTrailingSlash(path)}/${FileTabType.EXPROPRIATION}`)}
      onSave={handleSave}
    ></View>
  );
};

export default UpdateForm8Container;
