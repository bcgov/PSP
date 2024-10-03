import { createRef, useState } from 'react';
import { FaCheck, FaTimesCircle, FaUpload } from 'react-icons/fa';
import styled, { useTheme } from 'styled-components';

import GenericModal, { ModalSize } from '@/components/common/GenericModal';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { exists } from '@/utils';

import { BatchUploadResponseModel } from '../ComposedDocument';
import DocumentUploadContainer, { IDocumentUploadContainerRef } from './DocumentUploadContainer';

export interface IDocumentUploadModalProps {
  parentId: string;
  relationshipType: ApiGen_CodeTypes_DocumentRelationType;
  maxDocumentCount: number;
  display?: boolean;
  setDisplay?: (display: boolean) => void;
  onUploadSuccess: () => void;
  onClose: () => void;
}

export const DocumentUploadModal: React.FunctionComponent<IDocumentUploadModalProps> = props => {
  const [uploadResult, setUploadResult] = useState<BatchUploadResponseModel[]>(null);

  const [canUpload, setCanUpload] = useState(false);

  const uploadContainerRef = createRef<IDocumentUploadContainerRef>();

  const onSaveClick = () => {
    uploadContainerRef.current?.uploadDocument();
  };
  const onSuccess = (results: BatchUploadResponseModel[]) => {
    setCanUpload(false);
    setUploadResult(results);
  };

  const onClose = () => {
    setCanUpload(false);
    setUploadResult(null);
    props.onClose();
  };

  return (
    <GenericModal
      variant="info"
      display={props.display}
      setDisplay={props.setDisplay}
      headerIcon={<FaUpload size={22} />}
      title="Add Document"
      message={
        exists(uploadResult) ? (
          <UploadResultView uploadResult={uploadResult} />
        ) : (
          <DocumentUploadContainer
            ref={uploadContainerRef}
            parentId={props.parentId}
            relationshipType={props.relationshipType}
            onUploadSuccess={onSuccess}
            onCancel={props.onClose}
            setCanUpload={setCanUpload}
            maxDocumentCount={props.maxDocumentCount}
          />
        )
      }
      modalSize={ModalSize.LARGE}
      okButtonText={exists(uploadResult) ? 'Close' : 'Yes'}
      handleOk={() => (exists(uploadResult) ? onClose() : onSaveClick())}
      handleOkDisabled={exists(uploadResult) ? false : !canUpload}
      cancelButtonText={exists(uploadResult) ? undefined : 'No'}
      handleCancel={() => {
        onClose();
      }}
    />
  );
};

interface IUploadResultViewProps {
  uploadResult: BatchUploadResponseModel[];
}

const UploadResultView: React.FunctionComponent<IUploadResultViewProps> = props => {
  const theme = useTheme();

  const successResults: BatchUploadResponseModel[] = [];
  const failureResults: BatchUploadResponseModel[] = [];

  props.uploadResult.forEach(result => {
    if (result.isSuccess) {
      successResults.push(result);
    } else {
      failureResults.push(result);
    }
  });
  return (
    <>
      {successResults.length > 0 && (
        <>
          <div className="pb-2">
            <FaCheck className="mr-2" size="1.6rem" color={theme.bcTokens.iconsColorSuccess} />
            <span>{`${successResults.length} files successfully uploaded`}</span>
          </div>
          {successResults.map((result, index) => (
            <StyledSuccessDiv key={`document-upload-result-success-${index}`}>
              {result.fileName}
            </StyledSuccessDiv>
          ))}
        </>
      )}
      {failureResults.length > 0 && (
        <>
          <div className="py-2">
            <FaTimesCircle className="mr-2" size="1.6rem" color={theme.bcTokens.iconsColorDanger} />
            <span>{`${failureResults.length} files have not been uploaded`}</span>
          </div>
          {failureResults.map((result, index) => (
            <StyledFailDiv key={`document-upload-result-fail-${index}`}>
              {result.fileName}
              {` ( ${result.errorMessage})`}
            </StyledFailDiv>
          ))}
        </>
      )}
    </>
  );
};

const StyledSuccessDiv = styled.div`
  color: ${props => props.theme.bcTokens.iconsColorSuccess};
  padding-left: 2.6rem;
`;

const StyledFailDiv = styled.div`
  color: ${props => props.theme.bcTokens.iconsColorDanger};
  padding-left: 2.6rem;
`;
