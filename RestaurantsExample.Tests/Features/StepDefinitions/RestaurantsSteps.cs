
using BddSharp;
using NUnit.Framework;
using RestaurantsExample.Models;
using TechTalk.SpecFlow;
using WatiN.Core;

namespace RestaurantsExample.Tests.Features.StepDefinitions
{
    [Binding]
    public class RestaurantsSteps
    {
        [Then("I click on \\.(.*)")]
        public void ThenWhenIClickElementWithClass(string elementClass)
        {
            Link continueLink = WebBrowser.Current.Link(Find.ByClass(elementClass));
            continueLink.WaitUntilExists();
            continueLink.Click();
        }

        [Then("I click on #(.*)")]
        public void ThenWhenIClickElementWithId(string elementId)
        {
            Link continueLink = WebBrowser.Current.Link(Find.ById(elementId));
            continueLink.WaitUntilExists();
            continueLink.Click();
        }

        [Given(@"I have loaded test data fixtures")]
        public void Given_I_have_loaded_fixtures()
        {
            
        }

        [When(@"I visit the restaurants page")]
        public void WhenIVisitTheRestaurantsPage()
        {
            WebBrowser.Current.GoTo(Nav.Host);
            WebBrowser.Current.WaitForComplete();            
        }

        [Then("I should see \"(.*)\" restaurant listed")]
        public void Then_I_should_see_restaurant_in_results(string fixtureName)
        {
            Restaurant fixture = SpecHelper.dataFixtures[fixtureName];
            Assert.True(WebBrowser.Current.Div("restaurants").Text.Contains(fixture.Name));
        }
    }
}
