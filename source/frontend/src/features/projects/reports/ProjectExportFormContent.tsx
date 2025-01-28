import { useFormikContext } from 'formik';
import { useEffect } from 'react';
import { FaDownload } from 'react-icons/fa';

import { Button } from '@/components/common/buttons';
import { Multiselect } from '@/components/common/form';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { SectionField } from '@/components/common/Section/SectionField';
import { CodeTypeSelectOption } from '@/components/maps/leaflet/Control/AdvancedFilter/models';
import { formatApiPersonNames } from '@/utils/personUtils';

import { ExportProjectModel } from './models';
import { IProjectExportFormProps } from './ProjectExportForm';

export const ProjectExportFormContent: React.FunctionComponent<IProjectExportFormProps> = ({
  onExportTypeSelected,
  projects,
  teamMembers,
  loading,
}) => {
  const { values, resetForm } = useFormikContext<ExportProjectModel>();
  const isExportTypeSelected = values.exportType !== '';
  const projectOptions = projects.map<CodeTypeSelectOption>(x => {
    return {
      codeType: x?.id?.toString() ?? '',
      codeTypeDescription: `${x.code || ''}  ${x.description || ''}`,
    };
  });
  const teamMembersOptions = teamMembers.map<CodeTypeSelectOption>(x => {
    return {
      codeType: x.personId ? `P-${x.personId}` : `O-${x.organizationId}`,
      codeTypeDescription: x.personId ? formatApiPersonNames(x.person) : x.organization?.name ?? '',
    };
  });

  useEffect(() => {
    if (isExportTypeSelected && onExportTypeSelected) {
      onExportTypeSelected();
    } else {
      resetForm();
    }
  }, [isExportTypeSelected, onExportTypeSelected, resetForm]);

  if (loading) {
    return <LoadingBackdrop parentScreen show={loading} />;
  }
  if (isExportTypeSelected) {
    return (
      <>
        <SectionField label="Project" contentWidth="8" className="pl-7">
          <Multiselect
            field="projects"
            displayValue="codeTypeDescription"
            options={projectOptions}
            hidePlaceholder
            placeholder=""
          ></Multiselect>
        </SectionField>
        <SectionField label="Team Member" contentWidth="8" className="pl-7">
          <Multiselect
            field="acquisitionTeam"
            displayValue="codeTypeDescription"
            options={teamMembersOptions}
            placeholder=""
            hidePlaceholder
          ></Multiselect>
        </SectionField>
        <Button className="float-right" type="submit">
          <FaDownload className="text-white" /> Export
        </Button>
      </>
    );
  }
  return <></>;
};
