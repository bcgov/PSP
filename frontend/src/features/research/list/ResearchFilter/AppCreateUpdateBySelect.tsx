import { InputGroup, Select } from 'components/common/form';
import { useFormikContext } from 'formik';
import * as React from 'react';
import styled from 'styled-components';

interface IAppCreateUpdateBySelectProps {
  disabled?: boolean;
  options?: { label: string; value: string }[];
  placeholders?: Record<string, string>;
}

interface IAppCreatedUpdatedBy {
  createOrUpdateBy: string;
  createdByIdir: string;
  updatedByIdir: string;
}

const createUpdateUserOptions = [
  {
    label: 'Created by',
    value: 'createdByIdir',
  },
  {
    label: 'Updated by',
    value: 'updatedByIdir',
  },
];

/**
 * Provides a dropdown with list of search options for the user who created/updated the row.
 */
export const AppCreateUpdateBySelect: React.FC<IAppCreateUpdateBySelectProps &
  React.HTMLAttributes<HTMLElement>> = ({ disabled, options, placeholders, ...rest }) => {
  const state: {
    options: { label: string; value: string }[];
    placeholders: Record<string, string>;
  } = {
    options: options ?? createUpdateUserOptions,
    placeholders: placeholders ?? {
      updatedByIdir: `User's IDIR`,
      createdByIdir: `User's IDIR`,
    },
  };

  // access the form context values, no need to pass props
  const formikProps = useFormikContext<IAppCreatedUpdatedBy>();
  const {
    values: { createOrUpdateBy },
    setFieldValue,
  } = formikProps;
  const desc = state.placeholders[createOrUpdateBy] || '';

  const reset = () => {
    setFieldValue(createOrUpdateBy ? createOrUpdateBy : 'updatedByIdir', '');
  };

  return (
    <SmallInputGroup
      formikProps={formikProps}
      prepend={
        <Select
          field="createOrUpdateBy"
          options={state.options}
          onChange={reset}
          disabled={disabled}
        />
      }
      field={createOrUpdateBy}
      placeholder={desc}
      disabled={disabled}
      {...rest}
    />
  );
};

const SmallInputGroup = styled(InputGroup)`
  .input-group-prepend select {
    width: 15rem;
  }
  input {
    width: 18rem;
    max-width: 100%;
  }
`;
export default AppCreateUpdateBySelect;
