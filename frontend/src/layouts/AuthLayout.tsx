import { SideNavBar } from 'components/layout';
import { AuthStateContext } from 'contexts/authStateContext';
import React from 'react';
import Spinner from 'react-bootstrap/Spinner';
import styled from 'styled-components';

import PublicLayout from './PublicLayout';

const AuthLayout: React.FC = ({ children }) => {
  return (
    <AuthStateContext.Consumer>
      {context => {
        if (!context.ready) {
          return <Spinner animation="border"></Spinner>;
        }

        return (
          <PublicLayout className="authenticated">
            <SideNavBar />
            <Content className="content">{children}</Content>
          </PublicLayout>
        );
      }}
    </AuthStateContext.Consumer>
  );
};

const Content = styled.div`
  grid-area: content;
`;

export default AuthLayout;
