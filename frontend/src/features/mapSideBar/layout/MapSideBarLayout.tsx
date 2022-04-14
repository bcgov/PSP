import { ReactComponent as LotSvg } from 'assets/images/icon-lot.svg';
import classNames from 'classnames';
import * as Styled from 'components/common/styles';
import TooltipWrapper from 'components/common/TooltipWrapper';
import * as React from 'react';
import { FaWindowClose } from 'react-icons/fa';
import VisibilitySensor from 'react-visibility-sensor';
import styled from 'styled-components';

interface IMapSideBarLayoutProps {
  show: boolean;
  setShowSideBar: (show: boolean) => void;
  title: React.ReactNode;
  hidePolicy?: boolean;
  /** property name for title */
  propertyName?: string;
  header: React.ReactNode;
}

/**
 * SideBar layout with control bar and then form content passed as child props.
 * @param param0
 */
const MapSideBarLayout: React.FunctionComponent<IMapSideBarLayoutProps> = ({
  show,
  setShowSideBar,
  hidePolicy,
  title,
  propertyName,
  ...props
}) => {
  return (
    <StyledMapSideBarLayout
      className={classNames('map-side-drawer', {
        show: show,
      })}
    >
      <VisibilitySensor partialVisibility={true}>
        {({ isVisible }: any) => (
          <>
            <TitleBar>
              <Underline>
                <LotIcon className="mr-1" />
                <Styled.H1 className="mr-auto">{title}</Styled.H1>

                <TooltipWrapper toolTipId="close-sidebar-tooltip" toolTip="Close Form">
                  <CloseIcon title="close" onClick={() => setShowSideBar(false)} />
                </TooltipWrapper>
              </Underline>
            </TitleBar>
            <Header>{props.header}</Header>
            <Content>{isVisible ? props.children : null}</Content>
          </>
        )}
      </VisibilitySensor>
    </StyledMapSideBarLayout>
  );
};

const Content = styled.div`
  grid-area: content;
  width: 100%;
  height: 100%;
  top: 2rem;
  position: absolute;
  margin-top: 1rem;
`;

const Header = styled.div`
  grid-area: header;
  position: relative;
`;

const LotIcon = styled(LotSvg)`
  width: 3rem;
  height: 3rem;
  align-self: flex-end;
`;

const TitleBar = styled.div`
  grid-area: title;
  display: flex;
`;

const Underline = styled.div`
  width: 100%;
  display: flex;
  border-bottom: solid 0.5rem ${props => props.theme.css.primaryLightColor};
`;

const StyledMapSideBarLayout = styled.div`
  h1 {
    border-bottom: none;
  }
  height: calc(
    100vh - ${props => props.theme.css.headerHeight} - ${props => props.theme.css.footerHeight}
  );
  min-width: 93rem;
  margin-left: -93rem;
  max-width: 93rem;
  padding: 2.4rem 3.6rem;
  display: grid;
  grid: 4.2rem 7rem 1fr / 1fr;
  grid-template-areas:
    'title'
    'header'
    'content';
  transition: 1s;
  overflow: hidden;
  position: absolute;
  &.show {
    margin-left: 0;
  }
`;

const CloseIcon = styled(FaWindowClose)`
  color: ${props => props.theme.css.textColor};
  font-size: 30px;
  cursor: pointer;
`;

export default MapSideBarLayout;
