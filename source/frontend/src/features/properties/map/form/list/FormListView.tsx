import * as React from 'react';
import { useHistory, useRouteMatch } from 'react-router';

import { Section } from '@/components/common/Section/Section';
import { TableSort } from '@/components/Table/TableSort';
import * as API from '@/constants/API';
import { Claims } from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { IFormFilter } from '@/interfaces/IFormResults';
import { Api_FormDocumentFile, Api_FormDocumentType } from '@/models/api/FormDocument';

import { AddForm } from './AddForm';
import { FormFilter } from './FormFilter';
import { FormResults } from './FormResults';

export interface IFormListViewProps {
  saveForm: (formTypeId: string) => void;
  formFilter?: IFormFilter;
  setFormFilter: (filterValues: IFormFilter) => void;
  sort: TableSort<Api_FormDocumentType>;
  setSort: (value: TableSort<Api_FormDocumentFile>) => void;
  forms: Api_FormDocumentFile[];
  onDelete: (formFileId: number) => void;
}

export const FormListView: React.FunctionComponent<IFormListViewProps> = ({
  saveForm,
  formFilter,
  setFormFilter,
  sort,
  setSort,
  forms,
  onDelete,
}) => {
  const { getOptionsByType } = useLookupCodeHelpers();
  const formTypes = getOptionsByType(API.FORM_TYPES);
  const { hasClaim } = useKeycloakWrapper();

  const history = useHistory();
  const match = useRouteMatch();
  return (
    <Section>
      {hasClaim(Claims.FORM_ADD) && (
        <AddForm
          onAddForm={(formTypeCode: string) => {
            saveForm(formTypeCode);
          }}
          templateTypes={formTypes}
        ></AddForm>
      )}
      <FormFilter onSetFilter={setFormFilter} formFilter={formFilter} />
      <FormResults
        results={forms}
        loading={false}
        sort={sort}
        setSort={setSort}
        onShowForm={(form: Api_FormDocumentFile) => {
          history.push(`${match.url}/popup/form/${form?.id}`);
        }}
        onDelete={onDelete}
      />
    </Section>
  );
};

export default FormListView;
