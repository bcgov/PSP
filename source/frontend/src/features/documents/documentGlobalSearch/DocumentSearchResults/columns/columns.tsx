import { FaClock, FaEye, FaTimesCircle } from 'react-icons/fa';
import { generatePath } from 'react-router-dom';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import ViewButton from '@/components/common/buttons/ViewButton';
import { ExternalLink } from '@/components/common/ExternalLink';
import OverflowTip from '@/components/common/OverflowTip';
import { InlineFlexDiv } from '@/components/common/styles';
import { ColumnWithProps, renderGenTypeCode } from '@/components/Table';
import { Claims } from '@/constants/index';
import { documentQueueInError, documentQueueInProcess } from '@/features/documents/documentUtils';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_DocumentQueueStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_DocumentQueueStatusTypes';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_Concepts_DocumentSearchResult } from '@/models/api/generated/ApiGen_Concepts_DocumentSearchResult';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { ApiGen_Concepts_PropertyDocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_PropertyDocumentRelationship';
import { getApiPropertyName, relationshipTypeToPathName, stringToFragment } from '@/utils';

export interface IDocumentColumnProps {
  onViewDetails: (values: ApiGen_Concepts_DocumentRelationship) => void;
  onPreview: (mayanDocumentId: number) => void;
}

export const getDocumentSearchColumns = ({
  onViewDetails,
  onPreview,
}: IDocumentColumnProps): ColumnWithProps<ApiGen_Concepts_DocumentSearchResult>[] => {
  const documentColumns: ColumnWithProps<ApiGen_Concepts_DocumentSearchResult>[] = [
    {
      Header: 'Document name',
      accessor: 'fileName',
      width: 40,
      maxWidth: 40,
      sortable: false,
      Cell: renderFileName(onPreview),
    },
    {
      Header: 'Document type',
      accessor: 'documentType',
      align: 'left',
      sortable: false,
      width: 30,
      maxWidth: 30,
      Cell: renderDocumentType,
    },
    {
      Header: 'Status',
      accessor: 'statusTypeCode',
      sortable: false,
      width: 20,
      maxWidth: 20,
      Cell: renderGenTypeCode,
    },
    {
      Header: 'Location',
      accessor: 'documentRelationships',
      align: 'left',
      sortable: false,
      width: 30,
      maxWidth: 30,
      Cell: renderParentLinks(),
    },
    {
      Header: 'Property',
      accessor: 'propertiesDocuments',
      align: 'left',
      sortable: false,
      width: 30,
      maxWidth: 30,
      Cell: renderPropertyIdentifiers,
    },
    {
      Header: 'Actions',
      width: 10,
      maxWidth: 10,
      Cell: renderActions(onViewDetails),
    },
  ];

  return documentColumns;
};

function renderDocumentType({
  value,
}: CellProps<ApiGen_Concepts_DocumentSearchResult, ApiGen_Concepts_DocumentType | undefined>) {
  return stringToFragment(value?.documentTypeDescription ?? '');
}

const renderFileName = (onDisplayPreview: (mayanDocumentId: number) => void) => {
  return function (cell: CellProps<ApiGen_Concepts_DocumentSearchResult, string | undefined>) {
    const { hasClaim } = useKeycloakWrapper();
    const documentProcessed =
      (cell.row.original.mayanDocumentId &&
        cell.row.original.documentQueueStatusTypeCode?.id ===
          ApiGen_CodeTypes_DocumentQueueStatusTypes.SUCCESS) ||
      (cell.row.original.mayanDocumentId && cell.row.original.documentQueueStatusTypeCode === null);

    return (
      <StyledCellOverflow>
        {hasClaim(Claims.DOCUMENT_VIEW) && documentProcessed ? (
          <Button
            id={`document-view-filename-link-${cell.row.original?.id}`}
            data-testid={`document-view-filename-link-${cell.row.original?.id}`}
            onClick={() =>
              cell.row.original?.id && onDisplayPreview(cell.row.original.mayanDocumentId)
            }
            variant="link"
            title={cell.row.original.fileName}
          >
            {cell.value}
          </Button>
        ) : (
          <span
            id={`document-view-filename-text-${cell.row.original?.id}`}
            data-testid={`document-view-filename-text-${cell.row.original?.id}`}
          >
            {cell.value}
          </span>
        )}
      </StyledCellOverflow>
    );
  };
};

const renderParentLinks = () => {
  return function ({
    row: { original },
  }: CellProps<ApiGen_Concepts_DocumentSearchResult, ApiGen_Concepts_DocumentRelationship[]>) {
    const { hasClaim } = useKeycloakWrapper();

    const parentLinks = original.documentRelationships
      .filter(x => x.relationshipType !== ApiGen_CodeTypes_DocumentRelationType.Templates)
      .map(rel => {
        const fileType: string = relationshipTypeToPathName(rel.relationshipType).toString();
        const fileId: string = rel.parentId.toString();
        const pathToParent = generatePath(`/mapview/sidebar/:fileType/:fileId`, {
          fileType,
          fileId,
        });

        let hasAccesToParent = false;
        switch (rel.relationshipType) {
          case ApiGen_CodeTypes_DocumentRelationType.AcquisitionFiles:
            hasAccesToParent = hasClaim(Claims.ACQUISITION_VIEW);
            break;
          case ApiGen_CodeTypes_DocumentRelationType.Templates:
            hasAccesToParent = false;
            break;
          case ApiGen_CodeTypes_DocumentRelationType.ResearchFiles:
            hasAccesToParent = hasClaim(Claims.RESEARCH_VIEW);
            break;
          case ApiGen_CodeTypes_DocumentRelationType.Leases:
            hasAccesToParent = hasClaim(Claims.LEASE_VIEW);
            break;
          case ApiGen_CodeTypes_DocumentRelationType.Projects:
            hasAccesToParent = hasClaim(Claims.PROJECT_VIEW);
            break;
          case ApiGen_CodeTypes_DocumentRelationType.ManagementActivities:
          case ApiGen_CodeTypes_DocumentRelationType.ManagementFiles:
            hasAccesToParent = hasClaim(Claims.MANAGEMENT_VIEW);
            break;
          case ApiGen_CodeTypes_DocumentRelationType.DispositionFiles:
            hasAccesToParent = hasClaim(Claims.DISPOSITION_VIEW);
            break;
          case ApiGen_CodeTypes_DocumentRelationType.Properties:
            hasAccesToParent = hasClaim(Claims.PROPERTY_VIEW);
            break;
          default:
            hasAccesToParent = false;
            break;
        }

        if (hasAccesToParent) {
          return (
            <ExternalLink to={pathToParent} key={`${fileType}-${fileId}`}>
              {rel.parentNameOrNumber}
            </ExternalLink>
          );
        }

        return stringToFragment(rel.parentNameOrNumber);
      });

    return <div className="w-100">{parentLinks}</div>;
  };
};

function renderPropertyIdentifiers({
  value,
}: CellProps<
  ApiGen_Concepts_DocumentSearchResult,
  ApiGen_Concepts_PropertyDocumentRelationship[] | undefined
>) {
  const propertyIdentifiers = value.map(rel => {
    const propertyName = getApiPropertyName(rel.property);

    return (
      <OverflowTip
        fullText={`${propertyName.label}: ${propertyName.value}`}
        key={`${rel.relationshipType}-${rel.parentId}`}
        valueTestId={`property-id-${rel.parentId}`}
      />
    );
  });

  return <div className="w-100">{propertyIdentifiers}</div>;
}

const renderActions = (onViewDetails: (values: ApiGen_Concepts_DocumentRelationship) => void) => {
  return function ({
    row: { original, index },
  }: CellProps<ApiGen_Concepts_DocumentSearchResult, string>) {
    const { hasClaim } = useKeycloakWrapper();

    const documentInError = documentQueueInError(original.documentQueueStatusTypeCode?.id);
    const documentProcessing = documentQueueInProcess(original.documentQueueStatusTypeCode?.id);

    const canViewDocument =
      (original.mayanDocumentId && original.documentQueueStatusTypeCode === null) ||
      (original.documentQueueStatusTypeCode?.id ===
        ApiGen_CodeTypes_DocumentQueueStatusTypes.SUCCESS &&
        original.mayanDocumentId);

    if (documentProcessing) {
      return (
        <DocumentsActionsDiv>
          <FaClock
            id={`document-processing-${index}`}
            size={21}
            title="Upload in progress..."
            className="warning"
          />
        </DocumentsActionsDiv>
      );
    }

    if (documentInError) {
      return (
        <DocumentsActionsDiv>
          <FaTimesCircle
            id={`document-error-${index}`}
            size={21}
            title={original.documentQueueStatusTypeCode.description}
          />
        </DocumentsActionsDiv>
      );
    }

    return (
      <DocumentsActionsDiv>
        {hasClaim(Claims.DOCUMENT_VIEW) && canViewDocument && (
          <ViewButton
            id={`document-view-${index}`}
            data-testId="document-view-button"
            icon={<FaEye size={21} title="document view details" />}
            onClick={() => onViewDetails(original.documentRelationships[0])}
          ></ViewButton>
        )}
      </DocumentsActionsDiv>
    );
  };
};

const DocumentsActionsDiv = styled(InlineFlexDiv)`
  justify-content: center;
  gap: 1rem;
  align-items: center;
  flex-grow: 1;
  align-content: space-between;

  [id^='document-processing'] {
    color: ${props => props.theme.bcTokens.themeGold100};
  }
  [id^='document-error'] {
    color: ${props => props.theme.bcTokens.typographyColorDanger};
  }
  [id^='document-view'] {
    color: ${props => props.theme.css.activeActionColor};
  }

  .btn.btn-primary {
    background-color: transparent;
    padding: 0;
  }
`;

const StyledCellOverflow = styled('div')`
  display: contents;
  text-overflow: ellipsis;
  overflow: hidden;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  line-clamp: 2;
  -webkit-box-orient: vertical;

  button {
    display: contents !important;
  }
`;
