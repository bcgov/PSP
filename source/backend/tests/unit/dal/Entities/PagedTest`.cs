using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class PagedTest
    {
        #region Tests
        [Fact]
        public void Paged_Default_Constructor()
        {
            // Arrange
            // Act
            var paged = new Paged<PimsUser>();

            // Assert
            paged.Page.Should().Be(1);
            paged.Quantity.Should().Be(10);
            paged.Items.Should().BeEmpty();
            paged.Count.Should().Be(0);
            paged.IsReadOnly.Should().Be(false);
        }

        [Fact]
        public void Paged_Constructor_01()
        {
            // Arrange
            var items = new[] { new { Id = 1 }, new { Id = 2 } };
            var page = 1;
            var quantity = 1;
            var total = 6;

            // Act
            var paged = new Paged<object>(items, page, quantity, total);

            // Assert
            paged.Page.Should().Be(page);
            paged.Quantity.Should().Be(quantity);
            paged.Total.Should().Be(total);
            paged.Items.Should().BeEquivalentTo(items);
        }

        [Fact]
        public void Paged_Constructor_01_NullItems()
        {
            // Arrange
            var page = 2;
            var quantity = 4;
            var total = 6;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new Paged<object>(null, page, quantity, total));
            Assert.Throws<ArgumentNullException>(() => new Paged<PimsUser>(null, page, quantity, total));
        }

        [Fact]
        public void Paged_Constructor_01_MinPage()
        {
            // Arrange
            var items = new[] { new { Id = 1 }, new { Id = 2 } };
            var page = 0;
            var quantity = 4;
            var total = 6;

            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Paged<object>(items, page, quantity, total));
        }

        [Fact]
        public void Paged_Constructor_01_MinQuantity()
        {
            // Arrange
            var items = new[] { new { Id = 1 }, new { Id = 2 } };
            var page = 1;
            var quantity = 0;
            var total = 6;

            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Paged<object>(items, page, quantity, total));
        }

        [Fact]
        public void Paged_Constructor_01_MinTotal()
        {
            // Arrange
            var items = new[] { new { Id = 1 }, new { Id = 2 } };
            var page = 1;
            var quantity = 2;
            var total = -1;

            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Paged<object>(items, page, quantity, total));
        }

        #region To
        [Fact]
        public void Paged_To()
        {
            // Arrange
            var items = new[] { EntityHelper.CreateUser("one"), EntityHelper.CreateUser("two") };
            var page = 2;
            var quantity = 4;
            var total = 6;
            var paged = new Paged<PimsUser>(items, page, quantity, total);

            // Act
            var result = paged.To((items) => items.Select(i => new { Key = $"{i.Internal_Id}" }));

            // Assert
            result.Page.Should().Be(page);
            result.Quantity.Should().Be(quantity);
            result.Total.Should().Be(total);
            result.Items.Count().Should().Be(items.Length);
            result.Items.First().Key.Should().Be(items.First().Internal_Id.ToString());
        }

        [Fact]
        public void Paged_To_ArgumentNullException()
        {
            // Arrange
            var items = new[] { EntityHelper.CreateUser("one"), EntityHelper.CreateUser("two") };
            var page = 2;
            var quantity = 4;
            var total = 6;
            var paged = new Paged<PimsUser>(items, page, quantity, total);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => paged.To((Func<IEnumerable<PimsUser>, IEnumerable<object>>)null));
        }
        #endregion

        #region GetEnumerator
        [Fact]
        public void Paged_GetEnumerator()
        {
            // Arrange
            var items = new[] { EntityHelper.CreateUser("one"), EntityHelper.CreateUser("two") };
            var page = 2;
            var quantity = 4;
            var total = 6;
            var paged = new Paged<PimsUser>(items, page, quantity, total);

            // Act
            var weak = paged.AsWeakEnumerable();
            var result = weak.Cast<object>().ToArray();

            // Assert
            result.Count().Should().Be(2);
        }
        #endregion

        #region Add
        [Fact]
        public void Paged_Add()
        {
            // Arrange
            var items = new[] { EntityHelper.CreateUser("one"), EntityHelper.CreateUser("two") };
            var page = 2;
            var quantity = 4;
            var total = 6;
            var paged = new Paged<PimsUser>(items, page, quantity, total);

            // Act
            paged.Add(EntityHelper.CreateUser("three"));

            // Assert
            paged.Count().Should().Be(3);
        }
        #endregion

        #region Clear
        [Fact]
        public void Paged_Clear()
        {
            // Arrange
            var items = new[] { EntityHelper.CreateUser("one"), EntityHelper.CreateUser("two") };
            var page = 2;
            var quantity = 4;
            var total = 6;
            var paged = new Paged<PimsUser>(items, page, quantity, total);

            // Act
            paged.Clear();

            // Assert
            paged.Should().BeEmpty();
        }
        #endregion

        #region Contains
        [Fact]
        public void Paged_Contains()
        {
            // Arrange
            var items = new[] { EntityHelper.CreateUser("one"), EntityHelper.CreateUser("two") };
            var page = 2;
            var quantity = 4;
            var total = 6;
            var paged = new Paged<PimsUser>(items, page, quantity, total);

            // Act
            var result = paged.Contains(items.First());

            // Assert
            result.Should().BeTrue();
        }
        #endregion

        #region CopyTo
        [Fact]
        public void Paged_CopyTo()
        {
            // Arrange
            var items = new[] { EntityHelper.CreateUser("one"), EntityHelper.CreateUser("two") };
            var page = 2;
            var quantity = 4;
            var total = 6;
            var paged = new Paged<PimsUser>(items, page, quantity, total);
            var result = new PimsUser[3];

            // Act
            paged.CopyTo(result, 1);

            // Assert
            result.Count().Should().Be(3);
            result[0].Should().BeNull();
            result[1].Should().BeEquivalentTo(items[0]);
            result[2].Should().BeEquivalentTo(items[1]);
        }
        #endregion

        #region Remove
        [Fact]
        public void Paged_Remove()
        {
            // Arrange
            var items = new[] { EntityHelper.CreateUser("one"), EntityHelper.CreateUser("two") };
            var page = 2;
            var quantity = 4;
            var total = 6;
            var paged = new Paged<PimsUser>(items, page, quantity, total);

            // Act
            paged.Remove(items.First());

            // Assert
            paged.Count().Should().Be(1);
            paged.First().Should().BeEquivalentTo(items.Last());
        }
        #endregion
        #endregion
    }
}
