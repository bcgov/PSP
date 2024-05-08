import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/claims';
import { mockKeycloak, render, RenderOptions } from '@/utils/test-utils';

import ContactViewContainer from './Container';

// mock auth library

const history = createMemoryHistory();

describe('ContactViewContainer component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions = {}) => {
    const component = render(<ContactViewContainer />, {
      ...renderOptions,
      history,
    });
    return { ...component };
  };

  beforeEach(() => {
    mockKeycloak({ claims: [Claims.CONTACT_VIEW] });
  });

  it('renders as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  describe('when user has CONTACT_EDIT claim', () => {
    it('shows edit contact button', async () => {
      mockKeycloak({ claims: [Claims.CONTACT_VIEW, Claims.CONTACT_EDIT] });
      const { findByTitle } = setup();
      const editButton = await findByTitle(/edit contact/i);

      expect(editButton).toBeInTheDocument();
    });
  });

  describe(`when user doesn't have CONTACT_EDIT claim`, () => {
    it('does not show the edit button', () => {
      const { queryByTitle } = setup();
      const editButton = queryByTitle(/edit contact/i);

      expect(editButton).not.toBeInTheDocument();
    });
  });
});
