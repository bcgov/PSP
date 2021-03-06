import { Navbar } from 'react-bootstrap';
import styled from 'styled-components';

/**
 * Styled component provides consistent css for the page header.
 * Used by different layouts.
 * Displays the page header background, title, and logo.
 */
export const HeaderStyled = styled(Navbar)`
  padding: 0 10px;
  min-height: 70px;
  color: #ffffff;

  .longAppName {
    display: none;
  }
  .shortAppName {
    display: block;
  }

  .brand-box {
    padding: 10px 0;

    .pims-logo {
      margin: 0 10px;
    }
  }

  .title h1 {
    margin-top: 10px;
    padding-left: 0px;
    text-align: center;
    font-size: 24px;
    text-decoration: none solid rgb(255, 255, 255);
    font-family: 'BCSans', Fallback, sans-serif;
    font-weight: 700;
    white-space: normal;
  }

  .other {
    display: flex;
    flex-direction: row-reverse;
    align-items: center;
    padding: 0;
    :hover {
      color: red;
      cursor: pointer;
    }
  }

  .modal-content .label {
    font-weight: 700;
  }

  // show long App Name when space allows
  @media (min-width: 992px) {
    .longAppName {
      display: block;
      text-align: left;
      padding-left: 40px;
    }
    .shortAppName {
      display: none;
    }
  }
`;

export default HeaderStyled;
