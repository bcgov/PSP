import { ChangeEvent, FunctionComponent, useState } from 'react';
import styled from 'styled-components';

interface IFileDragAndDropProps {
  onSelectFiles: (files: File[]) => void;
  validExtensions: string[];
  multiple?: boolean;
}

const FileDragAndDrop: FunctionComponent<React.PropsWithChildren<IFileDragAndDropProps>> = ({
  onSelectFiles,
  validExtensions,
  multiple = true,
}) => {
  const validDocumentExtensions: string = validExtensions.map(x => `.${x}`).join(',');

  const [isDragging, setIsDragging] = useState(false);

  const handleFileInput = (changeEvent: ChangeEvent<HTMLInputElement>) => {
    // handle validations
    if (changeEvent.target !== null) {
      const target = changeEvent.target;
      if (target.files !== null && target.files.length > 0) {
        onSelectFiles([...target.files]);
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
      onSelectFiles([...files]);
      event.dataTransfer.clearData();
    }

    setIsDragging(false);
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
              multiple={multiple}
            />
          </StyledUploadLabel>
        </StyledContent>
      </DragDropZone>
    </div>
  );
};

export default FileDragAndDrop;

const DragDropZone = styled.div<{ isDragging: boolean }>`
  border: 1px solid ${({ theme }) => theme.css.borderOutlineColor};

  border-style: ${({ isDragging }) => (isDragging ? 'solid' : 'dashed')};

  border: ${props => (props.isDragging ? `1px solid ${props.theme.css.activeActionColor}` : '')};

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
