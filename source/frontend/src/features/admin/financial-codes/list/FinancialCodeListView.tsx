import { Button } from 'components/common/buttons/Button';
import { TableSort } from 'components/Table/TableSort';
import { Roles } from 'constants/roles';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_FinancialCode } from 'models/api/FinancialCode';
import React, { useCallback, useEffect, useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router';
import styled from 'styled-components';

import { useFinancialCodeRepository } from '../hooks/useFinancialCodeRepository';
import { FinancialCodeResults } from './FinancialCodeResults/FinancialCodeResults';
import * as Styled from './styles';

/**
 * Page that displays acquisition files information.
 */
export const FinancialCodeListView: React.FC = () => {
  const history = useHistory();
  const { hasRole } = useKeycloakWrapper();
  const {
    getFinancialCodes: {
      execute: getFinancialCodes,
      response: financialCodeResults,
      loading: financialCodesLoading,
    },
  } = useFinancialCodeRepository();

  const fetchData = useCallback(async () => {
    await getFinancialCodes();
  }, [getFinancialCodes]);

  useEffect(() => {
    if (financialCodeResults === undefined) {
      fetchData();
    }
  }, [fetchData, financialCodeResults]);

  // Sorting and filtering for this list view is performed client-side
  const [sort, setSort] = React.useState<TableSort<Api_FinancialCode>>({});

  const sortedFilteredFinancialCodes = useMemo(() => {
    if (financialCodeResults && financialCodeResults?.length > 0) {
      let codeItems = [...financialCodeResults];
      // TODO: ...
      return codeItems;
    } else {
      return [];
    }
  }, [financialCodeResults]);

  return (
    <Styled.ListPage>
      <Styled.Scrollable>
        <Styled.PageHeader>Financial Codes</Styled.PageHeader>
        <Styled.PageToolbar>
          <Row>
            <Col>{/* <AcquisitionFilter filter={filter} setFilter={changeFilter} /> */}</Col>
          </Row>
        </Styled.PageToolbar>
        {hasRole(Roles.SYSTEM_ADMINISTRATOR) && (
          <StyledAddButton onClick={() => history.push('/admin/financial-code/new')}>
            <FaPlus />
            &nbsp;Create financial code
          </StyledAddButton>
        )}
        <FinancialCodeResults
          results={sortedFilteredFinancialCodes}
          loading={financialCodesLoading}
          sort={sort}
          setSort={setSort}
        />
      </Styled.Scrollable>
    </Styled.ListPage>
  );
};

const StyledAddButton = styled(Button)`
  &.btn.btn-primary {
    background-color: ${props => props.theme.css.completedColor};
  }
`;

export default FinancialCodeListView;
