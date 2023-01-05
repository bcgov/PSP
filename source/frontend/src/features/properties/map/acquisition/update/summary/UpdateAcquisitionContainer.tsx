import GenericModal from 'components/common/GenericModal';
import { FormikHelpers, FormikProps } from 'formik';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React, { useState } from 'react';
import { toast } from 'react-toastify';
import styled from 'styled-components';
import { useAxiosErrorHandlerWithConfirmation } from 'utils';

import { useAcquisitionProvider } from '../../hooks/useAcquisitionProvider';
import { UpdateAcquisitionSummaryFormModel } from './models';
import { UpdateAcquisitionFileYupSchema } from './UpdateAcquisitionFileYupSchema';
import { UpdateAcquisitionForm } from './UpdateAcquisitionForm';

export interface IUpdateAcquisitionContainerProps {
  acquisitionFile: Api_AcquisitionFile;
  onSuccess: () => void;
}

export const UpdateAcquisitionContainer = React.forwardRef<
  FormikProps<any>,
  IUpdateAcquisitionContainerProps
>((props, formikRef) => {
  const { acquisitionFile, onSuccess } = props;

  const {
    updateAcquisitionFile: { execute: updateAcquisitionFile },
  } = useAcquisitionProvider();

  const [showMinistryModal, setShowMinistryModal] = useState(false);
  const [allowMinistryOverride, setAllowMinistryOverride] = useState(false);

  const handleAxiosErrors = useAxiosErrorHandlerWithConfirmation(
    setShowMinistryModal,
    'Failed to update Acquisition File',
  );

  // save handler
  const handleSubmit = async (
    values: UpdateAcquisitionSummaryFormModel,
    formikHelpers: FormikHelpers<UpdateAcquisitionSummaryFormModel>,
  ) => {
    try {
      setShowMinistryModal(false);
      const acquisitionFile = values.toApi();
      const response = await updateAcquisitionFile(acquisitionFile, allowMinistryOverride);

      if (!!response?.id) {
        if (acquisitionFile.fileProperties?.find(ap => !ap.property?.address && !ap.property?.id)) {
          toast.warn(
            'Address could not be retrieved for this property, it will have to be provided manually in property details tab',
            { autoClose: 15000 },
          );
        }
        formikHelpers?.resetForm();
        if (typeof onSuccess === 'function') {
          onSuccess();
        }
      }
    } catch (e) {
      handleAxiosErrors(e);
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  const saveOverride = async () => {
    setAllowMinistryOverride(true);
    // need to use type coercion here to make typescript happy
    const ref = formikRef as React.RefObject<FormikProps<UpdateAcquisitionSummaryFormModel>>;
    if (ref !== undefined) {
      ref.current?.setSubmitting(true);
      ref.current?.submitForm();
    }
  };

  return (
    <StyledFormWrapper>
      <UpdateAcquisitionForm
        ref={formikRef}
        initialValues={UpdateAcquisitionSummaryFormModel.fromApi(acquisitionFile)}
        onSubmit={handleSubmit}
        validationSchema={UpdateAcquisitionFileYupSchema}
      />
      <GenericModal
        title="Different Ministry region"
        message="The Ministry region has been changed, this will result in a change to the file's prefix. Do you wish to continue?"
        okButtonText="Continue Save"
        cancelButtonText="Cancel Update"
        display={showMinistryModal}
        handleOk={() => saveOverride()}
        handleCancel={() => {
          setAllowMinistryOverride(false);
          setShowMinistryModal(false);
        }}
      ></GenericModal>
    </StyledFormWrapper>
  );
});

export default UpdateAcquisitionContainer;

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;
