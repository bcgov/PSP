import { ContactMethodTypes } from '@/constants/contactMethodType';
import { Api_Insurance } from '@/models/api/Insurance';
import { Api_Lease } from '@/models/api/Lease';
import { Api_LeaseTenant } from '@/models/api/LeaseTenant';
import { Api_Property } from '@/models/api/Property';

import { Api_GenerateLease } from './GenerateLease';

describe('GenerateLease tests', () => {
  it('generates an empty lease without throwing an error', () => {
    const lease = new Api_GenerateLease({} as Api_Lease, [], [], []);
    expect(lease.file_number).toBe('');
  });

  it('generates a lease with a commencement_date', () => {
    const lease = new Api_GenerateLease(
      { terms: [{ startDate: '01-01-2020' }, { startDate: '02-02-2022' }] } as Api_Lease,
      [],
      [],
      [],
    );
    expect(lease.commencement_date).toBe(`January 01, 2020`);
  });

  it('generates a lease with a full land_string', () => {
    const lease = new Api_GenerateLease(
      {
        properties: [
          {
            property: { pid: 1, landLegalDescription: 'test' } as Api_Property,
            leaseArea: 1,
            areaUnitType: { description: 'square feet' },
          },
        ],
      } as Api_Lease,
      [],
      [],
      [],
    );
    expect(lease.land_string).toBe(`Parcel Identifier: 000-000-001\ntest\n1 square feet`);
  });

  it('generates a lease with a default area unit', () => {
    const lease = new Api_GenerateLease(
      {
        properties: [
          {
            property: { pid: 1, landLegalDescription: 'test' } as Api_Property,
            leaseArea: 1,
            areaUnitType: null,
          },
        ],
      } as Api_Lease,
      [],
      [],
      [],
    );
    expect(lease.land_string).toBe(`Parcel Identifier: 000-000-001\ntest\n1 m²`);
  });
  it('generates a lease with no pid', () => {
    const lease = new Api_GenerateLease(
      {
        properties: [
          {
            property: { pid: undefined, landLegalDescription: 'test' } as Api_Property,
            leaseArea: 1,
            areaUnitType: { description: 'square feet' },
          },
        ],
      } as Api_Lease,
      [],
      [],
      [],
    );
    expect(lease.land_string).toBe(`Parcel Identifier: \ntest\n1 square feet`);
  });

  it('generates a lease with no legal description', () => {
    const lease = new Api_GenerateLease(
      {
        properties: [
          {
            property: { pid: undefined, landLegalDescription: 'test' } as Api_Property,
            leaseArea: 1,
            areaUnitType: { description: 'square feet' },
          },
        ],
      } as Api_Lease,
      [],
      [],
      [],
    );
    expect(lease.land_string).toBe(`Parcel Identifier: \ntest\n1 square feet`);
  });

  it('generates a lease with no lease area', () => {
    const lease = new Api_GenerateLease(
      {
        properties: [
          {
            property: { pid: undefined, landLegalDescription: 'test' } as Api_Property,
            areaUnitType: { description: 'square feet' },
          },
        ],
      } as Api_Lease,
      [],
      [],
      [],
    );
    expect(lease.land_string).toBe(`Parcel Identifier: \ntest\n0 square feet`);
  });

  it('generates a lease with no insurance information', () => {
    const lease = new Api_GenerateLease({} as Api_Lease, [], [], []);
    expect(lease.cgl_limit).toBe(`$0.00`);
    expect(lease.marine_liability_limit).toBe(`$0.00`);
    expect(lease.vehicle_liability_limit).toBe(`$0.00`);
    expect(lease.aircraft_liability_limit).toBe(`$0.00`);
  });

  it('generates a lease with cgl information', () => {
    const lease = new Api_GenerateLease(
      {} as Api_Lease,
      [{ insuranceType: { id: 'GENERAL' }, coverageLimit: 1 }] as Api_Insurance[],
      [],
      [],
    );
    expect(lease.cgl_limit).toBe(`$1.00`);
    expect(lease.marine_liability_limit).toBe(`$0.00`);
    expect(lease.vehicle_liability_limit).toBe(`$0.00`);
    expect(lease.aircraft_liability_limit).toBe(`$0.00`);
  });

  it('generates a lease with marine information', () => {
    const lease = new Api_GenerateLease(
      {} as Api_Lease,
      [{ insuranceType: { id: 'MARINE' }, coverageLimit: 1 }] as Api_Insurance[],
      [],
      [],
    );
    expect(lease.cgl_limit).toBe(`$0.00`);
    expect(lease.marine_liability_limit).toBe(`$1.00`);
    expect(lease.vehicle_liability_limit).toBe(`$0.00`);
    expect(lease.aircraft_liability_limit).toBe(`$0.00`);
  });

  it('generates a lease with aircraft information', () => {
    const lease = new Api_GenerateLease(
      {} as Api_Lease,
      [{ insuranceType: { id: 'AIRCRAFT' }, coverageLimit: 1 }] as Api_Insurance[],
      [],
      [],
    );
    expect(lease.cgl_limit).toBe(`$0.00`);
    expect(lease.marine_liability_limit).toBe(`$0.00`);
    expect(lease.vehicle_liability_limit).toBe(`$0.00`);
    expect(lease.aircraft_liability_limit).toBe(`$1.00`);
  });

  it('generates a lease with vehicle information', () => {
    const lease = new Api_GenerateLease(
      {} as Api_Lease,
      [{ insuranceType: { id: 'VEHICLE' }, coverageLimit: 1 }] as Api_Insurance[],
      [],
      [],
    );
    expect(lease.cgl_limit).toBe(`$0.00`);
    expect(lease.marine_liability_limit).toBe(`$0.00`);
    expect(lease.vehicle_liability_limit).toBe(`$1.00`);
    expect(lease.aircraft_liability_limit).toBe(`$0.00`);
  });

  it('generates a lease with no tenant information', () => {
    const lease = new Api_GenerateLease({} as Api_Lease, [], [], []);
    expect(lease.tenants).toHaveLength(0);
  });

  it('generates a lease whilst ignoring tenants not of tenant type TEN', () => {
    const lease = new Api_GenerateLease(
      {} as Api_Lease,
      [],
      [
        { person: { firstName: 'first', middleNames: 'middle', surname: 'last' } },
      ] as Api_LeaseTenant[],
      [],
    );
    expect(lease.tenants).toHaveLength(0);
  });

  it('generates a lease with a person tenant', () => {
    const lease = new Api_GenerateLease(
      {} as Api_Lease,
      [],
      [
        {
          person: { firstName: 'first', middleNames: 'middle', surname: 'last' },
          tenantTypeCode: { id: 'TEN' },
        },
      ] as Api_LeaseTenant[],
      [],
    );
    expect(lease.tenants).toHaveLength(1);
    expect(lease.tenants[0].name).toBe(`first middle last`);
  });

  it('generates a lease with an organization tenant', () => {
    const lease = new Api_GenerateLease(
      {} as Api_Lease,
      [],
      [{ organization: { name: 'test org' }, tenantTypeCode: { id: 'TEN' } }] as Api_LeaseTenant[],
      [],
    );
    expect(lease.tenants).toHaveLength(1);
    expect(lease.tenants[0].name).toBe(`test org (Inc. No. )`);
  });

  it('generates a lease with an organization tenant that has an incorp number', () => {
    const lease = new Api_GenerateLease(
      {} as Api_Lease,
      [],
      [
        {
          organization: { name: 'test org', incorporationNumber: '1234' },
          tenantTypeCode: { id: 'TEN' },
        },
      ] as Api_LeaseTenant[],
      [],
    );
    expect(lease.tenants).toHaveLength(1);
    expect(lease.tenants[0].name).toBe(`test org (Inc. No. 1234)`);
  });

  it('generates a lease with an organization primary contact tenant', () => {
    const lease = new Api_GenerateLease(
      {} as Api_Lease,
      [],
      [
        {
          organization: { name: 'test org' },
          primaryContact: { firstName: 'first', middleNames: 'middle', surname: 'last' },
          tenantTypeCode: { id: 'TEN' },
        },
      ] as Api_LeaseTenant[],
      [],
    );
    expect(lease.tenants).toHaveLength(1);
    expect(lease.tenants[0].primary_contact?.full_name_string).toBe(`first middle last`);
  });

  it('primary contact lease tenant email takes priority if present', () => {
    const lease = new Api_GenerateLease(
      {} as Api_Lease,
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
          tenantTypeCode: { id: 'TEN' },
        },
      ] as Api_LeaseTenant[],
      [],
    );
    expect(lease.tenants).toHaveLength(1);
    expect(lease.tenants[0].email).toBe(`testprimary@test.ca`);
  });
});
