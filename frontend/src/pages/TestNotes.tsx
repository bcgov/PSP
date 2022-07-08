import { Button } from 'components/common/buttons';
import { NoteTypes } from 'constants/noteTypes';
import { AddNotesContainer } from 'features/notes/add/AddNotesContainer';
import NoteListView from 'features/notes/list/NoteListView';
import { useState } from 'react';
import { Col, Container, Row } from 'react-bootstrap';

export const TestNotes: React.FC = () => {
  const [show, setShow] = useState(false);

  return (
    <Container>
      <Row>
        <Col>
          <h1>Notes test page</h1>
        </Col>
      </Row>
      <Row className="py-5">
        <Button onClick={() => setShow(true)}>Add a Note</Button>
        <AddNotesContainer
          parentType="activity"
          parentId={1}
          showNotes={show}
          setShowNotes={setShow}
        />

        <NoteListView type={NoteTypes.Activity} />
      </Row>
    </Container>
  );
};
