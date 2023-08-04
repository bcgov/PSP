import React from 'react';

import { AsyncTypeahead } from '@/components/common/form';
import { useProjectTypeahead } from '@/hooks/useProjectTypeahead';
import { IAutocompletePrediction } from '@/interfaces';

export interface IProjectSelectorProps {
  /** The formik field name */
  field: string;
  /* Called whenever the component selection changes. Receives an array of the selected options. */
  onChange?: (selected: IAutocompletePrediction[]) => void;
}

export const ProjectSelector: React.FC<IProjectSelectorProps> = props => {
  const { handleTypeaheadSearch, isTypeaheadLoading, matchedProjects } = useProjectTypeahead();

  return (
    <AsyncTypeahead
      placeholder="Type to search for a Project"
      field={props.field}
      labelKey="text"
      multiple={false}
      isLoading={isTypeaheadLoading}
      options={matchedProjects}
      onSearch={handleTypeaheadSearch}
      onChange={props.onChange}
    />
  );
};
