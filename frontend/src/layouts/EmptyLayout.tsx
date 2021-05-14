import './PublicLayout.scss';

import React from 'react';
import { Container } from 'react-bootstrap';
import { Footer, EmptyHeader } from 'components/layout';
import HeaderStyled from './Header';
import FooterStyled from './Footer';

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
