using Moq;
using NetTopologySuite.Geometries;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Pims.Dal.Helpers.Extensions.Tests
{
    public class PropertyExtensionsTest
    {
        public static IEnumerable<object[]> GetPropertyNameTestData()
        {
            // PID present and valid
            yield return new object[] {
                new TestFilePropertyEntity
                {
                    Property = EntityHelper.CreateProperty(pid: 123456789)
                },
                "123-456-789"
            };

            // PID is 0 (should skip to next)
            yield return new object[] {
                new TestFilePropertyEntity
                {
                    Property = EntityHelper.CreateProperty(0, pin: 987654321)
                },
                "987654321"
            };

            // PIN present and valid
            yield return new object[] {
                new TestFilePropertyEntity
                {
                    Property = EntityHelper.CreateProperty(0, pin: 1234567)
                },
                "1234567"
            };

            // PIN is 0 (should skip to next)
            yield return new object[] {
                new TestFilePropertyEntity
                {
                    Property = EntityHelper.CreateProperty(0,  surveyPlanNumber: "SPN-001")
                },
                "SPN-001"
            };

            // SurveyPlanNumber present
            yield return new object[] {
                new TestFilePropertyEntity
                {
                    Property = EntityHelper.CreateProperty(0, surveyPlanNumber: "SPN-002")
                },
                "SPN-002"
            };

            // Location present
            yield return new object[] {
                new TestFilePropertyEntity
                {
                    Property = EntityHelper.CreateProperty(0)
                },
                "0, 0"
            };

            // Address present
            yield return new object[] {
                new TestFilePropertyEntity
                {
                    Property = EntityHelper.CreateProperty(0, address: EntityHelper.CreateAddress(1, "123 Main St"), noLocation: true)
                },
                "123 Main St"
            };
        }

        [Theory]
        [MemberData(nameof(GetPropertyNameTestData))]
        public void GetPropertyName(IFilePropertyEntity fileProperty, string expected)
        {
            var result = PropertyExtensions.GetPropertyName(fileProperty);
            Assert.Equal(expected, result);
        }

        // Helper classes for mocking
        private class TestFilePropertyEntity : IFilePropertyEntity
        {
            public PimsProperty Property { get; set; }
            public long PropertyId { get; set; }
            public Geometry Location { get; set; }
        }
    }
}
