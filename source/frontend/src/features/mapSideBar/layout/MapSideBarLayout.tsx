import React, { useCallback } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ExpandCollapseButton } from '@/components/common/buttons/ExpandCollapseButton';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import * as Styled from '@/components/common/styles';
import { H1 } from '@/components/common/styles';
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
> = ({ title, header, icon, showCloseButton, onClose, ...props }) => {
  const { mapSideBarViewState, toggleSidebarDisplay } = useMapStateMachine();

  const close = useCallback(() => {
    if (typeof onClose === 'function') {
      onClose();
    }
  }, [onClose]);

  return (
    <StyledSidebarWrapper className={mapSideBarViewState.isCollapsed ? 'collapsed' : 'expanded'}>
      {mapSideBarViewState.isCollapsed ? (
        <>
          <Row>
            <StyledCollapsedIconWrapper xs={12} className="justify-content-center d-flex">
              {icon}
            </StyledCollapsedIconWrapper>
          </Row>
          <Styled.Underline className="mb-4" />
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
                  <Styled.CloseIcon title="close" onClick={close} />
                </TooltipWrapper>
              )}
            </StyledButtonBar>
          </Row>
        </>
      ) : (
        <>
          <Row>
            <Col>
              <StyledExpandedHeader>
                <>
                  <div className="mr-2 mb-1">{icon}</div>
                  <div data-testid="form-title">{title}</div>
                </>
              </StyledExpandedHeader>
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
                  <Styled.CloseIcon title="close" onClick={close} />
                </TooltipWrapper>
              )}
            </StyledButtonBar>
          </Row>
          <Styled.Underline />
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
  &.collapsed .row:first {
    svg {
      width: 2.6rem;
      height: 2.6rem;
      margin-right: 0;
    }
  }

  &.expanded {
    min-width: 90rem;
    @media only screen and (max-width: 1199px) {
      min-width: 70rem;
    }
    height: 100%;
    position: relative;

    display: flex;
    flex-direction: column;
    overflow: hidden;
  }
  padding: 1.6rem;
  color: ${props => props.theme.bcTokens.typographyColorSecondary};
`;

const StyledCollapsedIconWrapper = styled(Col)`
  margin-bottom: 0.3rem;
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

const StyledExpandedHeader = styled(H1)`
  display: flex;
  align-items: end;
`;

export default MapSideBarLayout;
