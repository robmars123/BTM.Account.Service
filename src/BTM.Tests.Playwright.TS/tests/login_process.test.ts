import test, { chromium, expect, firefox, webkit } from "@playwright/test";


test('Browser support demo', async () =>{
    const browserType = chromium; //other browser types webkit, firefox
    const browser = await browserType.launch();
    const page = await browser.newPage();

    await page.goto('https://localhost:7282/');
        
    await page.getByRole('link', { name: 'Login' }).click();
    await page.getByRole('textbox', { name: 'Username' }).click();
    await page.getByRole('textbox', { name: 'Username' }).fill('admin');
    await page.getByRole('textbox', { name: 'Username' }).press('Tab');
    await page.getByRole('textbox', { name: 'Password' }).fill('tester');
    await page.getByRole('button', { name: 'Login' }).click();

    await expect(page.locator('#Email')).toContainText('BobSmith@email.com');
  
    //logs out
    await page.getByRole('link', { name: 'Log Out' }).click();
    await expect(page.getByRole('main')).toContainText('Account details not found. Please log in or create an account.');

    await page.screenshot({ path: 'login-page.png'});
    console.log('login was run successfully.');

})