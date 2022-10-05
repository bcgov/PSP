import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { DocumentRelationshipType } from 'constants/documentRelationshipType';
import DocumentListContainer from 'features/documents/list/DocumentListContainer';
import { StyledContainer } from 'features/documents/list/styles';
import { Api_ActivityTemplate } from 'models/api/Activity';
import Form from 'react-bootstrap/Form';
import styled from 'styled-components';

export interface IDocumentTemplateManagementViewProp {
  isLoading: boolean;
  activityTypes?: Api_ActivityTemplate[];
  activityTypeId: number;
  setActivityTemplateId: (a: number) => void;
}

export const DocumentTemplateManagementView: React.FunctionComponent<IDocumentTemplateManagementViewProp> = props => {
  console.log(props.activityTypes);

  const change = (par: React.ChangeEvent<HTMLInputElement>) => {
    console.log(par);
    console.log(par.target.value);
    props.setActivityTemplateId(Number.parseInt(par.target.value));
  };
  return (
    <StyledContainer>
      <LoadingBackdrop show={props.isLoading} />
      <StyledPageHeader>PIMS Document Template Management {props.activityTypeId}</StyledPageHeader>
      <Form.Group aria-label="Default select example">
        <Form.Control as="select" onChange={change}>
          <option>Open this select menu</option>
          {props.activityTypes?.map((types, index) => {
            return <option value={types.id}>{types.activityTemplateTypeCode?.description}</option>;
          })}
        </Form.Control>
      </Form.Group>
      <DocumentListContainer
        parentId={props.activityTypeId}
        relationshipType={DocumentRelationshipType.TEMPLATES}
      />
    </StyledContainer>
  );
};

export default DocumentTemplateManagementView;

const StyledPageHeader = styled.h3`
  text-align: left;
`;
