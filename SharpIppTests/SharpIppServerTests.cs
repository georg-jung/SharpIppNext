using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
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
}