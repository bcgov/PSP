import React from 'react';

import { AccessRequestStatus } from '@/constants/accessStatus';
import { render } from '@/utils/test-utils';

import { IAccessRequestModel } from '../interfaces';
import { AccessRequestDetails } from './Details';

describe('Access request details', () => {
  it('Snapshot matches', () => {
    const request: IAccessRequestModel = {
      id: 1,
      businessIdentifierValue: 'idir/bceid',
      userId: 2,
      firstName: 'firstName',
      surname: 'surname',
      email: 'user@email.com',
      position: 'position 1',
      role: 'Role',
      note: 'Note here',
      status: AccessRequestStatus.Received,
    };
    const { asFragment } = render(
      <div>
        <AccessRequestDetails request={request} onClose={() => {}} />
      </div>,
    );
    expect(asFragment()).toMatchSnapshot();
  });
});
