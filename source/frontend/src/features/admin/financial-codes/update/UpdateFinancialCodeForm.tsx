import axios, { AxiosError } from 'axios';
import { Formik } from 'formik';
import React from 'react';
import { ButtonToolbar, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import { FastDatePicker, Input } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import { FinancialCodeTypes } from '@/constants/index';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { IApiError } from '@/interfaces/IApiError';

import { formatFinancialCodeType } from '../financialCodeUtils';
import { FinancialCodeForm } from '../models';
import { IUpdateFinancialCodeFormProps } from './UpdateFinancialCodeContainer';

export const UpdateFinancialCodeForm: React.FC<IUpdateFinancialCodeFormProps> = ({
  financialCode,
  validationSchema,
  onSave,
  onCancel,
  onSuccess,
  onError,
}) => {
  // show confirmation message if user has made changes
  const { setModalContent, setDisplayModal } = useModalContext();
  const cancelFunc = (resetForm: () => void, dirty: boolean) => {
    const resetFormAndCancel = () => {
      resetForm();
      onCancel && onCancel();
    };
    if (!dirty) {
      resetFormAndCancel();
    } else {
      setModalContent({
        ...getCancelModalProps(),
        handleOk: () => {
          resetFormAndCancel();
          setDisplayModal(false);
        },
      });
      setDisplayModal(true);
    }
  };

  return (
    <Formik<FinancialCodeForm>
      enableReinitialize
      initialValues={
        financialCode ? FinancialCodeForm.fromApi(financialCode) : new FinancialCodeForm()
      }
      validationSchema={validationSchema}
      onSubmit={async (values, formikHelpers) => {
        try {
          const createdCode = await onSave(values.toApi());
          if (!!createdCode?.id) {
            formikHelpers.resetForm({
              values: FinancialCodeForm.fromApi(createdCode),
            });
            await onSuccess(createdCode);
          }
        } catch (e) {
          if (axios.isAxiosError(e)) {
            const axiosError = e as AxiosError<IApiError>;
            onError && onError(axiosError);
          }
        } finally {
          formikHelpers.setSubmitting(false);
        }
      }}
    >
      {formikProps => (
        <Container>
          <SectionField label="Code type" labelWidth="2">
            <span>{formatFinancialCodeType(formikProps.values.type as FinancialCodeTypes)}</span>
          </SectionField>
          <SectionField label="Code value" required labelWidth="2">
            <Input field="code" type="text" placeholder="Code value" />
          </SectionField>
          <SectionField label="Code description" required labelWidth="2">
            <Input field="description" type="text" placeholder="Code description" />
          </SectionField>
          <SectionField
            label="Effective date"
            required
            labelWidth="2"
            tooltip="Starting this date the code will be available in the system"
          >
            <FastDatePicker field="effectiveDate" formikProps={formikProps} />
          </SectionField>
          <SectionField
            label="Expiry date"
            labelWidth="2"
            tooltip="Starting this date the code will NOT be available in the system"
          >
            <FastDatePicker field="expiryDate" formikProps={formikProps} />
          </SectionField>
          <SectionField label="Display order" labelWidth="2">
            <Input field="displayOrder" type="number" />
          </SectionField>

          <Row className="justify-content-md-center">
            <ButtonToolbar className="cancelSave">
              <Button
                className="mr-5"
                variant="secondary"
                type="button"
                onClick={() => cancelFunc(formikProps.resetForm, formikProps.dirty)}
              >
                Cancel
              </Button>
              <Button className="mr-5" onClick={() => formikProps.submitForm()}>
                Save
              </Button>
            </ButtonToolbar>
          </Row>
        </Container>
      )}
    </Formik>
  );
};

export default UpdateFinancialCodeForm;

const Container = styled.div`
  .form-section {
    margin: 0;
    padding-left: 0;
  }

  .tab-pane {
    .form-section {
      margin: 1.5rem;
      padding-left: 1.5rem;
    }
  }
`;
