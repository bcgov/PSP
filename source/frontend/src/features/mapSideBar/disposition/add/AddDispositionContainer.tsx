import { FormikProps } from 'formik';
import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { useInitialMapSelectorProperties } from '@/hooks/useInitialMapSelectorProperties';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { featuresetToMapProperty } from '@/utils';

import { PropertyForm } from '../../shared/models';
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
  const history = useHistory();
  const mapMachine = useMapStateMachine();
  const selectedFeatureDataset = mapMachine.selectedFeatureDataset;

  const initialForm = useMemo(() => {
    const dispositionForm = new DispositionFormModel();
    // support creating a new disposition file from the map popup
    if (selectedFeatureDataset !== null) {
      dispositionForm.fileProperties = [
        PropertyForm.fromMapProperty(featuresetToMapProperty(selectedFeatureDataset)),
      ];
    }
    return dispositionForm;
  }, [selectedFeatureDataset]);

  const { bcaLoading, initialProperty } = useInitialMapSelectorProperties(selectedFeatureDataset);
  if (initialForm?.fileProperties.length && initialProperty) {
    initialForm.fileProperties[0].address = initialProperty.address;
  }

  useEffect(() => {
    if (!!initialForm && !!formikRef.current) {
      formikRef.current.resetForm();
      formikRef.current?.setFieldValue('fileProperties', initialForm.fileProperties);
    }
  }, [initialForm]);

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

  const handleSuccess = async (disposition: ApiGen_Concepts_DispositionFile) => {
    mapMachine.refreshMapProperties();
    history.replace(`/mapview/sidebar/disposition/${disposition.id}`);
  };

  const helper = useAddDispositionFormManagement({
    onSuccess: handleSuccess,
    formikRef,
  });

  return (
    <View
      formikRef={formikRef}
      dispositionInitialValues={initialForm}
      loading={helper.loading || bcaLoading}
      displayFormInvalid={!isFormValid}
      onSave={handleSave}
      onCancel={handleCancel}
      onSubmit={helper.handleSubmit}
    ></View>
  );
};

export default AddDispositionContainer;
