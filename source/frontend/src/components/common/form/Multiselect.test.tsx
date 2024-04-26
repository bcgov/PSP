import { act } from '@testing-library/react';
import { Formik } from 'formik';
import noop from 'lodash/noop';

import {
  focusOptionMultiselect,
  render,
  RenderOptions,
  userEvent,
  waitFor,
} from '@/utils/test-utils';

import { IMultiselectProps, Multiselect } from './Multiselect';

interface Option {
  id: number;
  text: string;
}
const fakeOptions: Option[] = [
  { id: 1, text: 'Foo' },
  { id: 2, text: 'Bar' },
  { id: 3, text: 'Baz' },
];

const BASIC_PROPS: IMultiselectProps<Option, Option> = {
  field: 'selectedOptions',
  options: fakeOptions,
  displayValue: 'text',
};

describe('Multiselect component', () => {
  // render component under test
  const setup = (
    props: IMultiselectProps<Option, Option> = BASIC_PROPS,
    initialValues: { selectedOptions?: Option[] } = {},
    renderOptions: RenderOptions = {},
  ) => {
    const { label = 'Test Label', ...rest } = props;
    const utils = render(
      <Formik initialValues={initialValues ?? {}} onSubmit={noop}>
        <Multiselect label={label} {...rest} />
      </Formik>,
      {
        ...renderOptions,
      },
    );

    return {
      ...utils,
      getInput: () => utils.getByLabelText(new RegExp(label, 'i')) as HTMLInputElement,
      findDropdownOption: (opt: Option) =>
        [
          ...utils.container.querySelectorAll('.multiselect-container .optionContainer li.option'),
        ].find(n => n.textContent === opt.text) as HTMLElement,
      findChip: (opt: Option) =>
        [...utils.container.querySelectorAll('.multiselect-container span.chip')].find(
          n => n.textContent === opt.text,
        ) as HTMLElement,
      findChipCloseIcon: (container: HTMLElement) =>
        container.querySelector('i svg') as HTMLElement,
    };
  };

  beforeEach(() => {});

  afterEach(() => {
    vi.resetAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays existing values if they exist', () => {
    const { getByText } = setup(BASIC_PROPS, { selectedOptions: [fakeOptions[0]] });
    // assert
    expect(getByText(fakeOptions[0].text)).toBeVisible();
    expect(getByText(fakeOptions[1].text)).not.toBeVisible();
    expect(getByText(fakeOptions[2].text)).not.toBeVisible();
  });

  it('calls onSelect callback when values are selected', async () => {
    const onSelectSpy = vi.fn();
    const optionSelected = fakeOptions[1];

    const { container, getInput, findDropdownOption } = setup({
      ...BASIC_PROPS,
      onSelect: onSelectSpy,
    });

    // click on the multi-select to show drop-down list
    await waitFor(async () => {
      await act(async () => userEvent.click(getInput()));
    });

    // select an option from the drop-down
    await focusOptionMultiselect(container, optionSelected, fakeOptions);

    // assert
    expect(onSelectSpy).toHaveBeenCalledWith([optionSelected]);
  });

  it('calls onRemove callback when clicking on X next to option', async () => {
    const onRemoveSpy = vi.fn();
    const optionToRemove = fakeOptions[1];
    const remainingOptions = fakeOptions.filter(n => n.id !== optionToRemove.id);

    const { findChip, findChipCloseIcon } = setup(
      {
        ...BASIC_PROPS,
        onRemove: onRemoveSpy,
      },
      { selectedOptions: [...fakeOptions] },
    );

    // find option to remove (multi-select chip) and click on the X icon
    const chip = findChip(optionToRemove);
    await act(async () => userEvent.click(findChipCloseIcon(chip)));

    // assert
    expect(onRemoveSpy).toHaveBeenCalledWith(remainingOptions);
  });
});
