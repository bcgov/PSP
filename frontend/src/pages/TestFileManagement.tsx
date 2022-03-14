import { useAxiosApi } from 'hooks/pims-api';
import { DocumentDetail, DocumentQueryResult, FileDownload } from 'models/api/DocumentManagement';
import { ExternalResult } from 'models/api/ExternalResult';
import { useEffect, useState } from 'react';
import { Button, Col, Container, Row } from 'react-bootstrap';
import { FaDownload, FaUpload } from 'react-icons/fa';
import { formatApiDateTime } from 'utils';

export const TestFileManagement: React.FunctionComponent = () => {
  const api = useAxiosApi();

  const [documentList, setDocumentList] = useState<
    DocumentQueryResult<DocumentDetail> | undefined
  >();

  const [selectedFile, setSelectedFile] = useState<File | null>(null);

  async function retrieveDocumentList() {
    var { data } = await api.get<ExternalResult<DocumentQueryResult<DocumentDetail>>>(`/documents`);
    setDocumentList(data.payload);
  }

  async function DownloadFile(documentId: number, fileId: number) {
    var { data } = await api.get<ExternalResult<FileDownload>>(
      `/documents/${documentId}/files/${fileId}/download`,
    );
    const aElement = document.createElement('a');
    aElement.href = `data:${data.payload.mimetype};base64,` + data.payload.filePayload;
    aElement.download = data.payload.fileName;
    aElement.click();
    //document.body.removeChild(a);
    window.URL.revokeObjectURL(aElement.href);
  }

  useEffect(() => {
    retrieveDocumentList();
  }, []);

  const handleFileInput = (e: any) => {
    console.log(e);
    // handle validations
    if (e.target !== null) {
      var target = e.target as HTMLInputElement;
      if (target.files !== null) {
        console.log(target.files[0]);
        setSelectedFile(target.files[0]);
      }
    }
  };

  const submitForm = () => {
    if (selectedFile !== null) {
      const formData = new FormData();
      formData.append('name', selectedFile.name);
      formData.append('file', selectedFile);

      api
        .post(`/documents/`, formData)
        .then(res => {
          console.log(res);
          alert('File Upload success');
          retrieveDocumentList();
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
      <Row>
        <Col>
          <h2>Upload</h2>
          <form>
            <input id="uploadInput" type="file" name="myFiles" onChange={handleFileInput} />
            <Button onClick={submitForm}>
              <FaUpload></FaUpload>
            </Button>
          </form>
        </Col>
      </Row>
      <Row>
        <Col>
          <h2>File List</h2>
          <Row>
            <Col>
              {documentList?.results.map((documentItem: DocumentDetail, index: number) => (
                <Row className="border" key={'document-' + index}>
                  <Col>{documentItem.label}</Col>
                  <Col>{formatApiDateTime(documentItem.datetime_created)}</Col>
                  <Col>{documentItem.label}</Col>
                  <Col>
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
