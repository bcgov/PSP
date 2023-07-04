import { FieldArray, Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import styled from 'styled-components';

import { Form } from '@/components/common/form/Form';
import GenericModal from '@/components/common/GenericModal';
import { Api_GenerateOwner } from '@/models/generate/GenerateOwner';
import { withNameSpace } from '@/utils/formUtils';

import { LetterRecipientModel } from './models/LetterRecipientModel';
import { LetterRecipientsForm } from './models/LetterRecipientsForm';
import { LetterRecipientsFormYupSchema } from './models/LetterRecipientsFormYupSchema';

export interface IGenerateLetterRecipientsModalProps {
  isOpened: boolean;
  recipientList: LetterRecipientModel[];
  onGenerateLetterOk: (recipients: Api_GenerateOwner[]) => void;
  onCancelClick: () => void;
  formikRef: React.Ref<FormikProps<LetterRecipientsForm>>;
}

const GenerateLetterRecipientsModal: React.FunctionComponent<
  React.PropsWithChildren<IGenerateLetterRecipientsModalProps>
> = props => {
  const { isOpened, onCancelClick, recipientList, onGenerateLetterOk, formikRef } = props;
  const initialValuesRecipients: LetterRecipientsForm = {
    recipients: [],
  };

  const handleGenerateLetterSubmit = (
    values: LetterRecipientsForm,
    formikHelpers: FormikHelpers<LetterRecipientsForm>,
  ) => {
    const selectedRecipientsIds = values.recipients.map(x => +x);
    const selectedGenerateOwner = recipientList
      .filter(x => selectedRecipientsIds.includes(x.id))
      .map(rec => rec.generateModel);

    onGenerateLetterOk(selectedGenerateOwner);
    formikHelpers.resetForm();
    formikHelpers.setSubmitting(false);
  };

  return (
    <Formik<LetterRecipientsForm>
      enableReinitialize
      innerRef={formikRef}
      validationSchema={LetterRecipientsFormYupSchema}
      initialValues={initialValuesRecipients}
      onSubmit={handleGenerateLetterSubmit}
    >
      {formikProps => (
        <StyledModal
          display={isOpened}
          title="Generate letter"
          message={
            <>
              <p>Select who should receive this letter from the following list</p>
              <p>
                <strong>Available recipients in this file:</strong>
              </p>

              <FieldArray
                name={withNameSpace('recipients')}
                render={arrayHelpers => (
                  <Form.Group>
                    {recipientList.map((rec: LetterRecipientModel, index: number) => (
                      <Form.Check
                        id={`recipient-${index}`}
                        type="checkbox"
                        name="recipients"
                        key={rec.id}
                      >
                        <Form.Check.Input
                          id={'recipient-' + index}
                          type="checkbox"
                          name="recipients"
                          value={rec.id}
                          onChange={formikProps.handleChange}
                        />
                        <Form.Check.Label htmlFor={'recipient-' + index}>
                          {rec.generateModel?.owner_string}
                        </Form.Check.Label>
                      </Form.Check>
                    ))}
                  </Form.Group>
                )}
              />
              {Object.values(formikProps.errors).length > 0 && (
                <div className="invalid-feedback" data-testid="team-profile-dup-error">
                  {formikProps.errors?.recipients?.toString()}
                </div>
              )}
            </>
          }
          closeButton
          okButtonText="Continue"
          cancelButtonText="Cancel"
          handleOk={() => formikProps.submitForm()}
          handleCancel={() => {
            formikProps.resetForm();
            onCancelClick();
          }}
        ></StyledModal>
      )}
    </Formik>
  );
};

export default GenerateLetterRecipientsModal;

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
