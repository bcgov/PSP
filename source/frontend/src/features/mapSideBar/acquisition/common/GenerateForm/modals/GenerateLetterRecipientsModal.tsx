import { FieldArray, Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import { FaExternalLinkAlt } from 'react-icons/fa';
import { Link } from 'react-router-dom';
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
    const ids = values.recipients.map<string>(x => x as unknown as string);
    const selectedGenerateOwner = recipientList
      .filter(x => ids.includes(x.id))
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

              <StyledDiv>
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
                            {rec.interestType === 'OWNR' && (
                              <label>
                                {rec.generateModel?.owner_string}
                                <span className="type">{rec.getInterestTypeString()}</span>
                              </label>
                            )}
                            {rec.interestType !== 'OWNR' && rec.getContactRouteParam() && (
                              <StyledLinkWrapper>
                                <StyledLink
                                  target="_blank"
                                  rel="noopener noreferrer"
                                  to={`/contact/${rec.getContactRouteParam()}`}
                                >
                                  <span>{rec.getDisplayName()}</span>
                                  <FaExternalLinkAlt className="ml-2" size="1rem" />
                                </StyledLink>
                                <span className="type">{rec.getInterestTypeString()}</span>
                              </StyledLinkWrapper>
                            )}
                          </Form.Check.Label>
                        </Form.Check>
                      ))}
                    </Form.Group>
                  )}
                />
              </StyledDiv>
              {Object.values(formikProps.errors).length > 0 && (
                <div className="invalid-feedback" data-testid="missing-recipient-error">
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

const StyledLinkWrapper = styled.div`
  display: flex;
  flex-direction: row;
`;

const StyledLink = styled(Link)`
  display: flex;
  align-items: center;
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
`;

const StyledDiv = styled.div`
  border: 0.1rem solid ${props => props.theme.css.lightVariantColor};
  border-radius: 0.5rem;
  max-height: 180px;
  overflow-y: auto;
  padding: 0.5rem 1.5rem;

  .form-check {
    input {
      margin-top: 0.6rem;
    }
  }

  .form-group {
    label {
      font-family: BcSans-Bold;
      line-height: 1rem;
      color: ${props => props.theme.css.textColor};

      span.type {
        font-size: 1.5rem;
        font-family: BCSans-Italic;
        font-style: italic;
        margin-left: 0.5rem;
      }
    }
  }
`;
