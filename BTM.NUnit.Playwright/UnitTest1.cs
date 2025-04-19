using Microsoft.Playwright;

namespace BTM.NUnit.Playwright
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
    {
        [Test]
        public async Task HomepageHasCorrectContent()
        {
            await Page.GotoAsync("https://localhost:7282/");

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync("Account Details - BTM.Account.Client");

            await Expect(Page.GetByText("Login")).ToBeVisibleAsync();

            await Page.ScreenshotAsync(new() { Path = "screenshot.png" });
        }

        [Test]
        public async Task LoginProcessWithUserAdmin()
        {
            await Page.GotoAsync("https://localhost:7282/");

            await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Username" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Username" }).FillAsync("admin");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Username" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("tester");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
            await Page.GetByText("BobSmith@email.com").ClickAsync();

            await Expect(Page.GetByText("BobSmith@email.com")).ToBeVisibleAsync();
            await Page.ScreenshotAsync(new() { Path = "screenshot.png" });
        }
    }
}
