import { FormikHelpers, FormikProps } from 'formik';
import React from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { mockProjectGetResponse, mockProjectPostResponse } from '@/mocks/projects.mock';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { IAddProjectFormProps } from '../../../add/AddProjectForm';
import { ProjectForm } from '../../../models';
import UpdateProjectContainer from './UpdateProjectContainer';

const mockApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

jest.mock('@/hooks/repositories/useProjectProvider', () => ({
  useProjectProvider: () => {
    return {
      updateProject: mockApi,
    };
  },
}));

jest.mock('@/hooks/repositories/useFinancialCodeRepository', () => ({
  useFinancialCodeRepository: () => {
    return {
      getFinancialCodesByType: {
        error: undefined,
        response: [],
        execute: jest.fn(),
        loading: false,
      },
    };
  },
}));

jest.mock('@/components/common/mapFSM/MapStateMachineContext');

let viewProps: IAddProjectFormProps | undefined;
const TestView = React.forwardRef<FormikProps<ProjectForm>, IAddProjectFormProps>(
  (props, formikRef) => {
    viewProps = props;
    return <span>Content Rendered</span>;
  },
);

const mockProject = mockProjectGetResponse();
const formValues: ProjectForm = ProjectForm.fromApi(mockProject);

const onSuccess = jest.fn();

describe('UpdateProjectContainer', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <SideBarContextProvider project={mockProject}>
        <UpdateProjectContainer project={mockProject} View={TestView} onSuccess={onSuccess} />
      </SideBarContextProvider>,
      {
        ...renderOptions,
      },
    );
    return { ...utils };
  };

  beforeEach(() => {
    viewProps = {
      initialValues: formValues,
      projectStatusOptions: [],
      businessFunctionOptions: [],
      costTypeOptions: [],
      workActivityOptions: [],
      onSubmit: jest.fn(),
    };
    jest.resetAllMocks();
    (useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);
  });

  it('renders the underlying form', () => {
    const { getByText } = setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('Maps the ApiGen_Concepts_Project values to the form passed to the view', () => {
    expect(formValues.id).toEqual(mockProject.id);
    expect(formValues.rowVersion).toEqual(mockProject.rowVersion);
    expect(formValues.projectName).toEqual(mockProject.description);
    expect(formValues.projectNumber).toEqual(mockProject.code);
    expect(formValues.region).toEqual(mockProject.regionCode?.id);
    expect(formValues.projectStatusType).toEqual(mockProject.projectStatusTypeCode?.id);
    expect(formValues.summary).toEqual(mockProject.note);
  });

  it('makes request to update the Project', async () => {
    setup();
    const formikHelpers: Partial<FormikHelpers<ProjectForm>> = {
      setSubmitting: jest.fn(),
      resetForm: jest.fn(),
    };

    mockApi.execute.mockResolvedValue(
      mockProjectPostResponse(
        1,
        2,
        'updated project name',
        'updated number',
        2,
        'AC',
        'updated summary',
      ),
    );

    await act(async () => {
      return viewProps?.onSubmit(
        viewProps.initialValues,
        formikHelpers as FormikHelpers<ProjectForm>,
      );
    });

    expect(mockApi.execute).toHaveBeenCalled();
    expect(onSuccess).toHaveBeenCalled();
    expect(formikHelpers.setSubmitting).toHaveBeenCalled();
    expect(formikHelpers.resetForm).toHaveBeenCalled();
  });
});
