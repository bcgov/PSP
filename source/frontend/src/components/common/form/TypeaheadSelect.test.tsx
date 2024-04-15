import { Formik, FormikProps } from 'formik';

import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { SelectOption } from './Select';
import { ITypeaheadSelectProps, TypeaheadSelect } from './TypeaheadSelect';
import { createRef } from 'react';

const countries: SelectOption[] = [
  { label: 'Austria', value: 'AT' },
  { label: 'United States', value: 'US' },
  { label: 'Ireland', value: 'IE' },
];

const onChange = vi.fn();
const onBlur = vi.fn();

describe('TypeaheadSelect component', () => {
  const setup = (
    options: RenderOptions & { props?: Partial<ITypeaheadSelectProps> } & {
      initialValues?: { country: SelectOption };
    } = {},
  ) => {
    const formikRef = createRef<FormikProps<any>>();

    const utils = render(
      <Formik
        innerRef={formikRef}
        initialValues={options?.initialValues ?? { country: undefined }}
        onSubmit={vi.fn()}
      >
        <TypeaheadSelect
          field="country"
          options={countries}
          placeholder={options?.props?.placeholder ?? 'Select a country'}
          {...(options?.props ?? {})}
          onChange={onChange}
          onBlur={onBlur}
        />
      </Formik>,
      { ...options },
    );

    return {
      ...utils,
      getFormikRef: () => formikRef,
      findInput: async () => screen.findByRole<HTMLInputElement>('combobox'),
      findMenu: async () => screen.findByRole('listbox'),
      findItems: async () => screen.findAllByRole('option'),
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays the placeholder text', async () => {
    const { findInput } = setup({ props: { placeholder: 'test placeholder' } });
    const input = await findInput();
    expect(input.placeholder).toBe('test placeholder');
  });

  it('shows a tooltip as intended', () => {
    setup({ props: { tooltip: 'test tooltip' } });
    expect(document.querySelector('svg[class="tooltip-icon"]')).toBeInTheDocument();
  });

  it('should disable the input if the component is disabled', async () => {
    const { findInput } = setup({ props: { disabled: true } });
    const input = await findInput();
    expect(input).toBeDisabled();
  });

  it('shows the menu when the input is focused', async () => {
    const { findInput, findMenu } = setup();
    const input = await findInput();
    await act(async () => input.focus());
    expect(await findMenu()).toBeInTheDocument();
  });

  it('displays the correct number of options', async () => {
    const { findInput } = setup();
    const input = await findInput();
    await act(async () => input.focus());
    expect(screen.getAllByRole('option').length).toBe(countries.length);
  });

  it(`changes the selected value and calls 'onChange' when a menu item is clicked`, async () => {
    const { findInput, findItems, getFormikRef } = setup();

    const input = await findInput();
    await act(async () => input.focus());
    const items = await findItems();
    items[0].style.pointerEvents = 'auto';
    await act(async () => userEvent.click(items[0]));

    expect(onChange).toHaveBeenCalledTimes(1);
    expect(getFormikRef().current?.values?.country).toBe(countries[0]);
  });

  it(`changes the selected value and calls 'onChange' when a menu item is selected via keyboard`, async () => {
    const { findInput, getFormikRef } = setup();

    const input = await findInput();
    await act(async () => input.focus());
    await act(async () => {
      userEvent.keyboard('{arrowdown}');
    });
    await act(async () => {
      userEvent.keyboard('{Enter}');
    });

    expect(onChange).toHaveBeenCalledTimes(1);
    expect(getFormikRef().current?.values?.country).toBe(countries[0]);
  });

  it(`calls 'onBlur' when input looses focus`, async () => {
    const { findInput } = setup();
    const input = await findInput();
    await act(async () => input.focus());
    await act(async () => input.blur());
    expect(onBlur).toHaveBeenCalledTimes(1);
  });
});
