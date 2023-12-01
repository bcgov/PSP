import { FormikProps } from 'formik';
import { useCallback, useRef, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { Api_DispositionFile } from '@/models/api/DispositionFile';

import { DispositionFormModel } from '../models/DispositionFormModel';
import { AddDispositionContainerViewProps } from './AddDispositionContainerView';

export interface IAddDispositionContainerProps {
  onClose?: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<AddDispositionContainerViewProps>>;
}

const AddDispositionContainer: React.FC<IAddDispositionContainerProps> = ({ onClose, View }) => {
  const [isFormValid, setIsFormValid] = useState<boolean>(true);
  const formikRef = useRef<FormikProps<DispositionFormModel>>(null);

  const history = useHistory();

  const {
    addDispositionFileApi: { execute: addDispositionFileApi, loading },
  } = useDispositionProvider();
  const initialValuesForm = new DispositionFormModel();

  const handleCancel = useCallback(() => onClose && onClose(), [onClose]);

  const handleSave = async () => {
    await formikRef?.current?.validateForm();
    if (formikRef?.current?.isValid) {
      setIsFormValid(true);
    } else {
      setIsFormValid(false);
    }

    formikRef.current?.setSubmitting(true);
    formikRef.current?.submitForm();
  };

  const handleSubmit = async (financialCode: Api_DispositionFile) => {
    return addDispositionFileApi(financialCode);
  };

  const handleSuccesss = async (disposition: Api_DispositionFile) => {
    // toast.success(`Financial code saved`);
    history.replace(`/admin/financial-code/list`);
  };

  return (
    <View
      formikRef={formikRef}
      dispositionInitialValues={initialValuesForm}
      loading={loading}
      displayFormInvalid={!isFormValid}
      onSave={handleSave}
      onCancel={handleCancel}
      onSubmit={handleSubmit}
      onSuccess={handleSuccesss}
    ></View>
  );
};

export default AddDispositionContainer;
