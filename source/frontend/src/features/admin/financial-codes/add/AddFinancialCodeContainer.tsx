import { AxiosError } from 'axios';
import { H1 } from 'components/common/styles';
import { IApiError } from 'interfaces/IApiError';
import { Api_FinancialCode } from 'models/api/FinancialCode';
import React, { useState } from 'react';
import { Alert, Col, Row } from 'react-bootstrap';
import { toast } from 'react-toastify';
import styled from 'styled-components';

export interface IAddFinancialCodeFormProps {
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

  const createFinancialCode = async (financialCode: Api_FinancialCode) => {
    // TODO: Implement
    return financialCode;
  };

  const onCancel = () => {
    // TODO: navigate back to list view
  };

  const onCreateSuccess = async (financialCode: Api_FinancialCode) => {
    // TODO:
  };

  const onCreateError = (e: AxiosError<IApiError>) => {
    // TODO: generic error handler
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
