using System.Globalization;
using System.Text.Json;

try
{
    var options = TagTaxOptions.FromArgs(args);

    if (args.Contains("--help") || args.Contains("-h"))
    {
        PrintHelp();
        return;
    }

    if (args.Contains("--demo"))
    {
        options = TagTaxOptions.Demo();
    }

    var result = TagTaxCalculator.Calculate(options);

    if (args.Contains("--json"))
    {
        PrintJson(result);
    }
    else
    {
        PrintReport(options, result);
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Goblin tripped over the input: {ex.Message}");
    Console.ResetColor();
    Environment.ExitCode = 1;
}

static void PrintReport(TagTaxOptions options, TagTaxResult result)
{
    Console.WriteLine();
    Console.WriteLine("======================================");
    Console.WriteLine("        TAG TAX GOBLIN REPORT");
    Console.WriteLine("======================================");
    Console.WriteLine();

    Console.WriteLine($"Plant tags:                 {options.PlantTags:N0}");
    Console.WriteLine($"Package tags:               {options.PackageTags:N0}");
    Console.WriteLine($"Transfers:                  {options.Transfers:N0}");
    Console.WriteLine($"Waste events:               {options.WasteEvents:N0}");
    Console.WriteLine($"Repackage/remediation:      {options.RepackageEvents:N0}");
    Console.WriteLine($"Tag cost:                   {Money(options.TagCost)}");
    Console.WriteLine($"Mistake rate:               {options.MistakeRatePercent:N2}%");
    Console.WriteLine($"Labor minutes/correction:   {options.LaborMinutesPerCorrection:N0}");
    Console.WriteLine($"Labor rate/hour:            {Money(options.LaborRatePerHour)}");
    Console.WriteLine();

    Console.WriteLine("--------- COST BREAKDOWN ---------");
    Console.WriteLine($"Total tags:                 {result.TotalTags:N0}");
    Console.WriteLine($"Direct tag cost:            {Money(result.DirectTagCost)}");
    Console.WriteLine($"Estimated corrections:      {result.EstimatedCorrections:N2}");
    Console.WriteLine($"Replacement tag cost:       {Money(result.ReplacementTagCost)}");
    Console.WriteLine($"Correction labor cost:      {Money(result.CorrectionLaborCost)}");
    Console.WriteLine($"Workflow friction cost:     {Money(result.WorkflowFrictionCost)}");
    Console.WriteLine();

    Console.WriteLine("------------- TOTAL -------------");
    Console.WriteLine($"Estimated tag-tax leakage:  {Money(result.TotalLeakage)}");
    Console.WriteLine($"Cost per package tag:       {Money(result.CostPerPackageTag)}");
    Console.WriteLine();

    Console.WriteLine($"Goblin rating:              {result.GoblinRating}");
    Console.WriteLine();

    Console.WriteLine(result.SpicySummary);
    Console.WriteLine();
}

static void PrintJson(TagTaxResult result)
{
    var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
    {
        WriteIndented = true
    });

    Console.WriteLine(json);
}

static string Money(decimal value)
{
    return value.ToString("C2", CultureInfo.GetCultureInfo("en-US"));
}

static void PrintHelp()
{
    Console.WriteLine("""
    tag-tax-goblin

    Estimate the real operational cost of cannabis compliance tags.

    Usage:
      dotnet run -- --plants 500 --packages 1200 --transfers 40 --waste 25 --repackages 60
      dotnet run -- --demo
      dotnet run -- --demo --json

    Options:
      --plants <number>          Plant tag count
      --packages <number>        Package tag count
      --transfers <number>       Transfer count
      --waste <number>           Waste/destruction event count
      --repackages <number>      Repackage/remediation count
      --tag-cost <decimal>       Cost per tag. Default: 0.25
      --mistake-rate <decimal>   Estimated correction/mistake rate %. Default: 3
      --labor-minutes <number>   Minutes per correction. Default: 12
      --labor-rate <decimal>     Labor rate per hour. Default: 25
      --json                     Output machine-readable JSON
      --demo                     Run with sample cannabis ops numbers
      --help                     Show help

    Example:
      dotnet run -- --plants 500 --packages 1200 --transfers 40 --waste 25 --repackages 60 --mistake-rate 3.5
    """);
}

public sealed class TagTaxOptions
{
    public int PlantTags { get; set; }
    public int PackageTags { get; set; }
    public int Transfers { get; set; }
    public int WasteEvents { get; set; }
    public int RepackageEvents { get; set; }

    public decimal TagCost { get; set; } = 0.25m;
    public decimal MistakeRatePercent { get; set; } = 3m;
    public int LaborMinutesPerCorrection { get; set; } = 12;
    public decimal LaborRatePerHour { get; set; } = 25m;

    public static TagTaxOptions Demo()
    {
        return new TagTaxOptions
        {
            PlantTags = 500,
            PackageTags = 1200,
            Transfers = 40,
            WasteEvents = 25,
            RepackageEvents = 60,
            TagCost = 0.25m,
            MistakeRatePercent = 3.5m,
            LaborMinutesPerCorrection = 12,
            LaborRatePerHour = 25m
        };
    }

    public static TagTaxOptions FromArgs(string[] args)
    {
        var options = new TagTaxOptions();

        for (var i = 0; i < args.Length; i++)
        {
            var key = args[i];

            string Next()
            {
                if (i + 1 >= args.Length)
                    throw new ArgumentException($"Missing value for {key}");

                return args[++i];
            }

            switch (key)
            {
                case "--plants":
                    options.PlantTags = ParseInt(Next(), key);
                    break;

                case "--packages":
                    options.PackageTags = ParseInt(Next(), key);
                    break;

                case "--transfers":
                    options.Transfers = ParseInt(Next(), key);
                    break;

                case "--waste":
                    options.WasteEvents = ParseInt(Next(), key);
                    break;

                case "--repackages":
                    options.RepackageEvents = ParseInt(Next(), key);
                    break;

                case "--tag-cost":
                    options.TagCost = ParseDecimal(Next(), key);
                    break;

                case "--mistake-rate":
                    options.MistakeRatePercent = ParseDecimal(Next(), key);
                    break;

                case "--labor-minutes":
                    options.LaborMinutesPerCorrection = ParseInt(Next(), key);
                    break;

                case "--labor-rate":
                    options.LaborRatePerHour = ParseDecimal(Next(), key);
                    break;

                case "--demo":
                case "--json":
                case "--help":
                case "-h":
                    break;

                default:
                    if (key.StartsWith("--", StringComparison.Ordinal))
                        throw new ArgumentException($"Unknown option: {key}");
                    break;
            }
        }

        return options;
    }

    private static int ParseInt(string value, string name)
    {
        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) || parsed < 0)
            throw new ArgumentException($"{name} must be a non-negative whole number.");

        return parsed;
    }

    private static decimal ParseDecimal(string value, string name)
    {
        if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed) || parsed < 0)
            throw new ArgumentException($"{name} must be a non-negative number.");

        return parsed;
    }
}

public sealed class TagTaxResult
{
    public int TotalTags { get; init; }
    public decimal DirectTagCost { get; init; }
    public decimal EstimatedCorrections { get; init; }
    public decimal ReplacementTagCost { get; init; }
    public decimal CorrectionLaborCost { get; init; }
    public decimal WorkflowFrictionCost { get; init; }
    public decimal TotalLeakage { get; init; }
    public decimal CostPerPackageTag { get; init; }
    public string GoblinRating { get; init; } = "";
    public string SpicySummary { get; init; } = "";
}

public static class TagTaxCalculator
{
    public static TagTaxResult Calculate(TagTaxOptions options)
    {
        var totalTags = options.PlantTags + options.PackageTags;

        var directTagCost = totalTags * options.TagCost;

        var estimatedCorrections = totalTags * (options.MistakeRatePercent / 100m);

        var replacementTagCost = estimatedCorrections * options.TagCost;

        var correctionLaborCost =
            estimatedCorrections *
            (options.LaborMinutesPerCorrection / 60m) *
            options.LaborRatePerHour;

        var workflowFrictionCost =
            (options.Transfers * 1.50m) +
            (options.WasteEvents * 2.00m) +
            (options.RepackageEvents * 3.00m);

        var totalLeakage =
            directTagCost +
            replacementTagCost +
            correctionLaborCost +
            workflowFrictionCost;

        var costPerPackageTag =
            options.PackageTags == 0
                ? 0
                : totalLeakage / options.PackageTags;

        return new TagTaxResult
        {
            TotalTags = totalTags,
            DirectTagCost = RoundMoney(directTagCost),
            EstimatedCorrections = Math.Round(estimatedCorrections, 2),
            ReplacementTagCost = RoundMoney(replacementTagCost),
            CorrectionLaborCost = RoundMoney(correctionLaborCost),
            WorkflowFrictionCost = RoundMoney(workflowFrictionCost),
            TotalLeakage = RoundMoney(totalLeakage),
            CostPerPackageTag = RoundMoney(costPerPackageTag),
            GoblinRating = GetGoblinRating(totalLeakage),
            SpicySummary = GetSpicySummary(totalLeakage)
        };
    }

    private static decimal RoundMoney(decimal value)
    {
        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }

    private static string GetGoblinRating(decimal totalLeakage)
    {
        return totalLeakage switch
        {
            < 50m => "Mild Gremlin",
            < 250m => "Margin Nibbler",
            < 1000m => "Compliance Goblin",
            < 5000m => "Tag-Tax Troll",
            _ => "Full Cryptid Event"
        };
    }

    private static string GetSpicySummary(decimal totalLeakage)
    {
        return totalLeakage switch
        {
            < 50m => "The goblin is present, but currently just chewing on a receipt printer cable.",
            < 250m => "This is not catastrophic, but it is absolutely not 'just a quarter bro.'",
            < 1000m => "The goblin has found the inventory office and is now touching spreadsheets.",
            < 5000m => "This is operational leakage with a compliance hat and a bad attitude.",
            _ => "The goblin has unionized, filed a variance, and is now billing you per package."
        };
    }
}