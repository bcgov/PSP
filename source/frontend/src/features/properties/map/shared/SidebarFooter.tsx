import { Button } from 'components/common/buttons/Button';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

interface ISidebarFooterProps {
  isOkDisabled?: boolean;
  onSave: () => void;
  showEdit?: boolean;
  editMode?: boolean;
  onEdit?: (editMode: boolean) => void;
  onCancel: () => void;
}

const SidebarFooter: React.FunctionComponent<ISidebarFooterProps> = ({
  showEdit,
  editMode,
  onEdit,
  onSave,
  onCancel,
  isOkDisabled,
}) => {
  return (
    <SidebarFooterBar className="justify-content-end mt-auto no-gutters">
      {(!showEdit || editMode) && (
        <>
          <Col xs="auto" className="pr-4">
            <Button variant="secondary" onClick={onCancel}>
              Cancel
            </Button>
          </Col>
          <Col xs="auto">
            <Button disabled={isOkDisabled} onClick={onSave}>
              Save
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
  bottom: 0;
  background: white;
  z-index: 10;
`;

export default SidebarFooter;
