import styled from 'styled-components';

import GridItem from './GridItem';

const formatAreas = (areas: string[]) => areas.map(area => `'${area}'`).join(' ');

const frGetter = (value: string | number) =>
  typeof value === 'number' ? `repeat(${value}, 1fr)` : value;

const gap = ({ gap = '0.8rem' }: IGridProps) => gap;

export interface IGridProps {
  inline?: boolean;
  height?: string;
  columns?: string | number;
  rows?: string;
  gap?: string;
  columnGap?: string;
  rowGap?: string;
  areas?: string[];
  justifyContent?: string;
  alignContent?: string;
}

/**
 * A thin wrapper to help make CSS Grid simpler and more expressive in React
 *
 * Basic Usage
 *
 *  <Grid columns='10px 20px 30px'>
 *    <MyComponent />  # assumes MyComponent is aware of how to position itself within the parent grid
 *  </Grid>
 *
 * ADVANCED Usage - to help with structuring your components, a Grid Item is also available.
 *
 *  <Grid columns="100px 1fr 100px" rows="50px auto 25px" areas={[
 *    'header header header',
 *    'sidebar content ads',
 *    'footer footer footer',
 *  ]}>
 *    <Grid.Item area="header">
 *      <MyHeader />
 *    </Grid.Item>
 *
 *    <Grid.Item area="sidebar">
 *      <MySidebar />
 *    </Grid.Item>
 *
 *    <Grid.Item area="content">
 *      <MyContentArea />
 *    </Grid.Item>
 *
 *    <Grid.Item area="ads">
 *      <MyAdsBanner />
 *    </Grid.Item>
 *
 *    <Grid.Item area="footer">
 *      <MyFooter />
 *    </Grid.Item>
 *  </Grid>
 *
 */
const StyledGrid = styled.div<IGridProps>`
  display: ${({ inline = false }) => (inline ? 'inline-grid' : 'grid')};
  height: ${({ height = 'auto' }) => height};
  ${({ rows }) => rows && `grid-template-rows: ${frGetter(rows)}`};
  grid-template-columns: ${({ columns = 12 }) => frGetter(columns)};
  gap: ${gap};
  ${({ columnGap }) => columnGap && `column-gap: ${columnGap}`};
  ${({ rowGap }) => rowGap && `row-gap: ${rowGap}`};
  ${({ areas }) => areas && `grid-template-areas: ${formatAreas(areas)}`};
  ${({ justifyContent }) => justifyContent && `justify-content: ${justifyContent}`};
  ${({ alignContent }) => alignContent && `align-content: ${alignContent}`};
`;

type GridType = typeof StyledGrid & {
  Item: typeof GridItem;
};

const Grid = StyledGrid as GridType;
Grid.Item = GridItem;

export default Grid;
