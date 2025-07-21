import clsx from 'classnames';
import React from 'react';
import { Col } from 'react-bootstrap';
import { useHistory } from 'react-router-dom';
import ReactVisibilitySensor from 'react-visibility-sensor';

import EditButton from '@/components/common/buttons/EditButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import * as Styled from '@/components/common/styles';
import { Claims } from '@/constants/index';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { ApiGen_Concepts_ManagementActivityInvoice } from '@/models/api/generated/ApiGen_Concepts_ManagementActivityInvoice';

import ActivityDetailInvoiceTotalsView from './ActivityDetailInvoiceTotalsView';
import PropertyActivityDetailsSubView from './ActivityDetailSubView';
import { InvoiceView } from './InvoiceView';

export interface IPropertyActivityDetailViewProps {
  propertyId: number;
  activity: ApiGen_Concepts_ManagementActivity | null;
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
    const invoices: ApiGen_Concepts_ManagementActivityInvoice[] = props.activity.invoices ?? [];

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

                <PropertyActivityDetailsSubView activity={props.activity} />

                {invoices.map((x: ApiGen_Concepts_ManagementActivityInvoice, index: number) => (
                  <InvoiceView
                    key={`activity-${x.managementActivityId}-invoice-${x.id}`}
                    activityInvoice={x}
                    index={index}
                  />
                ))}
                <ActivityDetailInvoiceTotalsView invoices={invoices} />
              </StyledSummarySection>
            </StyledFormWrapper>

            <DocumentListContainer
              title="File Documents"
              parentId={props.activity?.id.toString() ?? ''}
              addButtonText="Add a Management Document"
              relationshipType={ApiGen_CodeTypes_DocumentRelationType.ManagementActivities}
            />
          </Styled.TrayContent>
        </Styled.PopupTray>
      </ReactVisibilitySensor>
    );
  } else {
    return <></>;
  }
};
