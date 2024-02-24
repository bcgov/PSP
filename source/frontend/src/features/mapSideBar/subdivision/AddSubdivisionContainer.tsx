import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { useBcaAddress } from '@/features/properties/map/hooks/useBcaAddress';
import { usePropertyOperationRepository } from '@/hooks/repositories/usePropertyOperationRepository';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_PropertyOperation } from '@/models/api/generated/ApiGen_Concepts_PropertyOperation';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { featuresetToMapProperty } from '@/utils';

import { PropertyForm } from '../shared/models';
import { SubdivisionFormModel } from './AddSubdivisionModel';
import { IAddSubdivisionViewProps } from './AddSubdivisionView';

export interface IAddSubdivisionContainerProps {
  onClose?: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IAddSubdivisionViewProps>>;
}

const AddSubdivisionContainer: React.FC<IAddSubdivisionContainerProps> = ({ onClose, View }) => {
  const [isFormValid, setIsFormValid] = useState<boolean>(true);
  const formikRef = useRef<FormikProps<SubdivisionFormModel>>(null);
  const mapMachine = useMapStateMachine();
  const selectedFeatureDataset = mapMachine.selectedFeatureDataset;
  const history = useHistory();
  const { setModalContent, setDisplayModal } = useModalContext();
  const { getPrimaryAddressByPid, bcaLoading } = useBcaAddress();

  const {
    addPropertyOperationApi: { execute: addPropertyOperation, loading },
  } = usePropertyOperationRepository();

  const handleCancel = useCallback(() => {
    if (formikRef.current?.dirty) {
      setModalContent({
        ...getCancelModalProps(),
        handleOk: () => {
          onClose?.();
          setDisplayModal(false);
        },
      });
    } else {
      onClose?.();
    }
    setDisplayModal(true);
  }, [onClose, setDisplayModal, setModalContent]);

  const initialForm = useMemo(() => {
    const subdivisionFormModel = new SubdivisionFormModel();
    // support creating a new disposition file from the map popup
    if (selectedFeatureDataset !== null) {
      const property = PropertyForm.fromMapProperty(
        featuresetToMapProperty(selectedFeatureDataset),
      ).toApi();
      subdivisionFormModel.sourceProperty = property;
    }
    return subdivisionFormModel;
  }, [selectedFeatureDataset]);

  useEffect(() => {
    if (!!initialForm && !!formikRef.current) {
      formikRef.current.resetForm();
      formikRef.current?.setFieldValue('sourceProperty', initialForm.sourceProperty);
    }
  }, [initialForm]);

  const finishSelection = mapMachine.finishSelection;
  useEffect(() => {
    return () => finishSelection();
  }, [finishSelection]);

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<ApiGen_Concepts_DispositionFile | void>
  >('Failed to create Disposition File');

  const handleSave = async () => {
    await formikRef?.current?.validateForm();
    if (formikRef?.current?.isValid) {
      setIsFormValid(true);

      setModalContent({
        variant: 'info',
        cancelButtonText: 'Cancel',
        okButtonText: 'Save',
        title: 'Are you sure?',
        message: (
          <>
            <p>
              You are subdividing a property into two or more properties. The old parent property
              record will be retired, and the new child properties will be created
            </p>
            <p>
              If you proceed, you will be redirected to the old parent property record, where you
              can view changes and make updates to the new properties. Do you want to proceed?
            </p>
          </>
        ),
        handleOk: () => {
          formikRef.current?.setSubmitting(true);
          formikRef.current?.submitForm();
          setDisplayModal(false);
        },
        handleCancel: () => setDisplayModal(false),
      });
      setDisplayModal(true);
    } else {
      setIsFormValid(false);
    }
  };

  const handleSubmit = async (
    values: SubdivisionFormModel,
    formikHelpers: FormikHelpers<SubdivisionFormModel>,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    try {
      const propertyOperations = values.toApi();
      const response = await addPropertyOperation(propertyOperations, userOverrideCodes);

      if (response?.length) {
        formikHelpers?.resetForm();
        handleSuccess(propertyOperations);
      }
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  const handleSuccess = async (subdivisions: ApiGen_Concepts_PropertyOperation[]) => {
    mapMachine.refreshMapProperties();
    // eslint-disable-next-line no-debugger
    debugger;
    if (subdivisions.length === 0 || !subdivisions[0].sourceProperty) {
      history.replace(`/mapview`);
    }
    history.replace(`/mapview/sidebar/property/${subdivisions[0].sourceProperty?.id}`);
  };

  return (
    <View
      formikRef={formikRef}
      loading={loading || bcaLoading}
      onSubmit={(
        values: SubdivisionFormModel,
        formikHelpers: FormikHelpers<SubdivisionFormModel>,
      ) =>
        withUserOverride(
          (userOverrideCodes: UserOverrideCode[]) =>
            handleSubmit(values, formikHelpers, userOverrideCodes),
          [],
          (axiosError: AxiosError<IApiError>) => {
            setModalContent({
              variant: 'error',
              title: 'Error',
              message: axiosError?.response?.data.error,
              okButtonText: 'Close',
            });
            setDisplayModal(true);
          },
        )
      }
      onCancel={handleCancel}
      onSave={handleSave}
      subdivisionInitialValues={initialForm}
      displayFormInvalid={!isFormValid}
      getPrimaryAddressByPid={getPrimaryAddressByPid}
    ></View>
  );
};

export default AddSubdivisionContainer;
