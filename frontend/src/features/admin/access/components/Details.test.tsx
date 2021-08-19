import { AccessRequestStatus } from 'constants/accessStatus';
import { mount } from 'enzyme';
import React from 'react';

import { IAccessRequestModel } from '../interfaces';
import { AccessRequestDetails } from './Details';

describe('Access request details', () => {
  it('Snapshot matches', () => {
    const request: IAccessRequestModel = {
      id: 1,
      username: 'idir/bceid',
      userId: 2,
      firstName: 'firstName',
      lastName: 'lastName',
      email: 'user@email.com',
      position: 'position 1',
      role: 'Role',
      organization: 'Organization Name',
      note: 'Note here',
      status: AccessRequestStatus.OnHold,
    };
    const component = mount(
      <div>
        <AccessRequestDetails request={request} onClose={() => {}} />
      </div>,
    );
    expect(component).toMatchSnapshot();
  });
});
