import { Button } from 'components/common/buttons/Button';
import { Col, Row } from 'react-bootstrap';

interface ISidebarFooterProps {
  isOkDisabled?: boolean;
  onSave: () => void;
  onCancel: () => void;
}

const SidebarFooter: React.FunctionComponent<ISidebarFooterProps> = props => {
  return (
    <Row className="justify-content-end mt-auto no-gutters pt-3">
      <Col xs="auto" className="pr-4">
        <Button variant="secondary" onClick={props.onCancel}>
          Cancel
        </Button>
      </Col>
      <Col xs="auto">
        <Button disabled={props.isOkDisabled} onClick={props.onSave}>
          Save
        </Button>
      </Col>
    </Row>
  );
};

export default SidebarFooter;
