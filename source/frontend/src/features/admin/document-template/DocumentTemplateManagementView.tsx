import { Scrollable as ScrollableBase } from 'components/common/Scrollable/Scrollable';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import DocumentListContainer from 'features/documents/list/DocumentListContainer';
import { Section } from 'features/mapSideBar/tabs/Section';
import { Api_ActivityTemplate } from 'models/api/Activity';
import { Col, Row } from 'react-bootstrap';
import Form from 'react-bootstrap/Form';
import styled from 'styled-components';

export interface IDocumentTemplateManagementViewProp {
  isLoading: boolean;
  activityTypes?: Api_ActivityTemplate[];
  activityTypeId?: number;
  setActivityTemplateId: (templateId?: number) => void;
}

export const DocumentTemplateManagementView: React.FunctionComponent<
  React.PropsWithChildren<IDocumentTemplateManagementViewProp>
> = props => {
  const onSelectChange = (selectedType: React.ChangeEvent<HTMLInputElement>) => {
    var typeId = Number.parseInt(selectedType.target.value);
    if (!Number.isNaN(typeId)) {
      props.setActivityTemplateId(typeId);
    } else {
      props.setActivityTemplateId(undefined);
    }
  };

  return (
    <ListPage>
      <Scrollable>
        <LoadingBackdrop show={props.isLoading} />

        <StyledPageHeader>PIMS Document Template Management</StyledPageHeader>
        <Section>
          <Row>
            <Col xs="auto">Activity Type:</Col>
            <Col xs="auto">
              <Form.Group aria-label="Select activity type">
                <Form.Control as="select" onChange={onSelectChange}>
                  <option>Select an Activity type</option>
                  {props.activityTypes?.map(types => {
                    return (
                      <option value={types.id}>
                        {types.activityTemplateTypeCode?.description}
                      </option>
                    );
                  })}
                </Form.Control>
              </Form.Group>
            </Col>
          </Row>
        </Section>
        {props.activityTypeId !== undefined && (
          <DocumentListContainer
            parentId={props.activityTypeId}
            addButtonText="Add a Template"
            relationshipType={DocumentRelationshipType.TEMPLATES}
          />
        )}
      </Scrollable>
    </ListPage>
  );
};

export default DocumentTemplateManagementView;

const StyledPageHeader = styled.h3`
  text-align: left;
`;

export const ListPage = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  width: 100%;
  gap: 2.5rem;
  padding: 0;
`;

export const Scrollable = styled(ScrollableBase)`
  padding: 1.6rem 3.2rem;
  width: 100%;
`;
