# Contributing to Itch.io API

Thank you for your interest in contributing to the Itch.io API project! This document outlines the development workflow and guidelines.

## Development Workflow

### Branching Strategy

- **main**: The main production branch
- **feature/***: Feature development branches
- **bugfix/***: Bug fix branches
- **hotfix/***: Urgent production fixes

### Making Changes

1. **Fork and Clone**
   ```bash
   git clone https://github.com/yourusername/itch-io-api.git
   cd itch-io-api
   ```

2. **Create a Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make Your Changes**
   - Write clear, concise commit messages
   - Follow the existing code style
   - Add XML documentation comments for public APIs
   - Test your changes thoroughly

4. **Push Your Changes**
   ```bash
   git push -u origin feature/your-feature-name
   ```

5. **Create a Pull Request**
   - Provide a clear description of your changes
   - Reference any related issues
   - Ensure all CI checks pass

## Merge Policy

### Main Branch Protection

The `main` branch has the following protections configured:

#### Required Settings

- **Require pull request before merging**: ✅ Enabled
  - Require approvals: 1 (recommended)
  - Dismiss stale reviews: Enabled

- **Require status checks to pass**: ✅ Enabled
  - Require branches to be up to date: Enabled
  - Required checks:
    - `build-and-test`
    - `docker-build`

- **Require conversation resolution**: ✅ Enabled

- **Require linear history**: ✅ Enabled (enforces squash merging)

- **Do not allow bypassing the above settings**: ✅ Enabled

#### Merge Strategy

**All merges to `main` must use squash merge:**

- ✅ **Allow squash merging** (default and enforced)
- ❌ **Merge commits** (disabled)
- ❌ **Rebase merging** (disabled)

**Benefits of squash merging:**
- Keeps main branch history clean and linear
- Each feature appears as a single commit
- Easy to revert entire features
- Simplified git history

#### Automatic Branch Deletion

- **Automatically delete head branches**: ✅ Enabled
- Branches are automatically deleted after pull requests are merged
- Keeps repository clean and organized

### Repository Settings Configuration

To configure these settings in GitHub:

1. Go to **Settings** → **Branches**
2. Add branch protection rule for `main`
3. Configure the following options:

```yaml
Branch protection rule: main

☑ Require a pull request before merging
  ☑ Require approvals: 1
  ☑ Dismiss stale pull request approvals when new commits are pushed
  ☑ Require review from Code Owners (if CODEOWNERS file exists)

☑ Require status checks to pass before merging
  ☑ Require branches to be up to date before merging
  Required status checks:
    - build-and-test
    - docker-build

☑ Require conversation resolution before merging

☑ Require linear history

☑ Do not allow bypassing the above settings
```

4. Go to **Settings** → **General** → **Pull Requests**
   - ✅ Allow squash merging
   - ❌ Allow merge commits
   - ❌ Allow rebase merging
   - ✅ Automatically delete head branches

## Code Style Guidelines

### C# Conventions

- Follow [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use PascalCase for public members
- Use camelCase for private fields
- Use meaningful variable names
- Add XML documentation for public APIs

### Example

```csharp
/// <summary>
/// Gets a game by its unique identifier.
/// </summary>
/// <param name="id">The game identifier.</param>
/// <returns>The game object if found; otherwise, NotFound result.</returns>
[HttpGet("{id}")]
public ActionResult<Game> GetGame(int id)
{
    // Implementation
}
```

## Testing

- Write unit tests for new features
- Ensure all tests pass before submitting PR
- Run tests locally:
  ```bash
  dotnet test
  ```

## Building

- Verify your changes build successfully:
  ```bash
  dotnet build --configuration Release
  ```

## Documentation

- Update README.md if adding new features
- Add XML comments for public APIs
- Update this CONTRIBUTING.md if changing workflow

## Questions or Issues?

- Open an issue for bugs or feature requests
- Ask questions in pull request comments
- Follow the existing patterns in the codebase

Thank you for contributing! 🚀
