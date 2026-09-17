import { Locator, Page } from '@playwright/test';
import { LayoutPage } from '../layout/layout.page'

export class ManagementCreatePage extends LayoutPage {
    readonly page: Page;

    readonly fileNameInput: Locator;
    readonly filePurposeInput: Locator;
    readonly noticeOfClaimReceivedDateInput: Locator;
    readonly noticeOfClaimCommentInput: Locator;

    readonly cancelButton: Locator;
    readonly confirmButton: Locator;

    constructor(page: Page) {
        super(page);
        this.page = page;

        this.fileNameInput = page.locator('#input-fileName');
        this.filePurposeInput = page.locator('#input-purposeTypeCode');
        this.noticeOfClaimReceivedDateInput = page.locator('[id="datepicker-noticeOfClaim.receivedDate"]',);
        this.noticeOfClaimCommentInput = page.locator('[id="input-noticeOfClaim.comment"]',);

        this.cancelButton = page.locator('button[data-testid="cancel-button"]');
        this.confirmButton = page.locator('button[data-testid="save-button"]');
    }

    async goto() {
        await this.page.goto('/mapview/sidebar/management/new', { waitUntil: 'domcontentloaded'});
    }

    async setFileNameInput(fileName: string){
        await this.fileNameInput.fill(fileName);
    }

    async setFilePurposeInput(filePurpose: string){
        await this.filePurposeInput.selectOption({ value: filePurpose });
    }

    async setNoticeOfClaimReceivedDateInput(receivedDate: string){
        await this.noticeOfClaimReceivedDateInput.fill(receivedDate);
    }

    async setNoticeOfClaimCommentInput(comment: string){
        await this.noticeOfClaimCommentInput.fill(comment);
    }

    async confirmButtonClick(){
        await this.confirmButton.click();
    }

    async cancelButtonClick(){
        await this.cancelButton.click();
    }
}