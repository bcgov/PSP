import { Formik } from 'formik';
import { noop } from 'lodash';

import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { ResearchFileProjectFormModel } from '../models';
import { UpdateProjectsSubForm, WithProjectValues } from './UpdateProjectsSubForm';

describe('UpdateProjectsSubForm', () => {
  // render component under test
  const setup = (props: { initialForm: WithProjectValues }, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <Formik initialValues={props.initialForm} onSubmit={noop}>
        {formikProps => <UpdateProjectsSubForm field="researchFileProjects" />}
      </Formik>,
      { ...renderOptions },
    );

    return { ...utils };
  };

  let testForm: WithProjectValues;

  beforeEach(() => {
    testForm = { researchFileProjects: [] };
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected with minimal form data', () => {
    const { asFragment } = setup({ initialForm: testForm });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders as expected with pre-existing form data', () => {
    const researchProjects = getMockResearchFile().researchFileProjects || [];
    testForm.researchFileProjects = researchProjects.map(x =>
      ResearchFileProjectFormModel.fromApi(x),
    );
    const { asFragment } = setup({ initialForm: testForm });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a link to add new project', async () => {
    const { getByTestId } = setup({ initialForm: testForm });
    expect(getByTestId('add-project')).toBeVisible();
  });

  it('displays option to remove existing project', async () => {
    const { getByTestId } = setup({ initialForm: testForm });
    const addLink = getByTestId('add-project');
    await act(async () => userEvent.click(addLink));

    expect(getByTestId('remove-button')).toBeVisible();
  });

  it('removes an existing project when remove button is clicked', async () => {
    const researchProjects = getMockResearchFile().researchFileProjects || [];
    testForm.researchFileProjects = researchProjects.map(x =>
      ResearchFileProjectFormModel.fromApi(x),
    );
    const { getByTestId } = setup({ initialForm: testForm });

    let projectSelector = document.querySelector(
      `input[name="typeahead-researchFileProjects[0].project"]`,
    );
    expect(projectSelector).toHaveValue('CLAIMS');

    const removeBtn = getByTestId('remove-button');
    await act(async () => userEvent.click(removeBtn));

    projectSelector = document.querySelector(
      `input[name="typeahead-researchFileProjects[0].project"]`,
    );
    expect(projectSelector).toBeNull();
  });

  it('displays the project selector', async () => {
    const { getByTestId } = setup({ initialForm: testForm });
    const addLink = getByTestId('add-project');
    await act(async () => userEvent.click(addLink));

    const projectSelector = document.querySelector(
      `input[name="typeahead-researchFileProjects[0].project"]`,
    );
    expect(projectSelector).toBeVisible();
  });
});
