import { NoteTypes } from 'constants/index';
import NoteListView from 'features/notes/list/NoteListView';
import { Col, Container, Row } from 'react-bootstrap';

export const TestNotes: React.FC = () => {
  return (
    <Container>
      <Row>
        <Col>
          <h1>Notes test page</h1>
        </Col>
      </Row>
      <Row className="py-5">
        <NoteListView type={NoteTypes.Activity} entityId={1} />
      </Row>
    </Container>
  );
};
