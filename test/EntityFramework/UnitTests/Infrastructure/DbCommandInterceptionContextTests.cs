﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Infrastructure
{
    using System.Data.Common;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Internal;
    using System.Threading.Tasks;
    using Moq;
    using Xunit;

    public class DbCommandInterceptionContextTests : TestBase
    {
        [Fact]
        public void New_interception_context_has_no_state()
        {
            var interceptionContext = new DbCommandInterceptionContext<int>();

            Assert.Empty(interceptionContext.ObjectContexts);
            Assert.Empty(interceptionContext.DbContexts);
            Assert.Null(interceptionContext.Exception);
            Assert.False(interceptionContext.IsAsync);
            Assert.Equal((TaskStatus)0, interceptionContext.TaskStatus);
            Assert.Equal(CommandBehavior.Default, interceptionContext.CommandBehavior);
            Assert.Equal(0, interceptionContext.Result);
            Assert.False(interceptionContext.IsResultSet);
        }

        [Fact]
        public void Interception_context_can_be_associated_command_specific_state_and_all_state_is_preserved()
        {
            var objectContext = new ObjectContext();
            var dbContext = CreateDbContext(objectContext);
            var exception = new Exception();

            var interceptionContext = new DbCommandInterceptionContext<int> { Result = 77 }
                .WithDbContext(dbContext)
                .WithObjectContext(objectContext)
                .WithException(exception)
                .WithTaskStatus(TaskStatus.Running)
                .WithCommandBehavior(CommandBehavior.SchemaOnly)
                .AsAsync();

            Assert.Equal(new[] { objectContext }, interceptionContext.ObjectContexts);
            Assert.Equal(new[] { dbContext }, interceptionContext.DbContexts);
            Assert.Same(exception, interceptionContext.Exception);
            Assert.True(interceptionContext.IsAsync);
            Assert.Equal(TaskStatus.Running, interceptionContext.TaskStatus);
            Assert.Equal(CommandBehavior.SchemaOnly, interceptionContext.CommandBehavior);
            Assert.Equal(77, interceptionContext.Result);
            Assert.True(interceptionContext.IsResultSet);
        }

        [Fact]
        public void Result_can_be_mutated()
        {
            var interceptionContext = new DbCommandInterceptionContext<DbDataReader>();
            Assert.Null(interceptionContext.Result);
            Assert.False(interceptionContext.IsResultSet);

            var dataReader = new Mock<DbDataReader>().Object;
            interceptionContext.Result = dataReader;
            Assert.True(interceptionContext.IsResultSet);
            Assert.Same(dataReader, interceptionContext.Result);
        }

        [Fact]
        public void Association_with_a_null_ObjectContext_or_DbContext_throws()
        {
            Assert.Equal(
                "context",
                Assert.Throws<ArgumentNullException>(() => new DbCommandInterceptionContext<int>().WithObjectContext(null)).ParamName);

            Assert.Equal(
                "context",
                Assert.Throws<ArgumentNullException>(() => new DbCommandInterceptionContext<int>().WithDbContext(null)).ParamName);
        }

        private static DbContext CreateDbContext(ObjectContext objectContext)
        {
            var mockInternalContext = new Mock<InternalContextForMock>();
            mockInternalContext.Setup(m => m.ObjectContext).Returns(objectContext);
            var context = mockInternalContext.Object.Owner;
            objectContext.InterceptionContext = objectContext.InterceptionContext.WithDbContext(context);
            return context;
        }
    }
}