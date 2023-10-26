import {
  mockAcquisitionFileOwnersResponse,
  mockApiAcquisitionFileTeamOrganization,
  mockApiAcquisitionFileTeamPerson,
} from '@/mocks/acquisitionFiles.mock';
import { getMockApiDefaultCompensation } from '@/mocks/compensations.mock';
import { emptyApiInterestHolder } from '@/mocks/interestHolder.mock';
import { mockCompReqH120s } from '@/mocks/mockCompReqH120s.mock';
import { Api_AcquisitionFileOwner, Api_AcquisitionFileTeam } from '@/models/api/AcquisitionFile';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { Api_InterestHolder } from '@/models/api/InterestHolder';
import { formatMoney } from '@/utils';

import { Api_GenerateCompensationPayee } from './GenerateCompensationPayee';

describe('GenerateCompensationPayee tests', () => {
  it('can generate an empty payee without throwing an error', () => {
    const payee = new Api_GenerateCompensationPayee(null, []);
    expect(payee.name).toBe('');
    expect(payee.gst_number).toBe('');
    expect(payee.pre_tax_amount).toBe(formatMoney(0));
    expect(payee.tax_amount).toBe(formatMoney(0));
    expect(payee.total_amount).toBe(formatMoney(0));
    expect(payee.payment_in_trust).toBe(false);
  });

  it('adds h120 financial totals', () => {
    const payee = new Api_GenerateCompensationPayee(null, mockCompReqH120s());
    expect(payee.pre_tax_amount).toBe(formatMoney(1099));
    expect(payee.tax_amount).toBe(formatMoney(101));
    expect(payee.total_amount).toBe(formatMoney(1200));
  });

  it.each([
    ['JOHH DOE', mockAcquisitionFileOwnersResponse()[0]],
    ['FORTIS BC', mockAcquisitionFileOwnersResponse()[1]],
  ])(
    'can generate with owner payee: %s',
    (expectedName: string, owner: Api_AcquisitionFileOwner) => {
      const compensation: Api_CompensationRequisition = {
        ...getMockApiDefaultCompensation(),
        acquisitionOwner: owner,
      };
      const payee = new Api_GenerateCompensationPayee(compensation, []);
      expect(payee.name).toBe(expectedName);
    },
  );

  it.each([
    [
      'first last',
      {
        ...emptyApiInterestHolder,
        interestHolderId: 1,
        acquisitionFileId: 2,
        personId: 1,
        person: { id: 1, firstName: 'first', surname: 'last' },
      } as Api_InterestHolder,
    ],
    [
      'ABC Inc',
      {
        ...emptyApiInterestHolder,
        interestHolderId: 1,
        acquisitionFileId: 2,
        organizationId: 100,
        organization: { id: 100, name: 'ABC Inc' },
      } as Api_InterestHolder,
    ],
  ])(
    'can generate with interest holder payee: %s',
    (expectedName: string, ih: Api_InterestHolder) => {
      const compensation: Api_CompensationRequisition = {
        ...getMockApiDefaultCompensation(),
        interestHolder: ih,
      };
      const payee = new Api_GenerateCompensationPayee(compensation, []);
      expect(payee.name).toBe(expectedName);
    },
  );

  it.each([
    ['first last', mockApiAcquisitionFileTeamPerson()],
    ['ABC Inc', mockApiAcquisitionFileTeamOrganization()],
  ])(
    'can generate with acquisition team payee: %s',
    (expectedName: string, teamMember: Api_AcquisitionFileTeam) => {
      const compensation: Api_CompensationRequisition = {
        ...getMockApiDefaultCompensation(),
        acquisitionFileTeam: teamMember,
      };
      const payee = new Api_GenerateCompensationPayee(compensation, []);
      expect(payee.name).toBe(expectedName);
    },
  );

  it('can generate with legacy payee', () => {
    const compensation: Api_CompensationRequisition = {
      ...getMockApiDefaultCompensation(),
      legacyPayee: 'Chester Tester',
    };
    const payee = new Api_GenerateCompensationPayee(compensation, []);
    expect(payee.name).toBe('Chester Tester');
  });
});
