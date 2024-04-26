import './Filter.scss';

import { useState } from 'react';
import { Form } from 'react-bootstrap';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import { FaUndo } from 'react-icons/fa';
import styled from 'styled-components';

import { SearchButton } from '@/components/common/buttons';
import { Button } from '@/components/common/buttons/Button';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { IAccessRequestsFilterData } from '@/features/admin/access-request/IAccessRequestsFilterData';

interface IProps {
  initialValues?: IAccessRequestsFilterData;
  applyFilter: (filter: IAccessRequestsFilterData) => void;
}

export const defaultFilter: IAccessRequestsFilterData = {
  searchText: '',
  role: '',
  organization: '',
};

export const AccessRequestFilter = (props: IProps) => {
  const [filterState, setFilterState] = useState(props.initialValues || defaultFilter);

  const reset = () => {
    setFilterState(defaultFilter);
    props.applyFilter(defaultFilter);
  };

  const search = () => {
    props.applyFilter(filterState);
  };

  const handleSearchTextChange = (event: any) =>
    setFilterState({ ...filterState, searchText: event.target.value });

  return (
    <FilterBoxForm className="p-3" onSubmit={(e: any) => e.preventDefault()}>
      <Row className="filters">
        <Col className="d-flex" md={4}>
          <Form.Label className="mr-4 font-weight-bold">Search:</Form.Label>
          <Form.Control
            title="Search by IDIR/Last Name"
            type="text"
            placeholder="IDIR/Last Name"
            value={filterState.searchText}
            onChange={handleSearchTextChange}
          />
        </Col>
        <Col className="actions d-flex" md={1}>
          <TooltipWrapper tooltipId="access-filter-search-tooltip" tooltip="Search">
            <SearchButton type="submit" onClick={search} />
          </TooltipWrapper>
          <TooltipWrapper tooltipId="access-filter-reset-tooltip" tooltip="Reset Filter">
            <Button
              title="reset"
              variant="info"
              size="sm"
              onClick={reset}
              icon={<FaUndo size={20} />}
            />
          </TooltipWrapper>
        </Col>
      </Row>
    </FilterBoxForm>
  );
};

export const FilterBoxForm = styled(Form)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
`;
