import userEvent from '@testing-library/user-event';

import { act, fillInput, render, RenderOptions } from '@/utils/test-utils';

import {
  ContactFilterComponent,
  defaultFilter,
  IContactFilterComponentProps,
} from './ContactFilterComponent';

const setFilter = jest.fn();

// render component under test
const setup = (renderOptions: RenderOptions & IContactFilterComponentProps = { setFilter }) => {
  const { filter, setFilter: setFilterFn, ...rest } = renderOptions;
  const utils = render(
    <ContactFilterComponent filter={filter} setFilter={setFilterFn} showActiveSelector />,
    {
      ...rest,
    },
  );
  const searchButton = utils.getByTestId('search');
  const resetButton = utils.getByTestId('reset-button');
  return { searchButton, resetButton, setFilter: setFilterFn, ...utils };
};

describe('ContactFilterComponent', () => {
  beforeEach(() => {
    setFilter.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('searches by summary', async () => {
    const { container, searchButton } = setup();

    fillInput(container, 'summary', 'asummary');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        ...defaultFilter,
        summary: 'asummary',
      }),
    );
  });

  it('searches by city/municipality', async () => {
    const { container, searchButton } = setup();
    fillInput(container, 'municipality', 'victoria');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({ ...defaultFilter, municipality: 'victoria' }),
    );
  });

  it('searches all by default', async () => {
    const { container, searchButton } = setup();
    const allButton = container.querySelector(`#input-all`);
    act(() => allButton && userEvent.click(allButton));
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({ ...defaultFilter, searchBy: 'all' }),
    );
  });

  it('searches organizations if radio option selected', async () => {
    const { container, searchButton } = setup();
    const organizationsButton = container.querySelector(`#input-organizations`);
    act(() => organizationsButton && userEvent.click(organizationsButton));
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({ ...defaultFilter, searchBy: 'organizations' }),
    );
  });

  it('searches persons if radio option selected', async () => {
    const { container, searchButton } = setup();
    const personButton = container.querySelector(`#input-persons`);
    act(() => personButton && userEvent.click(personButton));
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({ ...defaultFilter, searchBy: 'persons' }),
    );
  });

  it('searches for active contacts by default', async () => {
    const { searchButton } = setup();
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(expect.objectContaining({ ...defaultFilter }));
  });

  it('searches for inactive contacts if checkbox unchecked', async () => {
    const { container } = setup();
    const activeCheck = container.querySelector(`#input-activeContactsOnly`);
    expect(activeCheck).not.toBeNull();
    await act(async () => userEvent.click(activeCheck as Element));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({ ...defaultFilter, activeContactsOnly: false }),
    );
  });

  it('resets the form', async () => {
    const { resetButton, container } = setup();

    const activeCheck = container.querySelector(`#input-activeContactsOnly`);
    expect(activeCheck).not.toBeNull();
    await act(async () => userEvent.click(activeCheck as Element));

    const personButton = container.querySelector(`#input-persons`);
    act(() => personButton && userEvent.click(personButton));

    fillInput(container, 'municipality', 'victoria');

    fillInput(container, 'summary', 'asummary');

    await act(async () => userEvent.click(resetButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({ ...defaultFilter, searchBy: 'persons' }),
    );
  });
});
