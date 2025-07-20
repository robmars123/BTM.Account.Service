import { test, expect } from '@playwright/test';

test('can login', async ({ page }) => {
  await page.goto('https://localhost:7282/');

  await page.getByRole('link', { name: 'Login' }).click();
  await page.getByRole('textbox', { name: 'Username' }).click();
  await page.getByRole('textbox', { name: 'Username' }).fill('admin');
  await page.getByRole('textbox', { name: 'Username' }).press('Tab');
  await page.getByRole('textbox', { name: 'Password' }).fill('tester');
  await page.getByRole('button', { name: 'Login' }).click();
  await page.goto('https://localhost:7282/');
  await expect(page.locator('#Email')).toContainText('BobSmith@email.com');

  console.log('test was run successfully.');
});

