import { BillingInfo, LtsaOrders, OrderParent } from '@/interfaces/ltsaModels';

export const getMockLtsaResponse: () => LtsaOrders = () => ({
  parcelInfo: {
    productType: OrderParent.ProductTypeEnum.ParcelInfo,
    status: OrderParent.StatusEnum.Processing,
    fileReference: 'folio',
    orderId: '6d888160-1fdc-46a4-a4a6-416a9187351e',
    orderedProduct: {
      fieldedData: {
        status: 'ACTIVE',
        parcelIdentifier: '005-359-821',
        registeredTitlesCount: 1,
        pendingApplicationCount: 0,
        miscellaneousNotes: 'P.P. 340\n',
        tombstone: {
          taxAuthorities: [
            {
              authorityName: 'Oak Bay, The Corporation of the District of',
            },
          ],
        },
        legalDescription: {
          fullLegalDescription: 'LOT 8, SECTION 69, VICTORIA DISTRICT, PLAN 396',
          subdividedShortLegals: [
            {
              planNumber1: '00000000000000000396',
              townshipOrTownSite2: '',
              range3: '',
              block4: '',
              subdivision5: '',
              lotOrDistrictLotOrSubLot6: '00008',
              subdivision7: '',
              lotOrParcel8: '',
              section9: '',
              quadrant10: '',
              blockOrLot11: '',
              lotOrParcel12: '',
              parcelOrBlock13: '',
              concatShortLegal: 'S/396/////8',
              marginalNotes: '*',
            },
          ],
          unsubdividedShortLegals: [],
        },
        associatedPlans: [
          {
            planType: 'POSTINGPLAN',
            planNumber: 'VIP340PP',
          },
          {
            planType: 'SUBDIVISIONPLAN',
            planNumber: 'VIP396',
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
      orderId: 'bbce4b54-a522-4271-a555-0d9edd8fc8b8',
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
            titleNumber: 'EL99999',
            landTitleDistrict: 'VICTORIA',
          },
          tombstone: {
            applicationReceivedDate: '1997-04-24T16:55:00Z',
            enteredDate: '1997-04-29T16:21:44Z',
            titleRemarks: '',
            marketValueAmount: '524000',
            fromTitles: [
              {
                titleNumber: 'EE142120',
                landTitleDistrict: 'VICTORIA',
              },
            ],
            natureOfTransfers: [
              {
                transferReason: 'FEE SIMPLE',
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
                  lastNameOrCorpName1: 'TESTER',
                  givenName: 'JOHN',
                  incorporationNumber: '',
                  occupationDescription: 'UBER DRIVER,',
                  address: {
                    addressLine1: '1660 TEST AVE,',
                    addressLine2: '',
                    city: 'VICTORIA',
                    province: 'BCBRITISHCOLUMBIA',
                    provinceName: 'BRITISH COLUMBIA',
                    country: 'CANADA',
                    postalCode: 'V8R 1Y5,',
                  },
                },
                {
                  lastNameOrCorpName1: 'TESTER',
                  givenName: 'CHESTER',
                  incorporationNumber: '',
                  occupationDescription: 'UBER DRIVER,',
                  address: {
                    addressLine1: '1660 TEST AVE,',
                    addressLine2: '',
                    city: 'VICTORIA',
                    province: 'BCBRITISHCOLUMBIA',
                    provinceName: 'BRITISH COLUMBIA',
                    country: 'CANADA',
                    postalCode: 'V8R 1Y5,',
                  },
                },
              ],
            },
          ],
          taxAuthorities: [
            {
              authorityName: 'Oak Bay, The Corporation of the District of',
            },
          ],
          descriptionsOfLand: [
            {
              parcelStatus: 'Active',
              parcelIdentifier: '005-359-821',
              fullLegalDescription: 'LOT 8, SECTION 69, VICTORIA DISTRICT, PLAN 396',
            },
          ],
          legalNotationsOnTitle: [],
          chargesOnTitle: [
            {
              status: 'REGISTERED',
              interAlia: 'NO',
              chargeNumber: 'CA1112222',
              enteredDate: '2012-05-19T16:18:55Z',
              chargeRemarks: '',
              chargeRelease: {},
              charge: {
                chargeNumber: 'CA1112222',
                transactionType: 'MORTGAGE',
                applicationReceivedDate: '2012-05-14T21:19:44Z',
                chargeOwnershipGroups: [
                  {
                    jointTenancyIndication: false,
                    interestFractionNumerator: '1',
                    interestFractionDenominator: '1',
                    ownershipRemarks: '',
                    chargeOwners: [
                      {
                        lastNameOrCorpName1: 'BANK OF MONTREAL',
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
          ],
          duplicateCertificatesOfTitle: [],
          titleTransfersOrDispositions: [],
        },
      },
    },
  ],
});
