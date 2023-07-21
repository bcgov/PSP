import React from 'react';
import { ErrorBoundary } from 'react-error-boundary';
import LoadingBar from 'react-redux-loading-bar';

import ErrorModal from '@/components/common/ErrorModal';
import { Footer, Header } from '@/components/layout';

import FooterStyled from './Footer';
import HeaderStyled from './Header';
import * as Styled from './styles';

const PublicLayout: React.FC<React.PropsWithChildren<React.HTMLAttributes<HTMLElement>>> = ({
  children,
  ...rest
}) => {
  return (
    <>
      <LoadingBar style={{ zIndex: 9999, backgroundColor: '#fcba19', height: '.3rem' }} />
      <Styled.AppGridContainer className="App" {...rest}>
        <HeaderStyled>
          <Header />
        </HeaderStyled>
        <ErrorBoundary FallbackComponent={ErrorModal}>{children}</ErrorBoundary>
        <FooterStyled>
          <Footer />
        </FooterStyled>
      </Styled.AppGridContainer>
    </>
  );
};

export default PublicLayout;
