import test, { chromium, expect, firefox, webkit } from "@playwright/test";


test('Browser support demo', async () =>{
    const browserType = chromium; //other browser types webkit, firefox
    const browser = await browserType.launch();
    const page = await browser.newPage();

    await page.goto('https://localhost:7282/');
        
    //registration process
    await page.getByRole('link', { name: 'Register' }).click();
    await page.screenshot({ path: 'registration-blank-page.png'});
    await page.getByRole('textbox', { name: 'Username' }).click();
    await page.getByRole('textbox', { name: 'Username' }).fill('tester1234');
    await page.getByRole('textbox', { name: 'Username' }).press('Tab');
    await page.getByRole('textbox', { name: 'Email Address' }).fill('test2@test.com');
    await page.getByRole('textbox', { name: 'Password', exact: true }).click();
    await page.getByRole('textbox', { name: 'Password', exact: true }).fill('tester');
    await page.getByRole('textbox', { name: 'Password', exact: true }).press('Tab');
    await page.getByRole('textbox', { name: 'Confirm Password' }).fill('tester');
    await page.getByRole('button', { name: 'Register' }).click();
    //await expect(page.getByRole('paragraph')).toContainText('Choose how to login');

    await page.screenshot({ path: 'registration-page.png'});
    console.log('registration was run successfully.');

})