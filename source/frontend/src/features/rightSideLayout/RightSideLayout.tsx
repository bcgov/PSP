import React from 'react';
import { FaWindowClose } from 'react-icons/fa';
import styled from 'styled-components';

import TooltipWrapper from '@/components/common/TooltipWrapper';

export interface IRightSideLayoutProps {
  isOpen: boolean;
  toggle: () => void;
  title: string;
  closeTooltipText: string;
  'data-testId': string;
}

const RightSideLayout: React.FC<React.PropsWithChildren<IRightSideLayoutProps>> = ({
  isOpen,
  toggle,
  children,
  title,
  closeTooltipText,
  ...rest
}) => {
  return (
    <StyledMapSideBar show={isOpen} data-testid={rest['data-testId']}>
      <>
        <StyledHeader>
          <StyledTitle>{title}</StyledTitle>
          <TooltipWrapper
            tooltipId={`close-sidebar-tooltip-${title?.toLocaleLowerCase()}`}
            tooltip={closeTooltipText}
          >
            <CloseIcon title="close" onClick={toggle} />
          </TooltipWrapper>
        </StyledHeader>
        <StyledContent>{children}</StyledContent>
      </>
    </StyledMapSideBar>
  );
};

export default RightSideLayout;

export const StyledMapSideBar = styled.div<{ show: boolean }>`
  display: flex;
  flex-flow: column;
  position: relative;
  margin: 0;
  min-height: 5.2rem;
  height: calc(100vh - 7.2rem - 4.8rem);
  max-width: ${props => (props.show ? `34.1rem` : `0`)};
  width: ${props => (props.show ? `100%` : `0`)};
  margin-left: ${props => (props.show ? `-1rem` : `0`)};
  background-color: ${props => props.theme.css.primaryBackgroundColor};
  visibility: ${props => (props.show ? `visible` : `hidden`)};
  opacity: ${props => (props.show ? 1 : 0)};
  border-radius: 0.4rem;
  box-shadow: -0.2rem 0.1rem 0.4rem rgba(0, 0, 0, 0.2);
  transition: all 1s ease-in-out;
  z-index: 1000;
  overflow: hidden;
`;

const StyledHeader = styled.div`
  display: flex;
  flex-direction: row;
  flex-shrink: 0;
  position: relative;
  width: 100%;
  height: 5.2rem;
  background-color: ${({ theme }) => theme.css.primaryColor};
  color: ${props => props.theme.css.primaryBackgroundColor};
  align-items: center;
  padding: 1rem 1.8rem;
  gap: 1.5rem;
`;

const StyledTitle = styled.p`
  font-size: 1.8rem;
  color: ${props => props.theme.css.primaryBackgroundColor};
  text-decoration: none solid rgb(255, 255, 255);
  line-height: 1.8rem;
  font-weight: bold;
  margin: 0;
`;

const CloseIcon = styled(FaWindowClose)`
  position: absolute;
  top: 0.6rem;
  right: 0.8rem;
  color: ${props => props.theme.css.primaryBackgroundColor};
  font-size: 22px;
  cursor: pointer;
`;

const StyledContent = styled.div`
  flex-grow: 1; /* ensures that the container will take up the full height of the parent container */
  overflow-y: auto; /* adds scroll to this container */
`;
