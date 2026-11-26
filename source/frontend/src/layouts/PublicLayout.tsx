import React from 'react';
import { ErrorBoundary } from 'react-error-boundary';
import LoadingBar from 'react-redux-loading-bar';

import ErrorModal from '@/components/common/ErrorModal';
import { Footer, Header } from '@/components/layout';

import FooterStyled from './Footer';
import HeaderStyled from './Header';
import * as Styled from './styles';

interface PublicLayoutProps extends React.HTMLAttributes<HTMLElement> {
  banner?: React.ReactNode;
}

const PublicLayout: React.FC<React.PropsWithChildren<PublicLayoutProps>> = ({
  children,
  banner,
  className,
  ...rest
}) => {
  return (
    <>
      <LoadingBar style={{ zIndex: 9999, backgroundColor: '#fcba19', height: '.3rem' }} />
      <Styled.AppGridContainer {...rest} className={`App ${className ?? ''}`}>
        {banner}
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
