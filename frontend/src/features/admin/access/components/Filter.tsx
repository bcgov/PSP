import './Filter.scss';

import { Button } from 'components/common/form/Button';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { IMenuItemProps, Menu } from 'components/menu/Menu';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import * as React from 'react';
import { Col, Container, Form, Row } from 'react-bootstrap';
import { FaCaretDown, FaSearch, FaUndo } from 'react-icons/fa';
import { IAccessRequestsFilterData } from 'store/slices/accessRequests';
import { useLookupCodes } from 'store/slices/lookupCodes';

interface IProps {
  initialValues?: IAccessRequestsFilterData;
  applyFilter: (filter: IAccessRequestsFilterData) => void;
}

export const defaultFilter: IAccessRequestsFilterData = { searchText: '', role: '', agency: '' };

export const AccessRequestFilter = (props: IProps) => {
  const [filterState, setFilterState] = React.useState(props.initialValues || defaultFilter);
  const lookupCodes = useLookupCodeHelpers();
  const { fetchLookupCodes } = useLookupCodes();
  React.useEffect(() => {
    fetchLookupCodes();
  }, [fetchLookupCodes]);

  const agencies: IMenuItemProps[] = lookupCodes.getByType('Agency').map(value => {
    return {
      label: value.name,
      onClick: () => setFilterState({ ...filterState, agency: value.name }),
    };
  });

  const roles: IMenuItemProps[] = lookupCodes.getByType('Role').map(value => {
    return {
      label: value.name,
      onClick: () => setFilterState({ ...filterState, role: value.name }),
    };
  });

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
    <Container className="Access-Requests-filter">
      <Row className="filters">
        <Col className="filter">
          <Menu
            searchPlaceholder="Filter agencies"
            enableFilter={true}
            alignLeft={true}
            width="260px"
            options={agencies}
            disableScrollToMenuElement={true}
          >
            {`Agency: ${filterState.agency || 'Show all'}`}&nbsp;&nbsp;
            <FaCaretDown />
          </Menu>
        </Col>
        <Col>
          <Menu
            disableScrollToMenuElement={true}
            searchPlaceholder="Filter roles"
            enableFilter={true}
            alignLeft={true}
            width="200px"
            options={roles}
          >
            {`Role: ${filterState.role || 'Show all'}`}&nbsp;&nbsp;
            <FaCaretDown />
          </Menu>
        </Col>
        <Col>
          <Form.Control
            type="text"
            placeholder="Search"
            value={filterState.searchText}
            onChange={handleSearchTextChange}
          />
        </Col>
        <Col className="actions">
          <TooltipWrapper toolTipId="map-filter-search-tooltip" toolTip="Search">
            <Button
              variant="warning"
              size="sm"
              onClick={search}
              className="bg-warning"
              icon={<FaSearch size={20} />}
            />
          </TooltipWrapper>
          <TooltipWrapper toolTipId="map-filter-reset-tooltip" toolTip="Reset Filter">
            <Button variant="secondary" size="sm" onClick={reset} icon={<FaUndo size={20} />} />
          </TooltipWrapper>
        </Col>
      </Row>
    </Container>
  );
};
