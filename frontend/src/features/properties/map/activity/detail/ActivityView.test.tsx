import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Claims } from 'constants/claims';
import { Formik } from 'formik';
import noop from 'lodash/noop';
import { mockLookups } from 'mocks';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { getMockActivityResponse } from 'mocks/mockActivities';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions } from 'utils/test-utils';

import { ActivityView, IActivityViewProps } from './ActivityView';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockAxios = new MockAdapter(axios);

jest.mock('@react-keycloak/web');

describe('ActivityView test', () => {
  const setup = (renderOptions?: RenderOptions & IActivityViewProps) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions?.activity ?? getMockActivityResponse()}>
        <ActivityView
          file={renderOptions?.file ?? { ...mockAcquisitionFileResponse(), id: 1 }}
          activity={renderOptions?.activity ?? { ...getMockActivityResponse(), id: 2 }}
          editMode={renderOptions?.editMode ?? false}
          setEditMode={renderOptions?.setEditMode ?? noop}
        />
      </Formik>,
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

  it('sections are expanded by default', async () => {
    const { getByTitle } = setup();
    expect(getByTitle('collapse-documents')).toBeInTheDocument();
    expect(getByTitle('collapse-notes')).toBeInTheDocument();
    expect(getByTitle('collapse-description')).toBeInTheDocument();
  });

  it('documents are displayed', async () => {
    const { getByText } = setup();
    expect(getByText('Document type')).toBeVisible();
  });

  it('notes are displayed', async () => {
    const { getByText } = setup();
    expect(getByText('Note')).toBeVisible();
  });
});
