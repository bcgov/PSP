import Multiselect from 'multiselect-react-dropdown';
import * as React from 'react';
import { FaExternalLinkAlt } from 'react-icons/fa';

import ContactLink from '@/components/common/ContactLink';
import { readOnlyMultiSelectStyle } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledLink } from '@/components/common/styles';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { exists, prettyFormatDate } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

interface IPropertyActivityDetailsSubViewProps {
  activity: ApiGen_Concepts_ManagementActivity | null;
}

const PropertyActivityDetailsSubView: React.FunctionComponent<
  IPropertyActivityDetailsSubViewProps
> = props => {
  const selectedSubTypes: ApiGen_Base_CodeType<string>[] =
    props.activity?.activitySubTypeCodes.map(x => x.managementActivitySubtypeCode).filter(exists) ??
    [];

  return (
    <Section header="Activity Details">
      <SectionField label="Activity type" contentWidth={{ xs: 7 }}>
        {props.activity.activityTypeCode?.description}
      </SectionField>
      <SectionField label="Sub-type(s)" contentWidth={{ xs: 7 }}>
        <Multiselect
          disable
          disablePreSelectedValues
          hidePlaceholder
          placeholder=""
          selectedValues={selectedSubTypes}
          displayValue="description"
          style={readOnlyMultiSelectStyle}
        />
      </SectionField>
      <SectionField label="Activity status" contentWidth={{ xs: 7 }}>
        {props.activity.activityStatusTypeCode?.description}
      </SectionField>
      <SectionField label="Commencement" contentWidth={{ xs: 7 }}>
        {prettyFormatDate(props.activity.requestAddedDateOnly)}
      </SectionField>
      <SectionField label="Completion" contentWidth={{ xs: 7 }}>
        {prettyFormatDate(props.activity.completionDateOnly)}
      </SectionField>
      <SectionField label="Description" contentWidth={{ xs: 7 }}>
        {props.activity.description}
      </SectionField>

      <SectionField label="Ministry contacts" contentWidth={{ xs: 7 }}>
        {props.activity.ministryContacts?.map(contact => (
          <>{contact.person !== null && <ContactLink person={contact.person} />}</>
        ))}
      </SectionField>
      <SectionField
        label="Requestor"
        contentWidth={{ xs: 7 }}
        tooltip="Document the source of the request by entering the name of the person, organization or other entity from which the request has been received"
      >
        <>
          {exists(props.activity.requestorPerson) && (
            <ContactLink person={props.activity.requestorPerson} />
          )}
          {exists(props.activity.requestorOrganization) && (
            <ContactLink organization={props.activity.requestorOrganization} />
          )}
        </>
      </SectionField>
      {exists(props.activity.requestorPrimaryContactId) && (
        <SectionField
          label="Primary contact"
          valueTestId="requestorPrimaryContact"
          contentWidth={{ xs: 7 }}
        >
          {props.activity?.requestorPrimaryContactId ? (
            <StyledLink
              target="_blank"
              rel="noopener noreferrer"
              to={`/contact/P${props.activity?.requestorPrimaryContactId}`}
            >
              <span>
                {props.activity?.requestorPrimaryContactId
                  ? formatApiPersonNames(props.activity?.requestorPrimaryContact)
                  : ''}
              </span>
              <FaExternalLinkAlt className="ml-2" size="1rem" />
            </StyledLink>
          ) : (
            'No contacts available'
          )}
        </SectionField>
      )}
      <SectionField label="External contacts" contentWidth={{ xs: 8 }}>
        {props.activity.involvedParties?.map(contact => (
          <>
            {contact.person !== null && <ContactLink person={contact.person} />}
            {contact.organization !== null && <ContactLink organization={contact.organization} />}
          </>
        ))}
      </SectionField>
      <SectionField label="Service provider" contentWidth={{ xs: 7 }}>
        <>
          {props.activity.serviceProviderPerson !== null && (
            <ContactLink person={props.activity.serviceProviderPerson} />
          )}
          {props.activity.serviceProviderOrg !== null && (
            <ContactLink organization={props.activity.serviceProviderOrg} />
          )}
        </>
      </SectionField>
    </Section>
  );
};

export default PropertyActivityDetailsSubView;
