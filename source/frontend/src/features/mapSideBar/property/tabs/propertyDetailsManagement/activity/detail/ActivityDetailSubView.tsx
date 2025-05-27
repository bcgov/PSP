import * as React from 'react';

import ContactLink from '@/components/common/ContactLink';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { prettyFormatDate } from '@/utils';

interface IPropertyActivityDetailsSubViewProps {
  activity: ApiGen_Concepts_PropertyActivity | null;
}

const PropertyActivityDetailsSubView: React.FunctionComponent<
  IPropertyActivityDetailsSubViewProps
> = props => {
  return (
    <Section header="Activity Details">
      <SectionField label="Activity type" contentWidth={{ xs: 7 }}>
        {props.activity.activityTypeCode?.description}
      </SectionField>
      <SectionField label="Sub-type" contentWidth={{ xs: 7 }}>
        {props.activity.activitySubtypeCode?.description}
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
        label="Contact manager"
        contentWidth={{ xs: 7 }}
        tooltip="Document the source of the request by entering the name of the person, organization or other entity from which the request has been received"
      >
        {props.activity.requestSource}
      </SectionField>
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
