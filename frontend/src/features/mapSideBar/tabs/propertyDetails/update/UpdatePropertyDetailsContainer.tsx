import { Scrollable } from 'components/common/Scrollable/Scrollable';
import useIsMounted from 'hooks/useIsMounted';
import React, { useEffect } from 'react';
import styled from 'styled-components';

export interface IUpdatePropertyDetailsContainerProps {
  pid: string;
}

export const UpdatePropertyDetailsContainer: React.FC<IUpdatePropertyDetailsContainerProps> = () => {
  const isMounted = useIsMounted();
  return (
    <>
      <Content vertical></Content>
      <Footer></Footer>
    </>
  );
};

const Content = styled(Scrollable)``;

const Footer = styled.div``;
