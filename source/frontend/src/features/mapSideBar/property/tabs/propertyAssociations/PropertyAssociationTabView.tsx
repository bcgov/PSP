import AcquisitionIcon from '@/assets/images/acquisition-grey-icon.svg?react';
import DispositionIcon from '@/assets/images/disposition-grey-icon.svg?react';
import LeaseIcon from '@/assets/images/lease-grey-icon.svg?react';
import ManagementIcon from '@/assets/images/management-grey-icon.svg?react';
import ResearchIcon from '@/assets/images/research-grey-icon.svg?react';
import { Section } from '@/components/common/Section/Section';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import TooltipIcon from '@/components/common/TooltipIcon';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseRenewal } from '@/models/api/generated/ApiGen_Concepts_LeaseRenewal';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';
import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';

import AssociationContent from './AssociationContent';
import AssociationHeader from './AssociationHeader';
import { LeaseAssociationContent } from './LeaseAssociationContent';

export interface IPropertyAssociationTabViewProps {
  isLoading: boolean;
  associations?: ApiGen_Concepts_PropertyAssociations;
  associatedLeases: ApiGen_Concepts_Lease[];
  associatedLeaseStakeholders: ApiGen_Concepts_LeaseStakeholder[];
  associatedLeaseRenewals: ApiGen_Concepts_LeaseRenewal[];
}

const PropertyAssociationTabView: React.FunctionComponent<
  React.PropsWithChildren<IPropertyAssociationTabViewProps>
> = props => {
  const leaseAssociations =
    props.associations?.leaseAssociations?.filter(
      x => x.statusCode !== ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE,
    ) ?? [];
  const { hasClaim } = useKeycloakWrapper();

  return (
    <StyledSummarySection>
      <Section>
        This property is associated with the following files:{' '}
        <TooltipIcon
          toolTipId="duplicate-files-tooltip"
          toolTip="Duplicate Lease/Licence files are hidden in the Property Information, PIMS Files tab. Duplicate files can be located in the Manage Lease/Licence Files advanced search."
        ></TooltipIcon>
      </Section>

      <Section
        header={
          <AssociationHeader
            icon={<ResearchIcon title="User Profile" />}
            title="Research"
            count={props.associations?.researchAssociations?.length}
          />
        }
        isCollapsable
      >
        <AssociationContent
          associationName="research"
          associations={props.associations?.researchAssociations ?? undefined}
          linkUrlMask="/mapview/sidebar/research/|id|"
          disable={!hasClaim(Claims.RESEARCH_VIEW)}
        />
      </Section>
      <Section
        header={
          <AssociationHeader
            icon={<AcquisitionIcon title="Acquisition-Files" />}
            title="Acquisition"
            count={props.associations?.acquisitionAssociations?.length}
          />
        }
        isCollapsable
      >
        <AssociationContent
          associationName="acquisition"
          associations={props.associations?.acquisitionAssociations ?? undefined}
          linkUrlMask="/mapview/sidebar/acquisition/|id|"
          disable={!hasClaim(Claims.ACQUISITION_VIEW)}
        />
      </Section>
      <Section
        header={
          <AssociationHeader
            icon={<LeaseIcon title="Leases-Licences" />}
            title="Leases/Licences"
            count={leaseAssociations?.length}
          />
        }
        isCollapsable
      >
        <LeaseAssociationContent
          associationName="lease"
          associations={leaseAssociations}
          linkUrlMask="/mapview/sidebar/lease/|id|"
          stakeholders={props.associatedLeaseStakeholders}
          renewals={props.associatedLeaseRenewals}
          leases={props.associatedLeases}
          disable={!hasClaim(Claims.LEASE_VIEW)}
        />
      </Section>
      <Section
        header={
          <AssociationHeader
            icon={<ManagementIcon title="Management-Files" />}
            title="Management"
            count={props.associations?.managementAssociations?.length}
          />
        }
        isCollapsable
      >
        <AssociationContent
          associationName="management"
          associations={props.associations?.managementAssociations ?? undefined}
          linkUrlMask="/mapview/sidebar/management/|id|"
          disable={!hasClaim(Claims.MANAGEMENT_VIEW)}
        />
      </Section>
      <Section
        header={
          <AssociationHeader
            icon={<DispositionIcon title="Disposition-Files" />}
            title="Disposition"
            count={props.associations?.dispositionAssociations?.length}
          />
        }
        isCollapsable
      >
        <AssociationContent
          associationName="disposition"
          associations={props.associations?.dispositionAssociations ?? undefined}
          linkUrlMask="/mapview/sidebar/disposition/|id|"
          disable={!hasClaim(Claims.DISPOSITION_VIEW)}
        />
      </Section>
    </StyledSummarySection>
  );
};

export default PropertyAssociationTabView;
