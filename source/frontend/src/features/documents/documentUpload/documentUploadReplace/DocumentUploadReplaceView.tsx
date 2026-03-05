import clsx from 'classnames';
import { FunctionComponent } from 'react';
import { Col, Row } from 'react-bootstrap';

import { Button } from '@/components/common/buttons';
import FileDragAndDrop from '@/components/common/form/FileDragAndDrop';
import ValidDocumentExtensions from '@/constants/ValidDocumentExtensions';
import { exists } from '@/utils/utils';

export interface IDocumentUploadReplaceViewProps {
  className?: string;
  file: File | null;
  onSelectedReplacementFile: (files: File[]) => void;
  onConfirmReplace: () => void;
  onCancelReplace: () => void;
}

const DocumentUploadReplaceView: FunctionComponent<IDocumentUploadReplaceViewProps> = ({
  file,
  className,
  onConfirmReplace,
  onCancelReplace,
  onSelectedReplacementFile,
}) => {
  return (
    <div>
      <Row className={clsx('no-gutters', 'pb-3', className)}>
        <Col>
          <FileDragAndDrop
            onSelectFiles={onSelectedReplacementFile}
            validExtensions={ValidDocumentExtensions}
            multiple={false}
          />
        </Col>
      </Row>

      <Row className={clsx('no-gutters', 'pb-3', className)}>
        <Col md="8">
          <p>
            Replace with file? : <span data-testid="file-name">{file?.name ?? ''}</span>
          </p>
        </Col>
        <Col md="2">
          <Button
            title="cancel-replace"
            variant={'secondary'}
            onClick={onCancelReplace}
            data-testid="cancel-replace-button"
          >
            {'Cancel'}
          </Button>
        </Col>
        <Col md="2">
          <Button
            title="confirm-replace"
            variant={'primary'}
            onClick={onConfirmReplace}
            data-testid="ok-replace-button"
            disabled={!exists(file)}
          >
            {'Replace'}
          </Button>
        </Col>
      </Row>
    </div>
  );
};

export default DocumentUploadReplaceView;
