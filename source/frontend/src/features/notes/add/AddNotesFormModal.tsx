import { Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import styled from 'styled-components';

import { TextArea } from '@/components/common/form';
import { GenericModal } from '@/components/common/GenericModal';

import { EntityNoteForm } from './models';

export interface IAddNotesFormModalProps {
  /** Whether to show the notes modal. Default: false */
  isOpened: boolean;
  /** Initial values of the form */
  initialValues: EntityNoteForm;
  /** A Yup Schema or a function that returns a Yup schema */
  validationSchema?: any | (() => any);
  /** Submission handler */
  onSubmit: (
    values: EntityNoteForm,
    formikHelpers: FormikHelpers<EntityNoteForm>,
  ) => void | Promise<any>;
  /** Optional - callback to notify when save button is pressed. */
  onSaveClick?: (noteForm: EntityNoteForm, formikProps: FormikProps<EntityNoteForm>) => void;
  /** Optional - callback to notify when cancel button is pressed. */
  onCancelClick?: (formikProps: FormikProps<EntityNoteForm>) => void;
}

export const AddNotesFormModal = React.forwardRef<
  FormikProps<EntityNoteForm>,
  IAddNotesFormModalProps
>((props, ref) => {
  const { isOpened, onSaveClick, onCancelClick, initialValues, validationSchema, onSubmit } = props;

  return (
    <Formik<EntityNoteForm>
      enableReinitialize
      innerRef={ref}
      validationSchema={validationSchema}
      initialValues={initialValues}
      onSubmit={onSubmit}
    >
      {formikProps => (
        <StyledModal
          display={isOpened}
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
            onSaveClick && onSaveClick(formikProps.values, formikProps);
          }}
          handleCancel={() => {
            onCancelClick && onCancelClick(formikProps);
          }}
        ></StyledModal>
      )}
    </Formik>
  );
});

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
