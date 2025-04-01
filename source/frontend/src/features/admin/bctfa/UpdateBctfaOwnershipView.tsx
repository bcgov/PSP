import clsx from 'classnames';
import { Formik } from 'formik';
import truncate from 'lodash/truncate';
import * as React from 'react';
import { Button, Col, Row } from 'react-bootstrap';
import { FaCheck } from 'react-icons/fa';
import styled, { useTheme } from 'styled-components';

import { DisplayError } from '@/components/common/form';
import FileDragAndDrop from '@/components/common/form/FileDragAndDrop';
import { Section } from '@/components/common/Section/Section';

export interface IUpdateBcftaOwnershipViewProps {
  onSubmit: (file: File) => Promise<void>;
}

export interface IBctfaOwnershipUploadForm {
  selectedFile: File;
}

export const UpdateBcftaOwnershipView: React.FunctionComponent<IUpdateBcftaOwnershipViewProps> = ({
  onSubmit,
}) => {
  const theme = useTheme();
  return (
    <Formik<IBctfaOwnershipUploadForm>
      initialValues={{ selectedFile: null }}
      onSubmit={async (values: IBctfaOwnershipUploadForm) => await onSubmit(values.selectedFile)}
    >
      {formikProps => (
        <Section header="Update BCTFA Ownership">
          <StyledBlueSection>
            Upload a file (csv, xlsx), that contains the list of all PIDs currently owned by BCTFA,
            as provided by LTSA. Uploading this file here will update the BCTFA ownership layer
            within PIMS to reflect the PIDS listed in the uploaded file.
          </StyledBlueSection>
          <FileDragAndDrop
            onSelectFiles={files => {
              if (files.length === 1) {
                formikProps.setFieldValue('selectedFile', files[0]);
              }
            }}
            validExtensions={['xlsx', 'csv']}
            multiple={false}
          />
          <Row className={clsx('no-gutters', 'pb-3')}>
            <Col>
              <span className="ml-4">
                {truncate(formikProps.values?.selectedFile?.name, { length: 50 })}
              </span>
              <FaCheck className="ml-2" size="1.6rem" color={theme.css.uploadFileCheckColor} />
            </Col>
          </Row>
          <DisplayError field="selectedFile" />
          <>
            <Col xs="auto" className="pr-6">
              <Button
                variant="secondary"
                onClick={() => formikProps.setFieldValue('selectedFile', null)}
              >
                Cancel
              </Button>
            </Col>
            <Col xs="auto">
              <Button
                disabled={formikProps.isSubmitting}
                onClick={() => formikProps.submitForm()}
                className="mr-9"
              >
                Save
              </Button>
            </Col>
          </>
        </Section>
      )}
    </Formik>
  );
};

const StyledBlueSection = styled.div`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
  padding: 1rem;
`;

export default UpdateBcftaOwnershipView;
