import TabView from 'components/common/TabView';
import * as React from 'react';
import { Col, Row, Tab } from 'react-bootstrap';

import { SectionField } from './SectionField';
import { StyledFormSection, StyledScrollable, StyledSectionHeader } from './SectionStyles';

interface IInventoryTabsProps {
  PropertyForm: React.ReactNode;
}

export enum InventoryTabNames {
  property = 'property',
  title = 'title',
  value = 'value',
  styles = 'styles',
}
/**
 * Tab wrapper, provides styling and nests form components within their corresponding tabs.
 * @param param0 object containing all react components for the corresponding tabs.
 */
export const InventoryTabs: React.FunctionComponent<IInventoryTabsProps> = ({ PropertyForm }) => {
  const longText =
    'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam porttitor nisl at elit vestibulum vestibulum. Nullam eget consectetur felis, id porta eros. Proin at massa rutrum, molestie lorem a, congue lorem.';
  return (
    <TabView defaultActiveKey={InventoryTabNames.property}>
      <Tab eventKey={InventoryTabNames.title} title="Title"></Tab>
      <Tab eventKey={InventoryTabNames.value} title="Value"></Tab>
      <Tab eventKey={InventoryTabNames.property} title="Property Details"></Tab>
      <Tab eventKey={InventoryTabNames.styles} title="STYLES">
        <StyledScrollable>
          <StyledFormSection>
            <StyledSectionHeader>Section Header</StyledSectionHeader>
            <SectionField label="Single line" content={['A test description']} />
            <SectionField
              label="Many lines"
              content={['Up', 'Down', 'Left', 'Right']}
              isMultiLine
            />
            <SectionField label="Multiple inline" content={['Yes', 'No', "I dont't know"]} />
          </StyledFormSection>

          <StyledFormSection>
            <StyledSectionHeader>Split section header</StyledSectionHeader>
            <Row>
              <Col>
                <SectionField label="A single label" content={['A test description']} />
              </Col>
              <Col>
                <SectionField
                  label="Many lines"
                  content={['Up', 'Down', 'Left', 'Right']}
                  isMultiLine
                />
                <SectionField label="Multiple inline" content={['Yes', 'No', "I dont't know"]} />
              </Col>
            </Row>
          </StyledFormSection>
          <StyledFormSection>
            <StyledSectionHeader>Section Header</StyledSectionHeader>
            {longText}
          </StyledFormSection>
        </StyledScrollable>
      </Tab>
    </TabView>
  );
};
