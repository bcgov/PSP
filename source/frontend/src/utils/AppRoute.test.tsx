import { createMemoryHistory } from 'history';
import React from 'react';
import { createRoot } from 'react-dom/client';
import { Router } from 'react-router-dom';

import { act } from '@/utils/test-utils';

import AppRoute from './AppRoute';

const history = createMemoryHistory();

describe('App Route', () => {
  it('Document title is updated', async () => {
    const title = 'PIMS - Test Title';
    const container = document.createElement('div');
    document.body.appendChild(container);
    const root = createRoot(container);

    await act(async () => {
      root.render(
        <Router history={history}>
          <AppRoute customComponent={() => <p>Title Test Page</p>} title={title} />
        </Router>,
      );
    });

    expect(document.title).toBe('PIMS - Test Title');
  });
});
