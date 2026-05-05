# tag-tax-goblin

A tiny cannabis operations calculator for estimating the real cost of compliance tags.

Everyone knows the visible tag cost.

This estimates the uglier number: replacement tags, correction labor, repackaging friction, waste events, transfers, and operational leakage.

Because "it's only $0.25 per tag" is how the goblin gets inside the building.

## What it calculates

- Direct tag cost
- Estimated replacement tag cost
- Correction labor cost
- Workflow friction from transfers, waste, and repackaging
- Total estimated tag-tax leakage
- Cost per package tag
- Goblin severity rating

## Example

```bash
dotnet run -- --plants 500 --packages 1200 --transfers 40 --waste 25 --repackages 60 --mistake-rate 3.5
```

## Example output

```bash
======================================
        TAG TAX GOBLIN REPORT
======================================

Plant tags:                 500
Package tags:               1,200
Transfers:                  40
Waste events:               25
Repackage/remediation:      60
Tag cost:                   $0.25
Mistake rate:               3.50%
Labor minutes/correction:   12
Labor rate/hour:            $25.00

--------- COST BREAKDOWN ---------
Total tags:                 1,700
Direct tag cost:            $425.00
Estimated corrections:      59.50
Replacement tag cost:       $14.88
Correction labor cost:      $297.50
Workflow friction cost:     $290.00

------------- TOTAL -------------
Estimated tag-tax leakage:  $1,027.38
Cost per package tag:       $0.86

Goblin rating:              Tag-Tax Troll

This is operational leakage with a compliance hat and a bad attitude.
```

## JSON output

```bash
dotnet run -- --demo --json
```

Example
```json
{
  "TotalTags": 1700,
  "DirectTagCost": 425.00,
  "EstimatedCorrections": 59.50,
  "ReplacementTagCost": 14.88,
  "CorrectionLaborCost": 297.50,
  "WorkflowFrictionCost": 290.00,
  "TotalLeakage": 1027.38,
  "CostPerPackageTag": 0.86,
  "GoblinRating": "Tag-Tax Troll",
  "SpicySummary": "This is operational leakage with a compliance hat and a bad attitude."
}
```

Options

```bash
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
```

## Why this exists

Cannabis compliance costs are often discussed as simple per-unit fees, but operators feel the cost through labor, corrections, delays, package handling, and inventory cleanup.

This project is intentionally small, but it models a real operational pain point: tiny compliance costs become real margin leakage when multiplied across plant, package, waste, transfer, and correction workflows.

Portfolio angle

This project demonstrates:

Cannabis operations domain knowledge
Compliance workflow modeling
CLI tool design
Cost modeling
Structured JSON output
Practical inventory analytics thinking