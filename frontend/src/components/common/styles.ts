import { CSSProperties } from 'react';
import { Breadcrumb as BsBreadcrumb, Row } from 'react-bootstrap';
import { Tabs as BsTabs } from 'react-bootstrap';
import styled, { css } from 'styled-components';

import { Form } from './form';
import GenericModal from './GenericModal';

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
  color: ${props => props.theme.css.completedColor};
`;

export const PrimaryGenericModal = styled(GenericModal)`
  .modal-header {
    background-color: ${({ theme }) => theme.css.primaryColor};
    .h4 {
      color: white;
      font-family: BcSans-Bold;
      font-size: 2.2rem;
      height: 2.75rem;
    }
  }
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

export const Tabs = styled(BsTabs)`
  background-color: white;
  color: ${props => props.theme.css.slideOutBlue};
  font-size: 1.4rem;
  border-color: transparent;
  .nav-tabs {
    height: 2.4rem;
  }
  .nav-item {
    min-width: 7rem;
    padding: 0.1rem 0.6rem;

    &:hover {
      border-color: transparent;
    }
    &.active {
      background-color: ${props => props.theme.css.filterBackgroundColor};
      font-family: 'BCSans-Bold';
      color: ${props => props.theme.css.slideOutBlue};
      border-color: transparent;
    }
  }
`;

export const NoPaddingRow = styled(Row)`
  [class^='col-'] {
    padding: 0;
  }
  margin: 0;
`;

export const H1 = styled.h1`
  color: ${props => props.theme.css.textColor};
  font-family: 'BCSans-Bold';
  font-size: 3.2rem;
  border-bottom: solid 0.5rem ${props => props.theme.css.primaryLightColor};
  width: 100%;
  text-align: left;
  margin-bottom: 2rem;
`;

export const H2 = styled.h2`
  color: ${props => props.theme.css.primaryColor};
  font-family: 'BCSans-Bold';
  font-size: 2.6rem;
  border-bottom: solid 0.2rem ${props => props.theme.css.primaryLightColor};
  width: 100%;
  text-align: left;
  margin-bottom: 2rem;
`;

export const H3 = styled.h3`
  color: ${props => props.theme.css.primaryColor};
  font-family: 'BCSans-Bold';
  font-size: 2rem;
  border-bottom: solid 0.2rem ${props => props.theme.css.discardedColor};
  width: 100%;
  text-align: left;
  margin-bottom: 2rem;
`;

export const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
`;
