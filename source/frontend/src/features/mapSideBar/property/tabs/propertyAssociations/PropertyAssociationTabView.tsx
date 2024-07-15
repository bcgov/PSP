import {
  MdFence,
  MdOutlineCallMissedOutgoing,
  MdOutlineRealEstateAgent,
  MdTopic,
} from 'react-icons/md';

import { Section } from '@/components/common/Section/Section';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseRenewal } from '@/models/api/generated/ApiGen_Concepts_LeaseRenewal';
import { ApiGen_Concepts_LeaseTenant } from '@/models/api/generated/ApiGen_Concepts_LeaseTenant';
import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';

import AssociationContent from './AssociationContent';
import AssociationHeader from './AssociationHeader';
import { LeaseAssociationContent } from './LeaseAssociationContent';

export interface IPropertyAssociationTabViewProps {
  isLoading: boolean;
  associations?: ApiGen_Concepts_PropertyAssociations;
  associatedLeases: ApiGen_Concepts_Lease[];
  associatedLeaseTenants: ApiGen_Concepts_LeaseTenant[];
  associatedLeaseRenewals: ApiGen_Concepts_LeaseRenewal[];
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
          associations={props.associations?.researchAssociations ?? undefined}
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
          associations={props.associations?.acquisitionAssociations ?? undefined}
          linkUrlMask="/mapview/sidebar/acquisition/|id|"
        />
      </Section>
      <Section
        header={
          <AssociationHeader
            icon={<MdFence title="Leases-Licences" size="2.5rem" />}
            title="Leases/Licences"
            count={props.associations?.leaseAssociations?.length}
          />
        }
        isCollapsable
      >
        <LeaseAssociationContent
          associationName="lease"
          associations={props.associations?.leaseAssociations ?? undefined}
          linkUrlMask="/mapview/sidebar/lease/|id|"
          tenants={props.associatedLeaseTenants}
          renewals={props.associatedLeaseRenewals}
          leases={props.associatedLeases}
        />
      </Section>
      <Section
        header={
          <AssociationHeader
            icon={<MdOutlineCallMissedOutgoing title="Disposition-Files" size="2.5rem" />}
            title="Disposition Files"
            count={props.associations?.dispositionAssociations?.length}
          />
        }
        isCollapsable
      >
        <AssociationContent
          associationName="disposition"
          associations={props.associations?.dispositionAssociations ?? undefined}
          linkUrlMask="/mapview/sidebar/disposition/|id|"
        />
      </Section>
    </StyledSummarySection>
  );
};

export default PropertyAssociationTabView;
