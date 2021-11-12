import styled from 'styled-components';

interface IScrollableProps {
  /** Scroll content vertically */
  vertical?: boolean;
  /** Scroll content horizontally */
  horizontal?: boolean;
}

/**
 * Scrollable creates a scroll container
 */
export const Scrollable = styled.div<IScrollableProps>`
  flex-grow: 1; // because all parents are flex and have flex-grow set to 1 this takes all available space - calc no longer needed!
  overflow-y: ${props => (props.vertical === false ? undefined : 'auto')}; // default: true
  overflow-x: ${props => (props.horizontal ? 'auto' : undefined)}; // default: false
`;
