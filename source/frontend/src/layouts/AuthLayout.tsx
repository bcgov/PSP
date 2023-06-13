import React from 'react';
import Spinner from 'react-bootstrap/Spinner';
import styled from 'styled-components';

import { SideNavBar } from '@/components/layout';
import { SidebarStateContextProvider } from '@/components/layout/SideNavBar/SideNavbarContext';
import { AuthStateContext } from '@/contexts/authStateContext';
import usePimsIdleTimer from '@/hooks/usePimsIdleTimer';

import PublicLayout from './PublicLayout';

const AuthLayout: React.FC<React.PropsWithChildren<unknown>> = ({ children }) => {
  usePimsIdleTimer();
  return (
    <AuthStateContext.Consumer>
      {context => {
        if (!context.ready) {
          return <Spinner animation="border"></Spinner>;
        }

        return (
          <SidebarStateContextProvider>
            <PublicLayout className="authenticated">
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
  min-width: 154rem;
  display: flex;
  flex-grow: 1;
  min-height: 0;
`;

export default AuthLayout;
