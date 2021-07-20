import './PublicLayout.scss';

import ErrorModal from 'components/common/ErrorModal';
import { Footer, Header } from 'components/layout';
import React from 'react';
import Container from 'react-bootstrap/Container';
import { ErrorBoundary } from 'react-error-boundary';
import LoadingBar from 'react-redux-loading-bar';

import FooterStyled from './Footer';
import HeaderStyled from './Header';

const PublicLayout: React.FC = ({ children }) => {
  return (
    <>
      <LoadingBar style={{ zIndex: 9999, backgroundColor: '#fcba19', height: '3px' }} />
      <Container fluid className="App">
        <HeaderStyled className="header-layout fixed-top">
          <Container className="px-0">
            <Header />
          </Container>
        </HeaderStyled>

        <main className="App-content">
          <ErrorBoundary FallbackComponent={ErrorModal}>{children}</ErrorBoundary>
        </main>

        <FooterStyled className="footer-layout fixed-bottom">
          <Container className="px-0">
            <Footer />
          </Container>
        </FooterStyled>
      </Container>
    </>
  );
};

export default PublicLayout;
