import { FormikContextType, useFormikContext } from 'formik';

import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';
import { render } from '@/utils/test-utils';

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
          pid: string;
          pin: string;
          address: string;
        },
        IPropertyFilter
      >
        field="searchBy"
        defaultKey="pid"
        selectOptions={[
          { label: 'PID', key: 'pid', placeholder: 'Enter a PID' },
          { label: 'PIN', key: 'pin', placeholder: 'Enter a PIN' },
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
          pid: string;
          pin: string;
          address: string;
        },
        IPropertyFilter
      >
        field="searchBy"
        defaultKey="pid"
        selectOptions={[
          { label: 'PID', key: 'pid', placeholder: 'Enter a PID' },
          { label: 'PIN', key: 'pin', placeholder: 'Enter a PIN' },
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
