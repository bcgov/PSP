import styled from 'styled-components';

export interface IGridItemProps {
  center?: boolean;
  middle?: boolean;
  area?: string;
}

const GridItem = styled.div<IGridItemProps>`
  height: 100%;
  min-width: 0;

  ${({ center }) => center && `text-align: center`};

  ${({ area }) => area && `grid-area: ${area}`};

  ${/* prettier-ignore */
  ({ middle }) => middle && `
    display: flex;
    flex-direction: column;
    justify-content: center;
    justify-self: stretch;
  `};
`;

export default GridItem;
