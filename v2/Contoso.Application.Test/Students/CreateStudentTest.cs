using System;
using System.Threading;
using System.Threading.Tasks;
using SaveOnCloud.Application.Students.Commands;
using SaveOnCloud.Core.Domain;
using SaveOnCloud.Core.Interfaces.Repositories;
using Moq;
using NUnit.Framework;

namespace SaveOnCloud.Application.Test.Students
{
    public class CreateStudentTest
    {
        private readonly Mock<IStudentRepository> studentRepository = new Mock<IStudentRepository>();

        [Test]
        public async Task CreateStudent_ValidStudent_CreateSuccessfully()
        {
            // Arrange
            var student = new CreateStudent("James", "Test", DateTime.Now);

            studentRepository.Setup(s => s.Add(It.IsAny<Student>())).ReturnsAsync(1);
            var handler = new CreateStudentHandler(studentRepository.Object);

            // Act
            var result = await handler.Handle(student, CancellationToken.None);

            // Assert
            result.Match(
                Left: error => Assert.Fail("Validation should have passed"),
                Right: result => Assert.Pass());

        }

        [Test]
        public async Task CreateStudent_InvalidStudent_Fails()
        {
            // Arrange
            var student = new CreateStudent("", "", DateTime.Now);

            studentRepository.Setup(s => s.Add(It.IsAny<Student>())).ReturnsAsync(1);
            var handler = new CreateStudentHandler(studentRepository.Object);

            // Act
            var result = await handler.Handle(student, CancellationToken.None);

            // Assert
            result.Match(
                Left: error => Assert.Pass(),
                Right: result => Assert.Fail("Validation should have failed"));
        }
    }
}
