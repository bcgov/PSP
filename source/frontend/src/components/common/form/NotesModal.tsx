import { getIn, useFormikContext } from 'formik';
import React from 'react';
import { useEffect } from 'react';
import { useState } from 'react';
import { FaRegFileAlt } from 'react-icons/fa';

import { StyledIconButton } from '@/components/common/buttons';
import { TextArea } from '@/components/common/form';
import GenericModal from '@/components/common/GenericModal';
import { withNameSpace } from '@/utils/formUtils';

export interface INotesModalProps {
  notesLabel: React.ReactNode;
  title: string;
  nameSpace?: string;
  onSave?: (values: any) => void;
  field?: string;
}

export const NotesModal: React.FunctionComponent<React.PropsWithChildren<INotesModalProps>> = ({
  nameSpace,
  onSave,
  notesLabel,
  title,
  field,
}) => {
  const { values, setFieldValue, errors } = useFormikContext();
  const [showNotes, setShowNotes] = useState(false);
  const [currentNote, setCurrentNote] = useState();
  const fieldWithNameSpace = withNameSpace(nameSpace, field ?? 'note');
  const noteValue = getIn(values, fieldWithNameSpace);
  const error = getIn(errors, fieldWithNameSpace);

  useEffect(() => {
    if (showNotes === false) {
      setCurrentNote(noteValue);
    }
  }, [showNotes, noteValue]);
  return (
    <>
      <StyledIconButton title="notes" onClick={() => setShowNotes(true)} variant="light">
        <FaRegFileAlt />
      </StyledIconButton>
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
        handleOk={() => {
          if (!error) {
            onSave && onSave(values);
            setShowNotes(false);
          }
        }}
        handleCancel={() => {
          setFieldValue(fieldWithNameSpace, currentNote);
          setShowNotes(false);
        }}
      ></GenericModal>
    </>
  );
};

export default NotesModal;
