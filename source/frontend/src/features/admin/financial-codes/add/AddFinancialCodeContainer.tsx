import { AxiosError } from 'axios';
import React, { useState } from 'react';
import { Alert, Col, Row } from 'react-bootstrap';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { H1 } from '@/components/common/styles';
import { FinancialCodeTypes } from '@/constants/index';
import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';
import { IApiError } from '@/interfaces/IApiError';
import { Api_FinancialCode } from '@/models/api/FinancialCode';

import { AddFinancialCodeYupSchema } from './AddFinancialCodeYupSchema';

export interface IAddFinancialCodeFormProps {
  validationSchema?: any;
  onSave: (financialCode: Api_FinancialCode) => Promise<Api_FinancialCode | undefined>;
  onCancel: () => void;
  onSuccess: (financialCode: Api_FinancialCode) => Promise<void>;
  onError: (e: AxiosError<IApiError>) => void;
}

export interface IAddFinancialCodeContainerProps {
  View: React.FC<IAddFinancialCodeFormProps>;
}

export const AddFinancialCodeContainer: React.FC<IAddFinancialCodeContainerProps> = ({ View }) => {
  const [duplicateError, setDuplicateError] = useState(false);
  const history = useHistory();
  const {
    addFinancialCode: { execute: addFinancialCode },
  } = useFinancialCodeRepository();

  const createFinancialCode = async (financialCode: Api_FinancialCode) => {
    setDuplicateError(false);
    return addFinancialCode(financialCode.type as FinancialCodeTypes, financialCode);
  };

  // navigate back to list view
  const onCancel = () => {
    history.replace(`/admin/financial-code/list`);
  };

  const onCreateSuccess = async (financialCode: Api_FinancialCode) => {
    toast.success(`Financial code saved`);
    history.replace(`/admin/financial-code/list`);
  };

  // generic error handler: 409 means duplicate active code already found on datastore.
  const onCreateError = (e: AxiosError<IApiError>) => {
    if (e?.response?.status === 409) {
      setDuplicateError(true);
    } else {
      if (e?.response?.status === 400) {
        toast.error(e?.response.data.error);
      } else {
        toast.error('Unable to save. Please try again.');
      }
    }
  };

  return (
    <StyledContainer>
      <Row>
        <Col md={7}>
          <H1>Create Financial Code</H1>
        </Col>
      </Row>
      {duplicateError && (
        <Row>
          <Col md={7}>
            <Alert variant="danger">Cannot create duplicate financial code</Alert>
          </Col>
        </Row>
      )}
      <Row>
        <Col md={7}>
          <View
            validationSchema={AddFinancialCodeYupSchema}
            onSave={createFinancialCode}
            onCancel={onCancel}
            onSuccess={onCreateSuccess}
            onError={onCreateError}
          />
        </Col>
      </Row>
    </StyledContainer>
  );
};

export default AddFinancialCodeContainer;

const StyledContainer = styled.div`
  width: 100%;
  overflow-y: auto;
  padding: 3rem;
  > .row {
    justify-content: center;
  }
  form {
    text-align: left;
  }
`;
