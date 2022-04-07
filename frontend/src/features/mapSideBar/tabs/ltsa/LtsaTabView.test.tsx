import { createMemoryHistory } from 'history';
import { IFormLease } from 'interfaces';
import { BillingInfo, LtsaOrders, OrderParent } from 'interfaces/ltsaModels';
import { render, RenderOptions } from 'utils/test-utils';

import LtsaTabView, { ILtsaTabViewProps } from './LtsaTabView';

const history = createMemoryHistory();

describe('LtsaTabView component', () => {
  const setup = (
    renderOptions: RenderOptions & ILtsaTabViewProps & { lease?: IFormLease } = {},
  ) => {
    // render component under test
    const component = render(
      <LtsaTabView
        ltsaData={renderOptions.ltsaData}
        ltsaRequestedOn={renderOptions.ltsaRequestedOn}
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

  it('renders a spinner when the ltsa data is loading', () => {
    const {
      component: { getByTestId },
    } = setup();

    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('renders as expected when provided valid ltsa data object and requested on datetime', () => {
    const { component } = setup({
      ltsaData: mockLtsaResponse,
      ltsaRequestedOn: new Date('06-Apr-2022 11:32 AM'),
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('does not throw an exception for an invalid ltsa data object', () => {
    const {
      component: { getByText },
    } = setup({ ltsaData: {} as LtsaOrders, ltsaRequestedOn: new Date() });
    expect(getByText('Title Details')).toBeVisible();
  });
});

export const mockLtsaResponse: LtsaOrders = {
  parcelInfo: {
    productType: OrderParent.ProductTypeEnum.ParcelInfo,
    status: OrderParent.StatusEnum.Processing,
    fileReference: 'folio',
    orderId: '0b5b0203-6125-4edc-98c5-704481c5e3c4',
    orderedProduct: {
      fieldedData: {
        status: 'ACTIVE',
        parcelIdentifier: '009-212-434',
        registeredTitlesCount: 1,
        pendingApplicationCount: 0,
        miscellaneousNotes: 'SRW PLAN 33784\nSRW PLAN 41816\nSRW PLAN 50351\nDF L24824\n',
        tombstone: {
          taxAuthorities: [
            {
              authorityName: 'Delta, City of',
            },
          ],
        },
        legalDescription: {
          fullLegalDescription:
            'LOT 4 EXCEPT: FIRSTLY: PART ON STATUTORY RIGHT OF WAY PLAN 30557;\nSECONDLY: PART ON STATUTORY RIGHT OF WAY PLAN 45999A;\nDISTRICT LOT 26 GROUP 2 AND SECTION 11 TOWNSHIP 6 NEW WESTMINSTER DISTRICT\nPLAN 24843',
          subdividedShortLegals: [
            {
              planNumber1: '00000000000000024843',
              townshipOrTownSite2: '',
              range3: '',
              block4: '',
              subdivision5: '',
              lotOrDistrictLotOrSubLot6: '00004',
              subdivision7: '',
              lotOrParcel8: '',
              section9: '',
              quadrant10: '',
              blockOrLot11: '',
              lotOrParcel12: '',
              parcelOrBlock13: '',
              concatShortLegal: 'S/24843/////4',
              marginalNotes: '*REM',
            },
            {
              planNumber1: '00000000000000024843',
              townshipOrTownSite2: '',
              range3: '',
              block4: '',
              subdivision5: '',
              lotOrDistrictLotOrSubLot6: '00004',
              subdivision7: '',
              lotOrParcel8: '',
              section9: '',
              quadrant10: '',
              blockOrLot11: '',
              lotOrParcel12: '',
              parcelOrBlock13: '',
              concatShortLegal: 'S/24843/////4',
              marginalNotes: 'PART HIGHWAY SRW PLAN 30557',
            },
            {
              planNumber1: '00000000000000024843',
              townshipOrTownSite2: '',
              range3: '',
              block4: '',
              subdivision5: '',
              lotOrDistrictLotOrSubLot6: '00004',
              subdivision7: '',
              lotOrParcel8: '',
              section9: '',
              quadrant10: '',
              blockOrLot11: '',
              lotOrParcel12: '',
              parcelOrBlock13: '',
              concatShortLegal: 'S/24843/////4',
              marginalNotes: 'PART HIGHWAY SRW PLAN 45999A',
            },
          ],
          unsubdividedShortLegals: [],
        },
        associatedPlans: [
          {
            planType: 'SUBDIVISIONPLAN',
            planNumber: 'NWP24843',
          },
          {
            planType: 'STATUTORYRIGHTOFWAYPLAN',
            planNumber: 'NWP30557',
          },
          {
            planType: 'STATUTORYRIGHTOFWAYPLAN',
            planNumber: 'NWP33784',
          },
          {
            planType: 'STATUTORYRIGHTOFWAYPLAN',
            planNumber: 'NWP50351',
          },
          {
            planType: 'PLAN',
            planNumber: 'NWP41618',
          },
        ],
      },
    },
  },
  titleOrders: [
    {
      productType: OrderParent.ProductTypeEnum.Title,
      status: OrderParent.StatusEnum.Processing,
      fileReference: 'folio',
      orderId: '5bbb421e-b7ba-42a0-a3d7-830fcd5c0941',
      billingInfo: {
        billingModel: BillingInfo.BillingModelEnum.PROV,
        productName: 'Searches',
        productCode: 'Search',
        feeExempted: true,
        productFee: 0,
        serviceCharge: 0,
        subtotalFee: 0,
        productFeeTax: 0,
        serviceChargeTax: 0,
        totalTax: 0,
        totalFee: 0,
      },
      orderedProduct: {
        fieldedData: {
          titleStatus: 'REGISTERED',
          titleIdentifier: {
            titleNumber: 'BL264819',
            landLandDistrict: 0,
          },
          tombstone: {
            applicationReceivedDate: '1997-08-18T18:30:00Z',
            enteredDate: '1997-08-18T18:46:34Z',
            titleRemarks: '',
            marketValueAmount: '',
            fromTitles: [
              {
                titleNumber: 'BK211498',
                landLandDistrict: 0,
              },
            ],
            natureOfTransfers: [
              {
                transferReason: 'SECTION 185 LAND TITLE ACT',
              },
            ],
          },
          ownershipGroups: [
            {
              interestFractionNumerator: '1',
              interestFractionDenominator: '1',
              ownershipRemarks: '',
              titleOwners: [
                {
                  lastNameOrCorpName1: 'BAYKEY',
                  givenName: 'MORRIS GRAVES',
                  incorporationNumber: '',
                  occupationDescription: 'RETIRED',
                  address: {
                    addressLine1: '',
                    addressLine2: '',
                    city: '',
                    province: 'BCBRITISHCOLUMBIA',
                    provinceName: 'BRITISH COLUMBIA',
                    country: 'CANADA',
                    postalCode: '',
                  },
                },
                {
                  lastNameOrCorpName1: 'BAYKEY',
                  givenName: 'KATHLEEN LOIS',
                  incorporationNumber: '',
                  occupationDescription: 'RETIRED',
                  address: {
                    addressLine1: '4431 SPANTON DRIVE',
                    addressLine2: '',
                    city: 'DELTA',
                    province: 'BCBRITISHCOLUMBIA',
                    provinceName: 'BRITISH COLUMBIA',
                    country: 'CANADA',
                    postalCode: 'V4K 2W4',
                  },
                },
                {
                  lastNameOrCorpName1: 'BAYKEY',
                  givenName: 'ROCK TYLER',
                  incorporationNumber: '',
                  occupationDescription: 'CARE TAKER',
                  address: {
                    addressLine1: '',
                    addressLine2: '',
                    city: '',
                    province: 'BCBRITISHCOLUMBIA',
                    provinceName: 'BRITISH COLUMBIA',
                    country: 'CANADA',
                    postalCode: '',
                  },
                },
                {
                  lastNameOrCorpName1: 'BAYKEY',
                  givenName: 'PATRICIA GAIL',
                  incorporationNumber: '',
                  occupationDescription: 'SELF EMPLOYED',
                  address: {
                    addressLine1: '4629 RIVER ROAD WEST',
                    addressLine2: '',
                    city: 'DELTA',
                    province: 'BCBRITISHCOLUMBIA',
                    provinceName: 'BRITISH COLUMBIA',
                    country: 'CANADA',
                    postalCode: 'V4K 1R9',
                  },
                },
                {
                  lastNameOrCorpName1: 'LIETZ',
                  givenName: 'CLIFFORD',
                  incorporationNumber: '',
                  occupationDescription: 'SHOP FOREMAN',
                  address: {
                    addressLine1: '',
                    addressLine2: '',
                    city: '',
                    province: 'BCBRITISHCOLUMBIA',
                    provinceName: 'BRITISH COLUMBIA',
                    country: 'CANADA',
                    postalCode: '',
                  },
                },
                {
                  lastNameOrCorpName1: 'LIETZ',
                  givenName: 'LEILAH MARIA',
                  incorporationNumber: '',
                  occupationDescription: 'HOMEMAKER',
                  address: {
                    addressLine1: '4369 - 41B STREET',
                    addressLine2: '',
                    city: 'DELTA',
                    province: 'BCBRITISHCOLUMBIA',
                    provinceName: 'BRITISH COLUMBIA',
                    country: 'CANADA',
                    postalCode: 'V4K 2K8',
                  },
                },
              ],
            },
          ],
          taxAuthorities: [
            {
              authorityName: 'Delta, City of',
            },
          ],
          descriptionsOfLand: [
            {
              parcelStatus: 'AActive',
              parcelIdentifier: '009-212-434',
              fullLegalDescription:
                'LOT 4 EXCEPT: FIRSTLY: PART ON STATUTORY RIGHT OF WAY PLAN 30557;\nSECONDLY: PART ON STATUTORY RIGHT OF WAY PLAN 45999A;\nDISTRICT LOT 26 GROUP 2 AND SECTION 11 TOWNSHIP 6 NEW WESTMINSTER DISTRICT\nPLAN 24843',
            },
          ],
          legalNotationsOnTitle: [
            {
              legalNotationNumber: 'CV1442411',
              status: 'ACTIVE',
              legalNotation: {
                originalLegalNotationNumber: 'CV1442411',
                legalNotationText:
                  'THIS CERTIFICATE OF TITLE MAY BE AFFECTED BY THE AGRICULTURAL LAND\nCOMMISSION ACT, SEE AGRICULTURAL LAND RESERVE PLAN NO. 2\nDEPOSITED JULY 30TH, 1974.',
              },
            },
            {
              legalNotationNumber: 'CV1442412',
              status: 'ACTIVE',
              legalNotation: {
                originalLegalNotationNumber: 'CV1442412',
                legalNotationText:
                  'ZONING REGULATION AND PLAN UNDER THE AERONAUTICS ACT (CANADA) FILED\n10.2.1981 UNDER NO. T17084 PLAN NO. 61216',
              },
            },
            {
              legalNotationNumber: 'CV1442414',
              status: 'ACTIVE',
              legalNotation: {
                originalLegalNotationNumber: 'CV1442414',
                legalNotationText:
                  'LAND HEREIN CHARGED UNDER THE\nMUNICIPAL ACT, PART 25\nD.F. D21890',
              },
            },
          ],
          chargesOnTitle: [
            {
              status: 'REGISTERED',
              interAlia: 'NO',
              chargeNumber: 'H107642',
              enteredDate: '1997-08-18T18:46:34Z',
              chargeRemarks: '',
              chargeRelease: {},
              charge: {
                chargeNumber: 'H107642',
                transactionType: 'STATUTORY RIGHT-OF-WAY',
                applicationReceivedDate: '1972-10-23T21:38:00Z',
                chargeOwnershipGroups: [
                  {
                    jointTenancyIndication: false,
                    interestFractionNumerator: '1',
                    interestFractionDenominator: '1',
                    ownershipRemarks: '',
                    chargeOwners: [
                      {
                        lastNameOrCorpName1: 'BRITISH COLUMBIA HYDRO AND POWER AUTHORITY',
                        incorporationNumber: '',
                      },
                    ],
                  },
                ],
                certificatesOfCharge: [],
                correctionsAltos1: [],
                corrections: [],
              },
            },
            {
              status: 'REGISTERED',
              interAlia: 'NO',
              chargeNumber: 'N35326',
              enteredDate: '1997-08-18T18:46:34Z',
              chargeRemarks: 'PLAN 50351\n',
              chargeRelease: {},
              charge: {
                chargeNumber: 'N35326',
                transactionType: 'STATUTORY RIGHT-OF-WAY',
                applicationReceivedDate: '1977-04-14T22:32:00Z',
                chargeOwnershipGroups: [
                  {
                    jointTenancyIndication: false,
                    interestFractionNumerator: '1',
                    interestFractionDenominator: '1',
                    ownershipRemarks: '',
                    chargeOwners: [
                      {
                        lastNameOrCorpName1: 'THE CORPORATION OF DELTA',
                        incorporationNumber: '',
                      },
                    ],
                  },
                ],
                certificatesOfCharge: [],
                correctionsAltos1: [],
                corrections: [],
              },
            },
            {
              status: 'REGISTERED',
              interAlia: 'NO',
              chargeNumber: 'BL158885',
              enteredDate: '1997-08-29T19:39:37Z',
              chargeRemarks: 'MODIFIED BY BR248606\n',
              chargeRelease: {},
              charge: {
                chargeNumber: 'BL158885',
                transactionType: 'MORTGAGE',
                applicationReceivedDate: '1997-05-07T16:51:00Z',
                chargeOwnershipGroups: [
                  {
                    jointTenancyIndication: false,
                    interestFractionNumerator: '1',
                    interestFractionDenominator: '1',
                    ownershipRemarks: '',
                    chargeOwners: [
                      {
                        lastNameOrCorpName1: 'RICHMOND SAVINGS CREDIT UNION',
                        incorporationNumber: '',
                      },
                    ],
                  },
                ],
                certificatesOfCharge: [],
                correctionsAltos1: [],
                corrections: [],
              },
            },
            {
              status: 'REGISTERED',
              interAlia: 'NO',
              chargeNumber: 'BR248606',
              enteredDate: '2001-09-27T16:28:50Z',
              chargeRemarks: 'MODIFICATION OF BL158885\n',
              chargeRelease: {},
              charge: {
                chargeNumber: 'BR248606',
                transactionType: 'MORTGAGE',
                applicationReceivedDate: '2001-09-25T18:11:00Z',
                chargeOwnershipGroups: [],
                certificatesOfCharge: [],
                correctionsAltos1: [],
                corrections: [],
              },
            },
            {
              status: 'REGISTERED',
              interAlia: 'NO',
              chargeNumber: 'CA5338545',
              enteredDate: '2016-07-21T22:16:04Z',
              chargeRemarks: '',
              chargeRelease: {},
              charge: {
                chargeNumber: 'CA5338545',
                transactionType: 'STATUTORY RIGHT OF WAY',
                applicationReceivedDate: '2016-07-13T21:01:28Z',
                chargeOwnershipGroups: [
                  {
                    jointTenancyIndication: false,
                    interestFractionNumerator: '1',
                    interestFractionDenominator: '1',
                    ownershipRemarks: '',
                    chargeOwners: [
                      {
                        lastNameOrCorpName1: 'ROGERS COMMUNICATIONS INC.',
                        incorporationNumber: 'BC0921753',
                      },
                    ],
                  },
                ],
                certificatesOfCharge: [],
                correctionsAltos1: [],
                corrections: [],
              },
            },
          ],
          duplicateCertificatesOfTitle: [
            {
              certificateIdentifier: {
                documentNumber: '12345',
                documentDistrictCode: 'test district code',
              },
              certificateDelivery: {
                certificateText: 'certificate text',
                intendedRecipientLastName: 'last',
                intendedRecipientGivenName: 'given',
              },
            },
          ],
          titleTransfersOrDispositions: [
            {
              disposition: 'disposition text',
              disposationDate: '2020-01-01',
              titleNumber: '54321',
              landLandDistrict: 'district',
            },
          ],
        },
      },
    },
  ],
};
