import { TextArea } from 'components/common/form';
import GenericModal from 'components/common/GenericModal';
import { IconButton } from 'components/common/styles';
import { getIn, useFormikContext } from 'formik';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import * as React from 'react';
import { useState } from 'react';
import { FaRegFileAlt } from 'react-icons/fa';
import { withNameSpace } from 'utils/formUtils';

interface IAddLeaseTenantNoteProps {
  nameSpace?: string;
}

const AddLeaseTenantNote: React.FunctionComponent<IAddLeaseTenantNoteProps> = ({ nameSpace }) => {
  const { values, setFieldValue } = useFormikContext();
  const [showNotes, setShowNotes] = useState(false);
  const [currentNote, setCurrentNote] = useState();
  const field = withNameSpace(nameSpace, 'note');
  const noteValue = getIn(values, field);
  const summaryValue = getIn(values, withNameSpace(nameSpace, 'summary'));

  useDeepCompareEffect(() => {
    if (showNotes === true) {
      setCurrentNote(noteValue);
    }
  }, [showNotes]);
  return (
    <>
      <IconButton onClick={() => setShowNotes(true)} variant="light">
        <FaRegFileAlt />
      </IconButton>
      <GenericModal
        display={showNotes}
        setDisplay={setShowNotes}
        title="Tenant Notes"
        message={
          <>
            <p>
              Notes pertaining to <b>{summaryValue}</b>
            </p>
            <TextArea field={field}></TextArea>
          </>
        }
        closeButton
        okButtonText="Ok"
        cancelButtonText="Cancel"
        handleCancel={() => {
          setFieldValue(field, currentNote);
        }}
      ></GenericModal>
    </>
  );
};

export default AddLeaseTenantNote;
