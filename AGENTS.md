# FailOr Repository Guidance

## Types And Immutability

- Prefer immutable types and `readonly` members where practical.
- Use private constructors with static factory methods for externally created value objects.
- Do not expose mutable backing collections directly; return read-only views instead.

## Syntax And Style

- Prefer expression-bodied members when they keep the code easier to read.
- Prefer collection expressions like `[]` for empty collections instead of `Array.Empty<T>()` or `Enumerable.Empty<T>()` where the target type is clear.

## Tools

- Use the repository-local CSharpier tool via `dotnet csharpier`.
- Do not run `dotnet tool restore` proactively.
- Run `dotnet tool restore` only when a required `dotnet` tool is not available and is needed for the task.

## Verification

- Run `dotnet csharpier format` for files after code changes or for new files to keep formatting consistent.
- Run the relevant automated tests after changes to verify behavior and guard against regressions.
- Prefer running the full repository test command `dotnet test --solution fail-or.slnx` unless a narrower test scope is more appropriate for the change.

## Validation And Control Flow

- Use early-return guard clauses for validation and invalid states.

## Documentation

- Add XML documentation comments to all public functions, including documented exceptions and a succinct example.
- When changing public APIs or observable behavior, review the documentation in `docs/` and `README.md` in the same change.
- Update existing documentation when a function, signature, behavior, or usage pattern changes.
- Add or expand documentation in `docs/` and `README.md` when new functionality is introduced and is not already documented.
