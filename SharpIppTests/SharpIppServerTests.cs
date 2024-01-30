using Moq;
using SharpIpp.Exceptions;
using SharpIpp.Models;
using SharpIpp.Protocol;
using SharpIpp.Protocol.Models;
using System.Diagnostics.CodeAnalysis;

namespace SharpIpp.Tests;

[TestClass]
[ExcludeFromCodeCoverage]
public class SharpIppServerTests
{
    [TestMethod]
    public void Constructor_Default_InstanceShouldBeCreated()
    {
        // Arrange & Act
        SharpIppServer server = new();
        // Assert
        server.Should().NotBeNull();
    }

    [TestMethod]
    public async Task ReceiveRawRequestAsync_Stream_ShouldBeWritten()
    {
        // Arrange
        Mock<IIppProtocol> ippProtocol = new();
        IppRequestMessage ippRequestMessage = new();
        ippProtocol.Setup( x => x.ReadIppRequestAsync( It.IsAny<Stream>(), It.IsAny<CancellationToken>() ) ).ReturnsAsync( ippRequestMessage );
        SharpIppServer server = new( ippProtocol.Object );
        MemoryStream memoryStream = new();
        // Act
        Func<Task<IIppRequestMessage>> act = async () => await server.ReceiveRawRequestAsync( memoryStream );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().Be( ippRequestMessage );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_CancelJob_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.CancelJob,
            Version = IppVersion.V1_1,
            RequestId = 123,
        };
        ippRequestMessage.OperationAttributes.AddRange( new[]
        {
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "ipp://127.0.0.1:631/" ),
            new IppAttribute( Tag.Uri, JobAttribute.JobId, 123 ),
            new IppAttribute( Tag.TextWithoutLanguage, JobAttribute.RequestingUserName, "test-user" )
        } );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new CancelJobRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            JobId = 123,
            PrinterUri = new Uri( "ipp://127.0.0.1:631/" ),
            RequestingUserName = "test-user"
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_CreateJob_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.CreateJob,
            Version = IppVersion.V1_1,
            RequestId = 123,
        };
        ippRequestMessage.OperationAttributes.AddRange( new[]
        {
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "ipp://127.0.0.1:631/" ),
            new IppAttribute( Tag.Uri, JobAttribute.JobId, 123 ),
            new IppAttribute( Tag.TextWithoutLanguage, JobAttribute.RequestingUserName, "test-user" )
        } );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new CreateJobRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "ipp://127.0.0.1:631/" ),
            RequestingUserName = "test-user",
            NewJobAttributes = new()
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_UnsupportedOperation_ShouldThrowError()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.Reserved1,
            Version = IppVersion.V1_1,
            RequestId = 123,
        };
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        await act.Should().ThrowAsync<IppRequestException>();
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_StreamIsNull_ShouldThrowError()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.Reserved1,
            Version = IppVersion.V1_1,
            RequestId = 123,
        };
        // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( stream: null );
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod()]
    public async Task ReceiveRawRequestAsync_Stream_ShouldReturnMessage()
    {
        // Arrange
        Mock<IIppProtocol> protocol = new();
        IppRequestMessage message = new();
        protocol.Setup( x => x.ReadIppRequestAsync( It.IsAny<Stream>(), It.IsAny<CancellationToken>() ) ).ReturnsAsync( message );
        SharpIppServer server = new( protocol.Object );
        // Act
        Func<Task<IIppRequestMessage>> act = async () => await server.ReceiveRawRequestAsync( Stream.Null );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().Be( message );
    }

    [TestMethod()]
    public async Task ReceiveRawRequestAsync_Null_ShouldThrowException()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Func<Task<IIppRequestMessage>> act = async () => await server.ReceiveRawRequestAsync( null );
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }
}