import axios, { AxiosError } from 'axios';
import GenericModal from 'components/common/GenericModal';
import { FormikHelpers, FormikProps } from 'formik';
import { useAcquisitionProvider } from 'hooks/repositories/useAcquisitionProvider';
import { IApiError } from 'interfaces/IApiError';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React, { useCallback, useState } from 'react';
import { toast } from 'react-toastify';
import styled from 'styled-components';

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
  const [ministryOverride, setMinistryOverride] = useState(false);

  const [showPropertiesModal, setShowPropertiesModal] = useState(false);
  const [propertiesOverride, setPropertiesOverride] = useState(false);

  const handleAxiosError = useCallback((error: unknown) => {
    if (axios.isAxiosError(error)) {
      const axiosError = error as AxiosError<IApiError>;
      if (axiosError?.response?.status === 409) {
        // The API sent a 409 error - indicating user confirmation is needed
        switch (axiosError?.response?.data?.errorCode) {
          case 'region_violation':
            setShowMinistryModal(true);
            break;
          case 'properties_of_interest_violation':
            setShowPropertiesModal(true);
            break;
          default:
            break;
        }
      } else if (axiosError?.response?.status === 400) {
        toast.error(axiosError?.response.data.error);
      } else {
        toast.error('Failed to update Acquisition File');
      }
    }
  }, []);

  // save handler
  const handleSubmit = async (
    values: UpdateAcquisitionSummaryFormModel,
    formikHelpers: FormikHelpers<UpdateAcquisitionSummaryFormModel>,
  ) => {
    try {
      setShowMinistryModal(false);
      setShowPropertiesModal(false);

      const acquisitionFile = values.toApi();
      const response = await updateAcquisitionFile(
        acquisitionFile,
        ministryOverride,
        propertiesOverride,
      );

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
      handleAxiosError(e);
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  const saveAfterConfirmation = async () => {
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
        handleOk={() => {
          setMinistryOverride(true);
          saveAfterConfirmation();
        }}
        handleCancel={() => {
          setMinistryOverride(false);
          setShowMinistryModal(false);
        }}
      />
      <GenericModal
        title="Warning"
        message="The properties of interest will be added to the inventory as acquired properties. Do you wish to continue?"
        okButtonText="Continue Save"
        cancelButtonText="Cancel Update"
        display={showPropertiesModal}
        handleOk={() => {
          setPropertiesOverride(true);
          saveAfterConfirmation();
        }}
        handleCancel={() => {
          setPropertiesOverride(false);
          setShowPropertiesModal(false);
        }}
      />
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
