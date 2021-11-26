import { createMemoryHistory } from 'history';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { render, RenderOptions } from 'utils/test-utils';

import { ContactContainer, IContactContainerProps } from '../..';

const history = createMemoryHistory();

const getContact = jest.fn();
jest.mock('hooks/pims-api/useApiContacts');
((useApiContacts as unknown) as jest.Mock<Partial<typeof useApiContacts>>).mockReturnValue({
  getContact,
});

describe('ContactContainer component', () => {
  const setup = (renderOptions?: RenderOptions & Partial<IContactContainerProps>) => {
    // render component under test
    const component = render(
      <ContactContainer
        match={
          renderOptions?.match ?? {
            params: { id: 'P0' },
            isExact: false,
            path: '',
            url: '',
          }
        }
        history={{} as any}
        location={{} as any}
        type={{} as any}
      />,
      {
        ...renderOptions,
        history,
      },
    );

    return {
      component,
    };
  };
  beforeEach(() => {
    getContact.mockReset();
    history.push('/Contact/1?ContactPageName=details');
  });

  it('renders as expected', () => {
    const { component } = setup({});
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('displays contact selector if no id is present', () => {
    const {
      component: { getByLabelText },
    } = setup({
      match: {
        params: { id: undefined },
        isExact: false,
        path: '',
        url: '',
      },
    });
    expect(getByLabelText('Individual')).toBeVisible();
  });

  it('hides contact selector if an id is present', () => {
    const {
      component: { queryByLabelText },
    } = setup({});
    expect(queryByLabelText('Individual')).toBeNull();
  });
});
