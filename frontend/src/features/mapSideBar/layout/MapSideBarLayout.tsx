import * as Styled from 'components/common/styles';
import TooltipWrapper from 'components/common/TooltipWrapper';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaWindowClose } from 'react-icons/fa';
import VisibilitySensor from 'react-visibility-sensor';
import styled from 'styled-components';

interface IMapSideBarLayoutProps {
  showSideBar: boolean;
  setShowSideBar: (show: boolean) => void;
  title: React.ReactNode;
  header?: React.ReactNode;
  icon: React.ReactNode | React.FunctionComponent;
  showCloseButton?: boolean;
}

/**
 * SideBar layout with control bar and then form content passed as child props.
 * @param param0
 */
const MapSideBarLayout: React.FunctionComponent<IMapSideBarLayoutProps> = ({
  showSideBar,
  setShowSideBar,
  title,
  header,
  icon,
  showCloseButton,
  ...props
}) => {
  return (
    <StyledMapSideBarLayout show={showSideBar}>
      <VisibilitySensor partialVisibility={true}>
        {({ isVisible }: any) => (
          <>
            <TitleBar>
              <Row>
                <Col>
                  <Styled.H1 className="mr-auto">
                    {icon}
                    {title}
                  </Styled.H1>
                </Col>

                {showCloseButton && (
                  <Col xs="auto">
                    <TooltipWrapper toolTipId="close-sidebar-tooltip" toolTip="Close Form">
                      <CloseIcon title="close" onClick={() => setShowSideBar(false)} />
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
          </>
        )}
      </VisibilitySensor>
    </StyledMapSideBarLayout>
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

const Header = styled.div``;

const TitleBar = styled.div``;

const Underline = styled.div`
  width: 100%;
  border-bottom: solid 0.5rem ${props => props.theme.css.primaryLightColor};
`;

const StyledMapSideBarLayout = styled.div<{ show: boolean }>`
  display: flex;
  flex-flow: column;
  h1 {
    border-bottom: none;
    margin-bottom: 0.2rem;
  }

  min-width: ${props => (props.show ? `93rem` : `0rem`)};
  width: ${props => (props.show ? `93rem` : `0rem`)};
  max-width: ${props => (props.show ? `93rem` : `0rem`)};

  padding: ${props => (props.show ? `1.4rem 3.6rem` : `0rem`)};
  padding-bottom: ${props => (props.show ? `2rem` : `0rem`)};

  overflow: hidden;
  transition: 1s;
`;

const CloseIcon = styled(FaWindowClose)`
  color: ${props => props.theme.css.textColor};
  font-size: 30px;
  cursor: pointer;
`;

export default MapSideBarLayout;
