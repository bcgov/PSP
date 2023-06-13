import styled from 'styled-components';

import { ApiVersionInfo } from '@/components/common/ApiVersionInfo';

/**
 * Display common links and the api version in the bottom footer.
 * @returns Footer component.
 */
function Footer() {
  return (
    <FooterStyled>
      <a
        target="_blank"
        rel="noopener noreferrer"
        href="http://www.gov.bc.ca/gov/content/home/disclaimer"
      >
        Disclaimer
      </a>
      <a
        target="_blank"
        rel="noopener noreferrer"
        href="http://www.gov.bc.ca/gov/content/home/privacy"
      >
        Privacy
      </a>
      <a
        target="_blank"
        rel="noopener noreferrer"
        href="http://www.gov.bc.ca/gov/content/home/accessible-government"
      >
        Accessibility
      </a>
      <a
        target="_blank"
        rel="noopener noreferrer"
        href="http://www.gov.bc.ca/gov/content/home/copyright"
      >
        Copyright
      </a>
      <ApiVersionInfo />
    </FooterStyled>
  );
}

/**
 * Styled footer component.
 */
const FooterStyled = styled('div')`
  padding: 1rem 20%;
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: flex-start;
  align-items: stretch;
  align-content: flex-start;

  a {
    display: inline;
    &:link,
    &:visited {
      color: #ffffff;
      font-size: 1.6rem;
      font-family: 'BCSans', Fallback, sans-serif;
      padding: 0 1rem;
      border-right: 0.1rem solid #666;
    }
    &:hover,
    &:focus {
      text-decoration: underline;
    }
  }

  div.version {
    color: #ffffff;
    font-size: 1.2rem;
    font-family: 'BCSans', Fallback, sans-serif;
    padding: 0 0 0 1rem;
    flex-grow: 1;
    text-align: right;
  }
`;

export default Footer;
