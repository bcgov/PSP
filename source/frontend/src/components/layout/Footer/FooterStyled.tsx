import styled from 'styled-components';

/**
 * Styled footer component.
 */
const FooterStyled = styled('div')`
  display: flex;

  .main-footer {
    width: 95%;
    padding: 1rem 20%;
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    justify-content: flex-start;
    align-items: stretch;
    align-content: flex-start;
    font-size: 1.2rem;
  }

  a {
    color: #ffffff;
    display: inline;
    &:link,
    &:visited {
      font-size: 1.2rem;
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

  .other {
    flex-grow: 1;
    display: flex;
    flex-direction: row-reverse;
    align-items: center;
    padding: 0;
    margin-right: 2rem;
    color: #ffffff;
    :hover {
      color: red;
      cursor: pointer;
    }
  }
`;

export default FooterStyled;
