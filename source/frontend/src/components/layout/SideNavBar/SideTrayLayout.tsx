import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { CloseIcon, Underline } from '@/components/common/styles';
import { TooltipWrapper } from '@/components/common/TooltipWrapper';

import * as Styled from './styles';

export interface ISideTrayLayoutProps {
  title: React.ReactNode;
  icon: React.ReactNode;
  onClose: () => void;
}

export const SideTrayLayout: React.FC<React.PropsWithChildren<ISideTrayLayoutProps>> = ({
  title,
  icon,
  onClose,
  children,
}) => {
  return (
    <Styled.SideTrayPage>
      <Row>
        <Col>
          <Styled.TrayHeader className="mr-auto">
            <>
              <div className="mr-2 mb-1">{icon}</div>
              {title}
            </>
          </Styled.TrayHeader>
        </Col>
        <Styled.ButtonBar xs="auto" className="d-flex">
          <TooltipWrapper tooltipId="close-sidebar-tooltip" tooltip="Close Tray">
            <CloseIcon title="close" onClick={onClose} />
          </TooltipWrapper>
        </Styled.ButtonBar>
      </Row>
      <Underline />
      <Styled.Content>{children}</Styled.Content>
    </Styled.SideTrayPage>
  );
};
