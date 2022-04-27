import { LinkButton } from 'components/common/buttons';
import noop from 'lodash/noop';
import React from 'react';
import { ListGroup } from 'react-bootstrap';

export interface ILayerPopupFlyoutProps {
  onViewPropertyInfo: () => void;
  onCreateResearchFile?: () => void;
}

export const LayerPopupFlyout: React.FC<ILayerPopupFlyoutProps> = ({
  onViewPropertyInfo,
  onCreateResearchFile,
}) => {
  return (
    <ListGroup variant="flush">
      <ListGroup.Item>
        <LinkButton onClick={onViewPropertyInfo}>View more property info</LinkButton>
      </ListGroup.Item>
      <ListGroup.Item>
        <LinkButton onClick={onCreateResearchFile ?? noop}>Research File - Create new</LinkButton>
      </ListGroup.Item>
    </ListGroup>
  );
};
