import clsx from 'classnames';
import * as React from 'react';
import { Tab, Tabs } from 'react-bootstrap';
import styled from 'styled-components';

interface IInventoryTabsProps {
  PropertyForm: React.ReactNode;
  disabled?: boolean;
}

export enum InventoryTabNames {
  property = 'property',
  title = 'title',
  owner = 'owner',
  value = 'value',
  history = 'history',
  notes = 'notes',
}

const StyledTabs = styled.div`
  .nav-tabs {
    border: none;
  }
  .nav-tabs .nav-item.nav-link {
    color: ${props => props.theme.css.slideOutBlue};
    border: #8c8c8c solid 3px;
    background-color: white;
    font-size: 16px;
    margin: 0px 4px -2px 0px;
    height: 45px;
  }
  .nav-tabs .nav-item.nav-link.active {
    color: ${props => props.theme.css.textColor};
    background-color: ${props => props.theme.css.filterBackgroundColor};
    border-bottom: none;
  }
  .tab-content {
    background-color: ${props => props.theme.css.filterBackgroundColor};
    border: #8c8c8c solid 3px;
    border-radius: 4px;
    padding: 0 2rem 2rem;
    height: calc(100% - 45px);
    display: flex;
    flex-direction: column;
  }
  .tab-pane {
    overflow-y: auto;
    position: relative;
  }

  &.disabled {
    opacity: 40%;
  }
`;

/**
 * Tab wrapper, provides styling and nests form components within their corresponding tabs.
 * @param param0 object containing all react components for the corresponding tabs.
 */
const InventoryTabs: React.FunctionComponent<IInventoryTabsProps> = ({
  PropertyForm,
  disabled,
}) => {
  return (
    <StyledTabs className={clsx('tab-wrapper', disabled ? 'disabled' : '')}>
      <Tabs defaultActiveKey={InventoryTabNames.property}>
        <Tab disabled={disabled} eventKey={InventoryTabNames.property} title="Property">
          {PropertyForm}
        </Tab>
        <Tab disabled={disabled} eventKey={InventoryTabNames.title} title="Title"></Tab>
        <Tab disabled={disabled} eventKey={InventoryTabNames.owner} title="Owner"></Tab>
        <Tab disabled={disabled} eventKey={InventoryTabNames.value} title="Value"></Tab>
        <Tab disabled={disabled} eventKey={InventoryTabNames.history} title="History"></Tab>
        <Tab disabled={disabled} eventKey={InventoryTabNames.notes} title="Notes"></Tab>
      </Tabs>
    </StyledTabs>
  );
};

export default InventoryTabs;
