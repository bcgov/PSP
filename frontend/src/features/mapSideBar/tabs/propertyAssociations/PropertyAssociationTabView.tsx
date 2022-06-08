import { Api_PropertyAssociations } from 'models/api/Property';
import * as React from 'react';
import {
  MdFence,
  MdOutlineCallMissedOutgoing,
  MdOutlineRealEstateAgent,
  MdTopic,
} from 'react-icons/md';
import styled from 'styled-components';

import { Section } from '../Section';
import { StyledFormSection } from '../SectionStyles';
import AssociationContent from './AssociationContent';
import AssociationHeader from './AssociationHeader';

export interface IPropertyAssociationTabViewProps {
  isLoading: boolean;
  associations?: Api_PropertyAssociations;
}

const PropertyAssociationTabView: React.FunctionComponent<IPropertyAssociationTabViewProps> = props => {
  return (
    <StyledSummarySection>
      <StyledFormSection>This property is associated with the following files.</StyledFormSection>

      <Section
        header={
          <AssociationHeader
            icon={<MdTopic title="User Profile" size="2.5rem" />}
            title="Research Files"
            count={props.associations?.researchAssociations?.length}
          />
        }
        isCollapsable
      >
        <AssociationContent
          associationName="research"
          associations={props.associations?.researchAssociations}
          linkUrlMask="/mapview/research/|id|/view"
        />
      </Section>
      <Section
        header={
          <AssociationHeader
            icon={<MdOutlineRealEstateAgent title="Acquisition-Files" size="2.5rem" />}
            title="Acquisition Files"
            count={props.associations?.acquisitionAssociations?.length}
          />
        }
        isCollapsable
      >
        <AssociationContent
          associationName="acquisition"
          associations={props.associations?.acquisitionAssociations}
          linkUrlMask="/acquisition/|id|/details"
        />
      </Section>
      <Section
        header={
          <AssociationHeader
            icon={<MdFence title="Leases-Licenses" size="2.5rem" />}
            title="Leases/Licenses"
            count={props.associations?.leaseAssociations?.length}
          />
        }
        isCollapsable
      >
        <AssociationContent
          associationName="lease"
          associations={props.associations?.leaseAssociations}
          linkUrlMask="/lease/|id|/details"
        />
      </Section>
      <Section
        header={
          <AssociationHeader
            icon={<MdOutlineCallMissedOutgoing title="Leases-Licenses" size="2.5rem" />}
            title="Disposition Files"
            count={props.associations?.dispositionAssociations?.length}
          />
        }
        isCollapsable
      >
        <AssociationContent
          associationName="disposition"
          associations={props.associations?.dispositionAssociations}
          linkUrlMask="/dispositions/|id|/details"
        />
      </Section>
    </StyledSummarySection>
  );
};

export default PropertyAssociationTabView;

const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;
