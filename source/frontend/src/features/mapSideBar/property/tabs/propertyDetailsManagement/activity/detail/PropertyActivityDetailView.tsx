import clsx from 'classnames';
import React from 'react';
import { MdClose } from 'react-icons/md';
import { useHistory } from 'react-router-dom';
import ReactVisibilitySensor from 'react-visibility-sensor';
import styled from 'styled-components';

import ContactLink from '@/components/common/ContactLink';
import EditButton from '@/components/common/EditButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import * as Styled from '@/components/common/styles';
import { Claims } from '@/constants/index';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { ApiGen_Concepts_PropertyActivityInvoice } from '@/models/api/generated/ApiGen_Concepts_PropertyActivityInvoice';
import { formatMoney, prettyFormatDate } from '@/utils';

import { InvoiceView } from './InvoiceView';

export interface IPropertyActivityDetailViewProps {
  propertyId: number;
  activity: ApiGen_Concepts_PropertyActivity | null;
  onClose: () => void;
  loading: boolean;
  show: boolean;
  setShow: (show: boolean) => void;
}

export const PropertyActivityDetailView: React.FunctionComponent<
  React.PropsWithChildren<IPropertyActivityDetailViewProps>
> = props => {
  const onCloseClick = () => {
    props.setShow(false);
    props.onClose();
  };

  const { hasClaim } = useKeycloakWrapper();
  const history = useHistory();

  if (props.activity !== null) {
    const invoices: ApiGen_Concepts_PropertyActivityInvoice[] = props.activity.invoices ?? [];
    return (
      <ReactVisibilitySensor
        onChange={(isVisible: boolean) => {
          !isVisible && props.setShow(true);
        }}
      >
        <Styled.PopupTray className={clsx({ show: props.show })}>
          <Styled.TrayHeader>
            Property Activity
            <Styled.CloseButton
              id="close-tray"
              icon={<MdClose size={20} />}
              title="close"
              onClick={onCloseClick}
            />
          </Styled.TrayHeader>
          <Styled.TrayContent>
            <StyledFormWrapper>
              <StyledSummarySection>
                <LoadingBackdrop show={props.loading} />
                <StyledEditWrapper className="mr-3 my-1">
                  {hasClaim(Claims.MANAGEMENT_EDIT) && (
                    <EditButton
                      title="Edit property activity"
                      onClick={() => {
                        history.push(
                          `/mapview/sidebar/property/${props.propertyId}/management/activity/${props.activity?.id}/edit`,
                        );
                      }}
                    />
                  )}
                </StyledEditWrapper>

                <Section header="Activity Details">
                  <SectionField label="Activity type" contentWidth="7">
                    {props.activity.activityTypeCode?.description}
                  </SectionField>
                  <SectionField label="Sub-type" contentWidth="7">
                    {props.activity.activitySubtypeCode?.description}
                  </SectionField>
                  <SectionField label="Activity status" contentWidth="7">
                    {props.activity.activityStatusTypeCode?.description}
                  </SectionField>
                  <SectionField label="Requested added date" contentWidth="7">
                    {prettyFormatDate(props.activity.requestAddedDateOnly)}
                  </SectionField>
                  <SectionField label="Completion date" contentWidth="7">
                    {prettyFormatDate(props.activity.completionDateOnly)}
                  </SectionField>
                  <SectionField label="Description" contentWidth="7">
                    {props.activity.description}
                  </SectionField>

                  <SectionField label="Ministry contacts" contentWidth="7">
                    {props.activity.ministryContacts?.map(contact => (
                      <>{contact.person !== null && <ContactLink person={contact.person} />}</>
                    ))}
                  </SectionField>
                  <SectionField label="Requested source" contentWidth="7">
                    {props.activity.requestSource}
                  </SectionField>
                  <SectionField label="Involved parties" contentWidth="8">
                    {props.activity.involvedParties?.map(contact => (
                      <>
                        {contact.person !== null && <ContactLink person={contact.person} />}
                        {contact.organization !== null && (
                          <ContactLink organization={contact.organization} />
                        )}
                      </>
                    ))}
                  </SectionField>
                  <SectionField label="Service provider" contentWidth="7">
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
                {invoices.map((x: ApiGen_Concepts_PropertyActivityInvoice, index: number) => (
                  <InvoiceView
                    key={`activity-${x.propertyActivityId}-invoice-${x.id}`}
                    activityInvoice={x}
                    index={index}
                  />
                ))}
                <Section header="Invoices Total">
                  <SectionField label="Total (before tax)" contentWidth="7">
                    {formatMoney(props.activity.pretaxAmt)}
                  </SectionField>
                  <SectionField label="GST amount" contentWidth="7">
                    {formatMoney(props.activity.gstAmt)}
                  </SectionField>
                  <SectionField label="PST amount" contentWidth="7">
                    {formatMoney(props.activity.pstAmt)}
                  </SectionField>
                  <SectionField label="Total amount" contentWidth="7">
                    {formatMoney(props.activity.totalAmt)}
                  </SectionField>
                </Section>
              </StyledSummarySection>
            </StyledFormWrapper>

            <DocumentListContainer
              title="File Documents"
              parentId={props.activity?.id.toString() ?? ''}
              addButtonText="Add a Management Document"
              relationshipType={ApiGen_CodeTypes_DocumentRelationType.ManagementFiles}
            />
          </Styled.TrayContent>
        </Styled.PopupTray>
      </ReactVisibilitySensor>
    );
  } else {
    return <></>;
  }
};

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
`;
