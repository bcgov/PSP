import { TextArea } from 'components/common/form';
import { GenericModal } from 'components/common/GenericModal';
import { FormikProps, useFormikContext } from 'formik';
import styled from 'styled-components';

import { EntityNoteForm } from './models';

export interface IAddNotesFormProps {
  /** Whether to show the notes modal. Default: false */
  showNotes: boolean;
  /** set the value of the externally tracked 'showNotes' prop above. */
  setShowNotes: (show: boolean) => void;
  /** Optional - callback to notify when save button is pressed. */
  onSave?: (noteForm: EntityNoteForm, formikProps: FormikProps<EntityNoteForm>) => void;
  /** Optional - callback to notify when cancel button is pressed. */
  onCancel?: (formikProps: FormikProps<EntityNoteForm>) => void;
}

export const AddNotesForm: React.FC<IAddNotesFormProps> = props => {
  const { showNotes, setShowNotes, onSave, onCancel } = props;
  const formikProps = useFormikContext<EntityNoteForm>();

  return (
    <StyledModal
      display={showNotes}
      setDisplay={setShowNotes}
      title="Notes"
      message={
        <TextArea
          rows={15}
          field="note.note"
          label="Type a note:"
          data-testid="note-field"
        ></TextArea>
      }
      closeButton
      okButtonText="Save"
      cancelButtonText="Cancel"
      handleOk={() => {
        onSave && onSave(formikProps.values, formikProps);
        setShowNotes(false);
      }}
      handleCancel={() => {
        onCancel && onCancel(formikProps);
        setShowNotes(false);
      }}
    ></StyledModal>
  );
};

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
