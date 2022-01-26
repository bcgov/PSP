import { TextArea } from 'components/common/form';
import GenericModal from 'components/common/GenericModal';
import { IconButton } from 'components/common/styles';
import { getIn, useFormikContext } from 'formik';
import * as React from 'react';
import { useEffect } from 'react';
import { useState } from 'react';
import { FaRegFileAlt } from 'react-icons/fa';
import { withNameSpace } from 'utils/formUtils';

export interface INotesModalProps {
  notesLabel: React.ReactNode;
  title: string;
  nameSpace?: string;
  onSave?: (values: any) => void;
  field?: string;
}

export const NotesModal: React.FunctionComponent<INotesModalProps> = ({
  nameSpace,
  onSave,
  notesLabel,
  title,
  field,
}) => {
  const { values, setFieldValue } = useFormikContext();
  const [showNotes, setShowNotes] = useState(false);
  const [currentNote, setCurrentNote] = useState();
  const fieldWithNameSpace = withNameSpace(nameSpace, field ?? 'note');
  const noteValue = getIn(values, fieldWithNameSpace);

  useEffect(() => {
    if (showNotes === false) {
      setCurrentNote(noteValue);
    }
  }, [showNotes, noteValue]);
  return (
    <>
      <IconButton title="notes" onClick={() => setShowNotes(true)} variant="light">
        <FaRegFileAlt />
      </IconButton>
      <GenericModal
        display={showNotes}
        setDisplay={setShowNotes}
        title={title}
        message={
          <>
            {notesLabel}
            <TextArea field={fieldWithNameSpace} data-testid="note-field"></TextArea>
          </>
        }
        closeButton
        okButtonText="Save"
        cancelButtonText="Cancel"
        handleOk={() => onSave && onSave(values)}
        handleCancel={() => {
          setFieldValue(fieldWithNameSpace, currentNote);
        }}
      ></GenericModal>
    </>
  );
};

export default NotesModal;
