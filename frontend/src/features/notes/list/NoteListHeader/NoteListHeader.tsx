import { Claims } from 'constants/index';
import { useKeycloakWrapper } from 'hooks/useKeycloakWrapper';
import { Col, Row } from 'react-bootstrap';
import { ImFileText2 } from 'react-icons/im';

import * as Styled from '../styles';

export interface INoteListHeaderProps {
  title: string;
  onAddNote?: () => void;
}

export const NoteListHeader: React.FunctionComponent<INoteListHeaderProps> = props => {
  const { hasClaim } = useKeycloakWrapper();
  const onClick = () => props.onAddNote && props.onAddNote();

  return (
    <Row className="no-gutters justify-content-between">
      <Col xs="auto" className="px-2 my-1">
        {props.title}
      </Col>
      <Col xs="auto" className="my-1">
        {hasClaim(Claims.NOTE_ADD) && (
          <Styled.AddNoteButton onClick={onClick}>
            <ImFileText2 size="2rem" />
            &nbsp;Add a Note
          </Styled.AddNoteButton>
        )}
      </Col>
    </Row>
  );
};
