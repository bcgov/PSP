import React from 'react';

import { EmptyHeader } from '@/components/layout';
import EmptyFooter from '@/components/layout/Footer/EmptyFooter';

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
          <EmptyFooter />
        </FooterStyled>
      </Styled.EmptyAppGridContainer>
    </>
  );
};

export default EmptyLayout;
