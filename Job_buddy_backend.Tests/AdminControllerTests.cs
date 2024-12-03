using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using job_buddy_backend.Controllers;
using job_buddy_backend.Core;
using job_buddy_backend.Models.DataContext;
using Microsoft.EntityFrameworkCore;
using job_buddy_backend.Models.UserModel;
using job_buddy_backend.Models;
using JobBuddyBackend.Models;

namespace job_buddy_backend.Tests
{
    public class AdminControllerTests
{
    [Fact]
    public async Task SetUserProfileStatus_ShouldReturnOk_WhenUserExists()
    {
        // Arrange
        var dbName = Guid.NewGuid().ToString();
        using var dbContext = new JobBuddyDbContext(new DbContextOptionsBuilder<JobBuddyDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options);

        SeedDatabase(dbContext);

        var adminService = new AdminService(dbContext);
        var controller = new AdminController(adminService);

        var userId = 1;
        var isActive = true;

        // Act
        var result = await controller.SetUserProfileStatus(userId, isActive);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Value.Should().Be("User status updated.");
    }

    private void SeedDatabase(JobBuddyDbContext dbContext)
    {
        dbContext.Users.Add(new User { UserID = 1, Email = "user@example.com", FullName = "Test User" });
        dbContext.SaveChanges();

        foreach (var entry in dbContext.ChangeTracker.Entries().ToList())
        {
            entry.State = EntityState.Detached;
        }
    }
}

}
