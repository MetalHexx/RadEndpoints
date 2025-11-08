# Phase 1: Comprehensive RadEndpoints Response Type Testing

## 🎯 Objective

Create a comprehensive suite of test endpoints and integration tests that demonstrate and validate all RadEndpoints response helper methods for both `RadEndpoint<TRequest, TResponse>` (WithRequest) and `RadEndpointWithoutRequest<TResponse>` (WithoutRequest) base classes. This phase ensures developers understand how to properly test endpoints using different response types (string, ProblemDetails, ValidationProblemDetails, etc.) and validates that the RadEndpoints.Testing library correctly handles serialization/deserialization for all response scenarios.

---

## 📚 Required Reading

**Feature Documentation:**

- [ ] [RadEndpoints README](../README.md) - Understanding RadEndpoints core concepts
- [ ] [RadEndpoints.Testing Documentation](../RadEndpoints.Testing/README.md) - Testing utilities and patterns
- [ ] [@RadEndpoints\Endpoint\RadEndpointWithRequest.cs](../RadEndpoints/Endpoint/RadEndpointWithRequest.cs) - WithRequest base class
- [ ] [@RadEndpoints\Endpoint\RadEndpointWithoutRequest.cs](../RadEndpoints/Endpoint/RadEndpointWithoutRequest.cs) - WithoutRequest base class

**Standards & Guidelines:**

- [ ] [Coding Standards](./CODING_STANDARDS.md) - General coding patterns and conventions (if applicable)
- [ ] [Testing Standards](./TESTING_STANDARDS.md) - Testing approaches and best practices (if applicable)

---

## 📂 File Structure Overview

```
MinimalApi/Features/ResultEndpoints/
├── WithRequest/
│   ├── NotFoundEndpoint.cs                  ✨ Exists - Test SendNotFound(string)
│   ├── ConflictEndpoint.cs                  ✨ New - Test SendConflict(string)
│   ├── ProblemEndpoint.cs                   ✨ New - Test SendInternalError/SendExternalError/SendExternalTimeout
│   ├── ValidationErrorEndpoint.cs           ✨ New - Test SendValidationError(string)
│   ├── UnauthorizedEndpoint.cs              ✨ New - Test SendUnauthorized(string) and SendUnauthorized()
│   ├── ForbiddenEndpoint.cs                 ✨ New - Test SendForbidden(string) and SendForbidden()
│   ├── CreatedAtEndpoint.cs                 ✨ New - Test SendCreatedAt(uri) and SendCreatedAt(uri, response)
│   ├── RedirectEndpoint.cs                  ✨ New - Test SendRedirect with different parameters
│   ├── SuccessEndpoint.cs                   ✨ New - Test Send() and Send(TResponse)
│   ├── SendProblemEndpoint.cs               ✨ New - Test SendProblem overloads
│   ├── BytesEndpoint.cs                     ✨ New - Test SendBytes(RadBytes)
│   ├── StreamEndpoint.cs                    ✨ New - Test SendStream(RadStream)
│   └── FileEndpoint.cs                      ✨ New - Test SendFile(RadFile)
└── WithoutRequest/
    ├── NotFoundWithoutRequestEndpoint.cs    ✨ New - Test SendNotFound without request
    ├── ConflictWithoutRequestEndpoint.cs    ✨ New - Test SendConflict without request
    ├── ProblemWithoutRequestEndpoint.cs     ✨ New - Test SendInternalError/SendExternalError/SendExternalTimeout
    ├── ValidationErrorWithoutRequestEndpoint.cs  ✨ New - Test SendValidationError
    ├── UnauthorizedWithoutRequestEndpoint.cs     ✨ New - Test SendUnauthorized variants
    ├── ForbiddenWithoutRequestEndpoint.cs        ✨ New - Test SendForbidden variants
    ├── CreatedAtWithoutRequestEndpoint.cs        ✨ New - Test SendCreatedAt variants
    ├── RedirectWithoutRequestEndpoint.cs         ✨ New - Test SendRedirect
    ├── SuccessWithoutRequestEndpoint.cs          ✨ New - Test Send() variants
    ├── SendProblemWithoutRequestEndpoint.cs      ✨ New - Test SendProblem overloads
    ├── BytesWithoutRequestEndpoint.cs            ✨ New - Test SendBytes
    ├── StreamWithoutRequestEndpoint.cs           ✨ New - Test SendStream
    └── FileWithoutRequestEndpoint.cs             ✨ New - Test SendFile

MinimalApi.Tests.Integration/Tests/ResultEndpoints/
├── WithRequest/
│   ├── NotFoundEndpointTests.cs             ✨ Exists - Tests for NotFoundEndpoint
│   ├── ConflictEndpointTests.cs             ✨ New - Tests for ConflictEndpoint
│   ├── ProblemEndpointTests.cs              ✨ New - Tests for ProblemEndpoint
│   ├── ValidationErrorEndpointTests.cs      ✨ New - Tests for ValidationErrorEndpoint
│   ├── UnauthorizedEndpointTests.cs         ✨ New - Tests for UnauthorizedEndpoint
│   ├── ForbiddenEndpointTests.cs            ✨ New - Tests for ForbiddenEndpoint
│   ├── CreatedAtEndpointTests.cs            ✨ New - Tests for CreatedAtEndpoint
│   ├── RedirectEndpointTests.cs             ✨ New - Tests for RedirectEndpoint
│   ├── SuccessEndpointTests.cs              ✨ New - Tests for SuccessEndpoint
│   ├── SendProblemEndpointTests.cs          ✨ New - Tests for SendProblem variants
│   ├── BytesEndpointTests.cs                ✨ New - Tests for SendBytes
│   ├── StreamEndpointTests.cs               ✨ New - Tests for SendStream
│   └── FileEndpointTests.cs                 ✨ New - Tests for SendFile
└── WithoutRequest/
    ├── NotFoundWithoutRequestEndpointTests.cs     ✨ New - Tests without request
    ├── ConflictWithoutRequestEndpointTests.cs     ✨ New - Tests without request
    ├── ProblemWithoutRequestEndpointTests.cs      ✨ New - Tests without request
    ├── ValidationErrorWithoutRequestEndpointTests.cs  ✨ New - Tests without request
    ├── UnauthorizedWithoutRequestEndpointTests.cs     ✨ New - Tests without request
    ├── ForbiddenWithoutRequestEndpointTests.cs        ✨ New - Tests without request
    ├── CreatedAtWithoutRequestEndpointTests.cs        ✨ New - Tests without request
    ├── RedirectWithoutRequestEndpointTests.cs         ✨ New - Tests without request
    ├── SuccessWithoutRequestEndpointTests.cs          ✨ New - Tests without request
    ├── SendProblemWithoutRequestEndpointTests.cs      ✨ New - Tests without request
    ├── BytesWithoutRequestEndpointTests.cs            ✨ New - Tests without request
    ├── StreamWithoutRequestEndpointTests.cs           ✨ New - Tests without request
    └── FileWithoutRequestEndpointTests.cs             ✨ New - Tests without request
```

---

<details open>
<summary><h3>Task 1: Create WithRequest Endpoints for String Response Types</h3></summary>

**Purpose**: Create test endpoints that use response helper methods returning strings (SendNotFound, SendConflict) to demonstrate proper usage and testing patterns.

**Response Methods Covered:**
- `SendNotFound(string title)` - Returns `NotFound<string>` (404)
- `SendConflict(string title)` - Returns `Conflict<string>` (409)

**Implementation Subtasks:**

- [x] **NotFoundEndpoint**: Already exists, verify namespace is correct
- [ ] **ConflictEndpoint**: Create endpoint at `/api/conflict/{id}` that uses SendConflict(string)
- [ ] **Add Response Models**: Create simple request/response models for each endpoint

**Testing Subtask:**

- [x] **NotFoundEndpointTests**: Already exists and passing
- [ ] **ConflictEndpointTests**: Create integration tests expecting `string` response type

**Key Implementation Notes:**

- Use simple `string Id` route parameter for all endpoints
- Response message should include the `Id` value for verification
- Follow the pattern established in NotFoundEndpoint.cs

**Testing Focus for Task 1:**

**Behaviors to Test:**

- [x] SendNotFound returns 404 status code with string body
- [ ] SendConflict returns 409 status code with string body
- [ ] Response content matches the exact string passed to Send method

</details>

---

<details open>
<summary><h3>Task 2: Create WithRequest Endpoints for ProblemDetails Response Types</h3></summary>

**Purpose**: Create test endpoints that use response helper methods returning ProblemDetails objects to demonstrate proper testing of structured error responses.

**Response Methods Covered:**
- `SendInternalError(string title)` - Returns ProblemDetails (500)
- `SendExternalError(string title)` - Returns ProblemDetails (502)
- `SendExternalTimeout(string title)` - Returns ProblemDetails (504)
- `SendUnauthorized(string title)` - Returns ProblemDetails (401)
- `SendForbidden(string title)` - Returns ProblemDetails (403)

**Implementation Subtasks:**

- [ ] **ProblemEndpoint**: Create endpoint with route `/api/problem/{id}` that routes to different problem types based on id parameter
- [ ] **UnauthorizedEndpoint**: Create endpoint at `/api/unauthorized/{id}` that uses SendUnauthorized(string)
- [ ] **ForbiddenEndpoint**: Create endpoint at `/api/forbidden/{id}` that uses SendForbidden(string)

**Testing Subtask:**

- [ ] **ProblemEndpointTests**: Create tests expecting `ProblemDetails` response type
- [ ] **UnauthorizedEndpointTests**: Create tests for unauthorized scenarios
- [ ] **ForbiddenEndpointTests**: Create tests for forbidden scenarios

**Key Implementation Notes:**

- These methods return proper ProblemDetails JSON objects, not plain strings
- Tests should use `BeProblem()` assertion extensions
- Verify both status code and title message in tests

**Testing Focus for Task 2:**

**Behaviors to Test:**

- [ ] SendInternalError returns 500 with ProblemDetails structure
- [ ] SendExternalError returns 502 with ProblemDetails structure
- [ ] SendExternalTimeout returns 504 with ProblemDetails structure
- [ ] SendUnauthorized(string) returns 401 with ProblemDetails structure
- [ ] SendForbidden(string) returns 403 with ProblemDetails structure
- [ ] ProblemDetails.Title matches the message passed to method

</details>

---

<details open>
<summary><h3>Task 3: Create WithRequest Endpoints for Special Response Types</h3></summary>

**Purpose**: Create test endpoints for validation errors, success responses, redirects, and created-at responses.

**Response Methods Covered:**
- `SendValidationError(string title)` - Returns ValidationProblemDetails (400)
- `Send()` / `Send(TResponse)` - Returns Ok<TResponse> (200)
- `SendCreatedAt(string uri)` / `SendCreatedAt(string uri, TResponse)` - Returns Created (201)
- `SendRedirect(string url, bool permanent, bool preserveMethod)` - Returns redirect result

**Implementation Subtasks:**

- [ ] **ValidationErrorEndpoint**: Create endpoint at `/api/validation-error/{id}` using SendValidationError
- [ ] **SuccessEndpoint**: Create endpoint at `/api/success/{id}` using Send() and Send(TResponse)
- [ ] **CreatedAtEndpoint**: Create endpoint at `/api/created/{id}` using SendCreatedAt variants
- [ ] **RedirectEndpoint**: Create endpoint at `/api/redirect/{id}` using SendRedirect

**Testing Subtask:**

- [ ] **ValidationErrorEndpointTests**: Test ValidationProblemDetails responses
- [ ] **SuccessEndpointTests**: Test successful response scenarios
- [ ] **CreatedAtEndpointTests**: Test created-at responses with Location header
- [ ] **RedirectEndpointTests**: Test redirect responses with different parameter combinations

**Key Implementation Notes:**

- ValidationProblemDetails has `Errors` dictionary, not just `Title`
- SendCreatedAt should include proper URI in Location header
- SendRedirect should test permanent and temporary variants (all 4 redirect types: 301, 302, 307, 308)

**Testing Focus for Task 3:**

**Behaviors to Test:**

- [ ] SendValidationError returns ValidationProblemDetails with Errors dictionary
- [ ] Send() returns OK with response object
- [ ] SendCreatedAt returns 201 with Location header set correctly
- [ ] SendCreatedAt(uri, response) returns 201 with custom response object
- [ ] SendRedirect returns appropriate redirect status based on parameters
- [ ] SendRedirect with permanent=true returns 301 or 308
- [ ] SendRedirect with permanent=false returns 302 or 307

</details>

---

<details open>
<summary><h3>Task 4: Create WithRequest Endpoints for File/Stream/Bytes Responses</h3></summary>

**Purpose**: Create test endpoints for binary response types (files, streams, bytes) to demonstrate file download and streaming scenarios.

**Response Methods Covered:**
- `SendBytes(RadBytes response)` - Returns bytes with content type
- `SendStream(RadStream response)` - Returns streaming response
- `SendFile(RadFile response)` - Returns physical file

**Implementation Subtasks:**

- [ ] **BytesEndpoint**: Create endpoint at `/api/bytes/{size}` that returns byte array of specified size
- [ ] **StreamEndpoint**: Create endpoint at `/api/stream/{type}` that returns stream response
- [ ] **FileEndpoint**: Create endpoint at `/api/file/{name}` that returns physical file

**Testing Subtask:**

- [ ] **BytesEndpointTests**: Test byte array downloads with correct Content-Type
- [ ] **StreamEndpointTests**: Test streaming responses
- [ ] **FileEndpointTests**: Test physical file downloads with proper headers

**Key Implementation Notes:**

- RadBytes includes ContentType, FileDownloadName, EnableRangeProcessing, LastModified
- RadStream includes EntityTag and similar metadata
- RadFile uses PhysicalFile path, requires actual files on disk for testing
- Tests should verify Content-Disposition headers for downloads
- Stream tests should verify chunked encoding or streaming behavior

**Testing Focus for Task 4:**

**Behaviors to Test:**

- [ ] SendBytes returns correct Content-Type header
- [ ] SendBytes sets Content-Disposition for downloads when FileDownloadName provided
- [ ] SendStream enables range requests when configured
- [ ] SendFile returns physical file with correct MIME type
- [ ] File download headers include filename when specified
- [ ] LastModified and EntityTag headers set when provided

</details>

---

<details open>
<summary><h3>Task 5: Create WithRequest Endpoints for SendProblem Overloads</h3></summary>

**Purpose**: Create test endpoints demonstrating the three SendProblem overloads for custom problem details scenarios.

**Response Methods Covered:**
- `SendProblem(ProblemHttpResult problem)` - Custom ProblemHttpResult
- `SendProblem(ValidationProblem problem)` - Custom ValidationProblem
- `SendProblem(IRadProblem problem)` - RadEndpoints problem abstraction

**Implementation Subtasks:**

- [ ] **SendProblemEndpoint**: Create endpoint at `/api/sendproblem/{type}` that routes to different SendProblem overloads
- [ ] **Create example IRadProblem implementation**: Custom domain error type
- [ ] **Test all three overload paths**: ProblemHttpResult, ValidationProblem, IRadProblem

**Testing Subtask:**

- [ ] **SendProblemEndpointTests**: Test all three overloads return proper problem details
- [ ] **Verify IRadProblem conversion**: Ensure custom domain errors convert correctly

**Key Implementation Notes:**

- ProblemHttpResult is ASP.NET Core's built-in type
- ValidationProblem is ASP.NET Core's validation-specific type
- IRadProblem is RadEndpoints' abstraction for domain errors
- GetProblemResult method converts IRadProblem to IResult

**Testing Focus for Task 5:**

**Behaviors to Test:**

- [ ] SendProblem(ProblemHttpResult) returns problem with custom status and details
- [ ] SendProblem(ValidationProblem) returns validation problem with errors
- [ ] SendProblem(IRadProblem) converts domain error to HTTP problem details
- [ ] Custom IRadProblem implementations serialize with correct status codes

</details>

---

<details open>
<summary><h3>Task 6: Create WithRequest Endpoints for Parameterless Variants</h3></summary>

**Purpose**: Test parameterless variants of SendUnauthorized and SendForbidden that return framework authentication results.

**Response Methods Covered:**
- `SendUnauthorized()` - Returns TypedResults.Unauthorized() (401 with challenge)
- `SendForbidden()` - Returns TypedResults.Forbid() (403 with challenge)

**Implementation Subtasks:**

- [ ] **Update UnauthorizedEndpoint**: Add test case for parameterless SendUnauthorized()
- [ ] **Update ForbiddenEndpoint**: Add test case for parameterless SendForbidden()

**Testing Subtask:**

- [ ] **Update UnauthorizedEndpointTests**: Test both string and parameterless variants
- [ ] **Update ForbiddenEndpointTests**: Test both string and parameterless variants

**Key Implementation Notes:**

- Parameterless variants return authentication challenges without custom messages
- These integrate with ASP.NET Core authentication/authorization middleware
- Response may include WWW-Authenticate headers depending on auth configuration

**Testing Focus for Task 6:**

**Behaviors to Test:**

- [ ] SendUnauthorized() returns 401 status code
- [ ] SendForbidden() returns 403 status code
- [ ] Parameterless variants work alongside string variants in same endpoint

</details>

---

<details open>
<summary><h3>Task 7: Create Comprehensive WithoutRequest Endpoints</h3></summary>

**Purpose**: Create complete set of test endpoints using `RadEndpointWithoutRequest<TResponse>` base class to verify ALL response methods work without request parameters.

**Implementation Subtasks:**

- [ ] **NotFoundWithoutRequestEndpoint**: Test SendNotFound(string)
- [ ] **ConflictWithoutRequestEndpoint**: Test SendConflict(string)
- [ ] **ProblemWithoutRequestEndpoint**: Test SendInternalError/SendExternalError/SendExternalTimeout
- [ ] **ValidationErrorWithoutRequestEndpoint**: Test SendValidationError
- [ ] **UnauthorizedWithoutRequestEndpoint**: Test both SendUnauthorized variants
- [ ] **ForbiddenWithoutRequestEndpoint**: Test both SendForbidden variants
- [ ] **CreatedAtWithoutRequestEndpoint**: Test both SendCreatedAt overloads
- [ ] **RedirectWithoutRequestEndpoint**: Test SendRedirect with different parameters
- [ ] **SuccessWithoutRequestEndpoint**: Test Send() and Send(TResponse)
- [ ] **SendProblemWithoutRequestEndpoint**: Test all SendProblem overloads
- [ ] **BytesWithoutRequestEndpoint**: Test SendBytes
- [ ] **StreamWithoutRequestEndpoint**: Test SendStream
- [ ] **FileWithoutRequestEndpoint**: Test SendFile

**Testing Subtask:**

- [ ] **Create corresponding test files for ALL 13 WithoutRequest endpoints**
- [ ] **Verify all tests use parameterless endpoint calls**: `GetAsync<TEndpoint, TResponse>()`

**Key Implementation Notes:**

- These endpoints have no request model, no route parameters, no query parameters
- All endpoints should have simple routes like `/api/norequest/notfound`
- Tests must use the `GetAsync<TEndpoint, TResponse>()` overload without TRequest
- Verify RadEndpoints.Testing library correctly handles endpoints without request

**Testing Focus for Task 7:**

**Behaviors to Test:**

- [ ] All 17 Send* methods work correctly in WithoutRequest context
- [ ] Response serialization identical to WithRequest variants
- [ ] Test client extensions handle parameterless calls
- [ ] HTTP verbs (GET, POST, PUT, PATCH, DELETE) all work without request models

</details>

---

<details open>
<summary><h3>Task 8: Documentation and Examples</h3></summary>

**Purpose**: Create comprehensive documentation showing developers how to test each response type with clear examples.

**Implementation Subtasks:**

- [ ] **Create RESPONSE_TYPES.md**: Document all Send* methods with return types
- [ ] **Update Integration Test README**: Add section on testing different response types
- [ ] **Add Code Comments**: Inline comments in test endpoints explaining patterns

**Testing Subtask:**

- [ ] **Verify All Tests Pass**: Run complete test suite and verify 100% pass rate

**Key Implementation Notes:**

- Documentation should clearly state which methods return strings vs ProblemDetails
- Include table mapping Send* methods to expected test response types
- Provide copy-paste examples for each scenario

**Testing Focus for Task 5:**

**Behaviors to Test:**

- [ ] All integration tests pass without serialization errors
- [ ] Documentation examples match actual test implementations

</details>

---

## 🗂️ Files Modified or Created

**New Endpoint Files:**

- `MinimalApi/Features/ResultEndpoints/WithRequest/ConflictEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithRequest/ProblemEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithRequest/ValidationErrorEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithRequest/UnauthorizedEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithRequest/ForbiddenEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithRequest/CreatedAtEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithRequest/RedirectEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithRequest/SuccessEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithRequest/SendProblemEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithRequest/BytesEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithRequest/StreamEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithRequest/FileEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithoutRequest/NotFoundWithoutRequestEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithoutRequest/ConflictWithoutRequestEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithoutRequest/ProblemWithoutRequestEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithoutRequest/ValidationErrorWithoutRequestEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithoutRequest/UnauthorizedWithoutRequestEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithoutRequest/ForbiddenWithoutRequestEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithoutRequest/CreatedAtWithoutRequestEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithoutRequest/RedirectWithoutRequestEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithoutRequest/SuccessWithoutRequestEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithoutRequest/SendProblemWithoutRequestEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithoutRequest/BytesWithoutRequestEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithoutRequest/StreamWithoutRequestEndpoint.cs`
- `MinimalApi/Features/ResultEndpoints/WithoutRequest/FileWithoutRequestEndpoint.cs`

**New Test Files:**

- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithRequest/ConflictEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithRequest/ProblemEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithRequest/ValidationErrorEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithRequest/UnauthorizedEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithRequest/ForbiddenEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithRequest/CreatedAtEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithRequest/RedirectEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithRequest/SuccessEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithRequest/SendProblemEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithRequest/BytesEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithRequest/StreamEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithRequest/FileEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithoutRequest/NotFoundWithoutRequestEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithoutRequest/ConflictWithoutRequestEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithoutRequest/ProblemWithoutRequestEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithoutRequest/ValidationErrorWithoutRequestEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithoutRequest/UnauthorizedWithoutRequestEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithoutRequest/ForbiddenWithoutRequestEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithoutRequest/CreatedAtWithoutRequestEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithoutRequest/RedirectWithoutRequestEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithoutRequest/SuccessWithoutRequestEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithoutRequest/SendProblemWithoutRequestEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithoutRequest/BytesWithoutRequestEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithoutRequest/StreamWithoutRequestEndpointTests.cs`
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithoutRequest/FileWithoutRequestEndpointTests.cs`

**Modified Files:**

- `MinimalApi/Features/ResultEndpoints/WithRequest/NotFoundEndpoint.cs` - Namespace updated
- `MinimalApi.Tests.Integration/Tests/ResultEndpoints/WithRequest/NotFoundEndpointTests.cs` - Namespace updated

**New Documentation Files:**

- `docs/RESPONSE_TYPES.md` - Comprehensive response type documentation

---

<details open>
<summary><h2>✅ Success Criteria</h2></summary>

**Functional Requirements:**

- [ ] All implementation tasks completed and checked off
- [ ] All Send* methods have corresponding test endpoints
- [ ] All endpoints follow consistent naming and routing conventions
- [ ] Code follows .NET and RadEndpoints coding standards

**Testing Requirements:**

- [ ] All integration tests pass without serialization errors
- [ ] Tests correctly expect `string` for SendNotFound/SendConflict
- [ ] Tests correctly expect `ProblemDetails` for Send*Error/Send*Unauthorized/Send*Forbidden methods
- [ ] Tests correctly expect `ValidationProblemDetails` for SendValidationError
- [ ] Parameterless SendUnauthorized() and SendForbidden() tested
- [ ] SendProblem overloads (ProblemHttpResult, ValidationProblem, IRadProblem) tested
- [ ] File/Stream/Bytes responses tested with proper Content-Type and headers
- [ ] CreatedAt responses include Location header
- [ ] Redirect responses test all redirect types (301, 302, 307, 308)
- [ ] WithoutRequest endpoints test successfully without request parameters
- [ ] Test coverage includes both success and error scenarios for ALL 21 Send* methods

**Quality Checks:**

- [ ] No TypeScript/C# compilation errors
- [ ] All tests pass: `dotnet test`
- [ ] Code formatting is consistent
- [ ] No warnings in build output

**Documentation:**

- [ ] RESPONSE_TYPES.md created with complete Send* method documentation
- [ ] Integration test README updated with testing patterns
- [ ] Inline code comments explain testing patterns

**Ready for Next Phase:**

- [ ] All success criteria met
- [ ] Developers can reference these endpoints as examples
- [ ] Testing patterns are clear and consistent

</details>

---

<details open>
<summary><h2>📝 Notes & Considerations</h2></summary>

### Design Decisions

- **Two Folder Structure**: Separate `WithRequest` and `WithoutRequest` folders mirror the base class hierarchy and make it clear which endpoint pattern is being demonstrated
- **Simple Route Parameters**: All endpoints use simple `{id}` route parameters to keep focus on response types rather than request complexity
- **One Endpoint Per Method**: Each Send* method gets its own dedicated endpoint for clarity and easy reference

### Implementation Constraints

- **Natural Minimal API Behavior**: We preserve ASP.NET Core's natural behavior where `TypedResults.NotFound(value)` returns typed results, not ProblemDetails
- **Testing Library Unchanged**: No changes to RadEndpoints.Testing library - tests must correctly specify expected response types

### Future Enhancements

- **Extended Response Types**: Add tests for `SendBytes`, `SendStream`, `SendFile` methods
- **Custom Serialization**: Add examples showing custom JSON serialization options
- **Complex Request Models**: Add examples with complex request binding scenarios

### Response Type Reference Table

| Send Method | Return Type | HTTP Status | Test Response Type | Notes |
|------------|-------------|-------------|-------------------|-------|
| `SendNotFound(string)` | `NotFound<string>` | 404 | `string` | Plain string body |
| `SendConflict(string)` | `Conflict<string>` | 409 | `string` | Plain string body |
| `SendInternalError(string)` | `ProblemDetails` | 500 | `ProblemDetails` | Structured error |
| `SendExternalError(string)` | `ProblemDetails` | 502 | `ProblemDetails` | Structured error |
| `SendExternalTimeout(string)` | `ProblemDetails` | 504 | `ProblemDetails` | Structured error |
| `SendUnauthorized(string)` | `ProblemDetails` | 401 | `ProblemDetails` | Structured error |
| `SendForbidden(string)` | `ProblemDetails` | 403 | `ProblemDetails` | Structured error |
| `SendUnauthorized()` | `UnauthorizedHttpResult` | 401 | N/A | Auth challenge |
| `SendForbidden()` | `ForbidHttpResult` | 403 | N/A | Auth challenge |
| `SendValidationError(string)` | `ValidationProblemDetails` | 400 | `ValidationProblemDetails` | Validation errors |
| `Send()` | `Ok<TResponse>` | 200 | `TResponse` | Success response |
| `Send(TResponse)` | `Ok<TResponse>` | 200 | `TResponse` | Success with data |
| `SendCreatedAt(uri)` | `Created<TResponse>` | 201 | `TResponse` | With Location header |
| `SendCreatedAt(uri, response)` | `Created<TResponse>` | 201 | `TResponse` | Custom response |
| `SendRedirect(url, ...)` | Redirect result | 301/302/307/308 | N/A | Check Location header |
| `SendBytes(RadBytes)` | Bytes result | 200 | byte[] | Binary download |
| `SendStream(RadStream)` | Stream result | 200 | Stream | Streaming response |
| `SendFile(RadFile)` | PhysicalFile result | 200 | File | File download |
| `SendProblem(ProblemHttpResult)` | `ProblemDetails` | Custom | `ProblemDetails` | Custom problem |
| `SendProblem(ValidationProblem)` | `ValidationProblemDetails` | Custom | `ValidationProblemDetails` | Custom validation |
| `SendProblem(IRadProblem)` | `ProblemDetails` | Custom | `ProblemDetails` | Domain error |

</details>
