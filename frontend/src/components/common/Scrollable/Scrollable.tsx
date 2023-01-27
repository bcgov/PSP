import styled from 'styled-components';

interface IScrollableProps {
  /** Scroll content vertically */
  vertical?: boolean;
  /** Scroll content horizontally */
  horizontal?: boolean;
}

/**
 * Scrollable creates a scroll container. Defaults to vertical scrolling.
 */
export const Scrollable = styled.div<IScrollableProps>`
  width: 100%;
  flex-grow: 1; // because all parents are flex and have flex-grow set to 1 this takes all available space - calc no longer needed!
  ${({ vertical = true }) => vertical && `overflow-y: auto`}
  ${({ horizontal = false }) => horizontal && `overflow-x: auto`}
`;
