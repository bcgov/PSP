import { screen, waitFor } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Claims } from 'constants/claims';
import { mockLookups } from 'mocks';
import { getMockApiPropertyFiles } from 'mocks/mockProperties';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, userEvent } from 'utils/test-utils';

import ActivityPropertyModal, { IActivityPropertyModalProps } from './ActivityPropertyModal';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockAxios = new MockAdapter(axios);

jest.mock('@react-keycloak/web');

const setDisplay = jest.fn();
const setSelectedFileProperties = jest.fn();
const onSave = jest.fn();

const defaultProps: IActivityPropertyModalProps = {
  display: true,
  setDisplay,
  originalSelectedProperties: [],
  allProperties: getMockApiPropertyFiles(),
  selectedFileProperties: [],
  setSelectedFileProperties,
  onSave,
};

describe('ActivityPropertyModal tests', () => {
  const setup = (
    renderOptions?: RenderOptions & { props?: Partial<IActivityPropertyModalProps> },
  ) => {
    // render component under test
    const component = render(
      <ActivityPropertyModal {...{ ...defaultProps, ...renderOptions?.props }} />,
      {
        ...renderOptions,
        store: storeState,
        claims: renderOptions?.claims ?? [Claims.ACTIVITY_EDIT, Claims.PROPERTY_EDIT],
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    mockAxios.onAny().reply(200, []);
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  it('Renders as expected', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays no selected properties by default', async () => {
    const { getAllByTitle } = setup();
    const checkBoxes = getAllByTitle('Toggle Row Selected');
    checkBoxes.forEach(checkBox => {
      expect(checkBox).not.toBeChecked();
    });
  });

  it('can select a property', async () => {
    const { getAllByTitle, getByText } = setup();

    const checkBoxes = getAllByTitle('Toggle Row Selected');
    userEvent.click(checkBoxes[0]);
    const saveButton = getByText('Save');
    userEvent.click(saveButton);

    expect(setSelectedFileProperties).toHaveBeenCalledWith(
      expect.arrayContaining([expect.anything()]),
    );
  });

  it('can select multiple properties', async () => {
    const { getAllByTitle } = setup();

    const checkBoxes = getAllByTitle('Toggle Row Selected');
    checkBoxes.forEach(checkBox => {
      userEvent.click(checkBox);
    });
    expect(setSelectedFileProperties).toHaveBeenCalledWith(
      expect.arrayContaining([expect.anything(), expect.anything()]),
    );
  });

  it('can preselect properties based on prop', async () => {
    const { getAllByTitle } = setup({
      props: { selectedFileProperties: [getMockApiPropertyFiles()[0]] },
    });

    const checkBoxes = getAllByTitle('Toggle Row Selected');

    expect(checkBoxes[0]).toBeChecked();
  });

  it('does not display cancel modal when no changes are made.', async () => {
    const { getByText } = setup();

    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);

    expect(screen.queryByText('Unsaved Changes')).toBeNull();
  });

  it('displays the cancel modal if there are unsaved changes', async () => {
    const { getByText } = setup({ props: { selectedFileProperties: getMockApiPropertyFiles() } });

    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);

    expect(screen.getByText('Unsaved Changes')).toBeVisible();
  });

  it('confirming the cancel modal closes the modal and resets the selected items', async () => {
    const { getByText } = setup({ props: { selectedFileProperties: getMockApiPropertyFiles() } });

    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);
    const confirmButton = screen.getByText('Confirm');
    userEvent.click(confirmButton);

    expect(setDisplay).toHaveBeenLastCalledWith(false);
    expect(setSelectedFileProperties).toHaveBeenLastCalledWith([]);
  });

  it('cancelling the cancel modal does not close the modal and does not reset the selected items', async () => {
    const { getByText } = setup({ props: { selectedFileProperties: getMockApiPropertyFiles() } });

    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);
    const noButton = screen.getByText('No');
    userEvent.click(noButton);

    await waitFor(() => {
      expect(screen.queryByText('Unsaved Changes')).toBeNull();
    });
    expect(screen.getByText('Related properties')).toBeVisible();
    expect(setSelectedFileProperties).not.toHaveBeenCalled();
  });

  it('confirming the modal calls onSave and hides the modal', async () => {
    onSave.mockResolvedValueOnce({});
    const { getByText } = setup({
      props: { selectedFileProperties: [getMockApiPropertyFiles()[0]], activityModel: {} as any },
    });

    const saveButton = getByText('Save');
    userEvent.click(saveButton);

    await waitFor(() => {
      expect(onSave).toHaveBeenCalledWith({
        actInstPropFiles: [
          {
            activityId: undefined,
            propertyFileId: 1,
          },
        ],
      });
    });
    expect(setDisplay).toHaveBeenCalledWith(false);
  });
});
