import { FormikProps } from 'formik';
import { useRef, useState } from 'react';
import { FaUpload } from 'react-icons/fa';
import styled from 'styled-components';

import GenericModal, { ModalSize } from '@/components/common/GenericModal';
import { exists } from '@/utils';

import { ShapeUploadModel, UploadResponseModel } from './models';
import ShapeUploadContainer from './ShapeUploadContainer';
import ShapeUploadForm from './ShapeUploadForm';
import ShapeUploadResultView from './ShapeUploadResultView';

export interface IShapeUploadModalProps {
  display?: boolean;
  setDisplay?: (display: boolean) => void;
  onClose: (result: UploadResponseModel | null) => void;
}

export const ShapeUploadModal: React.FunctionComponent<IShapeUploadModalProps> = ({
  display,
  setDisplay,
  onClose,
}) => {
  const formikRef = useRef<FormikProps<ShapeUploadModel>>(null);

  const [uploadResult, setUploadResult] = useState<UploadResponseModel>(null);
  const [displayConfirmation, setDisplayConfirmation] = useState(false);

  const onSaveHandler = () => {
    formikRef.current?.submitForm();
  };

  const onUploadFileHandler = (result: UploadResponseModel) => {
    setDisplayConfirmation(false);
    setUploadResult(result);
  };

  // Warn user if they are about to lose data when cancelling "Upload Shapefile" modal
  const onCloseHandler = () => {
    const dirty = formikRef.current?.dirty ?? false;
    if (dirty && !displayConfirmation) {
      setDisplayConfirmation(true);
    } else {
      setDisplayConfirmation(false);
      onClose(uploadResult);
      setUploadResult(null);
    }
  };

  return (
    <GenericModal
      variant="info"
      display={display}
      setDisplay={setDisplay}
      headerIcon={<FaUpload size={22} />}
      title="Upload Shapefile"
      message={
        <ShapeUploadContainer
          formikRef={formikRef}
          uploadResult={uploadResult}
          onUploadFile={onUploadFileHandler}
          View={ShapeUploadForm}
          ResultsView={ShapeUploadResultView}
        />
      }
      errorMessage={
        displayConfirmation ? (
          <StyledUnsavedChanges>
            Unsaved updates will be lost. Click &quot;<strong>No</strong>&quot; again to proceed
            without saving, or &quot;<strong>Yes</strong>&quot; to save the changes.
          </StyledUnsavedChanges>
        ) : undefined
      }
      modalSize={ModalSize.LARGE}
      okButtonText={exists(uploadResult) ? 'Close' : 'Yes'}
      handleOk={() => (exists(uploadResult) ? onCloseHandler() : onSaveHandler())}
      cancelButtonText={exists(uploadResult) ? undefined : 'No'}
      handleCancel={() => onCloseHandler()}
    />
  );
};

const StyledUnsavedChanges = styled.p`
  color: ${props => props.theme.bcTokens.iconsColorDanger};
  margin: 0;
  padding-right: 5rem;
`;
