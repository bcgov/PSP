import { Formik, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { act, fillInput, renderAsync, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';
import PropertySearchSelectorPidFormView, {
  IPropertySearchSelectorPidFormViewProps,
} from './PropertySelectorPidSearchView';
import { SubdivisionFormModel } from '@/features/mapSideBar/subdivision/AddSubdivisionModel';
import noop from 'lodash/noop';

const history = createMemoryHistory();

const onSearch = vi.fn();

describe('PropertySearchPidSelector component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IPropertySearchSelectorPidFormViewProps>;
    } = {},
  ) => {
    const ref = createRef<FormikProps<SubdivisionFormModel>>();
    const component = await renderAsync(
      <Formik<SubdivisionFormModel>
        innerRef={ref}
        onSubmit={noop}
        initialValues={new SubdivisionFormModel()}
      >
        <PropertySearchSelectorPidFormView
          loading={renderOptions.props?.loading ?? false}
          onSearch={onSearch}
        />
      </Formik>,
      {
        history,
        useMockAuthentication: true,
        claims: [],
        ...renderOptions,
      },
    );

    return {
      ...component,
      getFormikRef: () => ref,
    };
  };

  beforeEach(() => {
    vi.resetAllMocks();
  });

  it('renders correctly', async () => {
    const component = await setup();
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('disables searchButton when loading', async () => {
    const { getByTitle } = await setup({ props: { loading: true } });

    expect(getByTitle('search')).toBeDisabled();
  });

  it('calls onSearch', async () => {
    const { getByTitle, container } = await setup();

    await waitFor(async () => {
      fillInput(container, 'pid', '123-456-789');
    });

    const searchButton = getByTitle('search');
    await act(async () => {
      userEvent.click(searchButton);
    });

    expect(onSearch).toHaveBeenCalled();
  });
});
