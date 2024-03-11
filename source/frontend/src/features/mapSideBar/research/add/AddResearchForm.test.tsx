import { screen } from '@testing-library/react';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { fakeText, fillInput, render, RenderOptions } from '@/utils/test-utils';

import { AddResearchFileYupSchema } from './AddResearchFileYupSchema';
import AddResearchForm from './AddResearchForm';
import { ResearchForm } from './models';

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

jest.mock('@/components/common/mapFSM/MapStateMachineContext');
(useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);

describe('AddResearchForm component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { initialValues: ResearchForm }) => {
    const component = render(
      <Formik<ResearchForm>
        onSubmit={noop}
        initialValues={renderOptions.initialValues}
        validationSchema={AddResearchFileYupSchema}
      >
        {formikProps => <AddResearchForm />}
      </Formik>,
      {
        ...renderOptions,
        claims: [],
        store: storeState,
        history,
      },
    );

    return { ...component };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const initialValues = new ResearchForm();
    const { asFragment, findByText } = setup({ initialValues });

    expect(await findByText(/No results found for your search criteria./i)).toBeVisible();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the R-file name field', async () => {
    const initialValues = new ResearchForm();
    const { getByPlaceholderText, findByText } = setup({ initialValues });

    expect(await findByText(/No results found for your search criteria./i)).toBeVisible();
    const fileName = getByPlaceholderText(/Road name - Descriptive text/i);
    expect(fileName).toBeInTheDocument();
  });

  it('should have the Help with choosing a name text in the component', async () => {
    const initialValues = new ResearchForm();
    setup({ initialValues });

    expect(await screen.findByText(/No results found for your search criteria./i)).toBeVisible();
    expect(screen.getByText(`Help with choosing a name`)).toBeInTheDocument();
  });

  it('should validate R-File name', async () => {
    const initialValues = new ResearchForm();
    const { container, findByText } = setup({ initialValues });

    expect(await findByText(/No results found for your search criteria./i)).toBeVisible();

    // name is required
    await fillInput(container, 'name', '');
    expect(await findByText(/Research File name is required/i)).toBeVisible();
    // name cannot exceed 250 characters
    await fillInput(container, 'name', fakeText(300));
    expect(await findByText(/Research File name must be less than 250 characters/i)).toBeVisible();
  });
});
