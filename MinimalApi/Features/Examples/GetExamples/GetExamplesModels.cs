﻿using MinimalApi.Features.Examples._common;

namespace MinimalApi.Features.Examples.GetExamples
{
	public class GetExamplesRequest : RadRequest { }
    public class GetExamplesResponse : RadResponse<IEnumerable<ExampleDto>> { }
}