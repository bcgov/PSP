using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using nClam;
using Pims.Av;
using Pims.Av.Configuration;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Xunit;

namespace Pims.Dal.Test.Libraries.Av
{
    [Trait("category", "unit")]
    [Trait("category", "av")]
    [Trait("group", "notification")]
    [ExcludeFromCodeCoverage]
    public class ClamAvServiceTest
    {
        #region Tests
        [Fact]
        public async void Scan_Negative()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new ClamAvOptions());
            var service = helper.Create<ClamAvService>(options, user);

            var client = helper.GetService<Mock<IClamClient>>();
            var scanResult = new ClamScanResult("ok");
            client.Setup(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>())).ReturnsAsync(scanResult);

            // Execute
            await service.ScanAsync(new byte[] { 1, 2, 3 });

            // Verify
            client.Verify(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public async void Scan_Skip()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new ClamAvOptions() { DisableScan = true });
            var service = helper.Create<ClamAvService>(options, user);

            var client = helper.GetService<Mock<IClamClient>>();
            var scanResult = new ClamScanResult("ok");
            client.Setup(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>())).ReturnsAsync(scanResult);

            // Execute
            await service.ScanAsync(new byte[] { 1, 2, 3 });

            // Verify
            client.Verify(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>()), Times.Never);
        }

        [Fact]
        public async void Scan_Positive()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new ClamAvOptions() { });
            var service = helper.Create<ClamAvService>(options, user);

            var client = helper.GetService<Mock<IClamClient>>();
            var scanResult = new ClamScanResult("found");
            client.Setup(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>())).ReturnsAsync(scanResult);

            // Execute
            var exception = await Assert.ThrowsAsync<AvException>(async () => await service.ScanAsync(new byte[] { 1, 2, 3 }));

            // Verify
            exception.Message.Should().Contain("threats found");
            client.Verify(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public async void Scan_Error()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new ClamAvOptions() { });
            var service = helper.Create<ClamAvService>(options, user);

            var client = helper.GetService<Mock<IClamClient>>();
            var scanResult = new ClamScanResult("error");
            client.Setup(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>())).ReturnsAsync(scanResult);

            // Execute
            var exception = await Assert.ThrowsAsync<AvException>(async () => await service.ScanAsync(new byte[] { 1, 2, 3 }));

            // Verify
            exception.Message.Should().Contain("error");
            client.Verify(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public async void Scan_Unknown()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new ClamAvOptions() { });
            var service = helper.Create<ClamAvService>(options, user);

            var client = helper.GetService<Mock<IClamClient>>();
            var scanResult = new ClamScanResult(string.Empty);
            client.Setup(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>())).ReturnsAsync(scanResult);

            // Execute
            var exception = await Assert.ThrowsAsync<AvException>(async () => await service.ScanAsync(new byte[] { 1, 2, 3 }));

            // Verify
            exception.Message.Should().Contain("unknown");
            client.Verify(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public async void Scan_File_Negative()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new ClamAvOptions());
            var service = helper.Create<ClamAvService>(options, user);

            var client = helper.GetService<Mock<IClamClient>>();
            var scanResult = new ClamScanResult("ok");
            client.Setup(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>())).ReturnsAsync(scanResult);

            // Execute
            await service.ScanAsync(helper.GetFormFile(string.Empty));

            // Verify
            client.Verify(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public async void Scan_File_Skip()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new ClamAvOptions() { DisableScan = true });
            var service = helper.Create<ClamAvService>(options, user);

            var client = helper.GetService<Mock<IClamClient>>();
            var scanResult = new ClamScanResult("ok");
            client.Setup(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>())).ReturnsAsync(scanResult);

            // Execute
            await service.ScanAsync(helper.GetFormFile(string.Empty));

            // Verify
            client.Verify(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>()), Times.Never);
        }

        [Fact]
        public async void Scan_File_Positive()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new ClamAvOptions() { });
            var service = helper.Create<ClamAvService>(options, user);

            var client = helper.GetService<Mock<IClamClient>>();
            var scanResult = new ClamScanResult("found");
            client.Setup(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>())).ReturnsAsync(scanResult);

            // Execute
            var exception = await Assert.ThrowsAsync<AvException>(async () => await service.ScanAsync(helper.GetFormFile(string.Empty)));

            // Verify
            exception.Message.Should().Contain("threats found");
            client.Verify(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public async void Scan_File_Error()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new ClamAvOptions() { });
            var service = helper.Create<ClamAvService>(options, user);

            var client = helper.GetService<Mock<IClamClient>>();
            var scanResult = new ClamScanResult("error");
            client.Setup(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>())).ReturnsAsync(scanResult);

            // Execute
            var exception = await Assert.ThrowsAsync<AvException>(async () => await service.ScanAsync(helper.GetFormFile(string.Empty)));

            // Verify
            exception.Message.Should().Contain("error");
            client.Verify(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public async void Scan_File_Unknown()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new ClamAvOptions() { });
            var service = helper.Create<ClamAvService>(options, user);

            var client = helper.GetService<Mock<IClamClient>>();
            var scanResult = new ClamScanResult(string.Empty);
            client.Setup(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>())).ReturnsAsync(scanResult);

            // Execute
            var exception = await Assert.ThrowsAsync<AvException>(async () => await service.ScanAsync(helper.GetFormFile(string.Empty)));

            // Verify
            exception.Message.Should().Contain("unknown");
            client.Verify(_ => _.SendAndScanFileAsync(It.IsAny<byte[]>()), Times.Once);
        }
        #endregion
    }
}
