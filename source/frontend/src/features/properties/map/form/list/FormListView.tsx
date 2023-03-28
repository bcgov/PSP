import * as API from 'constants/API';
import { Claims } from 'constants/claims';
import { Section } from 'features/mapSideBar/tabs/Section';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import * as React from 'react';

import { AddForm } from './AddForm';

export interface IFormListViewProps {
  saveForm: (formTypeId: string) => void;
}

export const FormListView: React.FunctionComponent<IFormListViewProps> = ({ saveForm }) => {
  const { getOptionsByType } = useLookupCodeHelpers();
  const formTypes = getOptionsByType(API.FORM_TYPES);
  const { hasClaim } = useKeycloakWrapper();
  return (
    <Section>
      {hasClaim(Claims.FORM_ADD) && (
        <AddForm
          onAddForm={(formTypeId: string) => {
            saveForm(formTypeId);
          }}
          templateTypes={formTypes}
        ></AddForm>
      )}
    </Section>
  );
};

export default FormListView;
