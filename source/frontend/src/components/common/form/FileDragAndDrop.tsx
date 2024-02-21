import { ChangeEvent, FunctionComponent, useState } from 'react';
import styled from 'styled-components';

import { SectionField } from '../Section/SectionField';

interface IFileDragAndDropProps {
  onSelectFile: (file: File | null) => void;
  selectedFile: File | null;
  validExtensions: string[];
}

const FileDragAndDrop: FunctionComponent<React.PropsWithChildren<IFileDragAndDropProps>> = ({
  onSelectFile,
  selectedFile,
  validExtensions,
}) => {
  const validDocumentExtensions: string = validExtensions.map(x => `.${x}`).join(',');

  const [isDragging, setIsDragging] = useState(false);

  const handleFileInput = (changeEvent: ChangeEvent<HTMLInputElement>) => {
    // handle validations
    if (changeEvent.target !== null) {
      const target = changeEvent.target;
      if (target.files !== null && target.files.length > 0) {
        onSelectFile(target.files[0]);
      }
    }
  };

  const handleDragEnter = (event: React.DragEvent) => {
    event.preventDefault();
    event.stopPropagation();

    setIsDragging(true);
  };

  const handleDragLeave = (event: React.DragEvent) => {
    event.preventDefault();
    event.stopPropagation();

    setIsDragging(false);
  };

  const handleDragOver = (event: React.DragEvent) => {
    event.preventDefault();
    event.stopPropagation();

    setIsDragging(true);
  };

  const handleDrop = (event: React.DragEvent) => {
    event.preventDefault();
    event.stopPropagation();

    const files = [...event.dataTransfer.files];

    if (files && files.length > 0) {
      onSelectFile(files[0]);
      event.dataTransfer.clearData();
    }

    setIsDragging(false);
  };

  const shortenString = (text: string, maxLength: number, terminator = '...'): string => {
    if (text.length > maxLength) {
      return text.substring(0, maxLength - terminator.length) + terminator;
    }

    return text;
  };

  return (
    <div>
      <DragDropZone
        onDrop={handleDrop}
        onDragOver={handleDragOver}
        onDragEnter={handleDragEnter}
        onDragLeave={handleDragLeave}
        isDragging={isDragging}
      >
        <StyledContent>
          Drag files here to attach or{' '}
          <StyledUploadLabel>
            Browse
            <StyledFileInput
              data-testid="upload-input"
              id="uploadInput"
              type="file"
              name="documentFile"
              accept={validDocumentExtensions}
              onChange={handleFileInput}
              className=""
            />
          </StyledUploadLabel>
        </StyledContent>
      </DragDropZone>
      {selectedFile !== null && (
        <StyledSelectedFile>
          <SectionField label="File to add" labelWidth="3">
            {shortenString(selectedFile.name || '', 20)}
          </SectionField>
        </StyledSelectedFile>
      )}
    </div>
  );
};

export default FileDragAndDrop;

const DragDropZone = styled.div<{ isDragging: boolean }>`
  border: 1px solid ${({ theme }) => theme.css.lightVariantColor};

  border-style: ${({ isDragging }) => (isDragging ? 'solid' : 'dashed')};

  border: ${props => (props.isDragging ? `1px solid ${props.theme.css.draftColor}` : '')};

  background-color: ${props => (props.isDragging ? props.theme.css.filterBoxColor : '')};

  height: 10rem;
  line-height: 10rem;
  text-align: center;
`;

const StyledContent = styled.div`
  width: 100%;
  display: inline-block;
  vertical-align: middle;
  line-height: normal;
`;

const StyledUploadLabel = styled.label`
  display: inline-block;
  color: ${({ theme }) => theme.css.linkColor};
  cursor: pointer;

  &:hover {
    color: ${({ theme }) => theme.css.linkHoverColor};
    text-decoration: underline;
  }
`;

const StyledFileInput = styled.input`
  display: none;
`;

const StyledSelectedFile = styled.div`
  padding-top: 2rem;
  overflow: hide;
  word-wrap: break-word;
`;
