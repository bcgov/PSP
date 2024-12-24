import { Formik } from 'formik';
import { ReactNode } from 'react';
import React from 'react';
import { FaEye } from 'react-icons/fa';
import * as Yup from 'yup';

import { StyledIconButton } from '@/components/common/buttons';
import { TextArea } from '@/components/common/form';
import { useModalContext } from '@/hooks/useModalContext';
import { withNameSpace } from '@/utils/formUtils';

export interface INotesModalProps<T> {
  notesLabel: ReactNode;
  title: string;
  description: string;
  nameSpace?: string;
  onSave?: (values: T) => void;
  field?: string;
  initialValues?: T;
}

export const NotesModal = <T,>({
  nameSpace,
  onSave,
  notesLabel,
  title,
  description,
  field,
  initialValues,
}: INotesModalProps<T>) => {
  const fieldWithNameSpace = withNameSpace(nameSpace, field ?? 'note');
  const { setModalContent, setDisplayModal } = useModalContext();
  const formikRef = React.useRef(null);
  const schema = Yup.object().shape({
    [fieldWithNameSpace]: Yup.string().nullable().max(4000),
  });
  const onSaveForm = (values: T) => {
    onSave?.(values);
    setDisplayModal(false);
  };

  return (
    <StyledIconButton
      title={description}
      onClick={() => {
        setModalContent({
          variant: 'info',
          title: title,
          message: (
            <Formik
              initialValues={initialValues}
              onSubmit={onSaveForm}
              innerRef={formikRef}
              validationSchema={schema}
            >
              <>
                {notesLabel}
                <TextArea field={fieldWithNameSpace} data-testid="note-field"></TextArea>
                Would you like to save these {description}?
              </>
            </Formik>
          ),
          okButtonText: 'Yes',
          cancelButtonText: 'No',
          handleCancel: () => setDisplayModal(false),
          handleOk: async () => {
            await formikRef.current?.submitForm();
          },
        });
        setDisplayModal(true);
      }}
    >
      <FaEye size={22} />
    </StyledIconButton>
  );
};

export default NotesModal;
