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

  .btn.btn-primary {
    display: inline-block;
  }

  .btn.btn-primary {
    display: inline-block;
  }

  .btn.btn-primary {
    display: inline-block;
  }

  .unauth {
    font-size: 2.4rem;
    margin: 9.1rem auto;
    background-color: rgba(250, 250, 250, 0.9); //allows for slight transparency
    padding-top: 1.5rem;
    padding-bottom: 2rem;
    max-width: 75rem;
    border-radius: 0.4rem;
    .sign-in {
      justify-content: center;
    }
    .block {
      background-color: ${props => props.theme.css.filterBackgroundColor};
      padding: 2rem 0;
      max-width: 65rem;
      max-height: 47.5rem;
      text-align: center;
      border-radius: 0.4rem;
      min-width: 90%;
    }
    h1 {
      padding-bottom: 1.8rem;
      font-family: 'BCSans', Fallback, sans-serif;
      font-weight: 700;
      font-size: 2.4rem;
      font-style: italic;
      color: ${props => props.theme.css.primaryColor};
    }
    h6 {
      margin-bottom: 2rem;
    }
    p {
      font-size: 1.2rem;
      color: ${props => props.theme.css.textColor};
      font-family: 'BCSans', Fallback, sans-serif;
      line-height: 130%;
      margin-left: 3rem;
      margin-right: 3rem;
      margin-bottom: 0.2rem;
    }
    .btn {
      margin-top: 3rem;
      margin-bottom: 3rem;
    }
    .btn-link {
      margin-left: 3rem;
      margin-top: 1rem;
      font-size: 1.2rem;
    }
    .btn-link:focus,
    .btn-link:active {
      background: none !important;
    }
    .bceid {
      margin-bottom: -3rem;
    }
    .jumbotron {
      background-color: white;
      padding: 2rem;
      margin-left: 4rem;
      margin-right: 4rem;
      width: 90%;
      justify-content: left;
      border: 0.1rem;
      border-style: solid;
      border-color: #707070;
      text-align: left;
      p {
        margin: 1rem;
      }
    }
    .blockText {
      margin: 0 auto 3.8rem auto;
    }
    .or {
      margin: 2rem auto;
    }
    .border-dark {
      border-color: ${props => props.theme.css.primaryColor} !important;
      color: ${props => props.theme.css.textColor} !important;
    }
    .pims-logo {
      margin-bottom: 1rem;
    }
  }
`;

export default LoginStyled;
