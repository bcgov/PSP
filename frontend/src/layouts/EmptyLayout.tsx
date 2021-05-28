import './PublicLayout.scss';

import { EmptyHeader, Footer } from 'components/layout';
import React from 'react';
import { Container } from 'react-bootstrap';

import FooterStyled from './Footer';
import HeaderStyled from './Header';

const EmptyLayout: React.FC = ({ children }) => {
  return (
    <>
      <Container fluid className="App">
        <HeaderStyled className="header-layout fixed-top">
          <Container className="px-0">
            <EmptyHeader />
          </Container>
        </HeaderStyled>

        <main className="App-content">{children}</main>

        <FooterStyled className="footer-layout fixed-bottom">
          <Container className="px-0">
            <Footer />
          </Container>
        </FooterStyled>
      </Container>
    </>
  );
};

export default EmptyLayout;
