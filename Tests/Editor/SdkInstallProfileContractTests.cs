#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

public sealed class SdkInstallProfileContractTests
{
    #region Fields

    private const string ProfilePath = "Packages/com.actionfit.sdk.gameanalytics/Editor/SDKInstallProfile.json";
    private const string MatchingManifest = "{\n" +
        "  \"scopedRegistries\": [\n" +
        "    {\n" +
        "      \"name\": \"package.openupm.com\",\n" +
        "      \"url\": \"https://package.openupm.com\",\n" +
        "      \"scopes\": [\"com.gameanalytics\", \"com.google.external-dependency-manager\", \"jp.hadashikick.vcontainer\"]\n" +
        "    }\n" +
        "  ],\n" +
        "  \"dependencies\": {\n" +
        "    \"com.gameanalytics.sdk\": \"7.10.3\"\n" +
        "  }\n" +
        "}\n";

    private string _temporaryRoot;

    #endregion

    #region Initialization

    [SetUp]
    public void SetUp()
    {
        _temporaryRoot = Path.Combine(Path.GetTempPath(), "ActionFitGameAnalyticsSdkBridgeTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_temporaryRoot);
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(_temporaryRoot))
            Directory.Delete(_temporaryRoot, true);
    }

    #endregion

    #region Public Methods

    [Test]
    public void Profile_IsValidAndOwnedByThisBridgePackage()
    {
        ActionFitSdkInstallProfile profile = ActionFitSdkInstallApi.ReadProfile(ProfilePath);

        Assert.That(profile.BridgePackageId, Is.EqualTo("com.actionfit.sdk.gameanalytics"));
        Assert.That(profile.ProfileVersion, Is.EqualTo("7.10.3"));
        Assert.That(ActionFitSdkInstallProfileValidator.Validate(profile).Success, Is.True);
    }

    [Test]
    public void Profile_PinsOfficialRegistryPackageAndRequiredScopes()
    {
        ActionFitSdkInstallProfile profile = ActionFitSdkInstallApi.ReadProfile(ProfilePath);
        ActionFitSdkSourceDefinition source = profile.Sources.Single();
        ActionFitSdkScopedRegistryDefinition registry = profile.ScopedRegistries.Single();

        Assert.That(source.ResolveKind(), Is.EqualTo(ActionFitSdkSourceKind.Registry));
        Assert.That(source.Url, Is.EqualTo("https://package.openupm.com"));
        Assert.That(source.PackageId, Is.EqualTo("com.gameanalytics.sdk"));
        Assert.That(source.ImmutableVersion, Is.EqualTo("7.10.3"));
        Assert.That(registry.Scopes, Is.EquivalentTo(new[]
        {
            "com.gameanalytics",
            "com.google.external-dependency-manager",
        }));
    }

    [Test]
    public async Task ApplyAndRemove_MatchingInstallPreservesUserManagedEntries()
    {
        ActionFitSdkProjectContext context = CreateProject(MatchingManifest);
        ActionFitSdkInstallProfile profile = ActionFitSdkInstallApi.ReadProfile(ProfilePath);
        ActionFitSdkInstallPlan applyPlan = ActionFitSdkInstallApi.Plan(
            profile,
            new ActionFitSdkPlanRequest
            {
                Operation = ActionFitSdkInstallOperation.Apply,
                AdoptCompatible = true,
                TakeOwnershipOfCompatibleEntries = false,
            },
            context);

        Assert.That(applyPlan.Success, Is.True, applyPlan.Message);
        ActionFitSdkPlannedChange dependency = applyPlan.Changes.Single(item => item.Key == "com.gameanalytics.sdk");
        Assert.That(dependency.Action, Is.EqualTo(ActionFitSdkPlannedChangeAction.Adopt));
        Assert.That(dependency.OwnedByProfile, Is.False);

        ActionFitSdkExecutionResult applyResult = await ActionFitSdkInstallApi.ApplyAsync(applyPlan, applyPlan.PlanId);
        Assert.That(applyResult.Success, Is.True, applyResult.Message);

        ActionFitSdkInstallPlan removePlan = ActionFitSdkInstallApi.Plan(
            profile,
            new ActionFitSdkPlanRequest { Operation = ActionFitSdkInstallOperation.Remove },
            context);
        ActionFitSdkExecutionResult removeResult = await ActionFitSdkInstallApi.RemoveAsync(removePlan, removePlan.PlanId);

        Assert.That(removeResult.Success, Is.True, removeResult.Message);
        string manifest = File.ReadAllText(context.ManifestPath);
        Assert.That(manifest, Does.Contain("\"com.gameanalytics.sdk\": \"7.10.3\""));
        Assert.That(manifest, Does.Contain("\"com.gameanalytics\""));
        Assert.That(manifest, Does.Contain("\"com.google.external-dependency-manager\""));
        Assert.That(manifest, Does.Contain("\"jp.hadashikick.vcontainer\""));
    }

    [Test]
    public void Plan_LegacyAssetsInstallationBlocksRegistryApply()
    {
        ActionFitSdkProjectContext context = CreateProject(MatchingManifest);
        Directory.CreateDirectory(Path.Combine(context.ProjectRoot, "Assets", "GameAnalytics"));

        ActionFitSdkInstallPlan plan = ActionFitSdkInstallApi.Plan(
            ActionFitSdkInstallApi.ReadProfile(ProfilePath),
            new ActionFitSdkPlanRequest
            {
                Operation = ActionFitSdkInstallOperation.Apply,
                AdoptCompatible = true,
            },
            context);

        Assert.That(plan.Success, Is.False);
        Assert.That(plan.Code, Is.EqualTo("CONFLICT"));
        Assert.That(plan.Findings.Any(item => item.RuleId == "legacy-gameanalytics-assets"), Is.True);
    }

    #endregion

    #region Private Methods

    private ActionFitSdkProjectContext CreateProject(string manifest)
    {
        string projectRoot = Path.Combine(_temporaryRoot, "Project");
        Directory.CreateDirectory(Path.Combine(projectRoot, "Packages"));
        Directory.CreateDirectory(Path.Combine(projectRoot, "ProjectSettings"));
        Directory.CreateDirectory(Path.Combine(projectRoot, "UserSettings"));
        File.WriteAllText(Path.Combine(projectRoot, "Packages", "manifest.json"), manifest);
        return ActionFitSdkProjectContext.ForProjectRoot(projectRoot);
    }

    #endregion
}
#endif
