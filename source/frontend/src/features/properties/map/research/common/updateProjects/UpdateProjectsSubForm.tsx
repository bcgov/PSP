import { FieldArray, useFormikContext } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { LinkButton, RemoveButton } from '@/components/common/buttons';
import { ProjectSelector } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';

import { ResearchFileProjectFormModel } from '../models';

export type WithProjectValues = {
  [key: string]: ResearchFileProjectFormModel[];
};

export interface IUpdateProjectsSubFormProps {
  field: string;
  fileId?: number;
}

export const UpdateProjectsSubForm: React.FC<IUpdateProjectsSubFormProps> = ({ field, fileId }) => {
  const formik = useFormikContext<WithProjectValues>();
  const projects = formik.values[field];

  return (
    <FieldArray
      name={field}
      render={arrayHelpers => (
        <>
          {projects.map((_, index) => (
            <SectionField key={`project-row-${index}`} label="Ministry project">
              <Row>
                <Col xs="auto" xl="9">
                  <ProjectSelector field={`${field}[${index}].project`} />
                </Col>
                <Col xs="auto" xl="3" className="pl-0 mt-2">
                  <RemoveButton onRemove={() => arrayHelpers.remove(index)} />
                </Col>
              </Row>
            </SectionField>
          ))}
          <LinkButton
            data-testid="add-project"
            onClick={() => {
              const newProject = new ResearchFileProjectFormModel();
              newProject.fileId = fileId;
              arrayHelpers.push(newProject);
            }}
          >
            + Add another project
          </LinkButton>
        </>
      )}
    />
  );
};
