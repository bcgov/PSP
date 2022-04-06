import { Button } from 'components/common/form';
import noop from 'lodash/noop';
import React from 'react';
import { ListGroup } from 'react-bootstrap';

export interface ILayerPopupFlyoutProps {
  onViewPropertyInfo?: () => void;
  onCreateResearchFile?: () => void;
}

export const LayerPopupFlyout: React.FC<ILayerPopupFlyoutProps> = ({
  onViewPropertyInfo,
  onCreateResearchFile,
}) => {
  return (
    <ListGroup variant="flush">
      <ListGroup.Item>
        <Button variant="link" onClick={onViewPropertyInfo ?? noop}>
          View more property info
        </Button>
      </ListGroup.Item>
      <ListGroup.Item>
        <Button variant="link" onClick={onCreateResearchFile ?? noop}>
          Research File - Create new
        </Button>
      </ListGroup.Item>
    </ListGroup>
  );
};
