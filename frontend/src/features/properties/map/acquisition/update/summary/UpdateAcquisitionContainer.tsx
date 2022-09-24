import { FormikHelpers, FormikProps } from 'formik';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React, { useCallback, useRef } from 'react';
import styled from 'styled-components';

import { AcquisitionContainerState } from '../../AcquisitionContainer';
import { useAcquisitionProvider } from '../../hooks/useAcquisitionProvider';
import { UpdateAcquisitionSummaryFormModel } from './models';
import { UpdateAcquisitionForm } from './UpdateAcquisitionForm';

export interface IUpdateAcquisitionContainerProps {
  acquisitionFile: Api_AcquisitionFile;
  setContainerState: (value: Partial<AcquisitionContainerState>) => void;
  onSuccess: () => void;
}

export const UpdateAcquisitionContainer: React.FC<IUpdateAcquisitionContainerProps> = ({
  acquisitionFile,
  setContainerState,
  onSuccess,
}) => {
  const formikRef = useRef<FormikProps<UpdateAcquisitionSummaryFormModel>>(null);

  setContainerState({ formikRef });

  const {
    updateAcquisitionFile: { execute: updateAcquisitionFile },
  } = useAcquisitionProvider();

  // save handler
  const handleSubmit = useCallback(
    async (
      values: UpdateAcquisitionSummaryFormModel,
      formikHelpers: FormikHelpers<UpdateAcquisitionSummaryFormModel>,
    ) => {
      const acquisitionFile = values.toApi();
      const response = await updateAcquisitionFile(acquisitionFile);
      formikHelpers?.setSubmitting(false);

      if (!!response?.id) {
        formikHelpers?.resetForm();
        if (typeof onSuccess === 'function') {
          onSuccess();
        }
      }
    },
    [updateAcquisitionFile, onSuccess],
  );

  return (
    <StyledFormWrapper>
      <UpdateAcquisitionForm
        ref={formikRef}
        initialValues={UpdateAcquisitionSummaryFormModel.fromApi(acquisitionFile)}
        onSubmit={handleSubmit}
        // validationSchema={helper.validationSchema}
      />
    </StyledFormWrapper>
  );
};

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
