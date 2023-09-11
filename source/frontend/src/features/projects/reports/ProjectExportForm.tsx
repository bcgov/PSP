import { Form, Formik } from 'formik';
import * as React from 'react';

import { Select, SelectOption } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import { Api_Person } from '@/models/api/Person';
import { Api_Project } from '@/models/api/Project';
import { Api_ExportProjectFilter } from '@/models/api/ProjectFilter';

import { ExportProjectModel, ProjectExportTypes } from './models';
import { ProjectExportFormContent } from './ProjectExportFormContent';

export interface IProjectExportFormProps {
  onExportTypeSelected: () => void;
  onExport: (filter: Api_ExportProjectFilter) => Promise<void>;
  projects: Api_Project[];
  teamMembers: Api_Person[];
  loading: boolean;
}

const projectExportOptions = Object.keys(ProjectExportTypes).map<SelectOption>(
  (type: string | ProjectExportTypes, index) => ({
    label: Object.values(ProjectExportTypes)[index],
    value: type,
  }),
);

export const ProjectExportForm: React.FunctionComponent<IProjectExportFormProps> = ({
  onExportTypeSelected,
  onExport,
  projects,
  teamMembers,
  loading,
}) => {
  return (
    <Formik<ExportProjectModel>
      initialValues={new ExportProjectModel()}
      onSubmit={async (values: ExportProjectModel) => {
        await onExport(values.toApi());
      }}
    >
      <Form>
        <SectionField label="Export Type">
          <Select
            field="exportType"
            options={projectExportOptions}
            placeholder="Select Export Type..."
          ></Select>
        </SectionField>
        <ProjectExportFormContent
          onExportTypeSelected={onExportTypeSelected}
          projects={projects}
          teamMembers={teamMembers}
          loading={loading}
          onExport={onExport}
        />
      </Form>
    </Formik>
  );
};

export default ProjectExportForm;
