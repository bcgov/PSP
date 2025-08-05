import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';

interface ISidebarFooterProps {
  isOkDisabled?: boolean;
  onSave: () => void;
  showEdit?: boolean;
  editMode?: boolean;
  onEdit?: (editMode: boolean) => void;
  onCancel: () => void;
  displayRequiredFieldError: boolean;
  saveButtonLabel?: string;
  cancelButtonLabel?: string;
}

const SidebarFooter: React.FunctionComponent<ISidebarFooterProps> = ({
  showEdit,
  editMode,
  onEdit,
  onSave,
  onCancel,
  isOkDisabled,
  displayRequiredFieldError,
  saveButtonLabel,
  cancelButtonLabel,
}) => {
  return (
    <SidebarFooterBar className="justify-content-end pt-4 p-0 no-gutters">
      {(!showEdit || editMode) && (
        <>
          <Col xs="auto" className="pr-3">
            {displayRequiredFieldError && (
              <StyledError>Please check form fields for errors.</StyledError>
            )}
          </Col>
          <Col xs="auto" className="pr-6">
            <Button variant="secondary" onClick={onCancel} data-testid="cancel-button">
              {cancelButtonLabel ?? 'Cancel'}
            </Button>
          </Col>
          <Col xs="auto">
            <Button
              disabled={isOkDisabled}
              onClick={onSave}
              className="mr-9"
              data-testid="save-button"
            >
              {saveButtonLabel ?? 'Save'}
            </Button>
          </Col>
        </>
      )}
      {showEdit && !editMode && (
        <Col xs="auto">
          <Button
            onClick={() => {
              onEdit && onEdit(true);
            }}
          >
            Edit
          </Button>
        </Col>
      )}
    </SidebarFooterBar>
  );
};

const SidebarFooterBar = styled(Row)`
  position: sticky;
  padding-top: 2rem;
  bottom: 0;
  border-top: 2px solid #e5e5e5;
  background: white;
  z-index: 10;
`;

const StyledError = styled.div`
  padding-top: 0.7rem;
  color: red;
`;

export default SidebarFooter;
