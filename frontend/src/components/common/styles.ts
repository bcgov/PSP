import { CSSProperties } from 'react';
import { Breadcrumb as BsBreadcrumb, Button } from 'react-bootstrap';
import styled, { css } from 'styled-components';

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

export const IconButton = styled(Button)`
  &.btn.btn-light {
    background-color: unset;
    border: 0;
    :hover {
      background-color: unset;
    }
    svg {
      color: ${({ theme }) => theme.css.slideOutBlue};
      transition: all 0.3s ease-out;
    }
    svg:hover {
      transition: all 0.3s ease-in;
      color: ${({ theme }) => theme.css.secondaryVariantColor};
    }
  }
`;

/**
 * Styled component to help with basic flexbox layouts (rows or columns).
 * For more specific use cases, override it via styled(FlexBox)
 */
export const FlexBox = styled.div<{
  inline?: boolean;
  column?: boolean;
  center?: boolean;
  gap?: CSSProperties['gap'];
}>`
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
