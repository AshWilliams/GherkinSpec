using GivePenny.GherkinSpec.Model.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GivePenny.GherkinSpec.Model.UnitTests
{
    [TestClass]
    public class ParserShould
    {
        // TODO Ignore leading blank lines

        [TestMethod]
        public void ReadFeatureTitleAndIgnorePaddingWhitespace()
        {
            var text = "  Feature:  Adding things up ";
            var parser = new Parser();
            var feature = parser.Parse(text);
            Assert.AreEqual("Adding things up", feature.Title);
        }

        [TestMethod]
        public void ReadFeatureMotivationAfterFeatureTitleUntilScenarioLine()
        {
            var text = Resources.GetString("FeatureMotivationScenarioTitle.feature");
            var parser = new Parser();
            var feature = parser.Parse(text);
            Assert.AreEqual(@"As a developer
I want things to work
So that I can have more fun", feature.Narrative);
        }

        [TestMethod]
        public void ReadScenarioTitle()
        {
            var text = Resources.GetString("ScenarioTitle.feature");
            var parser = new Parser();
            var feature = parser.Parse(text);
            Assert.AreEqual(1, feature.Scenarios.Count());
            Assert.AreEqual(@"This is a scenario", feature.Scenarios.First().Title);
        }

        [TestMethod]
        public void ReadExampleAsAScenario()
        {
            var text = Resources.GetString("Example.feature");
            var parser = new Parser();
            var feature = parser.Parse(text);
            Assert.AreEqual(1, feature.Scenarios.Count());
            Assert.AreEqual(@"This is a scenario", feature.Scenarios.First().Title);
        }

        [TestMethod]
        public void SkipCommentLines()
        {
            var text = Resources.GetString("Comment.feature");
            var parser = new Parser();
            var feature = parser.Parse(text);
            Assert.IsTrue(string.IsNullOrEmpty(feature.Narrative));
        }

        [TestMethod]
        public void SkipWhitespaceLines()
        {
            var text = Resources.GetString("Whitespace.feature");
            var parser = new Parser();
            var feature = parser.Parse(text);
            Assert.AreEqual("Motivation", feature.Narrative);
        }

        [TestMethod]
        public void ReadMultipleScenarioTitles()
        {
            var text = Resources.GetString("ScenarioTitles.feature");
            var parser = new Parser();
            var feature = parser.Parse(text);
            Assert.AreEqual(3, feature.Scenarios.Count());
            Assert.AreEqual(@"This is a scenario", feature.Scenarios.First().Title);
            Assert.AreEqual(@"This is a 2nd scenario", feature.Scenarios.Second().Title);
            Assert.AreEqual(@"This is a 3rd scenario", feature.Scenarios.Third().Title);
        }

        [TestMethod]
        public void StoreScenarioStartingLineNumber()
        {
            var text = Resources.GetString("ScenarioTitles.feature");
            var parser = new Parser();
            var feature = parser.Parse(text);
            Assert.AreEqual(2, feature.Scenarios.First().StartingLineNumber);
        }

        [TestMethod]
        public void ReadScenarioSteps()
        {
            var text = Resources.GetString("ScenarioSteps.feature");
            var parser = new Parser();
            var feature = parser.Parse(text);

            Assert.AreEqual(2, feature.Scenarios.Count());
            Assert.AreEqual(4, feature.Scenarios.First().Steps.Count);
            Assert.AreEqual(3, feature.Scenarios.Second().Steps.Count);
        }

        [TestMethod]
        public void TreatAndAsTheLastDefinedGivenWhenOrThen()
        {
            var text = Resources.GetString("ScenarioSteps.feature");
            var parser = new Parser();
            var feature = parser.Parse(text);

            Assert.IsInstanceOfType(
                feature.Scenarios.First().Steps.Second(),
                typeof(GivenStep));
        }

        [TestMethod]
        public void TreatButAsTheLastDefinedGivenWhenOrThen()
        {
            var text = Resources.GetString("ScenarioSteps.feature");
            var parser = new Parser();
            var feature = parser.Parse(text);

            Assert.IsInstanceOfType(
                feature.Scenarios.Second().Steps.Third(),
                typeof(ThenStep));
        }

        [TestMethod]
        public void ReadFeatureBackground()
        {
            var text = Resources.GetString("Background.feature");
            var parser = new Parser();
            var feature = parser.Parse(text);

            Assert.AreEqual(2, feature.Background.Steps.Count);
            Assert.AreEqual(@"Given a first step", feature.Background.Steps.First().Title);
            Assert.AreEqual(@"And another", feature.Background.Steps.Second().Title);
        }
    }
}
