import { createMemoryHistory } from 'history';
import { Router } from 'react-router-dom/cjs/react-router-dom';
import renderer from 'react-test-renderer';

import { NotFoundPage } from './NotFoundPage';

const history = createMemoryHistory();

describe('NotFoundPage', () => {
  it('renders correctly', () => {
    const tree = renderer
      .create(
        <Router history={history}>
          <NotFoundPage />
        </Router>,
      )
      .toJSON();
    expect(tree).toMatchSnapshot();
  });
});
