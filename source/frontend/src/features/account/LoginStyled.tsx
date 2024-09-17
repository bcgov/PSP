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
    margin: 15rem auto;
    background-color: rgba(255, 255, 255);
    padding-top: 1.5rem;
    padding-bottom: 2rem;
    max-width: 75rem;
    border-radius: 0.4rem;

    .sign-in {
      justify-content: center;
    }

    .logo-title {
      display: flex;
      align-items: center;
      text-align: center;
    }

    h1 {
      padding-bottom: 1.8rem;
      font-family: 'BCSans', Fallback, sans-serif;
      font-weight: 700;
      font-size: 2.6rem;
      color: ${props => props.theme.css.headerTextColor};
    }

    h6 {
      margin-bottom: 2rem;
    }

    p {
      font-size: 1.4rem;
      color: ${props => props.theme.bcTokens.typographyColorSecondary};
      font-family: 'BCSans', Fallback, sans-serif;
      line-height: 130%;
      margin-bottom: 0.2rem;
    }

    .message-container {
      border: 1px solid rgba(205, 211, 216);
      box-shadow: 10px 5px 5px rgba(205, 211, 216, 0.2);
      max-width: 65rem;
      min-width: 90%;
    }

    .message-header {
      display: flex;
      height: 4.8rem;
      padding-left: 1.6rem;
      padding-right: 1.6rem;
      justify-content: flex-start;
      color: ${props => props.theme.bcTokens.surfaceColorBackgroundDarkBlue};
      background-color: ${props => props.theme.css.filterBoxColor};

      .message-title {
        font-family: BcSans-Bold;
        font-size: 2.2rem;
        align-self: center;
        margin-left: 0;
      }
    }

    .message-body {
      padding-top: 2.4rem;
      padding-bottom: 2.4rem;
      padding-left: 1.6rem;
      padding-right: 1.6rem;
      text-align: left;

      p {
        font-size: 1.7rem;
        margin-left: 3rem;
        margin-right: 3rem;
        margin-top: 1rem;
        margin-bottom: 1rem;
      }
    }

    .spacer {
      padding: 0px;
      margin: 0px;
      margin-left: 1.6rem;
      margin-right: 1.6rem;
    }

    .message-footer {
      align-items: right;
    }

    .foot-note {
      padding-top: 3rem;
    }

    .btn {
      margin-top: 3rem;
      margin-bottom: 3rem;
      margin-left: 10rem;
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
    .blockText {
      margin: 0 auto 3.8rem auto;
    }
    .or {
      margin: 2rem auto;
    }
    .border-dark {
      border-color: ${props => props.theme.css.headerBorderColor} !important;
      color: ${props => props.theme.bcTokens.typographyColorSecondary} !important;
    }
    .pims-logo {
      margin-bottom: 1rem;
    }
  }
`;

export default LoginStyled;
