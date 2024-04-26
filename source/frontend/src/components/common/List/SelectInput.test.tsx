import { render } from '@testing-library/react';
import { FormikContextType, useFormikContext } from 'formik';

import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';

import { SelectInput } from './SelectInput';

vi.mock('formik');

vi.mocked(useFormikContext).mockReturnValue({
  values: {},
  setFieldValue: vi.fn(),
} as unknown as FormikContextType<any>);
describe('SelectInput tests', () => {
  it('renders correctly...', () => {
    const { asFragment } = render(
      <SelectInput<
        {
          pinOrPid: string;
          address: string;
        },
        IPropertyFilter
      >
        field="searchBy"
        defaultKey="pinOrPid"
        selectOptions={[
          { label: 'PID/PIN', key: 'pinOrPid', placeholder: 'Enter a PID or PIN' },
          { label: 'Address', key: 'address', placeholder: 'Enter an address' },
        ]}
        className="idir-input-group"
      />,
    );
    expect(asFragment()).toMatchSnapshot();
  });

  it('disabled prop works as intended...', () => {
    const { container } = render(
      <SelectInput<
        {
          pinOrPid: string;
          address: string;
        },
        IPropertyFilter
      >
        field="searchBy"
        defaultKey="pinOrPid"
        selectOptions={[
          { label: 'PID/PIN', key: 'pinOrPid', placeholder: 'Enter a PID or PIN' },
          { label: 'Address', key: 'address', placeholder: 'Enter an address' },
        ]}
        className="idir-input-group"
        disabled
      />,
    );
    const searchBy = container.querySelector('select[name="searchBy"]');
    expect(searchBy).toHaveAttribute('disabled');
  });
});
