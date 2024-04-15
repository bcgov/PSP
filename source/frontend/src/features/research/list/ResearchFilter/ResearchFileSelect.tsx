import { useFormikContext } from 'formik';

import { InputGroup, Select } from '@/components/common/form';

interface IResearchFileSelectProps {
  disabled?: boolean;
  options?: { label: string; value: string }[];
  placeholders?: Record<string, string>;
}

interface IResearchFileSelect {
  researchSearchBy: string;
  appCreateUserid: string;
  appLastUpdateUserid: string;
}

const primaryResearchFilterOptions = [
  {
    label: 'Research file name',
    value: 'name',
  },
  {
    label: 'Research file #',
    value: 'rfileNumber',
  },
];

/**
 * Provides a dropdown with list of search options for the user who created/updated the row.
 */
export const ResearchFileSelect: React.FC<
  React.PropsWithChildren<IResearchFileSelectProps & React.HTMLAttributes<HTMLElement>>
> = ({ disabled, options, placeholders, ...rest }) => {
  const state: {
    options: { label: string; value: string }[];
    placeholders: Record<string, string>;
  } = {
    options: options ?? primaryResearchFilterOptions,
    placeholders: placeholders ?? {
      rfileNumber: '',
      name: '',
    },
  };

  // access the form context values, no need to pass props
  const formikProps = useFormikContext<IResearchFileSelect>();
  const {
    values: { researchSearchBy },
    setFieldValue,
  } = formikProps;
  const desc = state.placeholders[researchSearchBy] || '';

  const reset = () => {
    setFieldValue(researchSearchBy ? researchSearchBy : 'name', '');
  };

  return (
    <InputGroup
      prepend={
        <Select
          field="researchSearchBy"
          options={state.options}
          onChange={reset}
          disabled={disabled}
        />
      }
      field={researchSearchBy}
      placeholder={desc}
      disabled={disabled}
      {...rest}
    />
  );
};
export default ResearchFileSelect;
