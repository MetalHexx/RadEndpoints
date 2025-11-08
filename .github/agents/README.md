# RadEndpoints Expert Agent

This directory contains the configuration and instructions for the RadEndpoints Expert custom agent.

## Files

- **agent.yml** - Agent configuration file that defines permissions, capabilities, and metadata for GitHub Copilot's delegation system
- **rad-endpoints-agent.md** - Agent instructions containing expertise, guidelines, and working patterns

## Using the Agent

### Via CLI/Chat

To delegate work to the RadEndpoints Expert agent, use the `/delegate` command in GitHub Copilot CLI or chat:

```
/delegate to rad-endpoints-expert
```

Or when asking questions:
```
@rad-endpoints-expert how do I create a new endpoint with validation?
```

### What the Agent Does

The RadEndpoints Expert agent specializes in:

- Creating and modifying RadEndpoints following REPR pattern
- Implementing FluentValidation validators for request models
- Building IRadMapper implementations for entity transformations
- Writing integration tests using RadTestClientExtensions
- Writing unit tests using EndpointFactory
- Organizing code using vertical slice architecture
- Implementing proper error handling with RadProblem types
- Following RadEndpoints best practices and conventions

## Configuration

The `agent.yml` file defines:

- **Permissions**: What the agent can access (code, files, repository)
- **Capabilities**: What actions the agent can perform (editing, creating files, executing commands)
- **Tools**: Which tools the agent has access to (bash, view, edit, etc.)
- **Scope**: Which files the agent can work with (.cs, .csproj, .md, etc.)
- **Behavior**: Agent's expertise areas and preferred patterns

## Troubleshooting

### 401 Unauthorized Error

If you receive a 401 error when trying to delegate to the agent, ensure:

1. The `agent.yml` file exists in `.github/agents/`
2. The agent name in `agent.yml` matches the name used in delegation
3. Your GitHub token has the necessary permissions
4. The repository has GitHub Copilot enabled

### Agent Not Found

If the agent is not found:

1. Verify the `agent.yml` file is properly formatted
2. Check that the `instructions` field points to the correct file
3. Ensure the repository has been synced with GitHub Copilot

## Documentation References

- [RadEndpoints Documentation](../../README.md)
- [Copilot Instructions](../copilot-instructions.md)
- [Unit Testing Guide](../../RadEndpoints.Testing/UNIT-TESTING-GUIDE.md)
