# Copilot Instructions

## Project Guidelines
- User prefers using `IEnumerable` over `IQueryable` in their codebase/service layer LINQ usage, favoring `IEnumerable`-style in-memory processing over `IQueryable`-driven database query composition. However, for operations that rely on EF change tracking, remain `IQueryable`-based instead of materializing to `IEnumerable` first.