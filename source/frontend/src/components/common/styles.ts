import { CSSProperties } from 'react';
import { Breadcrumb as BsBreadcrumb } from 'react-bootstrap';
import styled, { css } from 'styled-components';

import { LoadingBackdropProps } from '@/components/common/LoadingBackdrop';

import { Button } from './buttons';
import { Form } from './form';

export const LeftAlignDiv = styled.div`
  text-align: left;
`;

export const InlineFlexDiv = styled.div`
  display: flex;
  flex-wrap: nowrap;
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
  font-size: 3.2rem;
  border-bottom: solid 0.5rem ${props => props.theme.css.headerBorderColor};
  width: 100%;
  text-align: left;
  margin-bottom: 2.4rem;
`;

export const H2 = styled.h2`
  color: ${props => props.theme.css.headerTextColor};
  font-family: 'BCSans-Bold';
  font-size: 2.6rem;
  border-bottom: solid 0.2rem ${props => props.theme.css.headerBorderColor};
  width: 100%;
  text-align: left;
  margin-bottom: 2.4rem;
`;

export const H3 = styled.h3`
  color: ${props => props.theme.css.headerTextColor};
  font-family: 'BCSans-Bold';
  font-size: 2.2rem;
  border-bottom: solid 0.2rem ${props => props.theme.css.actionColor};
  width: 100%;
  text-align: left;
  margin-bottom: 2.4rem;
`;

export const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
`;

export const StyledAddButton = styled(Button)`
  &.btn.btn-primary,
  &.btn.btn-primary:active {
    background-color: ${props => props.theme.bcTokens.iconsColorSuccess};
  }
  &.btn.btn-primary:hover {
    background-color: ${props => props.theme.css.pimsGreen80};
  }
`;

export const StyledSectionAddButton = styled(StyledAddButton)`
  && {
    display: inline-block;
    margin-left: 1.5rem;
    margin-bottom: 0.5rem;
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

export const TrayHeader = styled.div`
  font-size: 2rem;
  font-weight: bold;
  padding: 1rem;
  background-color: ${props => props.theme.bcTokens.surfaceColorPrimaryButtonDefault};
  color: white;
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
`;

export const VerticalLine = styled.div`
  border-left: 0.1rem solid ${props => props.theme.bcTokens.typographyColorSecondaryInvert};
  width: 0.1rem;
  height: 80%;
  margin: auto;
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
