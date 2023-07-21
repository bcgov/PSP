import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaWindowClose } from 'react-icons/fa';
import VisibilitySensor from 'react-visibility-sensor';
import styled from 'styled-components';

import * as Styled from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';

interface IMapSideBarLayoutProps {
  title: React.ReactNode;
  header?: React.ReactNode;
  icon: React.ReactNode | React.FunctionComponent<React.PropsWithChildren<unknown>>;
  footer?: React.ReactNode | React.FunctionComponent<React.PropsWithChildren<unknown>>;
  showCloseButton?: boolean;
  onClose?: () => void;
}

/**
 * SideBar layout with control bar and then form content passed as child props.
 * @param param0
 */
const MapSideBarLayout: React.FunctionComponent<
  React.PropsWithChildren<IMapSideBarLayoutProps>
> = ({ title, header, icon, showCloseButton, ...props }) => {
  return (
    <VisibilitySensor partialVisibility={true}>
      {({ isVisible }: any) => (
        <>
          <TitleBar>
            <Row>
              <Col>
                <Styled.H1 className="mr-auto">
                  <>
                    {icon}
                    {title}
                  </>
                </Styled.H1>
              </Col>

              {showCloseButton && (
                <Col xs="auto">
                  <TooltipWrapper toolTipId="close-sidebar-tooltip" toolTip="Close Form">
                    <CloseIcon title="close" onClick={props.onClose} />
                  </TooltipWrapper>
                </Col>
              )}
            </Row>
            <Underline />
          </TitleBar>

          {header && isVisible && <Header>{header}</Header>}

          <StyledBody>
            <Content>{isVisible ? props.children : null}</Content>
          </StyledBody>

          {props.footer && isVisible && <Footer>{props.footer as React.ReactNode}</Footer>}
        </>
      )}
    </VisibilitySensor>
  );
};

const StyledBody = styled.div`
  width: 100%;
  position: relative;
  overflow: auto;
  flex: 1;
`;

const Content = styled.div`
  height: 100%;
  width: 100%;
`;

const TitleBar = styled.div``;

const Header = styled.div`
  position: relative;
`;

const Footer = styled.div``;

const Underline = styled.div`
  width: 100%;
  border-bottom: solid 0.5rem ${props => props.theme.css.primaryLightColor};
`;

const CloseIcon = styled(FaWindowClose)`
  color: ${props => props.theme.css.textColor};
  font-size: 30px;
  cursor: pointer;
`;

export default MapSideBarLayout;
