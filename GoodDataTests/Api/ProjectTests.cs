﻿using System;
using NUnit.Framework;

namespace GoodDataTests.Api
{
	[TestFixture]
	public class ProjectTests
	{
		private readonly GoodDataService.ApiWrapper reportingService;
		private readonly string profileId;

		public ProjectTests()
		{
			reportingService = new GoodDataService.ApiWrapper();
			profileId = reportingService.Authenticate(reportingService.Config.Login, reportingService.Config.Password);
			reportingService.GetToken();
		}

		[Test]
		public void CreateProject_ExpectSucces()
		{
			var title = DateTime.Now.Ticks.ToString();
			var projectId = reportingService.CreateProject(profileId, title, "Summary" + title);

			var projects = reportingService.FindProjectByTitle(profileId, title);
			Assert.NotNull(projects);

			reportingService.DeleteProject(projectId);

			projects = reportingService.FindProjectByTitle(profileId, title);

			Assert.IsNull(projects);
		}
	}
}