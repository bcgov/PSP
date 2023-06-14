import { FieldArray, Form, Formik, FormikProps } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaTrash } from 'react-icons/fa';
import styled from 'styled-components';

import { Button, StyledRemoveLinkButton } from '@/components/common/buttons';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { Api_Agreement } from '@/models/api/Agreement';
import { ILookupCode } from '@/store/slices/lookupCodes';

import AgreementSubForm from './AgreementSubForm';
import { AgreementsFormModel, SingleAgreementFormModel } from './models';
import { UpdateAgreementsYupSchema } from './UpdateAgreementsYupSchema';

export interface IUpdateAgreementsFormProps {
  isLoading: boolean;
  formikRef: React.Ref<FormikProps<AgreementsFormModel>>;
  initialValues: AgreementsFormModel;
  agreementTypes: ILookupCode[];
  onSave: (apiAcquisitionFile: AgreementsFormModel) => Promise<Api_Agreement[] | undefined>;
}

export const UpdateAgreementsForm: React.FC<IUpdateAgreementsFormProps> = ({
  isLoading,
  formikRef,
  initialValues,
  agreementTypes,
  onSave,
}) => {
  const field = 'agreements';

  const { setModalContent, setDisplayModal } = useModalContext();

  const onRemove = (index: number, removeCallback: (index: number) => void) => {
    removeCallback(index);
  };

  return (
    <StyledFormWrapper>
      <Formik<AgreementsFormModel>
        enableReinitialize
        innerRef={formikRef}
        initialValues={initialValues}
        onSubmit={async values => {
          await onSave(values);
        }}
        validationSchema={UpdateAgreementsYupSchema}
      >
        {formikProps => (
          <Form>
            <LoadingBackdrop show={isLoading}></LoadingBackdrop>

            <FieldArray
              name={field}
              render={arrayHelpers => {
                return (
                  <>
                    <Button
                      className="m-4"
                      onClick={() => arrayHelpers.push(new SingleAgreementFormModel())}
                      variant="success"
                    >
                      + Create new agreement
                    </Button>
                    {formikProps.values.agreements.map((agreement, index) => (
                      <Section
                        key={`agreement-edit-${index}`}
                        header={`Agreement ${index + 1}`}
                        isCollapsable
                        initiallyExpanded
                      >
                        <Row className="align-items-end pb-0">
                          <Col />
                          <Col xs="auto">
                            <StyledRemoveLinkButton
                              title="Delete Agreement"
                              variant="light"
                              onClick={() => {
                                setModalContent({
                                  ...getDeleteModalProps(),
                                  handleOk: () => {
                                    onRemove(index, arrayHelpers.remove);
                                    setDisplayModal(false);
                                  },
                                });
                                setDisplayModal(true);
                              }}
                            >
                              <FaTrash size="2rem" />
                            </StyledRemoveLinkButton>
                          </Col>
                        </Row>
                        <AgreementSubForm
                          index={index}
                          nameSpace={`${field}.${index}`}
                          formikProps={formikProps}
                          agreementTypes={agreementTypes}
                        />
                      </Section>
                    ))}
                  </>
                );
              }}
            ></FieldArray>
          </Form>
        )}
      </Formik>
    </StyledFormWrapper>
  );
};

const StyledFormWrapper = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;
