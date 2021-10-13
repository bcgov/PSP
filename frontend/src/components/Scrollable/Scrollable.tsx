import React from 'react';
import styled from 'styled-components';

interface ScrollableProps {
  /** Content to display in scrollable area */
  children?: React.ReactNode;
  /** Scroll content vertically */
  vertical?: boolean;
  /** Scroll content horizontally */
  horizontal?: boolean;
}

export const Scrollable: React.FunctionComponent<ScrollableProps> = ({
  children,
  vertical = true,
  horizontal = false,
}) => {
  return (
    <ScrollPanel vertical={vertical} horizontal={horizontal}>
      {children}
    </ScrollPanel>
  );
};

const ScrollPanel = styled.div<ScrollableProps>`
  flex-grow: 1; // because all parents are flex and have flex-grow set to 1 this takes all available space - calc no longer needed!
  overflow-y: ${props => (props.vertical ? 'auto' : undefined)};
  overflow-x: ${props => (props.horizontal ? 'auto' : undefined)};
`;
