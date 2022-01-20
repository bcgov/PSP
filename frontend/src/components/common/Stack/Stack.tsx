import styled from 'styled-components';

interface IStackProps {
  /** Defines the space between immediate children. */
  gap?: string | number;
  /** Defines the flex-direction style property. It is applied for all screen sizes. */
  $direction?: 'row' | 'column';
  alignItems?: 'flex-start' | 'flex-end' | 'center' | 'stretch' | 'baseline';
  justifyContent?:
    | 'flex-start'
    | 'flex-end'
    | 'center'
    | 'space-between'
    | 'space-around'
    | 'space-evenly';
}

/**
 * The Stack component manages layout of immediate children along the vertical or horizontal axis
 * with optional spacing and/or dividers between each child.
 */
export const Stack = styled.div<IStackProps>`
  display: flex;
  width: 100%;
  flex-direction: ${({ $direction = 'column' }) => $direction};
  gap: ${({ gap = 0 }) => (typeof gap === 'number' ? gap + 'rem' : gap)};
  align-items: ${({ alignItems = 'flex-start' }) => alignItems};
  justify-content: ${({ justifyContent = 'flex-start' }) => justifyContent};
`;
