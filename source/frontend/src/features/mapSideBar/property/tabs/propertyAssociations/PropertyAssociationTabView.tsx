import * as React from 'react';
import {
  MdFence,
  MdOutlineCallMissedOutgoing,
  MdOutlineRealEstateAgent,
  MdTopic,
} from 'react-icons/md';

import { Section } from '@/components/common/Section/Section';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { Api_PropertyAssociations } from '@/models/api/Property';

import AssociationContent from './AssociationContent';
import AssociationHeader from './AssociationHeader';

export interface IPropertyAssociationTabViewProps {
  isLoading: boolean;
  associations?: Api_PropertyAssociations;
}

const PropertyAssociationTabView: React.FunctionComponent<
  React.PropsWithChildren<IPropertyAssociationTabViewProps>
> = props => {
  return (
    <StyledSummarySection>
      <Section>This property is associated with the following files.</Section>

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
          linkUrlMask="/mapview/sidebar/research/|id|"
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
          linkUrlMask="/mapview/sidebar/acquisition/|id|"
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
          linkUrlMask="/mapview/sidebar/lease/|id|"
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
