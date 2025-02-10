import orderBy from 'lodash/orderBy';
import React, { useCallback, useEffect, useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router';
import styled from 'styled-components';

import AdminIcon from '@/assets/images/admin-icon.svg?react';
import * as CommonStyled from '@/components/common/styles';
import { StyledAddButton } from '@/components/common/styles';
import { TableSort } from '@/components/Table/TableSort';
import { Roles } from '@/constants/roles';
import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { isExpiredCode } from '@/utils/financialCodeUtils';

import {
  defaultFinancialCodeFilter,
  FinancialCodeFilter,
  IFinancialCodeFilter,
} from './FinancialCodeFilter/FinancialCodeFilter';
import { FinancialCodeResults } from './FinancialCodeResults/FinancialCodeResults';

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
  const [sort, setSort] = React.useState<TableSort<ApiGen_Concepts_FinancialCode>>({});
  const [filter, setFilter] = React.useState<IFinancialCodeFilter>(defaultFinancialCodeFilter);

  const sortedFilteredFinancialCodes = useMemo(() => {
    if (financialCodeResults && financialCodeResults?.length > 0) {
      let records = [...financialCodeResults];
      if (filter) {
        records = records.filter(record => {
          // Apply all the filters
          if (filter.financialCodeType && record.type !== filter.financialCodeType) {
            return false;
          }
          if (
            filter.codeValueOrDescription &&
            !record.code?.toLowerCase().includes(filter.codeValueOrDescription.toLowerCase()) &&
            !record.description?.toLowerCase().includes(filter.codeValueOrDescription.toLowerCase())
          ) {
            return false;
          }
          if (filter.showExpiredCodes === false && isExpiredCode(record)) {
            return false;
          }
          // Finally return true because record has satisfied all conditions
          return true;
        });
      }

      if (sort) {
        const sortFields = Object.keys(sort);
        if (sortFields?.length > 0) {
          const sortBy = sortFields[0] as keyof ApiGen_Concepts_FinancialCode;
          const sortDirection = sort[sortBy];
          records = orderBy(records, sortBy, sortDirection);
        } else {
          // Need to replace "undefined" display order with Number.Infinity so they can be sorted last
          for (const fc of records) {
            fc.displayOrder = fc.displayOrder ?? Infinity;
          }
          // If no custom sorting is provided, then use default sorting:
          // - "Display the results sorted by Code type, sort order and in absence of sort order Code value"
          records = orderBy(records, ['type', 'displayOrder', 'code'], ['asc', 'asc', 'asc']);
        }
      }
      return records;
    } else {
      return [];
    }
  }, [filter, financialCodeResults, sort]);

  return (
    <CommonStyled.ListPage>
      <CommonStyled.PaddedScrollable>
        <CommonStyled.H1>
          <FlexDiv>
            <div>
              <AdminIcon
                title="Admin Tools icon"
                width="2.6rem"
                height="2.6rem"
                fill="currentColor"
              />
              <span className="ml-2">Financial Codes</span>
            </div>
            {hasRole(Roles.SYSTEM_ADMINISTRATOR) && (
              <StyledAddButton onClick={() => history.push('/admin/financial-code/new')}>
                <FaPlus />
                &nbsp;Add a Financial Code
              </StyledAddButton>
            )}
          </FlexDiv>
        </CommonStyled.H1>
        <CommonStyled.PageToolbar>
          <Row>
            <Col>
              <FinancialCodeFilter filter={filter} setFilter={setFilter} />
            </Col>
          </Row>
        </CommonStyled.PageToolbar>

        <FinancialCodeResults
          results={sortedFilteredFinancialCodes}
          loading={financialCodesLoading}
          sort={sort}
          setSort={setSort}
        />
      </CommonStyled.PaddedScrollable>
    </CommonStyled.ListPage>
  );
};

export default FinancialCodeListView;

const FlexDiv = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.25rem;
`;
