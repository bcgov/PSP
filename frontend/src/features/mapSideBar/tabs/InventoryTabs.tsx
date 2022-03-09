import TabView from 'components/common/TabView';
import * as React from 'react';
import { Col, Row, Tab } from 'react-bootstrap';

import { SectionField } from './SectionField';
import { StyledFormSection, StyledScrollable, StyledSectionHeader } from './SectionStyles';

interface IInventoryTabsProps {
  PropertyView: React.ReactNode;
  LtsaView: React.ReactNode;
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
export const InventoryTabs: React.FunctionComponent<IInventoryTabsProps> = ({
  PropertyView,
  LtsaView,
}) => {
  const longText =
    'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam porttitor nisl at elit vestibulum vestibulum. Nullam eget consectetur felis, id porta eros. Proin at massa rutrum, molestie lorem a, congue lorem.';
  return (
    <TabView defaultActiveKey={InventoryTabNames.property}>
      <Tab eventKey={InventoryTabNames.title} title="Title">
        {LtsaView}
      </Tab>
      <Tab eventKey={InventoryTabNames.value} title="Value"></Tab>
      <Tab eventKey={InventoryTabNames.property} title="Property Details">
        {PropertyForm}
      </Tab>
      <Tab eventKey={InventoryTabNames.styles} title="STYLES">
        <StyledScrollable>
          <StyledFormSection>
            <StyledSectionHeader>Section Header</StyledSectionHeader>
            <SectionField label="Single line">A test description</SectionField>
            <SectionField label="Many lines">
              <div>Up</div> <div>Down</div>
              <div>Left</div>
              <div>Right</div>
            </SectionField>
            <SectionField label="Multiple inline">
              <span>Up</span> <span>Down</span>
              <span>Left</span>
              <span>Right</span>
            </SectionField>
          </StyledFormSection>

          <StyledFormSection>
            <StyledSectionHeader>Split section header</StyledSectionHeader>
            <Row>
              <Col>
                <SectionField label="Single line">A test description</SectionField>
              </Col>
              <Col>
                <Row>
                  <Col>
                    <SectionField label="Single line">A test description</SectionField>
                  </Col>
                  <Col>
                    <SectionField label="Single line">A test description</SectionField>
                    <SectionField label="Multiple inline">
                      <span>Up</span> <span>Down</span>
                      <span>Left</span>
                      <span>Right</span>
                    </SectionField>
                  </Col>
                </Row>
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
