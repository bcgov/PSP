import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';

import { AcquisitionTeamFormModel } from '../common/models';
import { AcquisitionForm } from './models';

describe('Create acquisition model tests', () => {
  describe('fromParentFileApi', () => {
    let parentFile: ApiGen_Concepts_AcquisitionFile;
    beforeEach(() => {
      parentFile = {
        id: 1,
        parentAcquisitionFileId: null,
        assignedDate: '2023-07-01',
        deliveryDate: '2023-07-31',
        acquisitionTypeCode: {
          id: 'SECTN6',
          description: 'Section 6 Expropriation',
          isDisabled: false,
          displayOrder: null,
        },
        productId: 16592,
        product: {
          id: 16592,
          code: '35478RC',
          description: 'RECOVERY COSTS',
          rowVersion: 1,
        },
        projectId: 1834,
        project: {
          id: 1834,
          projectStatusTypeCode: null,
          businessFunctionCode: null,
          costTypeCode: null,
          workActivityCode: null,
          regionCode: null,
          code: '5535478',
          description: 'PPS COMMOTION CR (BIG HOLE)',
          rowVersion: 2,
        },
        regionCode: {
          id: 1,
          description: 'South Coast Region',
          isDisabled: false,
          displayOrder: null,
        },
        acquisitionTeam: [
          {
            id: 6,
            acquisitionFileId: 116106,
            personId: null,
            person: null,
            organizationId: 57404,
            organization: {
              id: 57404,
              parentOrganizationId: null,
              regionCode: null,
              districtCode: null,
              organizationTypeCode: 'OTHER',
              identifierTypeCode: 'OTHINCORPNO',
              organizationIdentifier: null,
              name: '? ATTORNEY',
              alias: null,
              incorporationNumber: null,
              website: null,
              comment: null,
              isDisabled: false,
              contactMethods: [],
              organizationAddresses: [],
              organizationPersons: [],
              parentOrganization: null,
              rowVersion: 1,
            },
            primaryContactId: null,
            primaryContact: null,
            teamProfileTypeCode: 'MOTILAWYER',
            teamProfileType: {
              id: 'MOTILAWYER',
              description: 'MoTI Solicitor',
              isDisabled: false,
              displayOrder: null,
            },
            rowVersion: 12,
          },
        ],
      } as ApiGen_Concepts_AcquisitionFile;
    });

    it('handles a minimal object', () => {
      let model = AcquisitionForm.fromParentFileApi({
        id: 1,
        parentAcquisitionFileId: null,
      } as ApiGen_Concepts_AcquisitionFile);

      expect(model).toBeDefined();
      expect(model?.parentAcquisitionFileId).toBe(1);
    });

    it('copies information from parent file', () => {
      let model = AcquisitionForm.fromParentFileApi(parentFile);

      expect(model).toBeDefined();
      expect(model?.parentAcquisitionFileId).toBe(1);
      expect(model?.formattedProject).toBe('5535478 - PPS COMMOTION CR (BIG HOLE)');
      expect(model?.formattedProduct).toBe('35478RC RECOVERY COSTS');
      expect(model.project?.id).toBe(1834);
      expect(model.product).toBe('16592');
      expect(model.assignedDate).toBe('2023-07-01');
      expect(model.deliveryDate).toBe('2023-07-31');
      expect(model.acquisitionType).toBe('SECTN6');
      expect(model.region).toBe('1');
      expect(model.team).toHaveLength(1);
    });
  });

  describe('toApi', () => {
    it('converts form values to the api format', () => {
      const teamPerson = new AcquisitionTeamFormModel('testType', 0, {
        id: 'P1',
        personId: 1,
        person: null,
        summary: null,
        surname: null,
        firstName: null,
        middleNames: null,
        organizationName: null,
        email: null,
        mailingAddress: null,
        municipalityName: null,
        provinceState: null,
        isDisabled: false,
      });
      const teamOrg = new AcquisitionTeamFormModel('testType', 0, {
        id: 'O99',
        organizationId: 99,
        organization: null,
        summary: null,
        organizationName: null,
        email: null,
        mailingAddress: null,
        municipalityName: null,
        provinceState: null,
        isDisabled: false,
      });
      teamOrg.primaryContactId = '1';

      const model = new AcquisitionForm();
      model.parentAcquisitionFileId = 100;
      model.fileName = 'Test sub-file';
      model.assignedDate = '2023-07-01';
      model.region = '1';
      model.product = '1234';
      model.project = { id: 789, text: 'test project' };
      model.team = [teamPerson, teamOrg];

      const apiAcquisitionFile = model.toApi();
      // file details
      expect(apiAcquisitionFile.id).toBe(0);
      expect(apiAcquisitionFile.parentAcquisitionFileId).toBe(100);
      expect(apiAcquisitionFile.fileName).toBe('Test sub-file');
      expect(apiAcquisitionFile.assignedDate).toBe('2023-07-01');
      expect(apiAcquisitionFile.regionCode).toEqual(expect.objectContaining({ id: 1 }));
      // project + product
      expect(apiAcquisitionFile.projectId).toBe(789);
      expect(apiAcquisitionFile.productId).toBe(1234);
      expect(apiAcquisitionFile.project).toBeNull();
      expect(apiAcquisitionFile.product).toBeNull();
      // acquisition team
      expect(apiAcquisitionFile.acquisitionTeam).toHaveLength(2);
      expect(apiAcquisitionFile.acquisitionTeam[0]).toEqual(
        expect.objectContaining({
          id: 0,
          personId: 1,
          organizationId: null,
          primaryContactId: null,
          person: null,
          organization: null,
          primaryContact: null,
        } as ApiGen_Concepts_AcquisitionFileTeam),
      );
      expect(apiAcquisitionFile.acquisitionTeam[1]).toEqual(
        expect.objectContaining({
          id: 0,
          personId: null,
          organizationId: 99,
          primaryContactId: 1,
          person: null,
          organization: null,
          primaryContact: null,
        } as ApiGen_Concepts_AcquisitionFileTeam),
      );
    });
  });
});
