import { Button } from 'components/common/buttons';
import { NoteTypes } from 'constants/index';
import { AddNotesContainer } from 'features/notes/add/AddNotesContainer';
import NoteListView from 'features/notes/list/NoteListView';
import { useModalManagement } from 'hooks/useModalManagement';
import { Col, Container, Row } from 'react-bootstrap';

export const TestNotes: React.FC = () => {
  const [isModalOpened, openModal, closeModal] = useModalManagement();

  return (
    <Container>
      <Row>
        <Col>
          <h1>Notes test page</h1>
        </Col>
      </Row>
      <Row className="py-5">
        <Button onClick={openModal}>Add a Note</Button>
        <AddNotesContainer
          type={NoteTypes.Activity}
          parentId={1}
          isOpened={isModalOpened}
          openModal={openModal}
          closeModal={closeModal}
        />

        <NoteListView type={NoteTypes.Activity} entityId={1} />
      </Row>
    </Container>
  );
};
