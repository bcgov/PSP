import Container from 'react-bootstrap/Container';
import styled from 'styled-components';

/**
 * Styled component for login container.
 * Provides css for the login page and applies the tenant configuration styling (background image and color).
 */
export const LoginStyled = styled(Container)`
  position: relative;
  background-image: ${props =>
    props.theme.tenant.login.backgroundImage
      ? `url("${props.theme.tenant.login.backgroundImage}")`
      : ''};
  background-size: cover;
  background-position: center;
  overflow: auto;
  grid-area: content;

  .unauth {
    font-size: 24px;
    margin: 91px auto;
    background-color: rgba(250, 250, 250, 0.9); //allows for slight transparency
    padding-top: 15px;
    padding-bottom: 20px;
    max-width: 750px;
    border-radius: 4px;
    .sign-in {
      justify-content: center;
    }
    .block {
      background-color: ${props => props.theme.css.filterBackgroundColor};
      padding: 20px 0;
      max-width: 650px;
      max-height: 475px;
      text-align: center;
      border-radius: 4px;
      min-width: 90%;
    }
    h1 {
      padding-bottom: 18px;
      font-family: 'BCSans', Fallback, sans-serif;
      font-weight: 700;
      font-size: 24px;
      font-style: italic;
      color: ${props => props.theme.css.primaryColor};
    }
    h6 {
      margin-bottom: 20px;
    }
    p {
      font-size: 12px;
      color: ${props => props.theme.css.textColor};
      font-family: 'BCSans', Fallback, sans-serif;
      line-height: 130%;
      margin-left: 30px;
      margin-right: 30px;
      margin-bottom: 2px;
    }
    .btn {
      margin-top: 30px;
      margin-bottom: 30px;
    }
    .btn-link {
      margin-left: 30px;
      margin-top: 10px;
      font-size: 12px;
    }
    .btn-link:focus,
    .btn-link:active {
      background: none !important;
    }
    .bceid {
      margin-bottom: -30px;
    }
    .jumbotron {
      background-color: white;
      padding: 20px;
      margin-left: 40px;
      margin-right: 40px;
      width: 90%;
      justify-content: left;
      border: 1px;
      border-style: solid;
      border-color: #707070;
      text-align: left;
      p {
        margin: 10px;
      }
    }
    .blockText {
      margin: 0 auto 38px auto;
    }
    .or {
      margin: 20px auto;
    }
    .border-dark {
      border-color: ${props => props.theme.css.primaryColor} !important;
      color: ${props => props.theme.css.textColor} !important;
    }
    .pims-logo {
      margin-bottom: 10px;
    }
  }
`;

export default LoginStyled;
