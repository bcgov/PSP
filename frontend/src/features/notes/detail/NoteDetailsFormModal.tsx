import { Button } from 'components/common/buttons';
import { GenericModal } from 'components/common/GenericModal';
import { UserNameTooltip } from 'components/common/UserNameTooltip';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Claims } from 'constants/index';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_Note } from 'models/api/Note';
import { Col, Container, Row } from 'react-bootstrap';
import Form from 'react-bootstrap/Form';
import { FaEdit } from 'react-icons/fa';
import styled from 'styled-components';
import { prettyFormatDate } from 'utils';

export interface INoteDetailsFormModalProps {
  /** Whether to show the notes modal. Default: false */
  isOpened: boolean;
  /** set the value of the externally tracked 'isOpened' prop above. */
  openModal: () => void;
  /** set the value of the externally tracked 'isOpened' prop above. */
  closeModal: () => void;
  /** Whether the to show a loading spinner instead of the form */
  loading?: boolean;
  /** The note details to show */
  note?: Api_Note;
  /** Edit note callback */
  onEdit?: (note?: Api_Note) => void;
}

export const NoteDetailsFormModal: React.FC<INoteDetailsFormModalProps> = props => {
  const keycloak = useKeycloakWrapper();
  const { loading, isOpened, onEdit, note } = props;

  const spinner = <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;

  const editButton = keycloak.hasClaim(Claims.NOTE_EDIT) ? (
    <Button variant="link" title="Edit note" onClick={() => onEdit && onEdit(note)}>
      <FaEdit size="2rem" />
    </Button>
  ) : null;

  const body = (
    <Container>
      <Row className="no-gutters">
        <Col>
          <span>
            Created: <strong>{prettyFormatDate(note?.appCreateTimestamp)}</strong> by{' '}
            <UserNameTooltip userName={note?.appCreateUserid} userGuid={note?.appCreateUserGuid} />
          </span>
        </Col>
      </Row>
      <Row className="no-gutters">
        <Col>
          <span>
            Last updated: <strong>{prettyFormatDate(note?.appLastUpdateTimestamp)}</strong> by{' '}
            <UserNameTooltip
              userName={note?.appLastUpdateUserid}
              userGuid={note?.appLastUpdateUserGuid}
            />
          </span>
        </Col>
        <StyledCol>{editButton}</StyledCol>
      </Row>
      <Row className="no-gutters">
        <Col>
          <Form.Control as="textarea" readOnly rows={15} value={note?.note} />
        </Col>
      </Row>
    </Container>
  );

  return (
    <StyledModal
      display={isOpened}
      title="Notes"
      message={loading ? spinner : body}
      closeButton
      showFooter={false}
    ></StyledModal>
  );
};

const StyledCol = styled(Col)`
  color: ${props => props.theme.css.primary};
  text-align: right;
`;

const StyledModal = styled(GenericModal)`
  min-width: 70rem;

  .modal-body {
    padding-left: 2rem;
    padding-right: 2rem;
  }

  .modal-footer {
    padding-left: 2rem;
    padding-right: 2rem;
  }

  .form-group {
    label {
      font-family: BcSans-Bold;
      line-height: 2rem;
      color: ${props => props.theme.css.textColor};
    }
  }
`;
