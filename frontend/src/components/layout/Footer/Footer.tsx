import { ApiVersionInfo } from 'components/common/ApiVersionInfo';
import React from 'react';
import styled from 'styled-components';

/**
 * Display common links and the api version in the bottom footer.
 * @returns Footer component.
 */
function Footer() {
  return (
    <FooterStyled>
      <a href="http://www.gov.bc.ca/gov/content/home/disclaimer">Disclaimer</a>
      <a href="http://www.gov.bc.ca/gov/content/home/privacy">Privacy</a>
      <a href="http://www.gov.bc.ca/gov/content/home/accessible-government">Accessibility</a>
      <a href="http://www.gov.bc.ca/gov/content/home/copyright">Copyright</a>
      <ApiVersionInfo />
    </FooterStyled>
  );
}

/**
 * Styled footer component.
 */
const FooterStyled = styled('div')`
  padding: 10px 20%;
  display: flex;
  flex-direction: row;

  a {
    display: inline;
    &:link,
    &:visited {
      color: #ffffff;
      font-size: 16px;
      font-family: 'BCSans', Fallback, sans-serif;
      padding: 0 10px;
      border-right: 1px solid #666;
    }
    &:hover,
    &:focus {
      text-decoration: underline;
    }
  }

  div.version {
    color: #ffffff;
    font-size: 12px;
    font-family: 'BCSans', Fallback, sans-serif;
    padding: 0 0 0 10px;
    flex-grow: 1;
    text-align: right;
  }
`;

export default Footer;
