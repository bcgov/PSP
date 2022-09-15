import { Button } from 'components/common/buttons/Button';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

interface ISidebarFooterProps {
  isOkDisabled?: boolean;
  onSave: () => void;
  onCancel: () => void;
}

const SidebarFooter: React.FunctionComponent<ISidebarFooterProps> = props => {
  return (
    <SidebarFooterBar className="justify-content-end mt-auto no-gutters">
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
