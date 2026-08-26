import userEvent from '@testing-library/user-event';

import { act, fillInput, render, RenderOptions } from '@/utils/test-utils';

import {
  ContactFilterComponent,
  defaultFilter,
  IContactFilterComponentProps,
} from './ContactFilterComponent';

const setFilter = vi.fn();

// render component under test
const setup = (renderOptions: RenderOptions & IContactFilterComponentProps = { setFilter }) => {
  const { filter, setFilter: setFilterFn, ...rest } = renderOptions;
  const utils = render(
    <ContactFilterComponent filter={filter} setFilter={setFilterFn} showActiveSelector />,
    {
      ...rest,
    },
  );
  const searchButton = utils.getByTestId('contact-filter-search');
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
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        ...defaultFilter,
        searchBy: ['pimsusers', 'persons', 'organizations'],
      }),
    );
  });

  it('searches organizations if other options are de-selected', async () => {
    const { container, searchButton } = setup();
    const individualsButton = container.querySelector(`#input-searchBy-persons`);
    const pimsUsersButton = container.querySelector(`#input-searchBy-pimsusers`);
    await act(async () => {
      individualsButton && userEvent.click(individualsButton);
      pimsUsersButton && userEvent.click(pimsUsersButton);
    });
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({ ...defaultFilter, searchBy: ['organizations'] }),
    );
  });

  it('searches persons only if other options are de-selected', async () => {
    const { container, searchButton } = setup();
    const prganizationButton = container.querySelector(`#input-searchBy-organizations`);
    const pimsUsersButton = container.querySelector(`#input-searchBy-pimsusers`);
    await act(async () => {
      prganizationButton && userEvent.click(prganizationButton);
      pimsUsersButton && userEvent.click(pimsUsersButton);
    });
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({ ...defaultFilter, searchBy: ['persons'] }),
    );
  });

  it('searches pims users only if other options are de-selected', async () => {
    const { container, searchButton } = setup();
    const prganizationButton = container.querySelector(`#input-searchBy-organizations`);
    const pimsPersonsButton = container.querySelector(`#input-searchBy-persons`);
    await act(async () => {
      prganizationButton && userEvent.click(prganizationButton);
      pimsPersonsButton && userEvent.click(pimsPersonsButton);
    });
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({ ...defaultFilter, searchBy: ['pimsusers'] }),
    );
  });

  it('searches for active contacts by default', async () => {
    const { searchButton } = setup();
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(expect.objectContaining({ ...defaultFilter }));
  });

  it('searches for inactive contacts if checkbox unchecked', async () => {
    const { container, searchButton } = setup();

    const activeCheck = container.querySelector(`#input-activeContactsOnly`);
    expect(activeCheck).not.toBeNull();
    await act(async () => userEvent.click(activeCheck as Element));
    await act(async () => userEvent.click(searchButton));

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
    await act(async () => {
      personButton && userEvent.click(personButton);
    });

    fillInput(container, 'municipality', 'victoria');

    fillInput(container, 'summary', 'asummary');

    await act(async () => userEvent.click(resetButton));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        ...defaultFilter,
        searchBy: ['pimsusers', 'persons', 'organizations'],
      }),
    );
  });
});
