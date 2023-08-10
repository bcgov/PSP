import { useCallback, useEffect, useState } from 'react';
import { useHistory, useLocation } from 'react-router-dom';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOption';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { Api_ExpropriationPayment } from '@/models/api/Form8';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';

import { Form8FormModel } from '../models/Form8FormModel';
import { IForm8FormProps } from '../UpdateForm8Form';

export interface IAddForm8ContainerProps {
  acquisitionFileId: number;
  View: React.FC<IForm8FormProps>;
}

export const AddForm8Container: React.FunctionComponent<
  React.PropsWithChildren<IAddForm8ContainerProps>
> = ({ acquisitionFileId, View }) => {
  const initialValues = new Form8FormModel(null, acquisitionFileId);

  const history = useHistory();
  const location = useLocation();
  const { getSystemConstant } = useSystemConstants();
  const gstConstant = getSystemConstant(SystemConstants.GST);
  const [payeeOptions, setPayeeOptions] = useState<PayeeOption[]>([]);

  const {
    postAcquisitionForm8: { execute: postAcquisitionForm8, error, loading },
    getAcquisitionOwners: { execute: retrieveAcquisitionOwners, loading: loadingAcquisitionOwners },
  } = useAcquisitionProvider();
  const {
    getAcquisitionInterestHolders: {
      execute: fetchInterestHolders,
      loading: loadingInterestHolders,
    },
  } = useInterestHolderRepository();

  const gstDecimalPercentage = gstConstant !== undefined ? parseFloat(gstConstant.value) / 100 : 0;
  const backUrl = location.pathname.split('/add')[0];

  const fetchContacts = useCallback(async () => {
    const acquisitionOwnersCall = retrieveAcquisitionOwners(acquisitionFileId);
    const interestHoldersCall = fetchInterestHolders(acquisitionFileId);

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
  }, [payeeOptions, acquisitionFileId, retrieveAcquisitionOwners, fetchInterestHolders]);

  const handleSave = async (form8: Api_ExpropriationPayment) => {
    return postAcquisitionForm8(acquisitionFileId, form8);
  };

  useEffect(() => {
    fetchContacts();
  }, [fetchContacts]);

  if (loading || loadingAcquisitionOwners || loadingInterestHolders) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <View
      initialValues={initialValues}
      payeeOptions={payeeOptions}
      gstConstant={gstDecimalPercentage}
      onCancel={() => history.push(backUrl)}
      onSave={handleSave}
    ></View>
  );
};

export default AddForm8Container;
