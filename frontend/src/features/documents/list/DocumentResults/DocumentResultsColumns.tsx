import { Button } from 'components/common/buttons/Button';
import { ColumnWithProps, DateCell, renderTypeCode } from 'components/Table';
import { Claims } from 'constants/index';
import DownloadDocumentButton from 'features/documents/DownloadDocumentButton';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_Document, Api_DocumentType } from 'models/api/Document';
import { Col, Row } from 'react-bootstrap';
import { FaEye, FaTrash } from 'react-icons/fa';
import { CellProps } from 'react-table';
import styled from 'styled-components';

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
      minWidth: 40,
      maxWidth: 40,
      Cell: renderDocumentType,
    },
    {
      Header: 'File name',
      accessor: 'fileName',
      sortable: true,
      width: 40,
      maxWidth: 40,
    },
    {
      Header: 'Upload date',
      accessor: 'appCreateTimestamp',
      sortable: true,
      width: 20,
      maxWidth: 20,
      Cell: DateCell,
    },
    {
      Header: 'Uploaded by',
      accessor: 'appCreateUserid',
      sortable: true,
      width: 20,
      maxWidth: 20,
    },
    {
      Header: 'Status',
      accessor: 'statusTypeCode',
      sortable: true,
      width: 20,
      maxWidth: 20,
      Cell: renderTypeCode,
    },
    {
      Header: 'Actions',
      minWidth: 17,
      maxWidth: 17,
      Cell: renderActions(onViewDetails, onDelete),
    },
  ];
};

function renderDocumentType({ value }: CellProps<any, Api_DocumentType>) {
  return value?.documentType ?? '';
}

const renderActions = (
  onViewDetails: (values: Api_Document) => void,
  onDelete: (values: Api_Document) => void,
) => {
  return function({ row: { original, index } }: CellProps<Api_Document, string>) {
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
              title="document view details"
              icon={<FaEye size={24} id={`document-view-${index}`} title="document view details" />}
              onClick={() => original?.id && onViewDetails(original)}
            ></Button>
          </Col>
        )}
        {hasClaim(Claims.DOCUMENT_DELETE) && (
          <Col>
            <Button
              title="document delete"
              icon={<FaTrash size={24} id={`document-delete-${index}`} title="document delete" />}
              onClick={() => original?.id && onDelete(original)}
            ></Button>
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
