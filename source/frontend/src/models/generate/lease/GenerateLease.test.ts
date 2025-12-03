import { ContactMethodTypes } from '@/constants/contactMethodType';
import { ApiGen_CodeTypes_LeaseInsuranceTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseInsuranceTypes';
import { ApiGen_CodeTypes_LeaseSecurityDepositTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseSecurityDepositTypes';
import { ApiGen_Concepts_Insurance } from '@/models/api/generated/ApiGen_Concepts_Insurance';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeasePeriod } from '@/models/api/generated/ApiGen_Concepts_LeasePeriod';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';
import { ApiGen_Concepts_SecurityDeposit } from '@/models/api/generated/ApiGen_Concepts_SecurityDeposit';

import { Api_GenerateLease } from './GenerateLease';

describe('GenerateLease tests', () => {
  it('generates an empty lease without throwing an error', () => {
    const lease = new Api_GenerateLease({} as ApiGen_Concepts_Lease, [], [], [], [], [], []);
    expect(lease.file_number).toBe('');
  });

  it('generates a lease with a commencement_date', () => {
    const lease = new Api_GenerateLease(
      { startDate: '2010-01-01' } as ApiGen_Concepts_Lease,
      [],
      [],
      [],
      [],
      [],
      [
        { startDate: '2020-01-01' } as ApiGen_Concepts_LeasePeriod,
        { startDate: '2022-02-02' } as ApiGen_Concepts_LeasePeriod,
      ],
    );
    expect(lease.commencement_date).toBe(`January 01, 2010`);
  });

  it('generates a lease with a full land_string', () => {
    const lease = new Api_GenerateLease(
      {} as ApiGen_Concepts_Lease,
      [],
      [],
      [],
      [],
      [
        {
          property: { pid: 1, landLegalDescription: 'test' } as ApiGen_Concepts_Property,
          leaseArea: 1,
          areaUnitType: { description: 'square feet' },
        } as ApiGen_Concepts_PropertyLease,
      ],
      [],
    );
    expect(lease.land_string).toBe(`Parcel Identifier: 000-000-001\ntest\n1 square feet`);
  });

  it('generates a lease with a default area unit', () => {
    const lease = new Api_GenerateLease(
      {} as ApiGen_Concepts_Lease,
      [],
      [],
      [],
      [],
      [
        {
          property: { pid: 1, landLegalDescription: 'test' } as ApiGen_Concepts_Property,
          leaseArea: 1,
          areaUnitType: null,
        } as ApiGen_Concepts_PropertyLease,
      ],
      [],
    );
    expect(lease.land_string).toBe(`Parcel Identifier: 000-000-001\ntest\n1 m²`);
  });

  it('generates a lease with no pid', () => {
    const lease = new Api_GenerateLease(
      {} as ApiGen_Concepts_Lease,
      [],
      [],
      [],
      [],
      [
        {
          property: { pid: null, landLegalDescription: 'test' } as ApiGen_Concepts_Property,
          leaseArea: 1,
          areaUnitType: { description: 'square feet' },
        } as ApiGen_Concepts_PropertyLease,
      ],
      [],
    );
    expect(lease.land_string).toBe(`Parcel Identifier: \ntest\n1 square feet`);
  });

  it('generates a lease with no legal description', () => {
    const lease = new Api_GenerateLease(
      {} as ApiGen_Concepts_Lease,
      [],
      [],
      [],
      [],
      [
        {
          property: { pid: 1, landLegalDescription: null } as ApiGen_Concepts_Property,
          leaseArea: 1,
          areaUnitType: { description: 'square feet' },
        } as ApiGen_Concepts_PropertyLease,
      ],
      [],
    );
    expect(lease.land_string).toBe(`Parcel Identifier: 000-000-001\n\n1 square feet`);
  });

  it('generates a lease with no lease area', () => {
    const lease = new Api_GenerateLease(
      {} as ApiGen_Concepts_Lease,
      [],
      [],
      [],
      [],
      [
        {
          property: { pid: null, landLegalDescription: 'test' } as ApiGen_Concepts_Property,
          areaUnitType: { description: 'square feet' },
        } as ApiGen_Concepts_PropertyLease,
      ],
      [],
    );
    expect(lease.land_string).toBe(`Parcel Identifier: \ntest\n0 square feet`);
  });

  it('generates a lease with primary arbitration city', () => {
    const lease = new Api_GenerateLease(
      { primaryArbitrationCity: 'VICTORIA' } as ApiGen_Concepts_Lease,
      [],
      [],
      [],
      [],
      [],
      [],
    );
    expect(lease.primary_arbitration_city).toBe(`VICTORIA`);
  });
  it('generates a lease with no insurance information', () => {
    const lease = new Api_GenerateLease({} as ApiGen_Concepts_Lease, [], [], [], [], [], []);
    expect(lease.cgl_limit).toBe(`$0.00`);
    expect(lease.marine_liability_limit).toBe(`$0.00`);
    expect(lease.vehicle_liability_limit).toBe(`$0.00`);
    expect(lease.aircraft_liability_limit).toBe(`$0.00`);
    expect(lease.uav_liability_limit).toBe(`$0.00`);
  });

  it.each([
    ['CGL', ApiGen_CodeTypes_LeaseInsuranceTypes.GENERAL, 'cgl_limit'],
    ['MARINE', ApiGen_CodeTypes_LeaseInsuranceTypes.MARINE, 'marine_liability_limit'],
    ['AIRCRAFT', ApiGen_CodeTypes_LeaseInsuranceTypes.AIRCRAFT, 'aircraft_liability_limit'],
    ['VEHICLE', ApiGen_CodeTypes_LeaseInsuranceTypes.VEHICLE, 'vehicle_liability_limit'],
    ['UAV DRONE', ApiGen_CodeTypes_LeaseInsuranceTypes.UAVDRONE, 'uav_liability_limit'],
  ])(
    'generates a lease with %s insurance information',
    (_, leaseInsuranceType: string, fieldName: string) => {
      const lease = new Api_GenerateLease(
        {} as ApiGen_Concepts_Lease,
        [
          { insuranceType: { id: leaseInsuranceType }, coverageLimit: 10000 },
        ] as ApiGen_Concepts_Insurance[],
        [],
        [],
        [],
        [],
        [],
      );

      expect(lease[fieldName]).toBe(`$10,000.00`);
    },
  );

  it('generates a lease with no security deposit information', () => {
    const lease = new Api_GenerateLease({} as ApiGen_Concepts_Lease, [], [], [], [], [], []);

    expect(lease.security_amount).toBe(`$0.00`);
  });

  it('generates a lease with single security deposit information', () => {
    const lease = new Api_GenerateLease(
      {} as ApiGen_Concepts_Lease,
      [],
      [],
      [],
      [
        {
          id: 6,
          leaseId: 56,
          description: 'test',
          amountPaid: 5000.0,
          depositDateOnly: '2024-04-10',
          depositType: {
            id: ApiGen_CodeTypes_LeaseSecurityDepositTypes.SECURITY,
            description: 'Security deposit',
            isDisabled: false,
            displayOrder: null,
          },
          otherTypeDescription: null,
          depositReturns: [],
          contactHolder: {
            id: 'P8',
            person: {
              id: 8,
              surname: 'Herrera',
              firstName: 'Eduardo',
              middleNames: null,
              nameSuffix: null,
              preferredName: null,
              birthDate: null,
              comment: null,
              addressComment: null,
              useOrganizationAddress: false,
              isDisabled: false,
              managementActivityId: null,
              contactMethods: [],
              personAddresses: [],
              personOrganizations: [],
              rowVersion: 1,
            },
            organization: null,
          },
          rowVersion: 1,
        },
      ] as ApiGen_Concepts_SecurityDeposit[],
      [],
      [],
    );

    expect(lease.security_amount).toBe(`$5,000.00`);
  });

  it('generates a lease with multiple security deposit information', () => {
    const lease = new Api_GenerateLease(
      {} as ApiGen_Concepts_Lease,
      [],
      [],
      [],
      [
        {
          id: 6,
          leaseId: 56,
          description: 'test',
          amountPaid: 5000.0,
          depositDateOnly: '2024-04-10',
          depositType: {
            id: ApiGen_CodeTypes_LeaseSecurityDepositTypes.SECURITY,
            description: 'Security deposit',
            isDisabled: false,
            displayOrder: null,
          },
          otherTypeDescription: null,
          depositReturns: [],
          contactHolder: {
            id: 'P8',
            person: {
              id: 8,
              surname: 'Herrera',
              firstName: 'Eduardo',
              middleNames: null,
              nameSuffix: null,
              preferredName: null,
              birthDate: null,
              comment: null,
              addressComment: null,
              useOrganizationAddress: false,
              isDisabled: false,
              managementActivityId: null,
              contactMethods: [],
              personAddresses: [],
              personOrganizations: [],
              rowVersion: 1,
            },
            organization: null,
          },
          rowVersion: 1,
        },
        {
          id: 7,
          leaseId: 56,
          description: 'test',
          amountPaid: 5000.0,
          depositDateOnly: '2024-04-15',
          depositType: {
            id: ApiGen_CodeTypes_LeaseSecurityDepositTypes.SECURITY,
            description: 'Security deposit',
            isDisabled: false,
            displayOrder: null,
          },
          otherTypeDescription: null,
          depositReturns: [],
          contactHolder: {
            id: 'P8',
            person: {
              id: 8,
              surname: 'Herrera',
              firstName: 'Eduardo',
              middleNames: null,
              nameSuffix: null,
              preferredName: null,
              birthDate: null,
              comment: null,
              addressComment: null,
              useOrganizationAddress: false,
              isDisabled: false,
              managementActivityId: null,
              contactMethods: [],
              personAddresses: [],
              personOrganizations: [],
              rowVersion: 1,
            },
            organization: null,
          },
          rowVersion: 1,
        },
      ] as ApiGen_Concepts_SecurityDeposit[],
      [],
      [],
    );

    expect(lease.security_amount).toBe(`$10,000.00`);
  });

  it('generates a lease with no tenant information', () => {
    const lease = new Api_GenerateLease({} as ApiGen_Concepts_Lease, [], [], [], [], [], []);
    expect(lease.tenants).toHaveLength(0);
  });

  it('generates a lease whilst ignoring tenants not of tenant type TEN', () => {
    const lease = new Api_GenerateLease(
      {} as ApiGen_Concepts_Lease,
      [],
      [
        { person: { firstName: 'first', middleNames: 'middle', surname: 'last' } },
      ] as ApiGen_Concepts_LeaseStakeholder[],
      [],
      [],
      [],
      [],
    );
    expect(lease.tenants).toHaveLength(0);
  });

  it('generates a lease with a person tenant', () => {
    const lease = new Api_GenerateLease(
      {} as ApiGen_Concepts_Lease,
      [],
      [
        {
          person: { firstName: 'first', middleNames: 'middle', surname: 'last' },
          stakeholderTypeCode: { id: 'TEN' },
        },
      ] as ApiGen_Concepts_LeaseStakeholder[],
      [],
      [],
      [],
      [],
    );
    expect(lease.tenants).toHaveLength(1);
    expect(lease.tenants[0].name).toBe(`first middle last`);
  });

  it('generates a lease with an organization tenant', () => {
    const lease = new Api_GenerateLease(
      {} as ApiGen_Concepts_Lease,
      [],
      [
        { organization: { name: 'test org' }, stakeholderTypeCode: { id: 'TEN' } },
      ] as ApiGen_Concepts_LeaseStakeholder[],
      [],
      [],
      [],
      [],
    );
    expect(lease.tenants).toHaveLength(1);
    expect(lease.tenants[0].name).toBe(`test org (Inc. No. )`);
  });

  it('generates a lease with an organization tenant that has an incorp number', () => {
    const lease = new Api_GenerateLease(
      {} as ApiGen_Concepts_Lease,
      [],
      [
        {
          organization: { name: 'test org', incorporationNumber: '1234' },
          stakeholderTypeCode: { id: 'TEN' },
        },
      ] as ApiGen_Concepts_LeaseStakeholder[],
      [],
      [],
      [],
      [],
    );
    expect(lease.tenants).toHaveLength(1);
    expect(lease.tenants[0].name).toBe(`test org (Inc. No. 1234)`);
  });

  it('generates a lease with an organization primary contact tenant', () => {
    const lease = new Api_GenerateLease(
      {} as ApiGen_Concepts_Lease,
      [],
      [
        {
          organization: { name: 'test org' },
          primaryContact: { firstName: 'first', middleNames: 'middle', surname: 'last' },
          stakeholderTypeCode: { id: 'TEN' },
        },
      ] as ApiGen_Concepts_LeaseStakeholder[],
      [],
      [],
      [],
      [],
    );
    expect(lease.tenants).toHaveLength(1);
    expect(lease.tenants[0].primary_contact?.full_name_string).toBe(`first middle last`);
  });

  it('primary contact lease tenant email takes priority if present', () => {
    const lease = new Api_GenerateLease(
      {} as ApiGen_Concepts_Lease,
      [],
      [
        {
          organization: {
            contactMethods: [
              { value: 'testOrg@test.ca', contactMethodType: { id: ContactMethodTypes.WorkEmail } },
            ],
          },
          primaryContact: {
            contactMethods: [
              {
                value: 'testprimary@test.ca',
                contactMethodType: { id: ContactMethodTypes.WorkEmail },
              },
            ],
          },
          stakeholderTypeCode: { id: 'TEN' },
        },
      ] as ApiGen_Concepts_LeaseStakeholder[],
      [],
      [],
      [],
      [],
    );
    expect(lease.tenants).toHaveLength(1);
    expect(lease.tenants[0].email).toBe(`testprimary@test.ca`);
  });
});
