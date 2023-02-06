import { SelectOption } from 'components/common/form';
import * as API from 'constants/API';
import { FormikProps } from 'formik';
import { GetMockLookUpsByType, mockLookups } from 'mocks/mockLookups';
import { mockProjectGetResponse } from 'mocks/mockProjects';
import { createRef } from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions } from 'utils/test-utils';

import { ProjectForm } from '../models';
import { IUpdateProjectContainerViewProps } from './UpdateProjectContainer';
import UpdateProjectContainerView from './UpdateProjectContainerView';

jest.mock('@react-keycloak/web');

const stubProject = mockProjectGetResponse();

let mockRegionOptions: SelectOption[] = GetMockLookUpsByType(API.REGION_TYPES);
let mockProjectStatuses: SelectOption[] = GetMockLookUpsByType(API.PROJECT_STATUS_TYPES);

const mockProps: IUpdateProjectContainerViewProps = {
  initialValues: ProjectForm.fromApi(stubProject),
  projectStatusOptions: mockProjectStatuses,
  projectRegionOptions: mockRegionOptions,
  formikRef: createRef<FormikProps<ProjectForm>>(),
  onSubmit: jest.fn(),
};

describe('Update Project Form View component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions = {}) => {
    const ref = createRef<FormikProps<ProjectForm>>();
    const utils = render(
      <UpdateProjectContainerView
        initialValues={mockProps.initialValues}
        projectStatusOptions={mockProps.projectStatusOptions}
        projectRegionOptions={mockProps.projectRegionOptions}
        formikRef={ref}
        onSubmit={mockProps.onSubmit}
      />,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        claims: [],
      },
    );
    return {
      ...utils,
      getFormikRef: () => ref,
      getNameTextbox: () =>
        utils.container.querySelector(`input[name="projectName"]`) as HTMLInputElement,
      getNumberTextbox: () =>
        utils.container.querySelector(`input[name="projectNumber"]`) as HTMLInputElement,
      getRegionDropdown: () =>
        utils.container.querySelector(`select[name="region"]`) as HTMLSelectElement,
      getStatusDropdown: () =>
        utils.container.querySelector(`select[name="projectStatusType"]`) as HTMLSelectElement,
      getSummaryTextbox: () =>
        utils.container.querySelector(`textarea[name="summary"]`) as HTMLInputElement,
      getProductCodeTextBox: (index: number) =>
        utils.container.querySelector(`input[name="products.${index}.code"]`) as HTMLInputElement,
    };
  };

  let initialValues: ProjectForm;

  beforeEach(() => {
    initialValues = mockProps.initialValues;
    jest.resetAllMocks();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Renders Component as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('Renders form fields witn project as expected', async () => {
    const {
      getNameTextbox,
      getNumberTextbox,
      getStatusDropdown,
      getRegionDropdown,
      getSummaryTextbox,
    } = setup();

    const input = getNameTextbox();
    const number = getNumberTextbox();
    const select = getRegionDropdown();
    const status = getStatusDropdown();
    const summary = getSummaryTextbox();

    expect(input).toBeVisible();
    expect(select).toBeVisible();
    expect(number).toBeVisible();
    expect(status).toBeVisible();
    expect(summary).toBeVisible();

    expect(input).toHaveDisplayValue(initialValues.projectName as string);
    expect(select).toHaveDisplayValue(stubProject.regionCode?.description as string);
    expect(number).toHaveDisplayValue(initialValues.projectNumber as string);
    expect(status).toHaveDisplayValue(stubProject.projectStatusTypeCode?.description as string);
    expect(summary).toHaveDisplayValue(initialValues.summary as string);
  });
});
