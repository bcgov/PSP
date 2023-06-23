import { title } from 'process';
import * as React from 'react';
import { FaWindowClose } from 'react-icons/fa';
import styled from 'styled-components';

import { ReactComponent as LotSvg } from '@/assets/images/icon-lot.svg';
import * as Styled from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';

interface IPropertyInfoLayoutProps {
  setShowSideBar: (show: boolean) => void;
}

const PropertyInfoLayout: React.FunctionComponent<
  React.PropsWithChildren<IPropertyInfoLayoutProps>
> = ({ children, setShowSideBar }) => {
  return (
    <StyledPropertyInfoLayout>
      <TitleBar>
        <Underline>
          <LotIcon className="mr-1" />
          <Styled.H1 className="mr-auto">{title}</Styled.H1>
        </Underline>
        <TooltipWrapper toolTipId="close-sidebar-tooltip" toolTip="Close Form">
          <CloseIcon title="close" onClick={() => setShowSideBar(false)} />
        </TooltipWrapper>
      </TitleBar>
      <Header>Placeholder</Header>
      <Content>{children}</Content>
    </StyledPropertyInfoLayout>
  );
};

const CloseIcon = styled(FaWindowClose)`
  color: ${props => props.theme.css.textColor};
  font-size: 30px;
  cursor: pointer;
`;

const Content = styled.div`
  grid-area: content;
  width: 100%;
  height: 100%;
  position: absolute;
  box-sizing: border-box;
`;

const Header = styled.div`
  grid-area: header;
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

const StyledPropertyInfoLayout = styled.div`
  h1 {
    border-bottom: none;
  }
  width: 100%;
  height: 100%;
  position: relative;
  display: grid;
  grid: 4.2rem 5.8rem 1fr / 1fr;
  grid-template-areas:
    'title'
    'header'
    'content';
`;

export default PropertyInfoLayout;
