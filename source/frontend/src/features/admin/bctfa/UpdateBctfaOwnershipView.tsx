import { Formik } from 'formik';
import truncate from 'lodash/truncate';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaCheck } from 'react-icons/fa';
import styled, { useTheme } from 'styled-components';

import { Button } from '@/components/common/buttons';
import { DisplayError } from '@/components/common/form';
import FileDragAndDrop from '@/components/common/form/FileDragAndDrop';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { exists } from '@/utils';

export interface IUpdateBcftaOwnershipViewProps {
  onSubmit: (file: File) => Promise<void>;
  isLoading?: boolean;
}

export interface IBctfaOwnershipUploadForm {
  selectedFile: File;
}

export const UpdateBcftaOwnershipView: React.FunctionComponent<IUpdateBcftaOwnershipViewProps> = ({
  onSubmit,
  isLoading,
}) => {
  const theme = useTheme();
  return (
    <Formik<IBctfaOwnershipUploadForm>
      initialValues={{ selectedFile: null }}
      onSubmit={async (values: IBctfaOwnershipUploadForm) => await onSubmit(values.selectedFile)}
    >
      {formikProps => (
        <Section header="Update BCTFA Ownership">
          <LoadingBackdrop show={isLoading} />
          <StyledBlueSection>
            Upload a csv file, that contains the list of all PIDs currently owned by BCTFA, as
            provided by LTSA. Uploading this file here will update the BCTFA ownership layer within
            PIMS to reflect the PIDS listed in the uploaded file.
          </StyledBlueSection>
          <FileDragAndDrop
            onSelectFiles={files => {
              if (files.length === 1) {
                formikProps.setFieldValue('selectedFile', files[0]);
              }
            }}
            validExtensions={['csv']}
            multiple={false}
            keyName={formikProps.values?.selectedFile?.name}
          />
          {exists(formikProps.values?.selectedFile) ? (
            <StyledSectionHeader>
              <Row className="justify-content-center">
                <Col xs={6} className="m-4">
                  <SectionField label="File" labelWidth={{ xs: 12 }}>
                    {truncate(formikProps.values?.selectedFile?.name, { length: 100 })}
                    <FaCheck
                      data-testid="file-check-icon"
                      className="ml-2"
                      size="1.6rem"
                      color={theme.css.uploadFileCheckColor}
                    />
                  </SectionField>
                </Col>
              </Row>

              <DisplayError field="selectedFile" />
              <Row className="justify-content-center">
                <Col xs="auto" className="pr-6">
                  <Button variant="secondary" onClick={() => formikProps.resetForm()}>
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
              </Row>
            </StyledSectionHeader>
          ) : null}
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

export const StyledSectionHeader = styled.h2<{ isStyledHeader?: boolean }>`
  font-size: 1.6rem;
  color: ${props => props.theme.css.headerTextColor};
  margin-bottom: 2.4rem;
`;

export default UpdateBcftaOwnershipView;
