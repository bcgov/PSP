import { IProjectFilter } from '@/features/projects/interfaces';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { IProjectFilterProps, ProjectFilter } from './ProjectFilter';
import { FormikProps } from 'formik';
import { ProjectFilterModel } from './models/ProjectFilterModel';
import { createRef } from 'react';
const setFilter = vi.fn();
const onResetFilter = vi.fn();

const mockFilterModel = new ProjectFilterModel();

describe('Project Filter', () => {
  const setup = async (renderOptions: RenderOptions & { props?: Partial<IProjectFilterProps> }) => {
    const formikRef = createRef<FormikProps<ProjectFilterModel>>();

    const utils = render(
      <ProjectFilter
        {...renderOptions.props}
        initialValues={renderOptions.props?.initialValues ?? mockFilterModel}
        pimsRegionsOptions={renderOptions.props?.pimsRegionsOptions ?? []}
        setFilter={setFilter}
        onResetFilter={onResetFilter}
        createdByOptions={renderOptions.props?.createdByOptions ?? []}
      />,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
      },
    );

    // wait for useEffects
    await act(async () => {});

    return {
      ...utils,
      formikRef,
      getSearchButton: () => utils.getByTestId('search'),
      getResetButton: () => utils.getByTestId('reset-button'),
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('matches snapshot', async () => {
    const { asFragment } = await setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('searches by project number', async () => {
    const { container, getSearchButton } = await setup({});

    fillInput(container, 'projectNumber', '1201');
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining<IProjectFilter>({
        projectNumber: '1201',
        projectStatusCode: null,
        projectName: null,
        regions: [],
      }),
    );
  });

  it('searches by project name', async () => {
    const { container, getSearchButton } = await setup({});

    fillInput(container, 'projectName', 'Hwy');
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        projectName: 'Hwy',
        regions: [],
      }),
    );
  });

  it('searches by region', async () => {
    const { container, getSearchButton } = await setup({});

    fillInput(container, 'projectRegionCode', '2', 'select');
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        regions: [],
      }),
    );
  });

  it('searches by status', async () => {
    const { container, getSearchButton } = await setup({});

    fillInput(container, 'projectStatusCode', 'PL', 'select');
    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        projectStatusCode: 'PL',
        regions: [],
      }),
    );
  });

  it('searches by created by', async () => {
    const selectedUser = [{ id: 'DSMITH', text: 'Devin Smith (DSMITH)' }];

    const initialValues = new ProjectFilterModel();
    initialValues.projectCreatedBy = selectedUser;

    const { getSearchButton } = await setup({
      props: {
        initialValues,
        createdByOptions: selectedUser,
      },
    });

    await act(async () => userEvent.click(getSearchButton()));

    expect(setFilter).toHaveBeenCalledWith(
      expect.objectContaining({
        projectCreatedBy: 'DSMITH',
        projectNumber: null,
        projectName: null,
        projectStatusCode: null,
        regions: [],
      }),
    );
  });
});
