import { Formik, FormikProps } from 'formik';

import {
  act,
  fireEvent,
  render,
  RenderOptions,
  screen,
  userEvent,
  waitForEffects,
} from '@/utils/test-utils';

import { Select, SelectOption, SelectProps } from './Select';
import { createRef } from 'react';

const countries: SelectOption[] = [
  { label: 'Austria', value: 'AT' },
  { label: 'United States', value: 'US' },
  { label: 'Ireland', value: 'IE' },
];

const onSubmit = vi.fn();

describe('Select component', () => {
  const setup = (
    options: RenderOptions & { props?: Partial<SelectProps> } & {
      initialValues?: { countryId: string };
    } = {},
  ) => {
    const formikRef = createRef<FormikProps<any>>();
    const utils = render(
      <Formik
        innerRef={formikRef}
        initialValues={options?.initialValues ?? { countryId: '' }}
        onSubmit={onSubmit}
      >
        <Select
          field="countryId"
          label="Test Dropdown"
          options={countries}
          placeholder={options?.props?.placeholder ?? 'Select a country'}
          {...(options?.props ?? {})}
        />
      </Formik>,
      { ...options },
    );

    return {
      ...utils,
      getFormikRef: () => formikRef,
    };
  };

  beforeEach(() => {});

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('should correctly set default option', () => {
    setup();
    const option: HTMLOptionElement = screen.getByRole('option', { name: 'Select a country' });
    expect(option.selected).toBe(true);
  });

  it('should display the correct number of options', () => {
    setup();
    expect(screen.getAllByRole('option').length).toBe(4);
  });

  it('should not render default option if placeholder property is undefined', () => {
    setup({ props: { placeholder: undefined } });
    expect(screen.getAllByRole('option').length).toBe(3);
  });

  it('allows the user to change selected value and calls onChange callback', async () => {
    const onChange = vi.fn();
    setup({ props: { onChange } });

    await act(async () =>
      userEvent.selectOptions(
        screen.getByRole('combobox'),
        screen.getByRole('option', { name: 'Ireland' }),
      ),
    );

    const option: HTMLOptionElement = screen.getByRole('option', { name: 'Ireland' });
    expect(option.selected).toBe(true);
    expect(onChange).toHaveBeenCalled();
  });

  it('supports disabled property as intended', () => {
    setup({ props: { disabled: true } });
    const select = screen.getByRole('combobox');
    expect(select).toHaveAttribute('disabled');
  });

  it('shows a tooltip as intended', () => {
    setup({ props: { tooltip: 'test tooltip' } });
    expect(document.querySelector('svg[class="tooltip-icon"]')).toBeInTheDocument();
  });

  it('should handle numeric values', () => {
    const numericOptions = countries.map((opt, i) => {
      opt.value = i;
      return opt;
    });
    setup({ props: { type: 'number', options: numericOptions } });
    expect(screen.getAllByRole('option').length).toBe(4);
  });

  it('should set numeric value in formik', async () => {
    const numericOptions = countries.map((opt, i) => {
      opt.value = i;
      return opt;
    });
    const { getFormikRef } = setup({ props: { type: 'number', options: numericOptions } });
    const formik = getFormikRef();

    // select a value and trigger a blur event on the select
    await act(async () =>
      userEvent.selectOptions(
        screen.getByRole('combobox'),
        screen.getByRole('option', { name: 'Ireland' }),
      ),
    );
    fireEvent.blur(screen.getByRole('combobox'));
    await waitForEffects();

    expect(formik.current?.values.countryId).toBe(2);
  });

  it('should set numeric value to undefined in formik when default option is selected', async () => {
    const numericOptions = countries.map((opt, i) => {
      opt.value = i;
      return opt;
    });
    const { getFormikRef } = setup({ props: { type: 'number', options: numericOptions } });
    const formik = getFormikRef();

    // select a value and trigger a blur event on the select
    await act(async () =>
      userEvent.selectOptions(
        screen.getByRole('combobox'),
        screen.getByRole('option', { name: 'Ireland' }),
      ),
    );
    await act(async () =>
      userEvent.selectOptions(
        screen.getByRole('combobox'),
        screen.getByRole('option', { name: 'Select a country' }),
      ),
    );
    await act(async () => {
      fireEvent.blur(screen.getByRole('combobox'));
    });

    expect(formik.current?.values.countryId).toBeUndefined();
  });
});
