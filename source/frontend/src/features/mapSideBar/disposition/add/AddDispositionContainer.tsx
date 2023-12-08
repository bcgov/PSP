import { FormikProps } from 'formik';
import { useCallback, useRef, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Api_DispositionFile } from '@/models/api/DispositionFile';

import useAddDispositionFormManagement from '../hooks/useAddDispositionFormManagement';
import { DispositionFormModel } from '../models/DispositionFormModel';
import { IAddDispositionContainerViewProps } from './AddDispositionContainerView';

export interface IAddDispositionContainerProps {
  onClose?: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IAddDispositionContainerViewProps>>;
}

const AddDispositionContainer: React.FC<IAddDispositionContainerProps> = ({ onClose, View }) => {
  const [isFormValid, setIsFormValid] = useState<boolean>(true);
  const formikRef = useRef<FormikProps<DispositionFormModel>>(null);

  const mapMachine = useMapStateMachine();
  const history = useHistory();

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

  const handleSuccesss = async (disposition: Api_DispositionFile) => {
    mapMachine.refreshMapProperties();
    history.replace(`/mapview/sidebar/disposition/${disposition.id}`);
  };

  const helper = useAddDispositionFormManagement({
    onSuccess: handleSuccesss,
    formikRef,
  });

  return (
    <View
      formikRef={formikRef}
      dispositionInitialValues={initialValuesForm}
      loading={helper.loading}
      displayFormInvalid={!isFormValid}
      onSave={handleSave}
      onCancel={handleCancel}
      onSubmit={helper.handleSubmit}
    ></View>
  );
};

export default AddDispositionContainer;
