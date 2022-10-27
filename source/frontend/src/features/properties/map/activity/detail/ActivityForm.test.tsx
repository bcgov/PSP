import { screen } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Input } from 'components/common/form';
import { Claims } from 'constants/claims';
import { FileTypes } from 'constants/fileTypes';
import { useFormikContext } from 'formik';
import { mockLookups } from 'mocks';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { getMockActivityResponse } from 'mocks/mockActivities';
import { act } from 'react-test-renderer';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';
import * as Yup from 'yup';

import { ActivityForm, IActivityFormProps } from './ActivityForm';
import { ActivityModel } from './models';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockAxios = new MockAdapter(axios);

jest.mock('@react-keycloak/web');
const onSave = jest.fn();
const onEditRelatedProperties = jest.fn();
const setEditMode = jest.fn();

describe('ActivityForm test', () => {
  const setup = (renderOptions?: RenderOptions & Partial<IActivityFormProps>) => {
    // render component under test
    const component = render(
      <ActivityForm
        file={
          renderOptions?.file ?? {
            ...mockAcquisitionFileResponse(),
            id: 1,
            fileType: FileTypes.Acquisition,
          }
        }
        activity={renderOptions?.activity ?? { ...getMockActivityResponse(), id: 2 }}
        editMode={renderOptions?.editMode ?? false}
        setEditMode={renderOptions?.setEditMode ?? setEditMode}
        onEditRelatedProperties={onEditRelatedProperties}
        onSave={onSave}
        isEditable={true}
        formContent={renderOptions?.formContent}
      />,
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
    const { asFragment, findByText } = setup();
    await findByText('No matching Documents found');
    expect(asFragment()).toMatchSnapshot();
  });

  it('hides save/cancel buttons when edit mode is false', async () => {
    const { queryByText, findByText } = setup({ editMode: false });
    await findByText('No matching Documents found');
    expect(queryByText('Save')).toBeNull();
    expect(queryByText('Cancel')).toBeNull();
  });

  it('displays save/cancel buttons when edit mode is true', async () => {
    const { getByText, findByText } = setup({ editMode: true });
    await findByText('No matching Documents found');
    expect(getByText('Save')).toBeVisible();
    expect(getByText('Cancel')).toBeVisible();
  });

  it('disables save button when form is not dirty', async () => {
    const { getByText, findByText } = setup({ editMode: true });
    await findByText('No matching Documents found');
    const saveButton = getByText('Save');

    expect(saveButton.closest('button')).toBeDisabled();
  });

  it('disables save button when save button is clicked and form is submitting', async () => {
    onSave.mockResolvedValue(getMockActivityResponse());
    const { getByText, container, findByText } = setup({ editMode: true });
    await findByText('No matching Documents found');
    const saveButton = getByText('Save').closest('button');
    await fillInput(container, 'description', 'another description', 'textarea');
    await waitFor(() => expect(saveButton).not.toBeDisabled());
    userEvent.click(getByText('Save'));

    await waitFor(() => expect(saveButton).toBeDisabled());
  });

  it('calls onCancel when form is not dirty and cancel is clicked', async () => {
    const { getByText, findByText } = setup({ editMode: true });
    await findByText('No matching Documents found');

    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);

    expect(setEditMode).toHaveBeenCalledWith(false);
  });

  it('displays modal when form is dirty and cancel is clicked', async () => {
    const { getByText, findByText, findByDisplayValue, container } = setup({ editMode: true });
    await findByText('No matching Documents found');

    await fillInput(container, 'description', 'another description', 'textarea');
    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);

    expect(screen.getByText('Unsaved Changes')).toBeVisible();
    expect(await findByDisplayValue('another description')).toBeVisible();
  });

  it('calls onCancel when form is dirty and cancel is clicked and modal is confirmed', async () => {
    const { getByText, findByText, findByDisplayValue, container } = setup({ editMode: true });
    await findByText('No matching Documents found');

    await fillInput(container, 'description', 'another description', 'textarea');
    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);
    expect(screen.getByText('Unsaved Changes')).toBeVisible();
    const confirmButton = getByText('Confirm');
    act(() => {
      userEvent.click(confirmButton);
    });

    expect(setEditMode).toHaveBeenCalledWith(false);
    expect(await findByDisplayValue('test description')).toBeVisible();
  });

  it('does not call onCancel when form is dirty and cancel is clicked and modal is not confirmed', async () => {
    const { getByText, findByText, findByDisplayValue, container } = setup({ editMode: true });
    await findByText('No matching Documents found');

    await fillInput(container, 'description', 'another description', 'textarea');
    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);
    expect(screen.getByText('Unsaved Changes')).toBeVisible();
    const noButton = getByText('No');
    act(() => {
      userEvent.click(noButton);
    });

    expect(setEditMode).not.toHaveBeenCalled();
    expect(await findByDisplayValue('another description')).toBeVisible();
  });

  describe('dynamic form content', () => {
    it('displays the view form in view mode', async () => {
      const { getByText, findByText } = setup({
        formContent: {
          header: 'Test',
          initialValues: {} as any,
          EditForm: props => <>Site Visit(Edit)</>,
          ViewForm: props => <>Site Visit(View)</>,
          validationSchema: Yup.object().shape({}),
          version: '1.0',
        },
      });
      await findByText('No matching Documents found');

      userEvent.click(getByText('Test'));

      expect(getByText('Site Visit(View)')).toBeVisible();
    });
    it('displays the edit form in edit mode', async () => {
      const { getByText, findByText } = setup({
        formContent: {
          header: 'Test',
          initialValues: {} as any,
          EditForm: props => <>Site Visit(Edit)</>,
          ViewForm: props => <>Site Visit(View)</>,
          validationSchema: Yup.object().shape({}),
          version: '1.0',
        },
        editMode: true,
        isEditable: true,
      });
      await findByText('No matching Documents found');

      expect(getByText('Site Visit(Edit)')).toBeVisible();
    });
    it('prepopulates the form based on loaded data', async () => {
      const { getByText, findByText } = setup({
        formContent: {
          header: 'Test',
          initialValues: {} as any,
          EditForm: props => <>Site Visit(Edit)</>,
          ViewForm: props => {
            const { values } = useFormikContext<ActivityModel>();
            return <>{values.activityData.test}</>;
          },
          validationSchema: Yup.object().shape({}),
          version: '1.0',
        },
        activity: { activityDataJson: '{"test":"test string"}' } as any,
      });
      await findByText('No matching Documents found');

      userEvent.click(getByText('Test'));

      expect(getByText('test string')).toBeVisible();
    });
    it('saves dynamic form data as expected', async () => {
      const { getByText, findByText, container } = setup({
        formContent: {
          header: 'Test',
          initialValues: { test: '' } as any,
          EditForm: props => <Input field="activityData.test" />,
          ViewForm: props => <></>,
          validationSchema: Yup.object().shape({}),
          version: '1.0',
        },
        editMode: true,
      });
      await findByText('No matching Documents found');

      await fillInput(container, 'activityData.test', 'test string');
      const saveButton = getByText('Save').closest('button');
      await waitFor(() => expect(saveButton).not.toBeDisabled());
      userEvent.click(saveButton!);

      await waitFor(() => {
        expect(onSave).toHaveBeenCalledWith(
          expect.objectContaining({ activityDataJson: '{"test":"test string","version":"1.0"}' }),
        );
      });
    });
    it('displays validation errors and does not save when given a validation schema', async () => {
      const { getByText, findByText, container } = setup({
        formContent: {
          header: 'Test',
          initialValues: { test: '' } as any,
          EditForm: props => <Input field="activityData.test" />,
          ViewForm: props => <></>,
          validationSchema: Yup.object().shape({
            activityData: Yup.object().shape({ test: Yup.string().required('test is required') }),
          }),
          version: '1.0',
        },
        editMode: true,
        isEditable: true,
      });
      await findByText('No matching Documents found');

      await fillInput(container, 'description', 'another description', 'textarea');
      const saveButton = getByText('Save').closest('button');
      await waitFor(() => expect(saveButton).not.toBeDisabled());
      userEvent.click(saveButton!);

      expect(await findByText('test is required')).toBeVisible();
    });
    it('displays validation errors and does not save when given a validation function', async () => {
      const { getByText, findByText, container } = setup({
        formContent: {
          header: 'Test',
          initialValues: { test: '' } as any,
          EditForm: props => <Input field="activityData.test" />,
          ViewForm: props => <></>,
          validationSchema: Yup.object().shape({}),
          validationFunction: (values: any) => ({ activityData: { test: 'test is required' } }),
          version: '1.0',
        },
        editMode: true,
      });
      await findByText('No matching Documents found');

      await fillInput(container, 'description', 'another description', 'textarea');
      const saveButton = getByText('Save').closest('button');
      await waitFor(() => expect(saveButton).not.toBeDisabled());
      userEvent.click(saveButton!);

      expect(await findByText('test is required')).toBeVisible();
    });
  });
});
