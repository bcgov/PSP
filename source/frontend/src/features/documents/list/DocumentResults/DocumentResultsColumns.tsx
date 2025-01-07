import { Col, Row } from 'react-bootstrap';
import { FaClock, FaEye, FaTimesCircle, FaTrash, FaUserAlt } from 'react-icons/fa';
import { CellProps } from 'react-table';
import styled from 'styled-components';

import { StyledRemoveLinkButton } from '@/components/common/buttons';
import { Button } from '@/components/common/buttons/Button';
import ViewButton from '@/components/common/buttons/ViewButton';
import { InlineFlexDiv } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { ColumnWithProps, renderGenTypeCode } from '@/components/Table';
import { Claims } from '@/constants/index';
import { DocumentRow } from '@/features/documents/ComposedDocument';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_DocumentQueueStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_DocumentQueueStatusTypes';
import { ApiGen_Concepts_DocumentRelationship } from '@/models/api/generated/ApiGen_Concepts_DocumentRelationship';
import { ApiGen_Concepts_DocumentType } from '@/models/api/generated/ApiGen_Concepts_DocumentType';
import { prettyFormatUTCDate, stringToFragment } from '@/utils';

export interface IDocumentColumnProps {
  onViewDetails: (values: ApiGen_Concepts_DocumentRelationship) => void;
  onDelete: (values: ApiGen_Concepts_DocumentRelationship) => void;
  onPreview: (values: ApiGen_Concepts_DocumentRelationship) => void;
}

export const getDocumentColumns = ({
  onViewDetails,
  onDelete,
  onPreview,
}: IDocumentColumnProps): ColumnWithProps<DocumentRow>[] => {
  return [
    {
      Header: 'Document type',
      accessor: 'documentType',
      align: 'left',
      sortable: true,
      width: 30,
      maxWidth: 30,
      Cell: renderDocumentType,
    },
    {
      Header: 'File name',
      accessor: 'fileName',
      width: 40,
      maxWidth: 40,
      sortable: true,
      Cell: renderFileName(onPreview),
    },
    {
      Header: 'Uploaded',
      accessor: 'appCreateTimestamp',
      sortable: true,
      width: 20,
      maxWidth: 20,
      Cell: renderUploaded,
    },
    {
      Header: 'Status',
      accessor: 'statusTypeCode',
      sortable: true,
      width: 20,
      maxWidth: 20,
      Cell: renderGenTypeCode,
    },
    {
      Header: 'Actions',
      width: 10,
      maxWidth: 10,
      Cell: renderActions(onViewDetails, onDelete),
    },
  ];
};

function renderDocumentType({
  value,
}: CellProps<DocumentRow, ApiGen_Concepts_DocumentType | undefined>) {
  return stringToFragment(value?.documentTypeDescription ?? '');
}

const renderFileName = (onViewDetails: (values: ApiGen_Concepts_DocumentRelationship) => void) => {
  return function (cell: CellProps<DocumentRow, string | undefined>) {
    const { hasClaim } = useKeycloakWrapper();
    const documentProcessed =
      (cell.row.original.mayanDocumentId &&
        cell.row.original.queueStatusTypeCode?.id ===
          ApiGen_CodeTypes_DocumentQueueStatusTypes.SUCCESS) ||
      (cell.row.original.mayanDocumentId && cell.row.original.queueStatusTypeCode === null);

    return (
      <StyledCellOverflow>
        {hasClaim(Claims.DOCUMENT_VIEW) === true && documentProcessed ? (
          <Button
            id={`document-view-filename-link-${cell.row.original?.id}`}
            data-testid={`document-view-filename-link-${cell.row.original?.id}`}
            onClick={() =>
              cell.row.original?.id && onViewDetails(DocumentRow.toApi(cell.row.original))
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

function renderUploaded(cell: CellProps<DocumentRow, string | undefined>) {
  return (
    <Row className="no-gutters">
      <Col>{prettyFormatUTCDate(cell.row.original.appCreateTimestamp)}</Col>
      <Col xs="auto">
        <StyledIcon>
          <TooltipIcon
            toolTipId="initiator-tooltip"
            toolTip={cell.row.original.appCreateUserid}
            customToolTipIcon={<FaUserAlt size={15} />}
          />
        </StyledIcon>
      </Col>
    </Row>
  );
}

const renderActions = (
  onViewDetails: (values: ApiGen_Concepts_DocumentRelationship) => void,
  onDelete: (values: ApiGen_Concepts_DocumentRelationship) => void,
) => {
  return function ({ row: { original, index } }: CellProps<DocumentRow, string>) {
    const { hasClaim } = useKeycloakWrapper();

    const documentInError =
      original.queueStatusTypeCode?.id === ApiGen_CodeTypes_DocumentQueueStatusTypes.PIMS_ERROR ||
      original.queueStatusTypeCode?.id === ApiGen_CodeTypes_DocumentQueueStatusTypes.MAYAN_ERROR;

    const documentProcessing =
      original.queueStatusTypeCode?.id === ApiGen_CodeTypes_DocumentQueueStatusTypes.PENDING ||
      original.queueStatusTypeCode?.id === ApiGen_CodeTypes_DocumentQueueStatusTypes.PROCESSING;

    const canViewDocument =
      (original.mayanDocumentId && original.queueStatusTypeCode === null) ||
      (original.queueStatusTypeCode?.id === ApiGen_CodeTypes_DocumentQueueStatusTypes.SUCCESS &&
        original.mayanDocumentId);

    const canDeleteDocument = documentInError || !documentProcessing;

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
            title={original.queueStatusTypeCode.description}
          />

          {hasClaim(Claims.DOCUMENT_DELETE) && (
            <StyledRemoveLinkButton
              data-testid="document-delete-button"
              icon={<FaTrash id={`document-delete-${index}`} size={21} title="document delete" />}
              onClick={() => original?.id && onDelete(DocumentRow.toApi(original))}
            ></StyledRemoveLinkButton>
          )}
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
            onClick={() => original?.id && onViewDetails(DocumentRow.toApi(original))}
          ></ViewButton>
        )}

        {hasClaim(Claims.DOCUMENT_DELETE) && canDeleteDocument && (
          <StyledRemoveLinkButton
            data-testid="document-delete-button"
            icon={<FaTrash size={21} id={`document-delete-${index}`} title="document delete" />}
            onClick={() => original?.id && onDelete(DocumentRow.toApi(original))}
          ></StyledRemoveLinkButton>
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

const StyledIcon = styled.span`
  .tooltip-icon {
    color: ${({ theme }) => theme.bcTokens.iconsColorDisabled};
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
