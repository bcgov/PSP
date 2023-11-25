import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';

import { StyledAddButton } from '@/components/common/styles';
import { Claims } from '@/constants';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';

import * as S from './styles';

/**
 * Page that displays Disposition files information.
 */
export const DispositionListView: React.FC<unknown> = () => {
  const { hasClaim } = useKeycloakWrapper();
  const history = useHistory();

  return (
    <S.ListPage>
      <S.Scrollable>
        <S.PageHeader>Disposition Files</S.PageHeader>
        <S.PageToolbar>
          <Row>
            <Col>{/* TODO: disposition filter goes here */}</Col>
          </Row>
        </S.PageToolbar>
        {hasClaim(Claims.DISPOSITION_ADD) && (
          <StyledAddButton onClick={() => history.push('/mapview/sidebar/disposition/new')}>
            <FaPlus />
            &nbsp;Add a Disposition File
          </StyledAddButton>
        )}
        {/* TODO: disposition search results go here */}
      </S.Scrollable>
    </S.ListPage>
  );
};
