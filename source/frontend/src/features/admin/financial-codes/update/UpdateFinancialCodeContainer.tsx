import { AxiosError } from 'axios';
import React, { useEffect, useState } from 'react';
import { Alert, Col, Row } from 'react-bootstrap';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { H1 } from '@/components/common/styles';
import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';

import { UpdateFinancialCodeYupSchema } from './UpdateFinancialCodeYupSchema';

export interface IUpdateFinancialCodeFormProps {
  financialCode?: ApiGen_Concepts_FinancialCode;
  validationSchema?: any;
  onSave: (
    financialCode: ApiGen_Concepts_FinancialCode,
  ) => Promise<ApiGen_Concepts_FinancialCode | undefined>;
  onCancel: () => void;
  onSuccess: (financialCode: ApiGen_Concepts_FinancialCode) => Promise<void>;
  onError: (e: AxiosError<IApiError>) => void;
}

export interface IUpdateFinancialCodeContainerProps {
  type: ApiGen_Concepts_FinancialCodeTypes;
  id: number;
  View: React.FC<IUpdateFinancialCodeFormProps>;
}

export const UpdateFinancialCodeContainer: React.FC<IUpdateFinancialCodeContainerProps> = ({
  type,
  id,
  View,
}) => {
  const [duplicateError, setDuplicateError] = useState(false);
  const history = useHistory();
  const {
    getFinancialCode: {
      execute: getFinancialCode,
      response: financialCode,
      loading: loadingFinancialCode,
    },
    updateFinancialCode: { execute: updateFinancialCode },
  } = useFinancialCodeRepository();

  useEffect(() => {
    if (type !== undefined && id) {
      getFinancialCode(type, id);
    }
  }, [getFinancialCode, id, type]);

  const onSave = async (financialCode: ApiGen_Concepts_FinancialCode) => {
    setDuplicateError(false);
    return updateFinancialCode(financialCode);
  };

  // navigate back to list view
  const onCancel = () => {
    history.replace(`/admin/financial-code/list`);
  };

  const onUpdateSuccess = async () => {
    toast.success(`Financial code saved`);
    history.replace(`/admin/financial-code/list`);
  };

  // generic error handler: 409 means duplicate active code already found on datastore.
  const onUpdateError = (e: AxiosError<IApiError>) => {
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

  if (loadingFinancialCode) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <StyledContainer>
      <Row>
        <Col md={7}>
          <H1>Update Financial Code</H1>
        </Col>
      </Row>
      {duplicateError && (
        <Row>
          <Col md={7}>
            <Alert variant="danger">Cannot update duplicate financial code</Alert>
          </Col>
        </Row>
      )}
      <Row>
        <Col md={7}>
          <View
            financialCode={financialCode}
            validationSchema={UpdateFinancialCodeYupSchema}
            onSave={onSave}
            onCancel={onCancel}
            onSuccess={onUpdateSuccess}
            onError={onUpdateError}
          />
        </Col>
      </Row>
    </StyledContainer>
  );
};

export default UpdateFinancialCodeContainer;

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
