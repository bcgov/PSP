import './Filter.scss';

import { Button } from 'components/common/buttons/Button';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { IMenuItemProps, Menu } from 'components/menu/Menu';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import * as React from 'react';
import Col from 'react-bootstrap/Col';
import Container from 'react-bootstrap/Container';
import Form from 'react-bootstrap/Form';
import Row from 'react-bootstrap/Row';
import { FaCaretDown, FaSearch, FaUndo } from 'react-icons/fa';
import { IAccessRequestsFilterData } from 'store/slices/accessRequests';
import { useLookupCodes } from 'store/slices/lookupCodes';

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
  const [filterState, setFilterState] = React.useState(props.initialValues || defaultFilter);
  const lookupCodes = useLookupCodeHelpers();
  const { fetchLookupCodes } = useLookupCodes();
  React.useEffect(() => {
    fetchLookupCodes();
  }, [fetchLookupCodes]);

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
        <Col>
          <Menu
            disableScrollToMenuElement={true}
            searchPlaceholder="Filter roles"
            enableFilter={true}
            alignLeft={true}
            width="20.0rem"
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
            <Button variant="info" size="sm" onClick={reset} icon={<FaUndo size={20} />} />
          </TooltipWrapper>
        </Col>
      </Row>
    </Container>
  );
};
