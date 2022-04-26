import { useAxiosApi } from 'hooks/pims-api';
import {
  DocumentDetail,
  DocumentQueryResult,
  FileDownload,
  Mayan_DocumentType,
} from 'models/api/DocumentManagement';
import { ExternalResult } from 'models/api/ExternalResult';
import { useCallback, useEffect, useState } from 'react';
import { Button, Col, Container, Row } from 'react-bootstrap';
import { FaDownload, FaUpload } from 'react-icons/fa';
import { formatApiDateTime } from 'utils';

export const TestFileManagement: React.FunctionComponent = () => {
  const api = useAxiosApi();

  const [documentList, setDocumentList] = useState<
    DocumentQueryResult<DocumentDetail> | undefined
  >();

  const [documentTypes, setDocumentTypes] = useState<
    DocumentQueryResult<Mayan_DocumentType> | undefined
  >();

  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [selectedType, setSelectedType] = useState<number>(1);

  const retrieveDocumentList = useCallback(async () => {
    var { data } = await api.get<ExternalResult<DocumentQueryResult<DocumentDetail>>>(`/documents`);
    setDocumentList(data.payload);
  }, [api]);

  const retrieveDocumentTypes = useCallback(async () => {
    var { data } = await api.get<ExternalResult<DocumentQueryResult<Mayan_DocumentType>>>(
      `/documents/types`,
    );
    setDocumentTypes(data.payload);
  }, [api]);

  async function DownloadFile(documentId: number, fileId: number) {
    var { data } = await api.get<ExternalResult<FileDownload>>(
      `/documents/${documentId}/files/${fileId}/download`,
    );
    const aElement = document.createElement('a');
    aElement.href = `data:${data.payload.mimetype};base64,` + data.payload.filePayload;
    aElement.download = data.payload.fileName;
    aElement.click();
    window.URL.revokeObjectURL(aElement.href);
  }

  useEffect(() => {
    retrieveDocumentList();
    retrieveDocumentTypes();
  }, [retrieveDocumentList, retrieveDocumentTypes]);

  const handleFileInput = (e: any) => {
    // handle validations
    if (e.target !== null) {
      var target = e.target as HTMLInputElement;
      if (target.files !== null) {
        setSelectedFile(target.files[0]);
      }
    }
  };

  const handleTypeSelect = (e: any) => {
    if (e.target !== null) {
      var target = e.target as HTMLSelectElement;
      if (target.selectedOptions !== null) {
        setSelectedType(Number(target.selectedOptions[0].value));
      }
    }
  };

  const submitForm = () => {
    if (selectedFile !== null) {
      const formData = new FormData();
      formData.append('documentType', selectedType.toString());
      formData.append('file', selectedFile);

      api
        .post(`/documents/`, formData)
        .then(() => {
          alert('File Upload success');
          retrieveDocumentList();
          setSelectedFile(null);
          setSelectedType(1);
        })
        .catch(err => alert('File Upload Error'));
    }
  };

  return (
    <Container>
      <Row>
        <Col>
          <h1>File Management test page</h1>
        </Col>
      </Row>
      <Row className="py-5">
        <Col className="border">
          <h2>Upload</h2>
          <form>
            <Row>
              <Col>
                <input id="uploadInput" type="file" name="myFiles" onChange={handleFileInput} />
              </Col>
              <Col xs="6">
                <label htmlFor="fileType" className="pr-2">
                  <strong>File type:</strong>
                </label>
                <select id="type" name="fileType" onChange={handleTypeSelect}>
                  {documentTypes?.results.map((docType: Mayan_DocumentType, index: number) => (
                    <option value={docType.id} key={`doc-type-${index}`}>
                      {docType.label}
                    </option>
                  ))}
                </select>
              </Col>
              <Col xs="2">
                <Button onClick={submitForm} disabled={selectedFile === null}>
                  Upload File
                  <FaUpload className="ml-3"></FaUpload>
                </Button>
              </Col>
            </Row>
          </form>
        </Col>
      </Row>
      <Row className="py-5">
        <Col className="border">
          <h2>File List</h2>
          <Row>
            <Col>
              <Row className="border">
                <Col>
                  <strong>Label</strong>
                </Col>
                <Col>
                  <strong>Type</strong>
                </Col>
                <Col>
                  <strong>Created timestamp</strong>
                </Col>
                <Col xs="1"></Col>
              </Row>
            </Col>
          </Row>
          <Row>
            <Col>
              {documentList?.results.map((documentItem: DocumentDetail, index: number) => (
                <Row className="border" key={'document-' + index}>
                  <Col>{documentItem.label}</Col>
                  <Col>{documentItem.document_type.label}</Col>
                  <Col>{formatApiDateTime(documentItem.datetime_created)}</Col>
                  <Col xs="1">
                    <Button
                      onClick={() => {
                        DownloadFile(documentItem.id, documentItem.file_latest.id);
                      }}
                    >
                      <FaDownload></FaDownload>
                    </Button>
                  </Col>
                </Row>
              ))}
            </Col>
          </Row>
        </Col>
      </Row>
    </Container>
  );
};
