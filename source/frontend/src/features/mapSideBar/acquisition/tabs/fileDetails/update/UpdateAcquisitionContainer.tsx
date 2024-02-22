import { AxiosError } from 'axios';
import { FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { isValidId } from '@/utils';

import { UpdateAcquisitionSummaryFormModel } from './models';
import { UpdateAcquisitionFileYupSchema } from './UpdateAcquisitionFileYupSchema';
import { IUpdateAcquisitionFormProps } from './UpdateAcquisitionForm';

export interface IUpdateAcquisitionContainerProps {
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  onSuccess: () => void;
  View: React.FC<IUpdateAcquisitionFormProps>;
}

export const RemoveSelfContractorContent = (): React.ReactNode => {
  return (
    <>
      <p>
        Contractors cannot remove themselves from a file. Please contact the admin at{' '}
        <a href="mailto: pims@gov.bc.ca">pims@gov.bc.ca</a>
      </p>
    </>
  );
};

export const UpdateAcquisitionContainer = React.forwardRef<
  FormikProps<UpdateAcquisitionSummaryFormModel>,
  IUpdateAcquisitionContainerProps
>((props, formikRef) => {
  const { acquisitionFile, onSuccess, View } = props;
  const { setModalContent, setDisplayModal } = useModalContext();

  const {
    updateAcquisitionFile: { execute: updateAcquisitionFile },
  } = useAcquisitionProvider();

  const withUserOverride = useApiUserOverride<
    (userOverrideCodes: UserOverrideCode[]) => Promise<ApiGen_Concepts_AcquisitionFile | void>
  >('Failed to update Acquisition File');

  const handleSubmit = async (
    values: UpdateAcquisitionSummaryFormModel,
    formikHelpers: FormikHelpers<UpdateAcquisitionSummaryFormModel>,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    try {
      const acquisitionFile = values.toApi();
      const response = await updateAcquisitionFile(acquisitionFile, userOverrideCodes);

      if (isValidId(response?.id)) {
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
    } finally {
      formikHelpers?.setSubmitting(false);
    }
  };

  return (
    <StyledFormWrapper>
      <View
        formikRef={formikRef}
        initialValues={UpdateAcquisitionSummaryFormModel.fromApi(acquisitionFile)}
        onSubmit={(
          values: UpdateAcquisitionSummaryFormModel,
          formikHelpers: FormikHelpers<UpdateAcquisitionSummaryFormModel>,
        ) =>
          withUserOverride(
            (userOverrideCodes: UserOverrideCode[]) =>
              handleSubmit(values, formikHelpers, userOverrideCodes),
            [],
            (axiosError: AxiosError<IApiError>) => {
              setModalContent({
                variant: 'error',
                title: 'Warning',
                message: axiosError?.response?.data.error,
                okButtonText: 'Close',
              });
              setDisplayModal(true);
            },
          )
        }
        validationSchema={UpdateAcquisitionFileYupSchema}
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
