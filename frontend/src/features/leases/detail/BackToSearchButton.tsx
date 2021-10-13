import * as React from 'react';
import { TiArrowBack } from 'react-icons/ti';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

/**
 * Back button specific to lease details page(s), onclick navigates to the lease and license listview.
 */
export const BackToSearchButton: React.FunctionComponent = () => {
  return (
    <StyledBackButton to="/lease/list" className="btn">
      <TiArrowBack size={24} />
      <i>Back to Search</i>
    </StyledBackButton>
  );
};

const StyledBackButton = styled(Link)`
  grid-area: backbutton;
  background-color: ${props => props.theme.css.filterBackgroundColor};
  margin: 0.5rem;
  color: ${props => props.theme.css.slideOutBlue};
  &:hover {
    color: ${props => props.theme.css.slideOutBlue};
    text-decoration: none;
  }
`;

export default BackToSearchButton;
