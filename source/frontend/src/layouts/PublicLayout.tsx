import React, { useState } from 'react';
import { ErrorBoundary } from 'react-error-boundary';
import LoadingBar from 'react-redux-loading-bar';

import ErrorModal from '@/components/common/ErrorModal';
import { Footer, Header } from '@/components/layout';
import { HealthcheckContainer } from '@/components/layout/Healthcheck/HealthcheckContainer';
import HealthcheckView from '@/components/layout/Healthcheck/HealthcheckView';

import FooterStyled from './Footer';
import HeaderStyled from './Header';
import * as Styled from './styles';

const PublicLayout: React.FC<React.PropsWithChildren<React.HTMLAttributes<HTMLElement>>> = ({
  children,
  ...rest
}) => {
  const [systemDegraded, setSystemDegraded] = useState<boolean>(false);

  const handleHealthcheckResult = async (degraded: boolean): Promise<void> => {
    setSystemDegraded(degraded);
  };

  return (
    <>
      <LoadingBar style={{ zIndex: 9999, backgroundColor: '#fcba19', height: '.3rem' }} />
      <Styled.AppGridContainer {...rest} className={`App ${systemDegraded ? 'healthcheck' : ''}`}>
        <HealthcheckContainer
          systemDegraded={systemDegraded}
          updateHealthcheckResult={handleHealthcheckResult}
          View={HealthcheckView}
        ></HealthcheckContainer>
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
