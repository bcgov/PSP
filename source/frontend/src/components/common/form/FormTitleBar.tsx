import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaWindowClose } from 'react-icons/fa';
import styled from 'styled-components';

import * as BaseStyles from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';

export interface IFormTitleBarProps {
  title: React.ReactNode;
  icon: React.ReactNode | React.FC<React.PropsWithChildren<unknown>>;
  showCloseButton?: boolean;
  onClose?: () => void;
}

export const FormTitleBar: React.FC<IFormTitleBarProps> = ({
  title,
  icon,
  showCloseButton,
  onClose,
}) => {
  return (
    <>
      <Row>
        <Col>
          <TitleBarH1 className="mr-auto">
            <>
              {icon}
              {title}
            </>
          </TitleBarH1>
        </Col>

        {showCloseButton && (
          <Col xs="auto">
            <TooltipWrapper tooltipId="close-sidebar-tooltip" tooltip="Close Form">
              <CloseIcon title="close" onClick={onClose} />
            </TooltipWrapper>
          </Col>
        )}
      </Row>
      <Underline />
    </>
  );
};

const Underline = styled.div`
  width: 100%;
  border-bottom: solid 0.5rem ${props => props.theme.bcTokens.surfaceColorBackgroundDarkBlue};
`;

const CloseIcon = styled(FaWindowClose)`
  color: ${props => props.theme.bcTokens.typographyColorSecondary};
  font-size: 30px;
  cursor: pointer;
`;

// override default H1 styling for the title bar
const TitleBarH1 = styled(BaseStyles.H1)`
  && {
    border-bottom: none;
    margin-bottom: 0.2rem;
  }
`;
