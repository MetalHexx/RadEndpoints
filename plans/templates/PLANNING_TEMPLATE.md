# High-Level Feature Planning Template

**Project Overview**: [One paragraph describing the feature being planned and its purpose]

**Standards Documentation** (Reference as applicable):

- **Coding Standards**: [CODING_STANDARDS.md](./CODING_STANDARDS.md) (if applicable)
- **Testing Standards**: [TESTING_STANDARDS.md](./TESTING_STANDARDS.md) (if applicable)
- **Architecture Guidelines**: [ARCHITECTURE.md](./ARCHITECTURE.md) (if applicable)
- **API Standards**: [API_STANDARDS.md](./API_STANDARDS.md) (if applicable)
- **Project-Specific Guidelines**: Link to any project-specific documentation

---

## 📝 Planning Document Guidelines

**Purpose**: This template creates non-technical, behavior-focused planning documents that describe **what** we're building and **why**, not **how**.

### Writing Principles

**Keep It High-Level:**

- Focus on user behaviors, system behaviors, and business outcomes
- Avoid technical implementation details (no specific function names, file paths, or code snippets)
- Refer to concepts generically (e.g., "state management", "UI components", "navigation controls")
- Think about what the user experiences, not what the code does

**Examples of Good vs Bad Descriptions:**

✅ **Good** (High-level, behavior-focused):

- "Create endpoint models that properly represent request and response structures"
- "Add response helper methods for different HTTP status codes"
- "Track and test endpoint behavior across different scenarios"

❌ **Bad** (Too technical, implementation-focused):

- "Create ExampleEndpoint.cs with TRequest and TResponse generic parameters"
- "Implement HandleAsync() method in endpoint class using base.SendAsync()"
- "Add ValidationRules: Dictionary<string, ValidationRule[]> to state interface"

**Generic Concept Language:**

- State structure, state management, data structure
- View components, UI components, display components
- Actions, operations, behaviors
- Selectors, queries, data retrieval
- Navigation controls, user controls, interaction elements
- Integration points, coordination, communication

**Focus Areas:**

- **User Value**: What benefit does this provide to users?
- **Behaviors**: What happens when users interact with the feature?
- **States**: What different modes or conditions exist?
- **Integration**: How does this work with existing features?
- **Flows**: What are the step-by-step user journeys?

### Document Structure

Each planning document should include:

1. **Project Objective**: Clear statement of user value and feature purpose
2. **Implementation Phases**: Break complex work into independently valuable phases
3. **Architecture Overview**: High-level design decisions and integration points
4. **Testing Strategy**: Categories of tests needed (unit, integration, E2E)
5. **Given-When-Then Scenarios**: Comprehensive behavioral scenarios
6. **Success Criteria**: Measurable outcomes that define completion
7. **Open Questions**: Decisions to be made during implementation (organized by phase)

### Phase Structure Best Practices

**Independent Value:**

- Each phase should deliver something demonstrable and valuable on its own
- Phases build on each other without requiring future phases to be useful
- Early phases validate core concepts before adding complexity

**Phase Components:**

- **Objective**: What this phase achieves
- **Key Deliverables**: Specific outcomes (checkbox format for tracking)
- **High-Level Tasks**: Major activities needed to complete the phase
- **Open Questions**: Decisions specific to this phase that need resolution

---

## 🎯 Project Objective

[2-3 paragraphs describing what this feature aims to achieve]

**First Paragraph**: What is the feature and what problem does it solve?

**Second Paragraph**: How will users interact with it and what value do they get?

**Third Paragraph** (optional): Any broader system benefits or architectural improvements.

**Example:**

> Implement comprehensive endpoint testing for all RadEndpoints response helper methods (SendNotFound, SendConflict, etc.). Create test endpoints that demonstrate each response type and integration tests that verify correct serialization and deserialization behavior. This ensures developers understand how to properly test endpoints using different response types and provides reference implementations.
>
> **Developer Value**: Developers gain confidence in using RadEndpoints response helpers with clear examples of how to test each type. The test suite validates that the testing library correctly handles different response formats, preventing integration test failures due to type mismatches.

---

## 📋 Implementation Phases

<details open>
<summary><h3>Phase 1: [Descriptive Phase Title]</h3></summary>

### Objective

[2-3 sentences describing what this phase delivers and why it's valuable on its own]

### Key Deliverables

- [ ] Deliverable A (specific, measurable outcome)
- [ ] Deliverable B (specific, measurable outcome)
- [ ] Deliverable C (specific, measurable outcome)
- [ ] Deliverable D (specific, measurable outcome)

### High-Level Tasks

1. **Task Name**: Brief description of what needs to be accomplished
2. **Task Name**: Brief description of what needs to be accomplished
3. **Task Name**: Brief description of what needs to be accomplished
4. **Task Name**: Brief description of what needs to be accomplished
5. **Task Name**: Brief description of what needs to be accomplished

### Open Questions for Phase 1

- **Question Category**: Specific decision that needs to be made with context
- **Question Category**: Specific decision that needs to be made with context

**Example:**

> - **Response Type Documentation**: Should each Send* method have inline documentation specifying return type, or should this be in a central documentation file?
> - **Test Coverage Scope**: What is the minimum set of response types that need comprehensive test coverage before considering this feature complete?

</details>

---

<details open>
<summary><h3>Phase 2: [Descriptive Phase Title]</h3></summary>

### Objective

[2-3 sentences describing what this phase delivers and how it builds on Phase 1]

### Key Deliverables

- [ ] Deliverable A
- [ ] Deliverable B
- [ ] Deliverable C
- [ ] Deliverable D

### High-Level Tasks

1. **Task Name**: Brief description
2. **Task Name**: Brief description
3. **Task Name**: Brief description
4. **Task Name**: Brief description

### Open Questions for Phase 2

- **Question Category**: Specific decision needed
- **Question Category**: Specific decision needed

</details>

---

<details open>
<summary><h3>Phase N: [Final Phase Title]</h3></summary>

### Objective

[2-3 sentences describing final integration and polish]

### Key Deliverables

- [ ] Deliverable A
- [ ] Deliverable B
- [ ] Deliverable C

### High-Level Tasks

1. **Task Name**: Brief description
2. **Task Name**: Brief description
3. **Task Name**: Brief description

### Open Questions for Phase N

- **Question Category**: Specific decision needed

</details>

---

<details open>
<summary><h2>🏗️ Architecture Overview</h2></summary>

### Key Design Decisions

- **Decision Name**: Explanation of the approach and rationale (2-3 sentences describing the "why" behind the decision)
- **Decision Name**: Explanation of the approach and rationale
- **Decision Name**: Explanation of the approach and rationale
- **Decision Name**: Explanation of the approach and rationale

**Example:**

> - **Consistent Testing Pattern**: Use consistent approach for testing all endpoint response types, making it easy for developers to follow the pattern
> - **Type Safety**: Ensure integration tests specify correct response types, preventing runtime serialization errors

### Integration Points

- **System/Component Name**: How this feature integrates with the existing system, what it depends on, and how they communicate
- **System/Component Name**: How this feature integrates with the existing system
- **System/Component Name**: How this feature integrates with the existing system
- **System/Component Name**: How this feature integrates with the existing system

**Example:**

> - **Endpoint Response Helpers**: Response methods integrate with ASP.NET Core's IResult system, using TypedResults for type-safe responses
> - **Testing Library**: Integration test extensions work with RadEndpoints' routing system to enable "routeless" testing
> - **Validation Pipeline**: FluentValidation integrates seamlessly with endpoint request validation

</details>

---

<details open>
<summary><h2>🧪 Testing Strategy</h2></summary>

### Unit Tests

- [ ] Test category or behavior area A
- [ ] Test category or behavior area B
- [ ] Test category or behavior area C
- [ ] Test category or behavior area D
- [ ] Test category or behavior area E

**Example:**

> - [ ] Endpoint response type behavior and serialization
> - [ ] Request model binding and validation
> - [ ] Mapper functionality for entity transformations
> - [ ] Error handling and problem details generation
> - [ ] Integration with FluentValidation

### E2E Tests

- [ ] Multi-component flow or interaction A
- [ ] Multi-component flow or interaction B
- [ ] Multi-component flow or interaction C
- [ ] Multi-component flow or interaction D

**Example:**

> - [ ] Endpoint request/response flow across different HTTP methods
> - [ ] Validation integration with endpoint pipeline
> - [ ] Response serialization for different response types
> - [ ] Error handling and problem details formatting

### E2E Tests

- [ ] Complete user scenario A
- [ ] Complete user scenario B
- [ ] Complete user scenario C
- [ ] Edge case or error scenario D

**Example:**

> - [ ] Developer creates endpoint with string response and tests correctly expect string
> - [ ] Developer creates endpoint with ProblemDetails and tests correctly deserialize it
> - [ ] Validation failures return properly formatted ValidationProblemDetails
> - [ ] Edge cases like null responses or empty bodies are handled gracefully

</details>

---

<details open>
<summary><h2>✅ Success Criteria</h2></summary>

- [ ] Primary outcome A achieved
- [ ] Primary outcome B achieved
- [ ] Primary outcome C achieved
- [ ] User experience goal D met
- [ ] Integration goal E completed
- [ ] Performance or quality goal F satisfied
- [ ] All unit, integration, and E2E tests pass successfully
- [ ] Feature ready for production deployment

**Example:**

> - [ ] All endpoint response types have corresponding test endpoints
> - [ ] Integration tests properly deserialize each response type (string, ProblemDetails, etc.)
> - [ ] Documentation clearly shows expected response types for each Send* method

</details>

---

<details open>
<summary><h2>🎭 User Scenarios</h2></summary>

> **Format Instructions**: Use collapsible `<details>` blocks with Gherkin code blocks for clean, readable scenarios. Each Given-When-Then statement should be on its own line within a code fence.

### [Scenario Category 1: Core Behavior Area]

<details open>
<summary><strong>Scenario 1: [Descriptive Scenario Name]</strong></summary>

```gherkin
Given [initial state or context]
When [user action or system event]
Then [expected outcome or behavior]
```

</details>

<details open>
<summary><strong>Scenario 2: [Descriptive Scenario Name]</strong></summary>

```gherkin
Given [initial state or context]
When [user action or system event]
Then [expected outcome or behavior]
```

</details>

<details open>
<summary><strong>Scenario 3: [Descriptive Scenario Name]</strong></summary>

```gherkin
Given [initial state or context]
When [user action or system event]
Then [expected outcome or behavior]
```

</details>

---

### [Scenario Category 2: Another Behavior Area]

<details open>
<summary><strong>Scenario 4: [Descriptive Scenario Name]</strong></summary>

```gherkin
Given [initial state or context]
When [user action or system event]
Then [expected outcome or behavior]
```

</details>

<details open>
<summary><strong>Scenario 5: [Descriptive Scenario Name]</strong></summary>

```gherkin
Given [initial state or context]
When [user action or system event]
Then [expected outcome or behavior]
```

</details>

---

### [Scenario Category 3: Edge Cases and Error Handling]

<details open>
<summary><strong>Scenario N: [Descriptive Scenario Name]</strong></summary>

```gherkin
Given [initial state or context]
When [user action or system event]
Then [expected outcome or behavior]
```

</details>

---

**Example of Complete Scenarios Section:**

<details>
<summary>Click to expand example scenarios</summary>

### History Tracking Scenarios

<details open>
<summary><strong>Scenario 1: Track File Launch in Directory Mode</strong></summary>

```gherkin
Given a user is browsing directory files
When the user double-clicks a file to launch it
Then the file is added to play history with a timestamp and Directory launch mode
```

</details>

<details open>
<summary><strong>Scenario 2: Track File Launch in Shuffle Mode</strong></summary>

```gherkin
Given a user is in shuffle mode
When a random file is launched
Then the file is added to play history with a timestamp and Shuffle launch mode
```

</details>

</details>

---

**Tips for Writing Scenarios:**

- Use `<details open>` blocks with `<summary>` for collapsible, scannable scenarios
- Wrap Given-When-Then in ` ```gherkin ` code blocks for syntax highlighting and visual boundaries
- Stack Given, When, Then vertically (one per line) for easy reading
- Group related scenarios under category headings with horizontal rules (`---`) between categories
- Cover happy path, alternative flows, and edge cases
- Focus on observable user behaviors and outcomes
- Include multi-device scenarios if relevant
- Include error handling and empty state scenarios
- Use specific, concrete examples

</details>

---

<details open>
<summary><h2>📚 Related Documentation</h2></summary>

- **Feature-Specific Design**: Link to relevant design documentation (if applicable)
- **Architecture Overview**: Link to architecture documentation (if applicable)
- **Coding Standards**: Link to coding standards (if applicable)
- **Testing Standards**: Link to testing standards (if applicable)
- **Project Guidelines**: Link to any project-specific guidelines

</details>

---

<details open>
<summary><h2>📝 Notes</h2></summary>

### Design Considerations

- **Consideration 1**: Important design factor to keep in mind
- **Consideration 2**: Technical constraint or limitation
- **Consideration 3**: User experience factor
- **Consideration 4**: Performance or scalability consideration

**Example:**

> - **Response Type Consistency**: Ensure all Send* methods have clear documentation about what type they return (string vs ProblemDetails)
> - **Testing Library Enhancement**: Consider adding overloads or helper methods to make testing different response types more intuitive

### Future Enhancement Ideas

- **Enhancement 1**: Potential future feature or improvement
- **Enhancement 2**: Additional capability that could be added later
- **Enhancement 3**: Integration with future features

**Example:**

> - **Extended Response Types**: Add test coverage for file downloads, streams, and other specialized response types
> - **Custom Serialization**: Document and test custom JSON serialization scenarios
> - **Error Handling**: Expand test coverage for error scenarios and edge cases

### Summary of Open Questions

[Consolidate all open questions from each phase for easy reference]

**Phase 1:**

- Question from Phase 1
- Question from Phase 1

**Phase 2:**

- Question from Phase 2

**Phase N:**

- Question from Phase N

</details>

---

## 💡 Tips for Using This Template

**Before Writing:**

1. Review the existing codebase to understand current behavior patterns
2. Identify similar features to use as behavior models
3. Understand user workflows and pain points
4. Consider how the feature fits into the broader architecture

**While Writing:**

1. Focus on behaviors users will see, not code structures
2. Use generic concept names instead of specific artifact names
3. Break work into phases that each deliver independent value
4. Include comprehensive Given-When-Then scenarios
5. Identify open questions that need resolution during implementation
6. Keep language non-technical and accessible

**After Writing:**

1. Verify each phase can stand alone and deliver value
2. Ensure scenarios cover happy path, alternatives, and edge cases
3. Check that success criteria are measurable and specific
4. Confirm open questions are organized by relevant phase
5. Review for any overly technical language that should be generalized

**Remember:** This document guides implementation without constraining it. The goal is to clearly communicate intent, user value, and expected behaviors - not to prescribe implementation details.

