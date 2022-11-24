import { StyledRemoveLinkButton } from 'components/common/buttons';
import { Button } from 'components/common/buttons/Button';
import TooltipIcon from 'components/common/TooltipIcon';
import { ColumnWithProps, renderTypeCode } from 'components/Table';
import { Claims } from 'constants/index';
import DownloadDocumentButton from 'features/documents/DownloadDocumentButton';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_Document, Api_DocumentType } from 'models/api/Document';
import { Col, Row } from 'react-bootstrap';
import { FaEye, FaTrash, FaUserAlt } from 'react-icons/fa';
import { CellProps } from 'react-table';
import styled from 'styled-components';
import { prettyFormatDate } from 'utils';

export interface IDocumentColumnProps {
  onViewDetails: (values: Api_Document) => void;
  onDelete: (values: Api_Document) => void;
}

export const getDocumentColumns = ({
  onViewDetails,
  onDelete,
}: IDocumentColumnProps): ColumnWithProps<Api_Document>[] => {
  return [
    {
      Header: 'Document type',
      accessor: 'documentType',
      align: 'left',
      sortable: true,
      Cell: renderDocumentType,
    },
    {
      Header: 'File name',
      accessor: 'fileName',
      sortable: true,
      Cell: renderFileName(onViewDetails),
    },
    {
      Header: 'Uploaded',
      accessor: 'appCreateTimestamp',
      sortable: true,
      Cell: renderUploaded,
    },
    {
      Header: 'Status',
      accessor: 'statusTypeCode',
      sortable: true,
      Cell: renderTypeCode,
    },
    {
      Header: 'Actions',
      width: '90',
      Cell: renderActions(onViewDetails, onDelete),
    },
  ];
};

function renderDocumentType({ value }: CellProps<Api_Document, Api_DocumentType>) {
  return value?.documentType ?? '';
}

const renderFileName = (onViewDetails: (values: Api_Document) => void) => {
  return function (cell: CellProps<Api_Document, string>) {
    const { hasClaim } = useKeycloakWrapper();
    return (
      <>
        {hasClaim(Claims.DOCUMENT_VIEW) === true ? (
          <Button
            data-testid="document-view-filename-link"
            onClick={() => cell.row.original?.id && onViewDetails(cell.row.original)}
            variant="link"
          >
            {cell.value}
          </Button>
        ) : (
          <span data-testid="document-view-filename-text">{cell.value}</span>
        )}
      </>
    );
  };
};

//export const DateCell = ({ cell: { value } }: CellProps<Date, string>) => prettyFormatDate(value);

function renderUploaded(cell: CellProps<Api_Document, string>) {
  return (
    <Row className="no-gutters">
      <Col>{prettyFormatDate(cell.row.original.appCreateTimestamp)}</Col>
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
  onViewDetails: (values: Api_Document) => void,
  onDelete: (values: Api_Document) => void,
) => {
  return function ({ row: { original, index } }: CellProps<Api_Document, string>) {
    const { hasClaim } = useKeycloakWrapper();
    return (
      <StyledIconsRow className="no-gutters">
        {hasClaim(Claims.DOCUMENT_VIEW) && original?.mayanDocumentId !== undefined && (
          <Col>
            <DownloadDocumentButton mayanDocumentId={original?.mayanDocumentId} />
          </Col>
        )}
        {hasClaim(Claims.DOCUMENT_VIEW) && (
          <Col>
            <Button
              data-testid="document-view-button"
              icon={<FaEye size={24} id={`document-view-${index}`} title="document view details" />}
              onClick={() => original?.id && onViewDetails(original)}
            ></Button>
          </Col>
        )}
        {hasClaim(Claims.DOCUMENT_DELETE) && (
          <Col>
            <StyledRemoveLinkButton
              title="document delete"
              data-testid="document-delete-button"
              icon={<FaTrash size={24} id={`document-delete-${index}`} title="document delete" />}
              onClick={() => original?.id && onDelete(original)}
            ></StyledRemoveLinkButton>
          </Col>
        )}
      </StyledIconsRow>
    );
  };
};

const StyledIconsRow = styled(Row)`
  [id^='document-view'] {
    color: ${props => props.theme.css.slideOutBlue};
  }
  [id^='document-delete'] {
    color: ${props => props.theme.css.discardedColor};
    :hover {
      color: ${({ theme }) => theme.css.dangerColor};
    }
  }
  .btn.btn-primary {
    background-color: transparent;
    padding: 0;
    margin-left: 0.5rem;
  }
`;

const StyledIcon = styled.span`
  .tooltip-icon {
    color: ${({ theme }) => theme.css.subtleColor};
  }
`;
