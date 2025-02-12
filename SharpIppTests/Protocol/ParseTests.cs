
using SharpIpp.Mapping;
using SharpIpp.Mapping.Profiles;
using SharpIpp.Models;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SharpIpp.Protocol.Tests;

[TestClass]
[ExcludeFromCodeCoverage]
public class ParseTests
{
    [DataTestMethod]
    [DataRow( "RawIppResponses/GetPrinterAttributes_Canon_MX490_series_low_supply.bin" )]
    [DataRow( "RawIppResponses/GetPrinterAttributes_HP_Color_LaserJet_MFP_M476dn.bin" )]
    public async Task ParseGetPrinterAttributes(string file)
    {
        var mapper = MapperFactory();
        var ippResponse = await ReadIppResponse(file);
        var mapped = mapper.Map<GetPrinterAttributesResponse>(ippResponse);
        Assert.IsNotNull(mapped);
    }

    private async Task<IIppResponseMessage> ReadIppResponse(string file)
    {
        var protocol = new IppProtocol();
        await using var stream = new FileStream(file, FileMode.Open, FileAccess.Read);
        var ippResponse = await protocol.ReadIppResponseAsync(stream);
        return ippResponse;
    }

    private static IMapper MapperFactory()
    {
        var mapper = new SimpleMapper();
        var assembly = Assembly.GetAssembly(typeof(TypesProfile));
        mapper.FillFromAssembly(assembly!);
        return mapper;
    }
}