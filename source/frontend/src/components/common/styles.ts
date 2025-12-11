import { CSSProperties } from 'react';
import { Breadcrumb as BsBreadcrumb, Col } from 'react-bootstrap';
import { FaWindowClose } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import styled, { css } from 'styled-components';

import { LoadingBackdropProps } from '@/components/common/LoadingBackdrop';
import { Scrollable as ScrollableBase } from '@/components/common/Scrollable/Scrollable';

import { Button } from './buttons';
import { Form } from './form';

export const LeftAlignDiv = styled.div`
  text-align: left;
`;

export const InlineFlexDiv = styled.div`
  display: flex;
  flex-wrap: nowrap;
`;

export const FlexRowNoGap = styled(InlineFlexDiv)`
  flex-direction: row;
  align-items: center;
  gap: 0;
`;

export const Breadcrumb = styled(BsBreadcrumb)`
  .breadcrumb-item:not(:first-child)::before {
    content: '>';
    width: 0px;
    margin-right: 1rem;
  }
  .breadcrumb {
    li a {
      max-height: 24px;
    }
  }
  grid-area: breadcrumb;
  ol {
    background-color: white;
  }
`;

export const SelectedText = styled.p`
  font-size: 1.4rem;
  color: ${props => props.theme.bcTokens.iconsColorSuccess};
`;

/**
 * Styled component to help with basic flexbox layouts (rows or columns).
 * For more specific use cases, override it via styled(FlexBox)
 */
export interface IFlexBoxProps {
  inline?: boolean;
  column?: boolean;
  center?: boolean;
  gap?: CSSProperties['gap'];
}

export const FlexBox = styled.div<IFlexBoxProps>`
  display: ${props => (props.inline ? 'inline-flex' : 'flex')};
  flex-direction: ${props => (props.column ? 'column' : 'row')};
  ${props =>
    props.gap &&
    css`
      gap: ${props.gap};
    `};

  ${props =>
    props.center &&
    css`
      align-items: center;
      justify-content: center;
    `}
`;

export const H1 = styled.h1`
  color: ${props => props.theme.bcTokens.typographyColorSecondary};
  font-family: 'BCSans-Bold';
  font-size: 2.8rem;
  border-bottom: solid 0.5rem ${props => props.theme.css.headerBorderColor};
  width: 100%;
  text-align: left;
  margin-bottom: 2.4rem;
`;

export const H2 = styled.h2`
  color: ${props => props.theme.css.headerTextColor};
  font-family: 'BCSans-Bold';
  font-size: 2.2rem;
  border-bottom: solid 0.2rem ${props => props.theme.css.headerBorderColor};
  width: 100%;
  text-align: left;
  margin-bottom: 2.4rem;
`;

export const H3 = styled.h3`
  color: ${props => props.theme.css.headerTextColor};
  font-family: 'BCSans-Bold';
  font-size: 1.7rem;
  border-bottom: solid 0.2rem ${props => props.theme.css.actionColor};
  width: 100%;
  text-align: left;
  margin-bottom: 1.6rem;
  margin-top: 1.6rem;
`;

export const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
  .form-select {
    min-width: 15rem;
  }
  .input-group: {
    min-width: 100%;
  }
  .idir-input-group {
    margin: 0;
    div {
      padding: 0;
    }
  }
`;

export const StyledAddButton = styled(Button)`
  &.btn.btn-primary,
  &.btn.btn-primary:active {
    background-color: ${props => props.theme.bcTokens.iconsColorSuccess};
  }
  &.btn.btn-primary:hover,
  &.btn.btn-primary:focus {
    background-color: ${props => props.theme.css.pimsGreen80};
    outline-color: ${props => props.theme.css.pimsGreen80};
  }
`;

export const StyledSectionAddButton = styled(StyledAddButton)`
  && {
    display: inline-block;
    margin-left: 1.5rem;
    margin-bottom: 0.5rem;
    float: right;
  }
`;

export const StyledDivider = styled.div`
  margin-top: 0.5rem;
  margin-bottom: 1.5rem;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
`;

export const StyledSectionParagraph = styled.p`
  color: ${props => props.theme.bcTokens.typographyColorSecondary};
  font-size: 1.6rem;
  text-decoration: none;
`;

export const TrayHeader = styled(H1)`
  font-size: 2.5rem;
  color: #313132;
  margin-bottom: 1rem;
  margin-top: 1rem;
  padding: 1.2rem;
  padding-left: 3rem;
  border-bottom: none;
`;

export const TrayHeaderContent = styled.div`
  background-color: ${props => props.theme.css.filterBoxColor};
  display: flex;
  align-items: anchor-center;
`;

export const TrayContent = styled.div``;

export const CloseButton = styled(Button)`
  &#close-tray {
    float: right;
    padding: 0rem;
    cursor: pointer;
    fill: ${props => props.theme.bcTokens.typographyColorSecondary};
    &:hover {
      fill: ${props => props.theme.bcTokens.typographyColorSecondaryInvert};
    }
  }
`;

export const PopupTray = styled.div`
  width: 100%;
  height: 100%;
  overflow: auto;
  border-radius: 1rem;
  text-align: left;
  transition: transform 0.5s ease-in-out;
  position: relative;
  min-width: 50rem;
`;

export const VerticalLine = styled.div`
  border-left: 0.1rem solid ${props => props.theme.bcTokens.typographyColorSecondaryInvert};
  width: 0.1rem;
  height: 80%;
  margin: auto;
`;

export const Underline = styled.div`
  width: 100%;
  border-bottom: solid 0.5rem ${props => props.theme.bcTokens.themeBlue80};
`;

export const Backdrop = styled.div<LoadingBackdropProps>`
  width: 100%;
  height: 100%;
  position: ${(props: any) => (props.parentScreen ? 'absolute' : 'fixed')};
  z-index: 999;
  top: 0;
  left: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  align-content: center;
  justify-items: center;
  justify-content: center;
`;

export const CloseIcon = styled(FaWindowClose)`
  color: ${props => props.theme.css.textColor};
  font-size: 2.4rem;
  cursor: pointer;
`;

export const PageToolbar = styled.div`
  align-items: center;
  padding: 0;
  padding-bottom: 2rem;
`;

export const ColButtons = styled(Col)`
  border-left: 0.2rem solid white;
  min-width: 2.5rem;
`;

export const PaddedScrollable = styled(ScrollableBase)`
  padding: 1.6rem 3.2rem;
  width: 100%;
`;

export const ListPage = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  width: 100%;
  gap: 2.5rem;
  padding: 0;
`;

export const StyledIconWrapper = styled.div`
  &.selected {
    background-color: ${props => props.theme.bcTokens.themeGold100};
  }

  background-color: ${props => props.theme.css.numberBackgroundColor};
  font-size: 1.5rem;
  border-radius: 50%;
  opacity: 0.8;
  width: 3.25rem;
  height: 3.25rem;
  padding: 1rem;
  display: flex;
  justify-content: center;
  align-items: center;
  font-family: 'BCSans-Bold';
`;

export const StyledLink = styled(Link)`
  padding: 0 0.4rem;
  display: block;
`;
