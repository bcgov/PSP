import classNames from 'classnames';
import { LandSvg } from 'components/common/Icons';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { SidebarContextType, SidebarSize } from 'features/mapSideBar/hooks/useQueryParamSideBar';
import * as React from 'react';
import { FaWindowClose } from 'react-icons/fa';
import VisibilitySensor from 'react-visibility-sensor';
import styled from 'styled-components';

interface IMapSideBarLayoutProps {
  /** toggle the display of the entire side bar drawer */
  show: boolean;
  /** function allowing the control of the side bar drawer */
  setShowSideBar: (
    show: boolean,
    contextName?: SidebarContextType,
    size?: SidebarSize,
    resetIds?: boolean,
  ) => void;
  /** the react component to display as the sidebar title */
  title: React.ReactNode;
  /** togglebar sidebar size */
  size?: SidebarSize;
  /** property name for title */
  propertyName?: string;
}

const HeaderRow = styled.div`
  display: flex;
  align-items: center;
  height: 4rem;
`;

const CloseIcon = styled(FaWindowClose)`
  color: ${props => props.theme.css.textColor};
  font-size: 30px;
  cursor: pointer;
`;

const Title = styled.span`
  font-size: 32px;
  font-weight: 700;
  width: 100%;
  text-align: left;
  border-bottom: 5px solid;
  border-color: ${props => props.theme.css.primaryColor};
  color: ${props => props.theme.css.textColor};
`;

const LargeLandSvg = styled(LandSvg)`
  height: 32px;
  width: 32px;
`;

const Layout = styled.div`
  .tab-wrapper {
    height: calc(100vh - 46px - 72px - 45px - 310px);
  }
`;

/**
 * SideBar layout with control bar and then form content passed as child props.
 * @param {IMapSideBarLayoutProps} param0
 */
export const MapSideBarLayout: React.FunctionComponent<IMapSideBarLayoutProps> = ({
  show,
  setShowSideBar,
  title,
  size,
  propertyName,
  ...props
}) => {
  return (
    <div
      className={classNames('map-side-drawer', show ? 'show' : null, {
        close: !show,
        narrow: size === 'narrow',
      })}
    >
      <Layout>
        <VisibilitySensor partialVisibility={true}>
          {({ isVisible }: any) => (
            <>
              <HeaderRow>
                <LargeLandSvg className="svg" />
                <Title className="mr-auto">{title}</Title>
                <TooltipWrapper toolTipId="close-sidebar-tooltip" toolTip="Close Form">
                  <CloseIcon
                    title="close"
                    onClick={() => setShowSideBar(false, undefined, undefined, true)}
                  />
                </TooltipWrapper>
              </HeaderRow>
              {isVisible ? props.children : null}
            </>
          )}
        </VisibilitySensor>
      </Layout>
    </div>
  );
};

export default MapSideBarLayout;
