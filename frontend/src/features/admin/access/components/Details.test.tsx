import { AccessRequestStatus } from 'constants/accessStatus';
import { mount } from 'enzyme';
import React from 'react';

import { IAccessRequestModel } from '../interfaces';
import { AccessRequestDetails } from './Details';

describe('Access request details', () => {
  it('Snapshot matches', () => {
    const request: IAccessRequestModel = {
      id: 1,
      businessIdentifier: 'idir/bceid',
      userId: 2,
      firstName: 'firstName',
      surname: 'surname',
      email: 'user@email.com',
      position: 'position 1',
      role: 'Role',
      organization: 'Organization Name',
      note: 'Note here',
      status: AccessRequestStatus.Received,
    };
    const component = mount(
      <div>
        <AccessRequestDetails request={request} onClose={() => {}} />
      </div>,
    );
    expect(component).toMatchSnapshot();
  });
});
