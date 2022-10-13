import styled from 'styled-components';

interface IBoxProps {
  /** Adds 1px border with theme.css.primaryBorderColor color */
  withBorder?: boolean;
}

/**
 * Box component renders white a background with optional border
 */
export const Box = styled.div<IBoxProps>`
  display: 'block';
  text-decoration: 'none';
  color: ${props => props.theme.css.textColor};
  background-color: ${props => props.theme.css.primaryBackgroundColor};
  border: ${props =>
    props.withBorder ? `1px solid ${props.theme.css.primaryBorderColor}` : undefined};
`;
