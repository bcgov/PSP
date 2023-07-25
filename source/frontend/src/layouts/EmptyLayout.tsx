import React from 'react';

import { EmptyHeader, Footer } from '@/components/layout';

import FooterStyled from './Footer';
import HeaderStyled from './Header';
import * as Styled from './styles';

const EmptyLayout: React.FC<React.PropsWithChildren<React.HTMLAttributes<HTMLElement>>> = ({
  children,
  ...rest
}) => {
  return (
    <>
      <Styled.EmptyAppGridContainer className="App" {...rest}>
        <HeaderStyled>
          <EmptyHeader />
        </HeaderStyled>
        {children}
        <FooterStyled>
          <Footer />
        </FooterStyled>
      </Styled.EmptyAppGridContainer>
    </>
  );
};

export default EmptyLayout;
