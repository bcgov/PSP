import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaWindowClose } from 'react-icons/fa';
import styled from 'styled-components';

import { ExpandCollapseButton } from '@/components/common/buttons/ExpandCollapseButton';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import * as Styled from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';

export interface IMapSideBarLayoutProps {
  title: React.ReactNode;
  header?: React.ReactNode;
  icon: React.ReactNode;
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
  const { mapSideBarViewState, toggleSidebarDisplay } = useMapStateMachine();
  return (
    <StyledSidebarWrapper className={mapSideBarViewState.isCollapsed ? '' : 'expanded'}>
      {mapSideBarViewState.isCollapsed ? (
        <>
          <Row>
            <Col xs={12} className="justify-content-center d-flex">
              {icon}
            </Col>
          </Row>
          <Underline className="mb-4" />
          <Row>
            <StyledButtonBar xs={12} className="align-items-center d-flex flex-column-reverse">
              <TooltipWrapper tooltipId="expand-sidebar-tooltip-expand" tooltip={'Expand Screen'}>
                <span>
                  <ExpandCollapseButton
                    expanded={!mapSideBarViewState.isCollapsed}
                    toggleExpanded={toggleSidebarDisplay}
                  />
                </span>
              </TooltipWrapper>
              {showCloseButton && (
                <TooltipWrapper tooltipId="close-sidebar-tooltip" tooltip="Close Form">
                  <CloseIcon title="close" onClick={props.onClose} />
                </TooltipWrapper>
              )}
            </StyledButtonBar>
          </Row>
        </>
      ) : (
        <>
          <Row>
            <Col>
              <Styled.H1 className="mr-auto">
                <>
                  <span className="mr-2">{icon}</span>
                  {title}
                </>
              </Styled.H1>
            </Col>

            <StyledButtonBar xs="auto" className="d-flex">
              <TooltipWrapper
                tooltipId="expand-sidebar-tooltip-collapse"
                tooltip={'Collapse Screen'}
              >
                <span>
                  <ExpandCollapseButton
                    expanded={!mapSideBarViewState.isCollapsed}
                    toggleExpanded={toggleSidebarDisplay}
                  />
                </span>
              </TooltipWrapper>
              <Styled.VerticalLine />
              {showCloseButton && (
                <TooltipWrapper tooltipId="close-sidebar-tooltip" tooltip="Close Form">
                  <CloseIcon title="close" onClick={props.onClose} />
                </TooltipWrapper>
              )}
            </StyledButtonBar>
          </Row>
          <Underline />
        </>
      )}
      <Content className={mapSideBarViewState.isCollapsed ? 'd-none' : ''}>
        {header && <Header>{header}</Header>}
        <StyledBody>{props.children}</StyledBody>
        {props.footer && <Footer>{props.footer as React.ReactNode}</Footer>}
      </Content>
    </StyledSidebarWrapper>
  );
};

const StyledSidebarWrapper = styled.div`
  &.expanded {
    min-width: 90rem;
    height: 100%;
    position: relative;

    display: flex;
    flex-direction: column;
    overflow: hidden;
  }
  padding: 1.6rem;
  color: ${props => props.theme.bcTokens.typographyColorSecondary};
`;

const StyledButtonBar = styled(Col)`
  gap: 1.5rem;
  align-items: center;
`;

const StyledBody = styled.div`
  width: 100%;
  position: relative;
  overflow: auto;
  flex: 1;
`;

const Content = styled.div`
  width: 100%;
  height: 100%;
  display: contents;
`;

const Header = styled.div`
  position: relative;
`;

const Footer = styled.div``;

const Underline = styled.div`
  width: 100%;
  border-bottom: solid 0.5rem ${props => props.theme.bcTokens.themeBlue80};
`;

const CloseIcon = styled(FaWindowClose)`
  color: ${props => props.theme.css.textColor};
  font-size: 2.4rem;
  cursor: pointer;
`;

export default MapSideBarLayout;
