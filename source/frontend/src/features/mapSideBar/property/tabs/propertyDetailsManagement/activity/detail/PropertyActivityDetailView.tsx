import clsx from 'classnames';
import React from 'react';
import { Col } from 'react-bootstrap';
import { useHistory } from 'react-router-dom';
import ReactVisibilitySensor from 'react-visibility-sensor';

import EditButton from '@/components/common/buttons/EditButton';
import ContactLink from '@/components/common/ContactLink';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import * as Styled from '@/components/common/styles';
import { Claims } from '@/constants/index';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
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

    let pretaxAmount = 0;
    let gstAmount = 0;
    let pstAmount = 0;
    let totalAmount = 0;

    for (let i = 0; i < invoices.length; i++) {
      pretaxAmount += invoices[i].pretaxAmount ?? 0;
      gstAmount += invoices[i].gstAmount ?? 0;
      pstAmount += invoices[i].pstAmount ?? 0;
      totalAmount += invoices[i].totalAmount ?? 0;
    }
    return (
      <ReactVisibilitySensor
        onChange={(isVisible: boolean) => {
          !isVisible && props.setShow(true);
        }}
      >
        <Styled.PopupTray className={clsx({ show: props.show })}>
          <Styled.TrayHeaderContent>
            <Styled.TrayHeader>Property Activity</Styled.TrayHeader>
            <Col xs="auto" className="text-right">
              <Styled.CloseIcon
                id="close-tray"
                title="close"
                onClick={onCloseClick}
              ></Styled.CloseIcon>
            </Col>
          </Styled.TrayHeaderContent>
          <Styled.TrayContent>
            <StyledFormWrapper>
              <StyledSummarySection>
                <LoadingBackdrop show={props.loading} />
                <StyledEditWrapper className="mr-3 my-1">
                  {hasClaim(Claims.MANAGEMENT_EDIT) && (
                    <EditButton
                      title="Edit Property Activity"
                      onClick={() => {
                        history.push(
                          `/mapview/sidebar/property/${props.propertyId}/management/activity/${props.activity?.id}/edit`,
                        );
                      }}
                      style={{ float: 'right' }}
                    />
                  )}
                </StyledEditWrapper>

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
                  <SectionField label="Requested added date" contentWidth={{ xs: 7 }}>
                    {prettyFormatDate(props.activity.requestAddedDateOnly)}
                  </SectionField>
                  <SectionField label="Completion date" contentWidth={{ xs: 7 }}>
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
                    {props.activity.requestSource}
                  </SectionField>
                  <SectionField label="Involved parties" contentWidth={{ xs: 8 }}>
                    {props.activity.involvedParties?.map(contact => (
                      <>
                        {contact.person !== null && <ContactLink person={contact.person} />}
                        {contact.organization !== null && (
                          <ContactLink organization={contact.organization} />
                        )}
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
                {invoices.map((x: ApiGen_Concepts_PropertyActivityInvoice, index: number) => (
                  <InvoiceView
                    key={`activity-${x.propertyActivityId}-invoice-${x.id}`}
                    activityInvoice={x}
                    index={index}
                  />
                ))}
                <Section header="Invoices Total">
                  <SectionField label="Total (before tax)" contentWidth={{ xs: 7 }}>
                    {formatMoney(pretaxAmount)}
                  </SectionField>
                  <SectionField label="GST amount" contentWidth={{ xs: 7 }}>
                    {formatMoney(gstAmount)}
                  </SectionField>
                  <SectionField label="PST amount" contentWidth={{ xs: 7 }}>
                    {formatMoney(pstAmount)}
                  </SectionField>
                  <SectionField label="Total amount" contentWidth={{ xs: 7 }}>
                    {formatMoney(totalAmount)}
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
