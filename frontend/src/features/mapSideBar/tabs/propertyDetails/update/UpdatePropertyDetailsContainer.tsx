import { Scrollable } from 'components/common/Scrollable/Scrollable';
import useIsMounted from 'hooks/useIsMounted';
import React, { useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import styled from 'styled-components';

import { useGetProperty } from '../hooks/useGetProperty';
import { fromApi, UpdatePropertyDetailsForm } from './models';

export interface IUpdatePropertyDetailsContainerProps {
  pid: string;
}

export const UpdatePropertyDetailsContainer: React.FC<IUpdatePropertyDetailsContainerProps> = props => {
  const isMounted = useIsMounted();
  const history = useHistory();
  const { retrieveProperty } = useGetProperty();

  const [initialForm, setForm] = useState<UpdatePropertyDetailsForm | undefined>(undefined);

  useEffect(() => {
    async function fetchProperty() {
      const retrieved = await retrieveProperty(props.pid);
      if (retrieved !== undefined) {
        setForm(fromApi(retrieved));
      }
    }
  }, [props.pid, retrieveProperty]);

  return (
    <>
      <Content vertical>{/* Formik form goes here */}</Content>
      <Footer></Footer>
    </>
  );
};

const Content = styled(Scrollable)``;

const Footer = styled.div``;
