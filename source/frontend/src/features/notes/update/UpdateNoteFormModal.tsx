import { Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import { Col, Container, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { TextArea } from '@/components/common/form';
import { GenericModal } from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { UserNameTooltip } from '@/components/common/UserNameTooltip';
import { prettyFormatUTCDate } from '@/utils';

import { NoteForm } from '../models';

export interface IUpdateNoteFormModalProps {
  /** Whether the to show a loading spinner instead of the form */
  loading?: boolean;
  /** Whether to show the notes modal. Default: false */
  isOpened: boolean;
  /** Initial values of the form */
  initialValues: NoteForm;
  /** A Yup Schema or a function that returns a Yup schema */
  validationSchema?: any | (() => any);
  /** Submission handler */
  onSubmit: (values: NoteForm, formikHelpers: FormikHelpers<NoteForm>) => void | Promise<any>;
  /** Optional - callback to notify when save button is pressed. */
  onSaveClick?: (noteForm: NoteForm, formikProps: FormikProps<NoteForm>) => void;
  /** Optional - callback to notify when cancel button is pressed. */
  onCancelClick?: (formikProps: FormikProps<NoteForm>) => void;
}

export const UpdateNoteFormModal = React.forwardRef<
  FormikProps<NoteForm>,
  IUpdateNoteFormModalProps
>((props, ref) => {
  const {
    loading,
    isOpened,
    onSaveClick,
    onCancelClick,
    initialValues,
    validationSchema,
    onSubmit,
  } = props;

  const spinner = <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;

  return (
    <Formik<NoteForm>
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
          message={loading ? spinner : <FormBody {...formikProps}></FormBody>}
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

const FormBody: React.FC<React.PropsWithChildren<FormikProps<NoteForm>>> = ({ values }) => {
  return (
    <Container>
      <Row className="no-gutters">
        <Col md={2} className="mr-2">
          Created:
        </Col>
        <Col>
          <span>
            <strong>{prettyFormatUTCDate(values?.appCreateTimestamp)}</strong> by{' '}
            <UserNameTooltip
              userName={values?.appCreateUserid}
              userGuid={values?.appCreateUserGuid}
            />
          </span>
        </Col>
      </Row>
      <Row className="no-gutters">
        <Col md={2} className="mr-2">
          Last updated:
        </Col>
        <Col>
          <span>
            <strong>{prettyFormatUTCDate(values?.appLastUpdateTimestamp)}</strong> by{' '}
            <UserNameTooltip
              userName={values?.appLastUpdateUserid}
              userGuid={values?.appLastUpdateUserGuid}
            />
          </span>
        </Col>
      </Row>
      <Row className="no-gutters mt-3">
        <Col>
          <TextArea rows={15} field="note" label="Type a note:" data-testid="note-field"></TextArea>
        </Col>
      </Row>
    </Container>
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
