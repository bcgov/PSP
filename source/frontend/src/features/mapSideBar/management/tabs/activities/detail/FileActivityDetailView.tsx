import clsx from 'classnames';
import React from 'react';
import { Col } from 'react-bootstrap';
import ReactVisibilitySensor from 'react-visibility-sensor';

import EditButton from '@/components/common/buttons/EditButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import * as Styled from '@/components/common/styles';
import FilePropertiesTable from '@/components/filePropertiesTable/FilePropertiesTable';
import { Claims } from '@/constants/index';
import DocumentActivityListContainer from '@/features/documents/list/DocumentActivityListContainer';
import DocumentListContainer from '@/features/documents/list/DocumentListContainer';
import ActivityDetailInvoiceTotalsView from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/detail/ActivityDetailInvoiceTotalsView';
import PropertyActivityDetailsSubView from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/detail/ActivityDetailSubView';
import { InvoiceView } from '@/features/mapSideBar/property/tabs/propertyDetailsManagement/activity/detail/InvoiceView';
import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_ManagementActivity } from '@/models/api/generated/ApiGen_Concepts_ManagementActivity';
import { ApiGen_Concepts_ManagementActivityInvoice } from '@/models/api/generated/ApiGen_Concepts_ManagementActivityInvoice';

export interface IFileActivityDetailViewProps {
  managementId: number;
  activity: ApiGen_Concepts_ManagementActivity | null;
  loading: boolean;
  show: boolean;
  canEditDocuments: boolean;
  canEditActivity: boolean;
  onClose: () => void;
  setShow: (show: boolean) => void;
}

export const FileActivityDetailView: React.FunctionComponent<
  React.PropsWithChildren<IFileActivityDetailViewProps>
> = props => {
  const onCloseClick = () => {
    props.setShow(false);
    props.onClose();
  };

  const { hasClaim } = useKeycloakWrapper();

  const pathGenerator = usePathGenerator();

  const activityAsFileProperties = props.activity?.activityProperties?.map(ap => ({
    id: ap.id,
    fileId: null,
    displayOrder: null,
    file: null,
    location: null,
    property: ap.property,
    propertyId: ap.propertyId,
    propertyName: null,
    rowVersion: null,
    isActive: null,
  }));

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
            <Styled.TrayHeader>File Activity</Styled.TrayHeader>
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
                  {hasClaim(Claims.MANAGEMENT_EDIT) && props.canEditActivity && (
                    <EditButton
                      title="Edit File Property Activity"
                      onClick={() => {
                        pathGenerator.editDetail(
                          'management',
                          props.managementId,
                          'activities',
                          props.activity?.id,
                        );
                      }}
                      style={{ float: 'right' }}
                    />
                  )}
                </StyledEditWrapper>

                <Section header="Activity Properties" isCollapsable={true}>
                  <FilePropertiesTable
                    disabledSelection
                    setSelectedFileProperties={null}
                    selectedFileProperties={activityAsFileProperties}
                    fileProperties={activityAsFileProperties}
                  />
                </Section>
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
              disableAdd={!props.canEditDocuments}
            />
            <DocumentActivityListContainer
              title={'Related Documents'}
              parentId={props.activity?.id.toString() ?? ''}
              relationshipType={ApiGen_CodeTypes_DocumentRelationType.ManagementActivities}
              disableAdd={true}
            />
          </Styled.TrayContent>
        </Styled.PopupTray>
      </ReactVisibilitySensor>
    );
  } else {
    return <></>;
  }
};
