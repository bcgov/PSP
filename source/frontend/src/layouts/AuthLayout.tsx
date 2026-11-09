import React, { useState } from 'react';
import Spinner from 'react-bootstrap/Spinner';
import styled from 'styled-components';

import { SideNavBar } from '@/components/layout';
import { HealthcheckContainer } from '@/components/layout/Healthcheck/HealthcheckContainer';
import HealthcheckView from '@/components/layout/Healthcheck/HealthcheckView';
import { SidebarStateContextProvider } from '@/components/layout/SideNavBar/SideNavbarContext';
import { AuthStateContext } from '@/contexts/authStateContext';
import usePimsIdleTimer from '@/hooks/usePimsIdleTimer';

import PublicLayout from './PublicLayout';

const AuthLayout: React.FC<React.PropsWithChildren<unknown>> = ({ children }) => {
  const [systemDegraded, setSystemDegraded] = useState(false);
  usePimsIdleTimer();
  return (
    <AuthStateContext.Consumer>
      {context => {
        if (!context.ready) {
          return <Spinner animation="border" data-testid="auth-layout-spinner"></Spinner>;
        }

        return (
          <SidebarStateContextProvider>
            <PublicLayout
              className={`authenticated ${systemDegraded ? 'healthcheck' : ''}`}
              banner={
                <HealthcheckContainer
                  systemDegraded={systemDegraded}
                  updateHealthcheckResult={setSystemDegraded}
                  View={HealthcheckView}
                />
              }
            >
              <SideNavBar />
              <Content className="content">{children}</Content>
            </PublicLayout>
          </SidebarStateContextProvider>
        );
      }}
    </AuthStateContext.Consumer>
  );
};

const Content = styled.div`
  grid-area: content;
  overflow-y: hidden;
  overflow-x: auto;
  display: flex;
  flex-grow: 1;
  min-height: 0;
`;

export default AuthLayout;
