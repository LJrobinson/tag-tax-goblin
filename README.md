# tag-tax-goblin

A tiny cannabis operations calculator for estimating the real cost of compliance tags.

Because "it's only $0.25 per tag" is how the goblin gets inside the building.

---

## The problem

Compliance tag costs are usually talked about like they are simple, clean, harmless little line items.

"Relax, bro. The tag is only a quarter."

Cool.

Now multiply that quarter across plant tags, package tags, repackaging, waste events, transfer workflows, correction labor, destroyed tags, inventory cleanup, and the human misery tax of fixing avoidable compliance spaghetti.

That is where the goblin lives.

`tag-tax-goblin` estimates the difference between the visible tag cost and the real operational drag created by tag-based cannabis compliance workflows.

---

## What it calculates

`tag-tax-goblin` estimates:

- Direct tag cost
- Estimated replacement tag cost
- Correction labor cost
- Workflow friction from transfers, waste, and repackaging
- Total estimated tag-tax leakage
- Cost per package tag
- Goblin severity rating

The goal is not to pretend this is a perfect accounting system.

The goal is to give operators a quick way to model the obvious thing everyone feels but nobody wants to put in a spreadsheet:

Tiny compliance costs become real margin leakage when they touch every part of the workflow.

---

## Example

```bash
dotnet run -- --plants 500 --packages 1200 --transfers 40 --waste 25 --repackages 60 --mistake-rate 3.5
```

---

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

---

## The punchline

The visible tag cost in this example is:

```bash
$425.00
```

The estimated real operational leakage is:

```bash
$1,027.38
```

That is the entire point.

The tag itself may only be a quarter.

The workflow attached to the tag is absolutely not.

---

## JSON output

```bash
dotnet run -- --demo --json
```

Example:

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

---

## Options

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

---

## Demo mode

Run the goblin with sample cannabis operations numbers:

```bash
dotnet run -- --demo
```

Run the same demo with JSON output:

```bash
dotnet run -- --demo --json
```

This is useful for quickly showing the difference between visible compliance cost and estimated operational leakage without having to enter a full scenario.

---

## Why this exists

Cannabis compliance costs are often discussed as simple per-unit fees.

Operators know better.

The real cost shows up in:

- Labor spent fixing packages
- Waste and destruction workflows
- Repackaging events
- Transfer friction
- Replacement tags
- Inventory cleanup
- Compliance-driven delays
- The quiet emotional damage of discovering one bad workflow touched 87 packages

This project is intentionally small, but it models a real operational pain point.

A $0.25 tag is not just a $0.25 tag when it lives inside a regulated inventory workflow.

It is a tiny paper gremlin with administrative privileges.

---

## Why this matters

Cannabis operators live inside systems where small compliance requirements compound quickly.

A single tag may be cheap.

A single mistake may be annoying.

A single correction may be manageable.

But at operational scale, those small costs stack into real leakage.

`tag-tax-goblin` gives that leakage a name, a number, and a goblin rating.

Because sometimes the best way to explain margin erosion is to show the monster eating it.

---

## Portfolio angle

This project demonstrates:

```bash
Cannabis operations domain knowledge
Compliance workflow modeling
CLI tool design
Cost modeling
Structured JSON output
Practical inventory analytics thinking
```

It is intentionally lightweight, but the business context is real:

- Cannabis inventory workflows are compliance-heavy
- Tagging systems create direct and indirect costs
- Operators need simple tools to explain hidden operational drag
- Small costs become meaningful when multiplied across plant, package, waste, transfer, and correction workflows

---

## Suggested use cases

Use this to estimate:

- Monthly tag cost exposure
- Package correction leakage
- Repackaging drag
- Waste event friction
- Transfer workflow overhead
- Cost per package tag after hidden labor is included

Or just run it because you too have looked at a compliance workflow and thought:

"Wow, this goblin has a badge."

---

## Disclaimer

This is not accounting software.

This is not compliance advice.

This is a lightweight modeling tool for estimating operational leakage around tag-based cannabis workflows.

Use real internal numbers if you want better estimates.

Use the default numbers if you just want to summon the goblin.