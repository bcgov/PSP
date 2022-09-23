import { screen, waitFor } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { SideBarContextProvider } from 'features/properties/map/context/sidebarContext';
import { mockLookups } from 'mocks/mockLookups';
import { getMockResearchFile } from 'mocks/mockResearchFile';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, render, RenderOptions, userEvent } from 'utils/test-utils';

import UpdateProperties, { IUpdatePropertiesProps } from './UpdateProperties';

const mockAxios = new MockAdapter(axios);
jest.mock('@react-keycloak/web');

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

const setIsShowingPropertySelector = jest.fn();
const onSuccess = jest.fn();
const updateFileProperties = jest.fn();

describe('UpdateProperties component', () => {
  // render component under test
  const setup = (props: Partial<IUpdatePropertiesProps>, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <SideBarContextProvider>
        <UpdateProperties
          file={props.file ?? getMockResearchFile()}
          setIsShowingPropertySelector={setIsShowingPropertySelector}
          onSuccess={onSuccess}
          updateFileProperties={updateFileProperties}
        />
      </SideBarContextProvider>,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
    mockAxios.onGet().reply(200, getMockResearchFile());
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    setup({});
    expect(document.body).toMatchSnapshot();
  });

  it('save button displays modal', async () => {
    const { getByText } = setup({});
    const saveButton = getByText('Save');
    userEvent.click(saveButton);

    expect(
      await screen.findByText(/You have made changes to the properties in this file./g),
    ).toBeVisible();
  });

  it('saving and confirming the modal saves the properties', async () => {
    updateFileProperties.mockResolvedValue(getMockResearchFile());
    const { getByText } = setup({});
    const saveButton = getByText('Save');
    userEvent.click(saveButton);

    const saveConfirmButton = await screen.findByTitle('ok-modal');
    userEvent.click(saveConfirmButton);

    await waitFor(() => {
      expect(updateFileProperties).toHaveBeenCalled();
      expect(setIsShowingPropertySelector).toHaveBeenCalledWith(false);
      expect(onSuccess).toHaveBeenCalled();
    });
  });

  it('cancel button cancels component if no actions taken', async () => {
    const { getByText } = setup({});
    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);

    expect(setIsShowingPropertySelector).toHaveBeenCalledWith(false);
  });

  it('cancel button displays modal', async () => {
    const { getByText, container } = setup({});

    await fillInput(container, 'properties.0.name', 'test property name');

    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);

    expect(
      await screen.findByText(/If you cancel now, this file will not be saved./g),
    ).toBeVisible();
  });

  it('cancelling and confirming the modal hides this component', async () => {
    updateFileProperties.mockResolvedValue(getMockResearchFile());
    const { getByText, container } = setup({});

    await fillInput(container, 'properties.0.name', 'test property name');

    const saveButton = getByText('Save');
    userEvent.click(saveButton);

    const cancelConfirmButton = await screen.findByTitle('ok-modal');
    userEvent.click(cancelConfirmButton);

    await waitFor(() => {
      expect(updateFileProperties).toHaveBeenCalled();
      expect(setIsShowingPropertySelector).toHaveBeenCalledWith(false);
      expect(onSuccess).toHaveBeenCalled();
    });
  });

  it('handles cancel as expected', async () => {
    setup({});
    expect(document.body).toMatchSnapshot();
  });
});
